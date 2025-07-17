#nullable enable
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Core.ViewModels;
using TodoApp.WinForms.ViewModels;
using Microsoft.Extensions.DependencyInjection;

// Using alias to avoid ambiguity
using TodoStatus = TodoApp.Core.Models.TodoStatus;
using TodoApp.Infrastructure.Services;

namespace TodoApp.WinForms.Forms;

public partial class AdminDashboardForm : Form
{
    private readonly IAdminDashboardService _dashboardService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITaskService _taskService;   // This was the missing field
    private readonly IUserService _userService;     // This is also needed for reassign
    private readonly IUserContext _userContext;   // This is also needed for delete

    private DashboardViewModel? _dashboardViewModel;
    private bool _isUpdatingUI = false;

    public AdminDashboardForm(
        IAdminDashboardService dashboardService,
        IServiceProvider serviceProvider,
        ITaskService taskService,
        IUserService userService,
        IUserContext userContext)
    {
        InitializeComponent();

        // Store all injected services
        _dashboardService = dashboardService;
        _serviceProvider = serviceProvider;
        _taskService = taskService;     // Assign the injected service to the field
        _userService = userService;     // Assign the injected service to the field
        _userContext = userContext;     // Assign the injected service to the field

        // --- Wire up all events ---
        this.Load += AdminDashboardForm_Load;

        // Filters
        this.txtSearch.TextChanged += Filter_Changed;
        this.cmbFilterPriority.SelectedIndexChanged += Filter_Changed;
        this.cmbFilterStatus.SelectedIndexChanged += Filter_Changed;
        this.chkFilterOverdue.CheckedChanged += Filter_Changed;
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
        // Call the new method to populate the status filter
        PopulateStatusFilter();
        PopulatePriorityFilter();
        await LoadAndDisplayDataAsync();
    }

    private void PopulateStatusFilter()
    {
        var statusItems = new Dictionary<string, TodoStatus?> { { "所有狀態", null } };
        foreach (var status in Enum.GetValues<TodoStatus>())
        {
            statusItems.Add(status.ToString(), status);
        }
        cmbFilterStatus.DataSource = new BindingSource(statusItems, null);
        cmbFilterStatus.DisplayMember = "Key";
        cmbFilterStatus.ValueMember = "Value";
    }

    private void PopulatePriorityFilter()
    {
        var priorityItems = new List<PriorityDisplayItem>
        {
            new PriorityDisplayItem { Name = "所有優先級", Value = null }
        };

        foreach (var level in Enum.GetValues<PriorityLevel>())
        {
            priorityItems.Add(new PriorityDisplayItem { Name = level.ToString(), Value = level });
        }

        cmbFilterPriority.DataSource = priorityItems;
        // --- STEP 2: Ensure these strings match the property names in the class above. ---
        cmbFilterPriority.DisplayMember = "Name";
        cmbFilterPriority.ValueMember = "Value"; // This tells the ComboBox to use the 'Value' property for its .SelectedValue
    }

