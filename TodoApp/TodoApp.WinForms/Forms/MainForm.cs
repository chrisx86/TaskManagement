#nullable enable
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.WinForms.ViewModels;
using TodoStatus = TodoApp.Core.Models.TodoStatus;

namespace TodoApp.WinForms.Forms;

public partial class MainForm : Form
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITaskService _taskService;
    private readonly IUserService _userService;
    private readonly IUserContext _userContext;
    private readonly User _currentUser;

    // Cache the list of users for dropdowns to avoid repeated DB calls.
    private List<User> _allUsers = new();

    // A BindingList is used as the DataSource for the DataGridView.
    // It automatically notifies the grid of changes (add, remove, reset).
    private readonly BindingList<TodoItem> _tasksBindingList = new();

    // --- NEW FIELDS for managing sorting state ---
    private DataGridViewColumn? _sortedColumn = null;
    private ListSortDirection _sortDirection = ListSortDirection.Ascending;

    // Timer for debouncing filter changes
    private System.Windows.Forms.Timer? _filterDebounceTimer;
    private bool _isUpdatingUI = false;
    private int _currentPage = 1;
    private int _pageSize = 20;
    private int _totalTasks = 0;
    private int _totalPages = 0;

    public MainForm(IServiceProvider serviceProvider, ITaskService taskService, IUserService userService, IUserContext userContext)
    {
        InitializeComponent();

        // Store injected services
        _serviceProvider = serviceProvider;
        _taskService = taskService;
        _userService = userService;
        _userContext = userContext;

        // Ensure a user is logged in before showing the main form.
        _currentUser = _userContext.CurrentUser
            ?? throw new InvalidOperationException("Cannot open MainForm without a logged-in user.");

        // --- Wire up events in the constructor for clean separation ---
        this.Load += MainForm_Load;
        this.dgvTasks.SelectionChanged += DgvTasks_SelectionChanged;
        this.dgvTasks.CellValueChanged += DgvTasks_CellValueChanged;
        this.dgvTasks.CellBeginEdit += DgvTasks_CellBeginEdit;
        this.dgvTasks.CellDoubleClick += DgvTasks_CellDoubleClick;
        this.dgvTasks.RowPrePaint += DgvTasks_RowPrePaint;
        this.dgvTasks.CellFormatting += DgvTasks_CellFormatting;
        this.dgvTasks.ColumnHeaderMouseClick += DgvTasks_ColumnHeaderMouseClick;
        this.tsbChangePassword.Click += TsbChangePassword_Click;
        this.tsbSwitchUser.Click += TsbSwitchUser_Click;
        this.tsbNewTask.Click += TsbNewTask_Click;
        this.tsbEditTask.Click += TsbEditTask_Click;
        this.tsbDeleteTask.Click += TsbDeleteTask_Click;
        this.tsbRefresh.Click += async (s, e) => await LoadTasksAsync();
        this.tsbUserManagement.Click += TsbUserManagement_Click;
        this.tsbAdminDashboard.Click += TsbAdminDashboard_Click;

        this.cmbFilterStatus.SelectedIndexChanged += Filter_Changed;
        this.cmbFilterByUserRelation.SelectedIndexChanged += Filter_Changed;
        this.cmbFilterByAssignedUser.SelectedIndexChanged += Filter_Changed;
        // --- Wire up events for pagination controls ---
        this.btnFirstPage.Click += (s, e) => ChangePage(1);
        this.btnPreviousPage.Click += (s, e) => ChangePage(_currentPage - 1);
        this.btnNextPage.Click += (s, e) => ChangePage(_currentPage + 1);
        this.btnLastPage.Click += (s, e) => ChangePage(_totalPages);
        this.cmbPageSize.SelectedIndexChanged += CmbPageSize_Changed;
        this.txtCurrentPage.KeyDown += TxtCurrentPage_KeyDown;
    }

    private void InitializePaginationControls()
    {
        cmbPageSize.Items.AddRange(new object[] { 10, 20, 50, 100 });
        cmbPageSize.SelectedItem = _pageSize;
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        this.Text = $"待辦事項清單 - [{_currentUser.Username}]";

        SetupDataGridView();
        SetupUIPermissions();
        await PopulateFilterDropDownsAsync();
        SetDefaultFiltersForCurrentUser();
        await LoadTasksAsync();
    }

    private void SetupDataGridView()
    {
        dgvTasks.AutoGenerateColumns = false;
        dgvTasks.DataSource = _tasksBindingList;

        // --- Performance & Appearance ---
        dgvTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        dgvTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvTasks.RowHeadersVisible = false;

        // Define columns once
        dgvTasks.Columns.Clear();

        // --- Set SortMode to Programmatic for all sortable columns ---
        var statusColumn = new DataGridViewComboBoxColumn
        {
            Name = "Status",
            HeaderText = "狀態",
            DataPropertyName = "Status",
            DataSource = Enum.GetValues<TodoStatus>(),
            Width = 100,
            FlatStyle = FlatStyle.Flat,
            SortMode = DataGridViewColumnSortMode.Programmatic // Enable manual sorting
        };
        dgvTasks.Columns.Add(statusColumn);

        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "標題", DataPropertyName = "Title", Width = 300, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Priority", HeaderText = "優先級", DataPropertyName = "Priority", Width = 80, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "DueDate", HeaderText = "到期日", DataPropertyName = "DueDate", Width = 110, DefaultCellStyle = { Format = "yyyy-MM-dd" }, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "AssignedTo", HeaderText = "指派給", DataPropertyName = "AssignedTo", Width = 100, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Creator", HeaderText = "建立者", DataPropertyName = "Creator", Width = 100, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Comments", HeaderText = "備註", DataPropertyName = "Comments", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
        // --- Requirement 6: Add Timestamp column (usually hidden) ---
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Timestamp", HeaderText = "版本戳", DataPropertyName = "Timestamp", Visible = false });
    }
    #region --- NEW User Action Event Handlers ---

    // --- Requirement 1: Change Password ---
    private async void TsbChangePassword_Click(object? sender, EventArgs e)
    {
        // Use a simple InputBox for password entry. For higher security, a custom dialog is better.
        string newPassword = Interaction.InputBox("請輸入您的新密碼：", "修改密碼", "");

        // Check if the user cancelled or entered an empty password.
        if (string.IsNullOrWhiteSpace(newPassword))
        {
            // If user clicks "Cancel", InputBox returns an empty string.
            // We can silently exit without showing a message.
            return;
        }

        string confirmPassword = Interaction.InputBox("請再次輸入您的新密碼以進行確認：", "確認密碼", "");
        if (newPassword != confirmPassword)
        {
            MessageBox.Show("兩次輸入的密碼不一致，請重試。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            var success = await _userService.ResetPasswordAsync(_currentUser.Id, newPassword);

            if (success)
            {
                MessageBox.Show("您的密碼已成功更新。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // This case should be rare, but it's good to handle it.
                MessageBox.Show("更新密碼失敗，找不到您的使用者帳號。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"修改密碼時發生錯誤: {ex.Message}", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // --- Requirement 2: Switch User ---
    private void TsbSwitchUser_Click(object? sender, EventArgs e)
    {
        // Show a confirmation dialog before logging out.
        var confirmResult = MessageBox.Show(
            "您確定要登出並切換使用者嗎？",
            "確認登出",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmResult == DialogResult.Yes)
        {
            // --- The cleanest and most reliable way to switch users in WinForms ---
            // This restarts the application, clears all state, and shows the login form again.
            Application.Restart();
        }
    }

    #endregion
    private void DgvTasks_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        var newSortColumn = dgvTasks.Columns[e.ColumnIndex];

        // Determine the new sort direction
        if (_sortedColumn == newSortColumn)
        {
            // If the same column is clicked, toggle the direction
            _sortDirection = (_sortDirection == ListSortDirection.Ascending)
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;
        }
        else
        {
            // If a new column is clicked, set it as the sort column and default to ascending
            _sortedColumn = newSortColumn;
            _sortDirection = ListSortDirection.Ascending;

            // Clear the sort glyph from the previous column
            foreach (DataGridViewColumn col in dgvTasks.Columns)
            {
                if (col != _sortedColumn)
                {
                    col.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
        }

        // Apply the sort and update the UI
        ApplySort();
    }
    // --- NEW HELPER METHOD to perform the actual sorting ---
    private void ApplySort()
    {
        if (_sortedColumn == null) return;

        // Get the current list of items
        var items = _tasksBindingList.ToList();

        // Use a switch statement to apply the correct sorting logic based on the column name
        switch (_sortedColumn.Name)
        {
            case "Status":
                items = (_sortDirection == ListSortDirection.Ascending)
                    ? items.OrderBy(t => t.Status).ToList()
                    : items.OrderByDescending(t => t.Status).ToList();
                break;
            case "Title":
                items = (_sortDirection == ListSortDirection.Ascending)
                    ? items.OrderBy(t => t.Title).ToList()
                    : items.OrderByDescending(t => t.Title).ToList();
                break;
            case "Priority":
                items = (_sortDirection == ListSortDirection.Ascending)
                    ? items.OrderBy(t => t.Priority).ToList()
                    : items.OrderByDescending(t => t.Priority).ToList();
                break;
            case "DueDate":
                // Sort by whether the date has a value first, then by the date itself
                items = (_sortDirection == ListSortDirection.Ascending)
                    ? items.OrderBy(t => t.DueDate.HasValue ? 0 : 1).ThenBy(t => t.DueDate).ToList()
                    : items.OrderByDescending(t => t.DueDate.HasValue ? 1 : 0).ThenByDescending(t => t.DueDate).ToList();
                break;
            case "AssignedTo":
                items = (_sortDirection == ListSortDirection.Ascending)
                    ? items.OrderBy(t => t.AssignedTo?.Username).ToList()
                    : items.OrderByDescending(t => t.AssignedTo?.Username).ToList();
                break;
            case "Creator":
                items = (_sortDirection == ListSortDirection.Ascending)
                    ? items.OrderBy(t => t.Creator.Username).ToList()
                    : items.OrderByDescending(t => t.Creator.Username).ToList();
                break;
            case "Comments":
                items = (_sortDirection == ListSortDirection.Ascending)
                    ? items.OrderBy(t => t.Comments).ToList()
                    : items.OrderByDescending(t => t.Comments).ToList();
                break;
        }

        // This is a common pattern to re-order a BindingList
        var raiseEvents = _tasksBindingList.RaiseListChangedEvents;
        _tasksBindingList.RaiseListChangedEvents = false;
        _tasksBindingList.Clear();
        items.ForEach(item => _tasksBindingList.Add(item));
        _tasksBindingList.RaiseListChangedEvents = raiseEvents;
        _tasksBindingList.ResetBindings(); // This tells the grid to refresh itself

        // Update the sort glyph on the column header to provide visual feedback
        _sortedColumn.HeaderCell.SortGlyphDirection = (_sortDirection == ListSortDirection.Ascending)
            ? SortOrder.Ascending
            : SortOrder.Descending;
    }

    private void DgvTasks_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        // We only need to handle the columns that require their value to be formatted.
        // The row-level styling is now handled by RowPrePaint.
        if (e.RowIndex < 0) return;

        if (dgvTasks.Rows[e.RowIndex].DataBoundItem is not TodoItem task) return;

        string columnName = dgvTasks.Columns[e.ColumnIndex].Name;

        switch (columnName)
        {
            case "Status":
                e.Value = task.Status.ToString();
                break;
            case "AssignedTo":
                if (e.Value is User assignedUser)
                {
                    e.Value = assignedUser.Username;
                    e.FormattingApplied = true;
                }
                break;
            case "Creator":
                if (e.Value is User creatorUser)
                {
                    e.Value = creatorUser.Username;
                    e.FormattingApplied = true;
                }
                break;
        }
    }

    private void SetupUIPermissions()
    {
        bool isAdmin = _currentUser.Role == UserRole.Admin;
        tsbUserManagement.Visible = isAdmin;
        tsbAdminDashboard.Visible = isAdmin;
        // --- Centralize UI visibility logic here ---
        lblFilterByAssignedUser.Visible = isAdmin;
        cmbFilterByAssignedUser.Visible = isAdmin;
    }


    private async Task PopulateFilterDropDownsAsync()
    {
        // --- Status Filter ---
        var statusItems = new List<StatusDisplayItem>
        {
            new StatusDisplayItem { Name = "所有狀態", Value = null }
        };
        foreach (var status in Enum.GetValues<TodoStatus>())
        {
            statusItems.Add(new StatusDisplayItem { Name = status.ToString(), Value = status });
        }
        cmbFilterStatus.DataSource = statusItems;
        cmbFilterStatus.DisplayMember = nameof(StatusDisplayItem.Name);
        cmbFilterStatus.ValueMember = nameof(StatusDisplayItem.Value);

        // --- User Relation Filter (no change needed, it binds to a simple enum) ---
        cmbFilterByUserRelation.DataSource = Enum.GetValues<UserTaskFilter>();

        // --- Assigned-To-User Filter (reusing the UserDisplayItem ViewModel) ---
        _allUsers = await _userService.GetAllUsersAsync();
        var userFilterItems = new List<UserDisplayItem>
        {
            new UserDisplayItem { Username = "所有人", Id = null }
        };
        foreach (var user in _allUsers.OrderBy(u => u.Username))
        {
            userFilterItems.Add(new UserDisplayItem { Username = user.Username, Id = user.Id });
        }
        cmbFilterByAssignedUser.DataSource = userFilterItems;
        cmbFilterByAssignedUser.DisplayMember = nameof(UserDisplayItem.Username);
        cmbFilterByAssignedUser.ValueMember = nameof(UserDisplayItem.Id);
    }

    private async Task LoadTasksAsync()
    {
        // --- Step 1: Guard Clauses - Prevent execution in invalid states ---
        // Ensure the form's handle is created and all necessary filter controls are initialized.
        if (!this.IsHandleCreated ||
            cmbFilterStatus.SelectedItem == null ||
            cmbFilterByUserRelation.SelectedItem == null ||
            cmbFilterByAssignedUser.SelectedItem == null)
        {
            return;
        }

        SetLoadingState(true);
        try
        {
            // --- Step 2: Get current filter values from the UI ---
            TodoStatus? statusFilter = (cmbFilterStatus.SelectedItem as StatusDisplayItem)?.Value;
            var userFilter = (UserTaskFilter)cmbFilterByUserRelation.SelectedItem;
            var assignedToUserIdFilter = (cmbFilterByAssignedUser.SelectedItem as UserDisplayItem)?.Id;

            // --- Step 3: Fetch the total count of items that match the filters ---
            // This is crucial for calculating the total number of pages.
            _totalTasks = await _taskService.GetTaskCountAsync(
                statusFilter,
                userFilter,
                _currentUser.Id,
                assignedToUserIdFilter
            );

            // --- Step 4: Calculate pagination variables ---
            _totalPages = (_pageSize > 0) ? (int)Math.Ceiling((double)_totalTasks / _pageSize) : 1;
            if (_totalPages == 0) _totalPages = 1; // Ensure there's always at least one page.
                                                   // If the current page is now out of bounds (e.g., after a filter reduced the total items), reset it.
            if (_currentPage > _totalPages)
            {
                _currentPage = _totalPages;
            }

            // --- Step 5: Fetch the actual data for the current page ---
            var tasks = await _taskService.GetAllTasksAsync(
                statusFilter,
                userFilter,
                _currentUser.Id,
                assignedToUserIdFilter,
                _currentPage,
                _pageSize
            );

            // --- Step 6: Update the UI with the new data and state ---
            // The sorting logic is now handled by the service, so the client just displays the data.
            var raiseEvents = _tasksBindingList.RaiseListChangedEvents;
            _tasksBindingList.RaiseListChangedEvents = false;
            _tasksBindingList.Clear();
            tasks.ForEach(task => _tasksBindingList.Add(task));
            _tasksBindingList.RaiseListChangedEvents = raiseEvents;
            _tasksBindingList.ResetBindings(); // This efficiently refreshes the grid.

            // Update the pagination controls' state (e.g., enable/disable buttons, update labels).
            UpdatePaginationUI();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"載入任務時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
            // After loading, trigger a selection change to update the comments preview panel.
            DgvTasks_SelectionChanged(null, EventArgs.Empty);
        }
    }

    private void SetLoadingState(bool isLoading)
    {
        this.UseWaitCursor = isLoading;
        panelFilters.Enabled = !isLoading;
        toolStrip1.Enabled = !isLoading;
        splitContainerMain.Enabled = !isLoading;
        lblStatus.Text = isLoading ? "正在載入..." : "就緒";
    }

    private async void Filter_Changed(object? sender, EventArgs e)
    {
        if (_isUpdatingUI) return;
        _currentPage = 1; // Reset to the first page when filters change
        await LoadTasksAsync();
    }

    #region --- Event Handlers ---
    /// <summary>
    /// Updates the UI of the pagination controls based on the current pagination state.
    /// </summary>
    private void UpdatePaginationUI()
    {
        // If there are no tasks, display a simple message.
        if (_totalTasks == 0)
        {
            lblPageInfo.Text = "沒有符合條件的任務";
        }
        else
        {
            lblPageInfo.Text = $"第 {_currentPage} / {_totalPages} 頁 (共 {_totalTasks} 筆)";
        }

        // Update the text box without triggering its own event.
        _isUpdatingUI = true;
        txtCurrentPage.Text = _currentPage.ToString();
        _isUpdatingUI = false;

        // Enable or disable navigation buttons based on the current page.
        btnFirstPage.Enabled = _currentPage > 1;
        btnPreviousPage.Enabled = _currentPage > 1;
        btnNextPage.Enabled = _currentPage < _totalPages;
        btnLastPage.Enabled = _currentPage < _totalPages;
    }

    // --- NEW: Method to handle page changes ---
    private async void ChangePage(int newPageNumber)
    {
        if (newPageNumber >= 1 && newPageNumber <= _totalPages && newPageNumber != _currentPage)
        {
            _currentPage = newPageNumber;
            await LoadTasksAsync();
        }
    }

    // --- NEW: Event handlers for pagination controls ---
    private async void CmbPageSize_Changed(object? sender, EventArgs e)
    {
        if (_isUpdatingUI) return;
        _pageSize = (int)cmbPageSize.SelectedItem!;
        _currentPage = 1; // Reset to first page when page size changes
        await LoadTasksAsync();
    }

    private void TxtCurrentPage_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            if (int.TryParse(txtCurrentPage.Text, out int pageNumber))
            {
                ChangePage(pageNumber);
            }
            e.SuppressKeyPress = true; // Prevent the "ding" sound
        }
    }

    private void SetDefaultFiltersForCurrentUser()
    {
        _isUpdatingUI = true;
        try
        {
            // Instead of setting SelectedValue to null, set the index to 0.
            // This is safer because we know the "All" item is always the first one.
            if (cmbFilterStatus.Items.Count > 0)
            {
                cmbFilterStatus.SelectedIndex = 0; // Select "所有狀態"
            }

            if (cmbFilterByAssignedUser.Items.Count > 0)
            {
                cmbFilterByAssignedUser.SelectedIndex = 0; // Select "所有人"
            }

            // The logic for UserTaskFilter can remain the same as it's bound to an enum.
            if (_currentUser.Role == UserRole.Admin)
            {
                cmbFilterByUserRelation.SelectedItem = UserTaskFilter.All;
            }
            else
            {
                cmbFilterByUserRelation.SelectedItem = UserTaskFilter.AssignedToMe;
                // Disabling is still correct.
                // cmbFilterByAssignedUser.Enabled = false; // This is now handled by SetupUIPermissions
            }
        }
        finally
        {
            _isUpdatingUI = false;
        }
    }

    private void DgvTasks_SelectionChanged(object? sender, EventArgs e)
    {
        bool isRowSelected = dgvTasks.SelectedRows.Count > 0;
        tsbEditTask.Enabled = isRowSelected;
        tsbDeleteTask.Enabled = isRowSelected;

        if (isRowSelected && dgvTasks.SelectedRows[0].DataBoundItem is TodoItem selectedTask)
        {
            txtCommentsPreview.Text = selectedTask.Comments ?? "此任務沒有備註。";
        }
        else
        {
            txtCommentsPreview.Text = string.Empty;
        }
    }

    private async void DgvTasks_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || dgvTasks.Columns[e.ColumnIndex].Name != "Status") return;
        if (dgvTasks.Rows[e.RowIndex].DataBoundItem is not TodoItem changedTask) return;

        try
        {
            await _taskService.UpdateTaskAsync(changedTask);
            lblStatus.Text = $"任務 '{changedTask.Title}' 的狀態已更新。";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"更新任務狀態失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            await LoadTasksAsync();
        }
    }

    private void DgvTasks_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
    {
        if (dgvTasks.Columns[e.ColumnIndex].Name != "Status") { e.Cancel = true; return; }

        if (dgvTasks.Rows[e.RowIndex].DataBoundItem is TodoItem task)
        {
            bool hasPermission = _currentUser.Role == UserRole.Admin
                || task.CreatorId == _currentUser.Id
                || task.AssignedToId == _currentUser.Id;
            if (!hasPermission)
            {
                e.Cancel = true;
                lblStatus.Text = "您沒有權限修改此任務的狀態。";
            }
        }
    }

    private void DgvTasks_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) { TsbEditTask_Click(sender, e); }
    }

    private void DgvTasks_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
    {
        // Ignore the row for new records if it's visible.
        if (e.RowIndex < 0 || e.RowIndex == dgvTasks.NewRowIndex) return;

        // Get the underlying TodoItem for the current row.
        if (dgvTasks.Rows[e.RowIndex].DataBoundItem is not TodoItem task) return;

        // --- Requirement #5: Conditional Row Styling Logic ---
        var now = DateTime.UtcNow;
        bool isOverdue = task.DueDate.HasValue && task.DueDate < now && task.Status != TodoStatus.Completed;
        bool isDueSoon = task.DueDate.HasValue && task.DueDate >= now && task.DueDate < now.AddDays(2) && task.Status != TodoStatus.Completed;

        var row = dgvTasks.Rows[e.RowIndex];

        if (isOverdue)
        {
            row.DefaultCellStyle.BackColor = Color.MistyRose;
            row.DefaultCellStyle.ForeColor = Color.DarkRed;
            row.DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);
        }
        else if (isDueSoon)
        {
            row.DefaultCellStyle.BackColor = Color.Yellow;
            row.DefaultCellStyle.ForeColor = Color.Orange;
            row.DefaultCellStyle.Font = new Font(this.Font, FontStyle.Regular);
        }
        else
        {
            // --- CRITICAL: Reset to default style for all other rows ---
            row.DefaultCellStyle.BackColor = SystemColors.Window;
            row.DefaultCellStyle.ForeColor = SystemColors.ControlText;
            row.DefaultCellStyle.Font = new Font(this.Font, FontStyle.Regular);
        }
    }

    private async void TsbNewTask_Click(object? sender, EventArgs e)
    {
        using var scope = _serviceProvider.CreateScope();
        var taskDialog = scope.ServiceProvider.GetRequiredService<TaskDetailDialog>();
        if (taskDialog.ShowDialog(this) == DialogResult.OK) { await LoadTasksAsync(); }
    }

    private async void TsbEditTask_Click(object? sender, EventArgs e)
    {
        if (dgvTasks.SelectedRows.Count == 0 || dgvTasks.SelectedRows[0].DataBoundItem is not TodoItem selectedTask) return;

        using var scope = _serviceProvider.CreateScope();
        var taskDialog = scope.ServiceProvider.GetRequiredService<TaskDetailDialog>();
        taskDialog.SetTaskForEdit(selectedTask);

        if (taskDialog.ShowDialog(this) == DialogResult.OK) { await LoadTasksAsync(); }
    }

    private async void TsbDeleteTask_Click(object? sender, EventArgs e)
    {
        if (dgvTasks.SelectedRows.Count == 0 || dgvTasks.SelectedRows[0].DataBoundItem is not TodoItem selectedTask) return;

        var confirmResult = MessageBox.Show($"您確定要刪除任務 '{selectedTask.Title}' 嗎？", "確認刪除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmResult != DialogResult.Yes) return;

        try
        {
            await _taskService.DeleteTaskAsync(selectedTask.Id, _currentUser.Id, _currentUser.Role == UserRole.Admin);
            _tasksBindingList.Remove(selectedTask);
            lblStatus.Text = "任務已成功刪除！";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"刪除任務失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            await LoadTasksAsync();
        }
    }

    private void TsbUserManagement_Click(object? sender, EventArgs e)
    {
        using var scope = _serviceProvider.CreateScope();
        var userDialog = scope.ServiceProvider.GetRequiredService<UserManagementDialog>();
        userDialog.ShowDialog(this);
    }

    private void TsbAdminDashboard_Click(object? sender, EventArgs e)
    {
        using var scope = _serviceProvider.CreateScope();
        var dashboard = scope.ServiceProvider.GetRequiredService<AdminDashboardForm>();
        dashboard.ShowDialog(this);
    }
    #endregion
}