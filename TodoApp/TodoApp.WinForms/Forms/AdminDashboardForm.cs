#nullable enable
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Core.ViewModels;
using TodoApp.WinForms.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Comparers;

namespace TodoApp.WinForms.Forms;

public partial class AdminDashboardForm : Form
{
    private CardFilterType _activeCardFilter = CardFilterType.None;
    private readonly IAdminDashboardService _dashboardService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITaskService _taskService;
    private readonly IUserService _userService;
    private readonly IUserContext _userContext;

    private DashboardViewModel? _dashboardViewModel;
    private bool _isUpdatingUI;
    private bool _isUpdatingStatisticCardsUI = true;
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
        WireUpCardClickEvents();
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
        this.btnViewHistory.Click += BtnViewHistory_Click;
        this.cardTotalTasks.Cursor = Cursors.Hand;
        this.cardUncompleted.Cursor = Cursors.Hand;
        this.cardOverdue.Cursor = Cursors.Hand;
        this.cardUnassigned.Cursor = Cursors.Hand;
        this.cardRejected.Cursor = Cursors.Hand;
        this.cardCompleted.Cursor = Cursors.Hand;

    }
    private void WireUpCardClickEvents()
    {
        // Card 1: Total Tasks
        cardTotalTasks.Click += Card_Click;
        lblTotalTasksTitle.Click += Card_Click;
        lblTotalTasksValue.Click += Card_Click;

        // Card 2: Uncompleted
        cardUncompleted.Click += Card_Click;
        lblUncompletedTitle.Click += Card_Click;
        lblUncompletedValue.Click += Card_Click;

        // Card 3: Overdue
        cardOverdue.Click += Card_Click;
        lblOverdueTitle.Click += Card_Click;
        lblOverdueValue.Click += Card_Click;

        // Card 4: Unassigned
        cardUnassigned.Click += Card_Click;
        lblUnassignedTitle.Click += Card_Click;
        lblUnassignedValue.Click += Card_Click;

        // Card 5: Rejected
        cardRejected.Click += Card_Click;
        lblRejectedTitle.Click += Card_Click;
        lblRejectedValue.Click += Card_Click;

        // Card 6: Completed
        cardCompleted.Click += Card_Click;
        lblCompletedTitle.Click += Card_Click;
        lblCompletedValue.Click += Card_Click;
    }

    private void Card_Click(object? sender, EventArgs e)
    {
        Panel? clickedCard = null;
        if (sender is Panel panel)
            clickedCard = panel;
        else if (sender is Label label && label.Parent is Panel parentPanel)
            clickedCard = parentPanel;
        if (clickedCard is null) return;
        var clickedFilterType = clickedCard.Name switch
        {
            "cardTotalTasks" => CardFilterType.Total,
            "cardUncompleted" => CardFilterType.Uncompleted,
            "cardOverdue" => CardFilterType.Overdue,
            "cardUnassigned" => CardFilterType.Unassigned,
            "cardRejected" => CardFilterType.Rejected,
            "cardCompleted" => CardFilterType.Completed,
            _ => CardFilterType.None
        };

        _activeCardFilter = (_activeCardFilter == clickedFilterType) ? CardFilterType.None : clickedFilterType;
        _isUpdatingStatisticCardsUI = false;
        ApplyFiltersAndPopulateTree();
        _isUpdatingStatisticCardsUI = true;
    }

    private void HighlightActiveCard()
    {
        var cardMap = new Dictionary<CardFilterType, Panel>
        {
            [CardFilterType.Total] = cardTotalTasks,
            [CardFilterType.Uncompleted] = cardUncompleted,
            [CardFilterType.Overdue] = cardOverdue,
            [CardFilterType.Unassigned] = cardUnassigned,
            [CardFilterType.Rejected] = cardRejected,
            [CardFilterType.Completed] = cardCompleted
        };

        foreach (var card in cardMap.Values)
            card.BorderStyle = BorderStyle.FixedSingle;

        if (_activeCardFilter != CardFilterType.None && cardMap.TryGetValue(_activeCardFilter, out var selectedCard))
            selectedCard.BorderStyle = BorderStyle.Fixed3D;
    }

    private async void AdminDashboardForm_Load(object? sender, EventArgs e)
    {
        await PopulateUserFilterAsync();
        PopulateStatusFilter();
        PopulatePriorityFilter();
        await LoadAndDisplayDataAsync();
    }

    #region --- Setup & Data Loading ---

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
        _activeCardFilter = CardFilterType.None;
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
            _activeCardFilter = CardFilterType.None;
            ClearManualFilters();
        }
        finally
        {
            _isUpdatingUI = false;
        }

        ApplyFiltersAndPopulateTree();
    }

    private void ApplyFiltersAndPopulateTree()
    {
        if (_dashboardViewModel is null) return;

        tvTasks.BeginUpdate();
        tvTasks.Nodes.Clear();

        var now = DateTime.Now;
        IEnumerable<TodoItem> tasksToDisplay = _dashboardViewModel.GroupedTasks.SelectMany(pair => pair.Value);

        var searchTerm = txtSearch.Text.Trim().ToLowerInvariant();
        PriorityLevel? priorityFilter = (cmbFilterPriority.SelectedItem as PriorityDisplayItem)?.Value;
        TodoStatus? statusFilter = (cmbFilterStatus.SelectedItem as StatusDisplayItem)?.Value;
        var onlyShowOverdue = chkFilterOverdue.Checked;
        int? userFilterId = (cmbFilterByUser.SelectedItem as UserDisplayItem)?.Id;

        if (!string.IsNullOrEmpty(searchTerm))
            tasksToDisplay = tasksToDisplay.Where(t => (t.Title?.ToLowerInvariant().Contains(searchTerm) ?? false) || (t.Comments?.ToLowerInvariant().Contains(searchTerm) ?? false));
        if (priorityFilter.HasValue)
            tasksToDisplay = tasksToDisplay.Where(t => t.Priority == priorityFilter.Value);
        if (statusFilter.HasValue)
            tasksToDisplay = tasksToDisplay.Where(t => t.Status == statusFilter.Value);
        if (onlyShowOverdue)
            tasksToDisplay = tasksToDisplay.Where(t => t.DueDate < now.AddDays(-1) && t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject);
        if (userFilterId.HasValue && userFilterId > 0)
            tasksToDisplay = tasksToDisplay.Where(t => (t.AssignedToId ?? t.CreatorId) == userFilterId);

        if (_activeCardFilter != CardFilterType.None)
        {
            tasksToDisplay = _activeCardFilter switch
            {
                CardFilterType.Uncompleted => tasksToDisplay.Where(t => t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject),
                CardFilterType.Overdue => tasksToDisplay.Where(t => t.DueDate < now.AddDays(-1) && t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject),
                CardFilterType.Unassigned => tasksToDisplay.Where(t => t.AssignedToId is null),
                CardFilterType.Rejected => tasksToDisplay.Where(t => t.Status == TodoStatus.Reject),
                CardFilterType.Completed => tasksToDisplay.Where(t => t.Status == TodoStatus.Completed),
                _ => tasksToDisplay
            };
        }

        var filteredTaskList = tasksToDisplay.ToList();
        if (_isUpdatingStatisticCardsUI)
        {
            PopulateStatisticCards(filteredTaskList);
        }
        PopulateTreeView(filteredTaskList);
        HighlightActiveCard();

        tvTasks.ExpandAll();
        if (tvTasks.Nodes.Count > 0)
        {
            tvTasks.SelectedNode = tvTasks.Nodes[0];
            tvTasks.Nodes[0].EnsureVisible();
        }
        tvTasks.EndUpdate();
    }

    private void ClearManualFilters()
    {
        txtSearch.Clear();
        if (cmbFilterPriority.Items.Count > 0) cmbFilterPriority.SelectedIndex = 0;
        if (cmbFilterStatus.Items.Count > 0) cmbFilterStatus.SelectedIndex = 0;
        if (cmbFilterByUser.Items.Count > 0) cmbFilterByUser.SelectedIndex = 0;
        chkFilterOverdue.Checked = false;
    }

    private TreeNode CreateTaskNode(TodoItem task, DateTime now)
    {
        var statusPrefix = task.Status switch
        {
            TodoStatus.Completed => "[✓]",
            TodoStatus.InProgress => "[→]",
            TodoStatus.Reject => "[✗]",
            _ => "[ ]"
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
        return taskNode;
    }

    private void PopulateStatisticCards(List<TodoItem> tasks)
    {
        if (_dashboardViewModel is null) return;
        var sourceTasks = tasks ?? _dashboardViewModel.GroupedTasks.SelectMany(kv => kv.Value).ToList();
        var totalCount = sourceTasks.Count;
        var completedCount = sourceTasks.Count(t => t.Status == TodoStatus.Completed);
        var uncompletedCount = sourceTasks.Count(t => t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject);
        var overdueCount = sourceTasks.Count(t => t.DueDate < DateTime.Now.AddDays(-1) && t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject);
        var unassignedCount = sourceTasks.Count(t => t.AssignedToId is null);
        var rejectedCount = sourceTasks.Count(t => t.Status == TodoStatus.Reject);

        lblTotalTasksValue.Text = totalCount.ToString();
        lblCompletedValue.Text = completedCount.ToString();
        lblUncompletedValue.Text = uncompletedCount.ToString();
        lblOverdueValue.Text = overdueCount.ToString();
        lblUnassignedValue.Text = unassignedCount.ToString();
        lblRejectedValue.Text = rejectedCount.ToString();

        cardOverdue.BackColor = overdueCount > 0 ? Color.MistyRose : SystemColors.Control;
        cardUnassigned.BackColor = unassignedCount > 0 ? Color.LightGoldenrodYellow : SystemColors.Control;
        cardRejected.BackColor = rejectedCount > 0 ? Color.Gainsboro : SystemColors.Control;
        cardCompleted.BackColor = completedCount > 0 ? Color.Honeydew : SystemColors.Control;
    }

    /// <summary>
    /// Populates the TreeView with a given list of tasks, grouping them by user.
    /// </summary>
    /// <param name="filteredTasks">The list of tasks to display, assumed to be already filtered.</param>
    private void PopulateTreeView(List<TodoItem> filteredTasks)
    {
        var now = DateTime.Now;

        var tasksGroupedForTree = filteredTasks
            .GroupBy(t => t.AssignedTo ?? t.Creator, new UserEqualityComparer())
            .OrderBy(g => g.Key.Username);

        foreach (var userGroup in tasksGroupedForTree)
        {
            if (userGroup.Key is null) continue;

            var user = userGroup.Key;
            var userTasks = userGroup.ToList();

            int uncompletedCount = userTasks.Count(t => t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject);
            var userNode = new TreeNode($"{user.Username} ({uncompletedCount} / {userTasks.Count})")
            {
                Tag = user
            };

            var sortedUserTasks = userTasks
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
                userNode.Nodes.Add(CreateTaskNode(task, now));
            }

            tvTasks.Nodes.Add(userNode);
        }
    }

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
            var fullTaskDetails = await _taskService.GetTaskByIdAsync(selectedTaskInfo.Id);

            if (fullTaskDetails is not null)
            {
                _selectedTaskForDetails = fullTaskDetails;
                PopulateTaskDetails(fullTaskDetails);
                panelTaskDetails.Visible = true;
                SetActionButtonsState(true);
            }
            else
            {
                MessageBox.Show("無法獲取任務詳情，該任務可能已被刪除。視圖將會刷新。", "找不到任務", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                await LoadAndDisplayDataAsync();
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

        lblDetailCreator.Text = task.Creator?.Username ?? "N/A";
        lblDetailAssignedTo.Text = task.AssignedTo?.Username ?? "(未指派)";

        lblDetailCreationDate.Text = task.CreationDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
        lblDetailLastModified.Text = task.LastModifiedDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm");

        txtDetailComments.Text = task.Comments ?? string.Empty;
        _isUpdatingUI = false;
        btnSaveComment.Enabled = false;
        lblStatus.Text = $"已選取任務: {task.Title}";
        lblDetailDueDate.ForeColor = (task.DueDate < DateTime.Now && task.Status != TodoStatus.Completed)
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
        lblStatus.Text = "準備就緒";
    }

    private void SetActionButtonsState(bool enabled)
    {
        btnDetailEdit.Enabled = enabled;
        btnDetailReassign.Enabled = enabled;
        btnDetailDelete.Enabled = enabled;
        btnSaveComment.Enabled = false;
        btnViewHistory.Enabled = enabled;
    }

    private async void BtnDetailEdit_Click(object? sender, EventArgs e)
    {
        if (tvTasks.SelectedNode?.Tag is not TodoItem selectedTaskInfo) return;
        var taskIdToSelect = selectedTaskInfo.Id;
        try
        {
            var taskToEdit = await _taskService.GetTaskByIdAsync(selectedTaskInfo.Id);
            if (taskToEdit is null)
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
                await LoadAndDisplayDataAsync();
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
                    tvTasks.SelectedNode = taskNode;
                    taskNode.EnsureVisible();
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
        if (_selectedTaskForDetails is null || _userContext.CurrentUser is null) return;

        var newComments = txtDetailComments.Text.Trim();
        if (_selectedTaskForDetails.Comments == newComments)
        {
            btnSaveComment.Enabled = false;
            return;
        }

        try
        {
            SetLoadingState(true);

            var taskToUpdate = _selectedTaskForDetails;
            taskToUpdate.Comments = newComments;

            var updatedTask = await _taskService.UpdateTaskAsync(_userContext.CurrentUser, taskToUpdate);

            _selectedTaskForDetails = updatedTask;

            var owner = updatedTask.AssignedTo ?? updatedTask.Creator;
            if (owner is not null && _dashboardViewModel.GroupedTasks.TryGetValue(owner, out var taskList))
            {
                var index = taskList.FindIndex(t => t.Id == updatedTask.Id);
                if (index != -1)
                {
                    taskList[index] = updatedTask;
                }
            }

            if (tvTasks.SelectedNode is not null && tvTasks.SelectedNode.Tag is TodoItem)
            {
                tvTasks.SelectedNode.Tag = updatedTask;
                PopulateTaskDetails(updatedTask);
            }

            btnSaveComment.Enabled = false;
            lblStatus.Text = "備註已成功儲存。";
        }
        catch (DbUpdateConcurrencyException ex)
        {
            MessageBox.Show($"儲存備註失敗: {ex.Message}", "並行衝突", MessageBoxButtons.OK, MessageBoxIcon.Error);
            await LoadAndDisplayDataAsync();
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

    private void BtnViewHistory_Click(object? sender, EventArgs e)
    {
        if (tvTasks.SelectedNode?.Tag is not TodoItem selectedTask)
        {
            MessageBox.Show("請先選擇一個任務以查看其歷史記錄。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            // --- Manually create the dialog and pass dependencies from the current scope ---
            // This is a clean way to handle dialogs with runtime parameters.
            var historyService = _serviceProvider.GetRequiredService<ITaskHistoryService>();
            var userService = _serviceProvider.GetRequiredService<IUserService>();

            using (var historyDialog = new TaskHistoryDialog(
                selectedTask.Id,
                selectedTask.Title,
                historyService,
                userService))
            {
                historyDialog.ShowDialog(this);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"開啟歷史記錄視窗時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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