    private async Task LoadAndDisplayDataAsync()
    {
        SetLoadingState(true);
        try
        {
            _dashboardViewModel = await _dashboardService.GetDashboardDataAsync();

            // --- THIS IS THE NEW LOGIC (Task 2.2) ---
            // Populate the new statistic card Labels with data from the ViewModel.
            PopulateStatisticCards();

            // The TreeView population logic remains the same.
            ApplyFiltersAndPopulateTree();

            // The status bar can now be used for general status messages.
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

    private void PopulateStatisticCards()
    {
        if (_dashboardViewModel == null) return;

        lblTotalTasksValue.Text = _dashboardViewModel.TotalTaskCount.ToString();
        lblUncompletedValue.Text = _dashboardViewModel.UncompletedTaskCount.ToString();
        lblOverdueValue.Text = _dashboardViewModel.OverdueTaskCount.ToString();
        lblUnassignedValue.Text = _dashboardViewModel.UnassignedTaskCount.ToString();

        // Optional: Add visual cues. If there are overdue tasks, make the card stand out.
        cardOverdue.BackColor = _dashboardViewModel.OverdueTaskCount > 0
            ? Color.MistyRose
            : SystemColors.Control;

        cardUnassigned.BackColor = _dashboardViewModel.UnassignedTaskCount > 0
            ? Color.LightGoldenrodYellow
            : SystemColors.Control;
    }

    private void SetLoadingState(bool isLoading)
    {
        // Set the cursor for the entire form.
        this.UseWaitCursor = isLoading;
        panelLeft.Enabled = !isLoading;
        tvTasks.Enabled = !isLoading;
        panelRight.Enabled = !isLoading;

        // Update the status label to provide feedback to the user.
        lblStatus.Text = isLoading ? "正在載入資料..." : "準備就緒";
        tvTasks.BackColor = isLoading ? SystemColors.ControlLight : SystemColors.Window;
    }

    private void Filter_Changed(object? sender, EventArgs e)
    {
        // --- STEP 3: Check the flag at the beginning of the event handler. ---
        // If we are in the middle of a programmatic update, ignore the event.
        if (_isUpdatingUI)
        {
            return;
        }

        // The rest of the event handler logic remains the same.
        ApplyFiltersAndPopulateTree();
    }

    private void BtnClearFilter_Click(object? sender, EventArgs e)
    {
        _isUpdatingUI = true;
        try
        {
            txtSearch.Clear();
            if (cmbFilterPriority.Items.Count > 0) cmbFilterPriority.SelectedIndex = 0;
            if (cmbFilterStatus.Items.Count > 0) cmbFilterStatus.SelectedIndex = 0;
            chkFilterOverdue.Checked = false;
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
        string searchTerm = txtSearch.Text.Trim().ToLowerInvariant();
        PriorityLevel? priorityFilter = (cmbFilterPriority.SelectedItem as PriorityDisplayItem)?.Value;
        TodoStatus? statusFilter = (TodoStatus?)cmbFilterStatus.SelectedValue;
        bool onlyShowOverdue = chkFilterOverdue.Checked;
        var now = DateTime.UtcNow; // Get current time once for consistent comparison

        foreach (var userTasksPair in _dashboardViewModel.GroupedTasks.OrderBy(p => p.Key.Username))
        {
            var user = userTasksPair.Key;
            var allUserTasks = userTasksPair.Value;

            // Filtering logic remains the same
            IEnumerable<TodoItem> filteredTasks = allUserTasks;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredTasks = filteredTasks.Where(t =>
                    (t.Title?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                    (t.Comments?.ToLowerInvariant().Contains(searchTerm) ?? false)
                );
            }
            if (priorityFilter.HasValue)
            {
                filteredTasks = filteredTasks.Where(t => t.Priority == priorityFilter.Value);
            }
            if (statusFilter.HasValue) // New filter
            {
                filteredTasks = filteredTasks.Where(t => t.Status == statusFilter.Value);
            }
            if (onlyShowOverdue) // New filter
            {
                filteredTasks = filteredTasks.Where(t => t.DueDate < now && t.Status != TodoStatus.Completed);
            }
            var filteredTaskList = filteredTasks.ToList();

            if (filteredTaskList.Any() || (string.IsNullOrEmpty(searchTerm) && !priorityFilter.HasValue && !statusFilter.HasValue && !onlyShowOverdue))
            {
                int pendingCount = allUserTasks.Count(t => t.Status != TodoStatus.Completed);
                int totalCount = allUserTasks.Count;

                var userNode = new TreeNode($"{user.Username} ({pendingCount} / {totalCount})");
                userNode.Tag = user;

                foreach (var task in filteredTaskList.OrderBy(t => t.CreationDate))
                {
                    // 1. Determine the status prefix icon
                    string statusPrefix = task.Status switch
                    {
                        TodoStatus.Completed => "[✓]",
                        TodoStatus.InProgress => "[→]",
                        TodoStatus.Pending => "[ ]",
                        _ => "[?]"
                    };

                    // 2. Determine the priority prefix
                    string priorityPrefix = $"[{task.Priority}]";

                    // 3. Determine the due date suffix, handling nulls
                    string dueDateSuffix = task.DueDate.HasValue
                        ? $" (Due: {task.DueDate.Value.ToLocalTime():yyyy-MM-dd})"
                        : "";

                    // 4. Combine all parts into the final node text
                    string nodeText = $"{statusPrefix} {priorityPrefix} {task.Title}{dueDateSuffix}";

                    var taskNode = new TreeNode(nodeText);
                    taskNode.Tag = task;

                    // 5. Apply color highlighting for critical tasks
                    bool isOverdue = task.DueDate < now && task.Status != TodoStatus.Completed;
                    bool isHighPriority = task.Priority == PriorityLevel.High;

                    if (isOverdue)
                    {
                        // Overdue tasks are the most critical, use a strong color.
                        taskNode.ForeColor = Color.Red;
                        // Optional: Make it bold to stand out more.
                        taskNode.NodeFont = new Font(tvTasks.Font, FontStyle.Bold);
                    }
                    else if (isHighPriority)
                    {
                        // High priority tasks that are not yet overdue can use a different color.
                        taskNode.ForeColor = Color.DarkOrange;
                    }

                    userNode.Nodes.Add(taskNode);
                }

                tvTasks.Nodes.Add(userNode);
            }
        }

        tvTasks.ExpandAll();
        tvTasks.EndUpdate();
    }

    private void TvTasks_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node == null)
        {
            ClearAndHideDetails();
            return;
        }
        if (e.Node.Tag is TodoItem selectedTask)
        {
            PopulateTaskDetails(selectedTask);
            panelTaskDetails.Visible = true;

            // A task is selected, so enable the action buttons.
            SetActionButtonsState(true);
        }
        else
        {
            // A user node or nothing is selected, so clear details and disable buttons.
            ClearAndHideDetails();
        }
    }

    // --- NEW HELPER METHOD to manage UI state ---
    private void ClearAndHideDetails()
    {
        panelTaskDetails.Visible = false;
        SetActionButtonsState(false);

        // Optionally clear the labels to avoid showing stale data
        lblDetailTitle.Text = "請選擇一個任務以查看詳情";
        lblDetailStatus.Text = "-";
        lblDetailPriority.Text = "-";
        // ... clear other labels ...
        txtDetailComments.Clear();
    }

    // --- NEW HELPER METHOD to enable/disable action buttons ---
    private void SetActionButtonsState(bool enabled)
    {
        btnDetailEdit.Enabled = enabled;
        btnDetailReassign.Enabled = enabled;
        btnDetailDelete.Enabled = enabled;
    }

    private void PopulateTaskDetails(TodoItem task)
    {
        lblDetailTitle.Text = task.Title;
        lblDetailStatus.Text = task.Status.ToString();
        lblDetailPriority.Text = task.Priority.ToString();

        // Format the DueDate nicely, handling nulls.
        lblDetailDueDate.Text = task.DueDate.HasValue
            ? task.DueDate.Value.ToLocalTime().ToString("yyyy-MM-dd")
            : "(未設定)";

        lblDetailCreator.Text = task.Creator?.Username ?? "N/A";
        lblDetailAssignedTo.Text = task.AssignedTo?.Username ?? "(未指派)";
        txtDetailComments.Text = task.Comments ?? string.Empty;

        // Bonus: Add color coding for overdue tasks in the detail view as well.
        if (task.DueDate < DateTime.UtcNow && task.Status != TodoStatus.Completed)
        {
            lblDetailDueDate.ForeColor = Color.Red;
        }
        else
        {
            lblDetailDueDate.ForeColor = SystemColors.ControlText;
        }
    }

    private async void BtnDetailEdit_Click(object? sender, EventArgs e)
    {
        // 1. Get the currently selected task from the TreeView.
        //    We check the SelectedNode and its Tag property.
        if (tvTasks.SelectedNode?.Tag is not TodoItem selectedTaskInfo)
        {
            MessageBox.Show("請先選擇一個有效的任務。", "無效操作", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            // --- THIS IS THE KEY FIX ---
            // Re-fetch the task from the database before editing.
            // This ensures we have a fully-tracked entity with all navigation properties correctly loaded.
            //
            // 關鍵修正：
            // 在編輯前，從資料庫重新獲取一次任務。
            // 這能確保我們得到的是一個完整的、可被追蹤的、所有導覽屬性都已正確載入的實體。
            var taskToEdit = await _taskService.GetTaskByIdAsync(selectedTaskInfo.Id);

            if (taskToEdit == null)
            {
                MessageBox.Show("找不到該任務，可能已被其他使用者刪除。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await LoadAndDisplayDataAsync(); // Refresh the view
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var taskDialog = scope.ServiceProvider.GetRequiredService<TaskDetailDialog>();

            taskDialog.SetTaskForEdit(taskToEdit); // Pass the fresh, tracked entity

            if (taskDialog.ShowDialog(this) == DialogResult.OK)
            {
                await LoadAndDisplayDataAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"打開編輯視窗時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnDetailDelete_Click(object? sender, EventArgs e)
    {
        // 1. Get the currently selected task from the TreeView.
        if (tvTasks.SelectedNode?.Tag is not TodoItem selectedTask)
        {
            MessageBox.Show("請先在樹狀圖中選擇一個要刪除的任務。", "無效操作", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // 2. Show a confirmation dialog to prevent accidental deletion. This is a critical UX step.
        var confirmResult = MessageBox.Show(
            $"您確定要永久刪除任務 '{selectedTask.Title}' 嗎？\n\n此操作無法復原。",
            "確認刪除",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2 // Default focus on "No" button
        );

        if (confirmResult != DialogResult.Yes)
        {
            return; // User cancelled the operation.
        }

        // 3. If user confirms, call the delete service.
        try
        {
            var currentUser = _userContext.CurrentUser;
            if (currentUser == null)
            {
                MessageBox.Show("無法驗證使用者身分，請重新登入。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // The UI is disabled during the async operation.
            SetLoadingState(true);
            lblStatus.Text = "正在刪除任務...";

            await _taskService.DeleteTaskAsync(currentUser, selectedTask.Id);

            lblStatus.Text = "任務已刪除，正在重新整理儀表板...";

            // 4. On success, reload the entire dashboard to reflect the changes.
            await LoadAndDisplayDataAsync();
        }
        catch (UnauthorizedAccessException ex)
        {
            // Catch specific permission errors from the service layer.
            MessageBox.Show(ex.Message, "權限不足", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"刪除任務時發生錯誤: {ex.Message}", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // Even on failure, it's good to reload to ensure UI is in a consistent state.
            await LoadAndDisplayDataAsync();
        }
        finally
        {
            // Ensure the UI is always re-enabled.
            SetLoadingState(false);
        }
    }

    private async void BtnDetailReassign_Click(object? sender, EventArgs e)
    {
        if (tvTasks.SelectedNode?.Tag is not TodoItem selectedTask)
        {
            MessageBox.Show("請先在樹狀圖中選擇一個要重新指派的任務。", "無效操作", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            // Fetch all users to populate the selection dialog.
            // This now correctly uses the injected _userService.
            var allUsers = await _userService.GetAllUsersAsync();

            // 2. Create and show the new UserSelectionDialog.
            using (var userDialog = new UserSelectionDialog(allUsers))
            {
                // 3. If the user selects a new user and clicks "OK"...
                if (userDialog.ShowDialog(this) == DialogResult.OK && userDialog.SelectedUser != null)
                {
                    var currentUser = _userContext.CurrentUser;
                    if (currentUser == null) return;
                    var newAssignee = userDialog.SelectedUser;
                    // Prevent reassigning to the same user.
                    if (selectedTask.AssignedToId == newAssignee.Id)
                    {
                        lblStatus.Text = $"任務 '{selectedTask.Title}' 已經指派給 {newAssignee.Username}。";
                        return;
                    }

                    // 4. Update the task object and call the update service.
                    selectedTask.AssignedToId = newAssignee.Id;

                    SetLoadingState(true);
                    lblStatus.Text = $"正在將任務指派給 {newAssignee.Username}...";

                    // This now correctly uses the injected _taskService.
                    await _taskService.UpdateTaskAsync(currentUser, selectedTask);

                    lblStatus.Text = "任務已重新指派，正在重新整理儀表板...";

                    // 5. On success, reload the dashboard to reflect the change.
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

    private void TreeViewContextMenu_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        // Check if there is a node selected in the TreeView.
        // We use SelectedNode because TvTasks_MouseUp ensures it's set correctly.
        var selectedNode = tvTasks.SelectedNode;

        // If a valid task node is selected, show the context menu.
        if (selectedNode?.Tag is TodoItem)
        {
            // All items are applicable to a task, so no need to hide any.
            // If you had user-specific actions, you would control their visibility here.
            ctxEditTask.Visible = true;
            ctxReassignTask.Visible = true;
            ctxDeleteTask.Visible = true;
        }
        else
        {
            // If a user node or nothing is selected, cancel the showing of the entire menu.
            e.Cancel = true;
        }
    }

    // --- NEW EVENT HANDLER to select the node under the cursor on right-click ---
    private void TvTasks_MouseUp(object? sender, MouseEventArgs e)
    {
        // Check if the right mouse button was clicked.
        if (e.Button == MouseButtons.Right)
        {
            // Find the node at the mouse's location.
            TreeNode? nodeAtMousePosition = tvTasks.GetNodeAt(e.X, e.Y);

            // If a node is found, select it.
            // This ensures that when the context menu opens, the SelectedNode property is correct.
            if (nodeAtMousePosition != null)
            {
                tvTasks.SelectedNode = nodeAtMousePosition;
            }
        }
    }
}