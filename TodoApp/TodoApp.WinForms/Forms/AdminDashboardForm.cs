#nullable enable
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Core.ViewModels;
using TodoApp.WinForms.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.WinForms.Forms;

public partial class AdminDashboardForm : Form
{
    private readonly IAdminDashboardService _dashboardService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITaskService _taskService;
    private readonly IUserService _userService;
    private readonly IUserContext _userContext;

    private DashboardViewModel? _dashboardViewModel;
    private bool _isUpdatingUI;

    public AdminDashboardForm(
        IAdminDashboardService dashboardService,
        IServiceProvider serviceProvider,
        ITaskService taskService,
        IUserService userService,
        IUserContext userContext)
    {
        InitializeComponent();
        _dashboardService = dashboardService;
        _serviceProvider = serviceProvider;
        _taskService = taskService;
        _userService = userService;
        _userContext = userContext;

        WireUpEvents();
    }

    private void WireUpEvents()
    {
        this.Load += AdminDashboardForm_Load;

        // Filters
        this.txtSearch.TextChanged += Filter_Changed;
        this.cmbFilterPriority.SelectedIndexChanged += Filter_Changed;
        this.cmbFilterStatus.SelectedIndexChanged += Filter_Changed;
        this.chkFilterOverdue.CheckedChanged += Filter_Changed;
        this.cmbFilterByUser.SelectedIndexChanged += Filter_Changed;
        this.btnClearFilter.Click += BtnClearFilter_Click;

        // TreeView
        this.tvTasks.AfterSelect += TvTasks_AfterSelect;
        this.tvTasks.MouseUp += TvTasks_MouseUp;

        // Detail Panel Buttons
        this.btnDetailEdit.Click += BtnDetailEdit_Click;
        this.btnDetailReassign.Click += BtnDetailReassign_Click;
        this.btnDetailDelete.Click += BtnDetailDelete_Click;

        // Context Menu
        this.treeViewContextMenu.Opening += TreeViewContextMenu_Opening;
        this.ctxEditTask.Click += BtnDetailEdit_Click;
        this.ctxReassignTask.Click += BtnDetailReassign_Click;
        this.ctxDeleteTask.Click += BtnDetailDelete_Click;
    }

    private async void AdminDashboardForm_Load(object? sender, EventArgs e)
    {
        await PopulateUserFilterAsync();
        PopulateStatusFilter();
        PopulatePriorityFilter();
        await LoadAndDisplayDataAsync();
    }

    #region --- Setup & Data Loading ---

    private class PriorityDisplayItem
    {
        public string Name { get; set; } = string.Empty;
        public PriorityLevel? Value { get; set; }
    }

    private void PopulateStatusFilter()
    {
        var statusItems = new List<StatusDisplayItem> { new() { Name = "所有狀態", Value = null } };
        foreach (var status in Enum.GetValues<TodoStatus>()) { statusItems.Add(new StatusDisplayItem { Name = status.ToString(), Value = status }); }
        cmbFilterStatus.DataSource = statusItems;
        cmbFilterStatus.DisplayMember = nameof(StatusDisplayItem.Name);
        cmbFilterStatus.ValueMember = nameof(StatusDisplayItem.Value);
    }

    private void PopulatePriorityFilter()
    {
        var priorityItems = new List<PriorityDisplayItem> { new() { Name = "所有優先級", Value = null } };
        foreach (var level in Enum.GetValues<PriorityLevel>()) { priorityItems.Add(new PriorityDisplayItem { Name = level.ToString(), Value = level }); }
        cmbFilterPriority.DataSource = priorityItems;
        cmbFilterPriority.DisplayMember = nameof(PriorityDisplayItem.Name);
        cmbFilterPriority.ValueMember = nameof(PriorityDisplayItem.Value);
    }

