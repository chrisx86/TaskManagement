#nullable enable
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Core.ViewModels;
using TodoApp.WinForms.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

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
    private TodoItem? _selectedTaskForDetails;

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

        this.txtDetailComments.TextChanged += TxtDetailComments_TextChanged;
        this.btnSaveComment.Click += BtnSaveComment_Click;
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

        tvTasks.BeginUpdate();
        tvTasks.Nodes.Clear();

        var searchTerm = txtSearch.Text.Trim().ToLowerInvariant();
        PriorityLevel? priorityFilter = (cmbFilterPriority.SelectedItem as PriorityDisplayItem)?.Value;
        TodoStatus? statusFilter = (cmbFilterStatus.SelectedItem as StatusDisplayItem)?.Value;
        var onlyShowOverdue = chkFilterOverdue.Checked;
        var now = DateTime.UtcNow;

        var allFilteredTasks = new List<TodoItem>();

        foreach (var userTasksPair in _dashboardViewModel.GroupedTasks.OrderBy(p => p.Key.Username))
        {
            var user = userTasksPair.Key;
            IEnumerable<TodoItem> tasksToFilter = userTasksPair.Value;

            // --- Filtering logic remains the same ---
            if (!string.IsNullOrEmpty(searchTerm))
                tasksToFilter = tasksToFilter.Where(t => (t.Title?.ToLowerInvariant().Contains(searchTerm) ?? false) || (t.Comments?.ToLowerInvariant().Contains(searchTerm) ?? false));
            if (priorityFilter.HasValue)
                tasksToFilter = tasksToFilter.Where(t => t.Priority == priorityFilter.Value);
            if (statusFilter.HasValue)
                tasksToFilter = tasksToFilter.Where(t => t.Status == statusFilter.Value);
            if (onlyShowOverdue)
                tasksToFilter = tasksToFilter.Where(t => t.DueDate < now && t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject);

            var filteredUserTasks = tasksToFilter.ToList();
            allFilteredTasks.AddRange(filteredUserTasks);

            if (filteredUserTasks.Any() || !IsAnyFilterActive())
            {
                var uncompletedCount = filteredUserTasks.Count(t => t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject);
                var totalInListCount = filteredUserTasks.Count;
                var userNode = new TreeNode($"{user.Username} ({uncompletedCount} / {totalInListCount})");
                userNode.Tag = user;

                var sortedUserTasks = filteredUserTasks
                    .OrderBy(t => t.Status switch {
                        TodoStatus.InProgress => 0,
                        TodoStatus.Pending => 1,
                        TodoStatus.Reject => 2,
                        TodoStatus.Completed => 3,
                        _ => 4
                    })
                    .ThenByDescending(t => t.Priority)
                    .ThenBy(t => t.DueDate ?? DateTime.MaxValue);

                foreach (var task in sortedUserTasks)
                {
                    var statusPrefix = task.Status switch
                    {
                        TodoStatus.Completed => "［✓］",
                        TodoStatus.InProgress => "［→］",
                        TodoStatus.Reject => "［✗］",
                        _ => "［ ］"
                    };
                    var priorityPrefix = $"[{task.Priority}]";
                    var dueDateSuffix = task.DueDate.HasValue ? $" (Due: {task.DueDate.Value.ToLocalTime():yyyy-MM-dd})" : "";
                    var nodeText = $"{statusPrefix} {priorityPrefix} {task.Title}{dueDateSuffix}";

                    var taskNode = new TreeNode(nodeText) { Tag = task };

                    var isOverdue = task.DueDate < now && task.Status != TodoStatus.Completed && task.Status != TodoStatus.Reject;
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

        PopulateStatisticCards(allFilteredTasks);
        tvTasks.ExpandAll();
        tvTasks.EndUpdate();
        if (tvTasks.Nodes.Count > 0)
        {
            tvTasks.SelectedNode = tvTasks.Nodes[0];
            tvTasks.Nodes[0].EnsureVisible();
        }
    }

    private void PopulateStatisticCards(List<TodoItem> tasks)
    {
        if (_dashboardViewModel is null) return;
        var sourceTasks = tasks ?? _dashboardViewModel.GroupedTasks.SelectMany(kv => kv.Value).ToList();
        var totalCount = sourceTasks.Count;
        var completedCount = sourceTasks.Count(t => t.Status == TodoStatus.Completed || t.Status == TodoStatus.Reject);
        var uncompletedCount = sourceTasks.Count(t => t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject);
        var overdueCount = sourceTasks.Count(t => t.DueDate < DateTime.UtcNow && t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject);
        var unassignedCount = sourceTasks.Count(t => t.AssignedToId is null);
        var rejectedCount = sourceTasks.Count(t => t.Status == TodoStatus.Reject);

        // --- Update the UI Labels ---
        lblTotalTasksValue.Text = totalCount.ToString();
        lblCompletedValue.Text = completedCount.ToString(); // New
        lblUncompletedValue.Text = uncompletedCount.ToString();
        lblOverdueValue.Text = overdueCount.ToString();
        lblUnassignedValue.Text = unassignedCount.ToString();
        lblRejectedValue.Text = rejectedCount.ToString();

        // --- Update the Card background colors for visual cues ---
        cardOverdue.BackColor = overdueCount > 0 ? Color.MistyRose : SystemColors.Control;
        cardUnassigned.BackColor = unassignedCount > 0 ? Color.LightGoldenrodYellow : SystemColors.Control;
        cardRejected.BackColor = rejectedCount > 0 ? Color.Gainsboro : SystemColors.Control;
        cardCompleted.BackColor = completedCount > 0 ? Color.Honeydew : SystemColors.Control; // New
    }


    private bool IsAnyFilterActive() => !string.IsNullOrEmpty(txtSearch.Text) || cmbFilterPriority.SelectedIndex > 0 || cmbFilterStatus.SelectedIndex > 0 || chkFilterOverdue.Checked;

    #endregion

    #region --- Detail Panel and Actions ---

    private async void TvTasks_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not TodoItem selectedTaskInfo)
        {
            ClearAndHideDetails();
            return;
        }

        try
        {
            // --- THIS IS THE KEY FIX ---
            // Before populating the details panel, re-fetch the full entity from the database.
            // This guarantees that all navigation properties (Creator, AssignedTo) are loaded.
            var fullTaskDetails = await _taskService.GetTaskByIdAsync(selectedTaskInfo.Id);

            if (fullTaskDetails != null)
            {
                _selectedTaskForDetails = fullTaskDetails; // Update the cache for editing actions
                PopulateTaskDetails(fullTaskDetails);
                panelTaskDetails.Visible = true;
                SetActionButtonsState(true);
            }
            else
            {
                // The task might have been deleted by another user in the meantime.
                MessageBox.Show("無法獲取任務詳情，該任務可能已被刪除。視圖將會刷新。", "找不到任務", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                await LoadAndDisplayDataAsync(); // Refresh the entire dashboard
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"獲取任務詳情時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ClearAndHideDetails();
        }
    }

    private void PopulateTaskDetails(TodoItem task)
    {
        lblDetailTitle.Text = task.Title;
        lblDetailStatus.Text = task.Status.ToString();
        lblDetailPriority.Text = task.Priority.ToString();

        lblDetailDueDate.Text = task.DueDate.HasValue
            ? task.DueDate.Value.ToLocalTime().ToString("yyyy-MM-dd")
            : "(未設定)";

        // --- These lines will now work correctly ---
        lblDetailCreator.Text = task.Creator?.Username ?? "N/A";
        lblDetailAssignedTo.Text = task.AssignedTo?.Username ?? "(未指派)";

        lblDetailCreationDate.Text = task.CreationDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
        lblDetailLastModified.Text = task.LastModifiedDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm");

        // The ReadOnly property is set in the designer, but we can confirm it here.
        _isUpdatingUI = true;
        txtDetailComments.Text = task.Comments ?? string.Empty;
        txtDetailComments.ReadOnly = false; // Allow editing
        _isUpdatingUI = false;
        btnSaveComment.Enabled = false;

        // Color coding logic
        lblDetailDueDate.ForeColor = (task.DueDate < DateTime.UtcNow && task.Status != TodoStatus.Completed)
            ? Color.Red
            : SystemColors.ControlText;
    }

    private void TxtDetailComments_TextChanged(object? sender, EventArgs e)
    {
        if (!_isUpdatingUI) btnSaveComment.Enabled = true;
    }

    private void ClearAndHideDetails()
    {
        SetActionButtonsState(false);
        _selectedTaskForDetails = null;
    }

    private void SetActionButtonsState(bool enabled)
    {
        btnDetailEdit.Enabled = enabled;
        btnDetailReassign.Enabled = enabled;
        btnDetailDelete.Enabled = enabled;
        btnSaveComment.Enabled = false;

    }

    private async void BtnDetailEdit_Click(object? sender, EventArgs e)
    {
        if (tvTasks.SelectedNode?.Tag is not TodoItem selectedTaskInfo) return;
        int taskIdToSelect = selectedTaskInfo.Id;
        try
        {
            var taskToEdit = await _taskService.GetTaskByIdAsync(selectedTaskInfo.Id);
            if (taskToEdit == null)
            {
                MessageBox.Show("找不到該任務，可能已被刪除。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await LoadAndDisplayDataAsync();
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var taskDialog = scope.ServiceProvider.GetRequiredService<TaskDetailDialog>();
            taskDialog.SetTaskForEdit(taskToEdit);

            if (taskDialog.ShowDialog(this) == DialogResult.OK)
            {
                // --- STEP 2: Perform the full reload. ---
                await LoadAndDisplayDataAsync();

                // --- STEP 3: Restore the selection after reload. ---
                SelectTaskInTreeView(taskIdToSelect);
            }
        }
        catch (Exception ex) { MessageBox.Show($"打開編輯視窗時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void SelectTaskInTreeView(int taskId)
    {
        foreach (TreeNode userNode in tvTasks.Nodes)
        {
            foreach (TreeNode taskNode in userNode.Nodes)
            {
                if (taskNode.Tag is TodoItem item && item.Id == taskId)
                {
                    // Found the node. Select it.
                    tvTasks.SelectedNode = taskNode;

                    // Ensure it's visible to the user.
                    taskNode.EnsureVisible();

                    // Once found, we can exit the loops.
                    return;
                }
            }
        }
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
                    if (currentUser is null) return;

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
            if (currentUser is null) return;

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

    private async void BtnSaveComment_Click(object? sender, EventArgs e)
    {
        if (_selectedTaskForDetails == null || _userContext.CurrentUser == null) return;

        var newComments = txtDetailComments.Text.Trim();
        if (_selectedTaskForDetails.Comments == newComments)
        {
            btnSaveComment.Enabled = false;
            return;
        }

        try
        {
            SetLoadingState(true);

            // Use the currently selected task object directly.
            var taskToUpdate = _selectedTaskForDetails;
            taskToUpdate.Comments = newComments;

            // Call the update service. It will return the latest version of the task.
            var updatedTask = await _taskService.UpdateTaskAsync(_userContext.CurrentUser, taskToUpdate);

            // --- Local Update Logic ---

            // 1. Update the cached object for the details panel.
            _selectedTaskForDetails = updatedTask;

            // 2. Update the object in the master ViewModel cache.
            var owner = updatedTask.AssignedTo ?? updatedTask.Creator;
            if (owner != null && _dashboardViewModel.GroupedTasks.TryGetValue(owner, out var taskList))
            {
                var index = taskList.FindIndex(t => t.Id == updatedTask.Id);
                if (index != -1)
                {
                    taskList[index] = updatedTask;
                }
            }

            // 3. Update the object in the TreeView node's Tag and refresh the details panel.
            if (tvTasks.SelectedNode != null && tvTasks.SelectedNode.Tag is TodoItem)
            {
                tvTasks.SelectedNode.Tag = updatedTask;
                // Re-populate the details panel with the absolutely latest data.
                PopulateTaskDetails(updatedTask);
            }

            btnSaveComment.Enabled = false;
            lblStatus.Text = "備註已成功儲存。";
        }
        catch (DbUpdateConcurrencyException ex)
        {
            MessageBox.Show($"儲存備註失敗: {ex.Message}", "並行衝突", MessageBoxButtons.OK, MessageBoxIcon.Error);
            await LoadAndDisplayDataAsync(); // Fallback to full reload on error
        }
        catch (Exception ex)
        {
            MessageBox.Show($"儲存備註時發生錯誤: {ex.Message}", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
        }
    }
    #endregion

    #region --- Context Menu Handlers ---

    private void TreeViewContextMenu_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = tvTasks.SelectedNode?.Tag is not TodoItem;
    }

    private void TvTasks_MouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right) tvTasks.SelectedNode = tvTasks.GetNodeAt(e.X, e.Y);
    }

    #endregion
}