    private async Task LoadAndDisplayDataAsync()
    {
        SetLoadingState(true);
        try
        {
            _dashboardViewModel = await _dashboardService.GetDashboardDataAsync();
            ApplyFiltersAndPopulateTree();
            lblStatus.Text = "儀表板資料已更新。";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"載入儀表板資料時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = "資料載入失敗！";
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    #endregion

    #region --- UI State & Filtering ---

    private void SetLoadingState(bool isLoading)
    {
        this.UseWaitCursor = isLoading;
        panelLeft.Enabled = !isLoading;
        tvTasks.Enabled = !isLoading;
        panelRight.Enabled = !isLoading;
        lblStatus.Text = isLoading ? "正在載入..." : "準備就緒";
        tvTasks.BackColor = isLoading ? SystemColors.ControlLight : SystemColors.Window;
    }

    private void Filter_Changed(object? sender, EventArgs e)
    {
        if (_isUpdatingUI) return;
        ApplyFiltersAndPopulateTree();
    }

    private async Task PopulateUserFilterAsync()
    {
        var allUsers = await _userService.GetAllUsersAsync();
        var userDataSource = new List<UserDisplayItem>
        {
            new UserDisplayItem { Id = 0, Username = "所有使用者" }
        };
        foreach (var user in allUsers.OrderBy(u => u.Username))
        {
            userDataSource.Add(new UserDisplayItem { Id = user.Id, Username = user.Username });
        }

        cmbFilterByUser.DataSource = userDataSource;
        cmbFilterByUser.DisplayMember = nameof(UserDisplayItem.Username);
        cmbFilterByUser.ValueMember = nameof(UserDisplayItem.Id);
    }

    private void BtnClearFilter_Click(object? sender, EventArgs e)
    {
        _isUpdatingUI = true;
        try
        {
            txtSearch.Clear();
            if (cmbFilterPriority.Items.Count > 0) cmbFilterPriority.SelectedIndex = 0;
            if (cmbFilterStatus.Items.Count > 0) cmbFilterStatus.SelectedIndex = 0;
            if (cmbFilterByUser.Items.Count > 0) cmbFilterByUser.SelectedIndex = 0;
            chkFilterOverdue.Checked = false;
        }
        finally { _isUpdatingUI = false; }
        ApplyFiltersAndPopulateTree();
    }

    private void ApplyFiltersAndPopulateTree()
    {
        if (_dashboardViewModel is null) return;

        // Use BeginUpdate/EndUpdate to prevent flickering and improve performance during large updates.
        tvTasks.BeginUpdate();
        tvTasks.Nodes.Clear();

        // --- Get current filter values from the UI controls ---
        var searchTerm = txtSearch.Text.Trim().ToLowerInvariant();
        PriorityLevel? priorityFilter = (cmbFilterPriority.SelectedItem as PriorityDisplayItem)?.Value;
        TodoStatus? statusFilter = (cmbFilterStatus.SelectedItem as StatusDisplayItem)?.Value;
        var onlyShowOverdue = chkFilterOverdue.Checked;
        var now = DateTime.UtcNow;

        int? userFilterId = (cmbFilterByUser.SelectedItem as UserDisplayItem)?.Id;
        // This list will hold all tasks from all users that match the current filters.
        // It's used to update the statistic cards dynamically.
        var allFilteredTasks = new List<TodoItem>();
        // Iterate through the cached user data, ordered by username for consistent display.
        foreach (var userTasksPair in _dashboardViewModel.GroupedTasks.OrderBy(p => p.Key.Username))
        {
            var user = userTasksPair.Key;
            IEnumerable<TodoItem> tasksToFilter = userTasksPair.Value;
            if (userFilterId.HasValue && userFilterId > 0 && user.Id != userFilterId)
            {
                continue; // Skip this user entirely
            }
            // --- Apply all filters sequentially to the user's task list ---
            if (!string.IsNullOrEmpty(searchTerm))
                tasksToFilter = tasksToFilter.Where(t =>
                    (t.Title?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                    (t.Comments?.ToLowerInvariant().Contains(searchTerm) ?? false));

            if (priorityFilter.HasValue)
                tasksToFilter = tasksToFilter.Where(t => t.Priority == priorityFilter.Value);

            if (statusFilter.HasValue)
                tasksToFilter = tasksToFilter.Where(t => t.Status == statusFilter.Value);

            if (onlyShowOverdue)
                tasksToFilter = tasksToFilter.Where(t => t.DueDate < now && t.Status != TodoStatus.Completed);

            var filteredUserTasks = tasksToFilter.ToList();
            allFilteredTasks.AddRange(filteredUserTasks);

            // --- Logic to decide whether to show the user node ---
            // Show the user if they have tasks matching the filter, or if no filters are active.
            if (filteredUserTasks.Any() || !IsAnyFilterActive())
            {
                // The user node now shows counts based on the *filtered* list for that user.
                var userNode = new TreeNode($"{user.Username} ({filteredUserTasks.Count(t => t.Status != TodoStatus.Completed)} / {filteredUserTasks.Count})");
                userNode.Tag = user;

                // --- THIS IS THE NEW SORTING LOGIC ---
                // Apply the custom multi-level sorting to the user's filtered task list.
                var sortedUserTasks = filteredUserTasks
                    .OrderBy(t => t.Status == TodoStatus.InProgress ? 0 : (t.Status == TodoStatus.Pending ? 1 : 2))
                    .ThenByDescending(t => t.Priority)
                    .ThenBy(t => t.DueDate ?? DateTime.MaxValue);

                // Now, iterate through the *sorted* list to create task nodes.
                foreach (var task in sortedUserTasks)
                {
                    // Format the node text to be rich with information.
                    var statusPrefix = task.Status switch
                    {
                        TodoStatus.Completed => "[✓]",
                        TodoStatus.InProgress => "[→]",
                        _ => "[ ]"
                    };
                    var priorityPrefix = $"[{task.Priority}]";
                    var dueDateSuffix = task.DueDate.HasValue ? $" (Due: {task.DueDate.Value.ToLocalTime():yyyy-MM-dd})" : "";
                    var nodeText = $"{statusPrefix} {priorityPrefix} {task.Title}{dueDateSuffix}";

                    var taskNode = new TreeNode(nodeText) { Tag = task };

                    // Apply color highlighting for critical tasks.
                    var isOverdue = task.DueDate < now && task.Status != TodoStatus.Completed;
                    if (isOverdue)
                    {
                        taskNode.ForeColor = Color.Red;
                        taskNode.NodeFont = new Font(tvTasks.Font, FontStyle.Bold);
                    }
                    else if (task.Priority == PriorityLevel.Urgent)
                    {
                        taskNode.ForeColor = Color.DarkMagenta;
                    }

                    userNode.Nodes.Add(taskNode);
                }
                tvTasks.Nodes.Add(userNode);
            }
        }

        // --- Update statistic cards based on the aggregated filtered results ---
        PopulateStatisticCards(allFilteredTasks);

        tvTasks.ExpandAll();
        tvTasks.EndUpdate();
    }

    private void PopulateStatisticCards(List<TodoItem> tasks)
    {
        lblTotalTasksValue.Text = tasks.Count.ToString();
        lblUncompletedValue.Text = tasks.Count(t => t.Status != TodoStatus.Completed).ToString();
        lblOverdueValue.Text = tasks.Count(t => t.DueDate < DateTime.UtcNow && t.Status != TodoStatus.Completed).ToString();
        lblUnassignedValue.Text = tasks.Count(t => t.AssignedToId == null).ToString();

        cardOverdue.BackColor = int.Parse(lblOverdueValue.Text) > 0 ? Color.MistyRose : SystemColors.Control;
        cardUnassigned.BackColor = int.Parse(lblUnassignedValue.Text) > 0 ? Color.LightGoldenrodYellow : SystemColors.Control;
    }

    private bool IsAnyFilterActive() => !string.IsNullOrEmpty(txtSearch.Text) || cmbFilterPriority.SelectedIndex > 0 || cmbFilterStatus.SelectedIndex > 0 || chkFilterOverdue.Checked;

    #endregion

    #region --- Detail Panel and Actions ---

    private void TvTasks_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is TodoItem selectedTask)
        {
            PopulateTaskDetails(selectedTask);
            panelTaskDetails.Visible = true;
            SetActionButtonsState(true);
        }
        else
        {
            ClearAndHideDetails();
        }
    }

    private void PopulateTaskDetails(TodoItem task)
    {
        lblDetailTitle.Text = task.Title;
        lblDetailStatus.Text = task.Status.ToString();
        lblDetailPriority.Text = task.Priority.ToString();
        lblDetailDueDate.Text = task.DueDate.HasValue ? task.DueDate.Value.ToLocalTime().ToString("yyyy-MM-dd") : "(未設定)";
        lblDetailCreator.Text = task.Creator?.Username ?? "N/A";
        lblDetailAssignedTo.Text = task.AssignedTo?.Username ?? "(未指派)";
        lblDetailCreationDate.Text = task.CreationDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
        lblDetailLastModified.Text = task.LastModifiedDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
        txtDetailComments.Text = task.Comments ?? string.Empty;

        lblDetailDueDate.ForeColor = (task.DueDate < DateTime.UtcNow && task.Status != TodoStatus.Completed) ? Color.Red : SystemColors.ControlText;
    }

    private void ClearAndHideDetails()
    {
        panelTaskDetails.Visible = false;
        SetActionButtonsState(false);
    }

    private void SetActionButtonsState(bool enabled)
    {
        btnDetailEdit.Enabled = enabled;
        btnDetailReassign.Enabled = enabled;
        btnDetailDelete.Enabled = enabled;
    }

    private async void BtnDetailEdit_Click(object? sender, EventArgs e)
    {
        if (tvTasks.SelectedNode?.Tag is not TodoItem selectedTaskInfo) return;

        try
        {
            var taskToEdit = await _taskService.GetTaskByIdAsync(selectedTaskInfo.Id);
            if (taskToEdit == null)
            {
                MessageBox.Show("找不到該任務，可能已被其他使用者刪除。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                await LoadAndDisplayDataAsync();
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var taskDialog = scope.ServiceProvider.GetRequiredService<TaskDetailDialog>();
            taskDialog.SetTaskForEdit(taskToEdit);

            if (taskDialog.ShowDialog(this) == DialogResult.OK) { await LoadAndDisplayDataAsync(); }
        }
        catch (Exception ex) { MessageBox.Show($"打開編輯視窗時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private async void BtnDetailReassign_Click(object? sender, EventArgs e)
    {
        if (tvTasks.SelectedNode?.Tag is not TodoItem selectedTask) return;

        try
        {
            var allUsers = await _userService.GetAllUsersAsync();
            using (var userDialog = new UserSelectionDialog(allUsers))
            {
                if (userDialog.ShowDialog(this) == DialogResult.OK && userDialog.SelectedUser != null)
                {
                    var currentUser = _userContext.CurrentUser;
                    if (currentUser == null) return;

                    var newAssignee = userDialog.SelectedUser;
                    if (selectedTask.AssignedToId == newAssignee.Id) return;

                    selectedTask.AssignedToId = newAssignee.Id;
                    SetLoadingState(true);
                    await _taskService.UpdateTaskAsync(currentUser, selectedTask);
                    await LoadAndDisplayDataAsync();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"重新指派任務時發生錯誤: {ex.Message}", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetLoadingState(false);
        }
    }

    private async void BtnDetailDelete_Click(object? sender, EventArgs e)
    {
        if (tvTasks.SelectedNode?.Tag is not TodoItem selectedTask) return;

        var confirmResult = MessageBox.Show($"您確定要永久刪除任務 '{selectedTask.Title}' 嗎？", "確認刪除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmResult != DialogResult.Yes) return;

        try
        {
            var currentUser = _userContext.CurrentUser;
            if (currentUser == null) return;

            SetLoadingState(true);
            await _taskService.DeleteTaskAsync(currentUser, selectedTask.Id);
            await LoadAndDisplayDataAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"刪除任務時發生錯誤: {ex.Message}", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            await LoadAndDisplayDataAsync();
        }
        finally { SetLoadingState(false); }
    }

    #endregion

    #region --- Context Menu Handlers ---

    private void TreeViewContextMenu_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = tvTasks.SelectedNode?.Tag is not TodoItem;
    }

    private void TvTasks_MouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            tvTasks.SelectedNode = tvTasks.GetNodeAt(e.X, e.Y);
        }
    }

    #endregion
}