#nullable enable
using System.ComponentModel;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.WinForms.ViewModels;
using TodoStatus = TodoApp.Core.Models.TodoStatus;

namespace TodoApp.WinForms.Forms;

public partial class MainForm : Form
{
    // --- Injected Services & Context ---
    private readonly IServiceProvider _serviceProvider;
    private readonly ITaskService _taskService;
    private readonly IUserService _userService;
    private readonly IUserContext _userContext;
    private readonly User _currentUser;
    private readonly Font _regularFont;
    private readonly Font _boldFont;
    private readonly Font _strikeoutFont;

    // --- UI State & Data Cache ---
    private readonly BindingList<TodoItem> _tasksBindingList = new();
    private List<User> _allUsers = new();

    // --- Sorting State ---
    private DataGridViewColumn? _sortedColumn;
    private ListSortDirection _sortDirection = ListSortDirection.Ascending;

    // --- Pagination State ---
    private int _currentPage = 1;
    private int _pageSize = 20;
    private int _totalTasks;
    private int _totalPages;

    // --- Control Flags ---
    private bool _isUpdatingUI;

    public MainForm(
        IServiceProvider serviceProvider,
        ITaskService taskService,
        IUserService userService,
        IUserContext userContext)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
        _taskService = taskService;
        _userService = userService;
        _userContext = userContext;

        _currentUser = _userContext.CurrentUser
            ?? throw new InvalidOperationException("Cannot open MainForm without a logged-in user.");

        _regularFont = new Font(this.Font, FontStyle.Regular);
        _boldFont = new Font(this.Font, FontStyle.Bold);
        _strikeoutFont = new Font(this.Font, FontStyle.Strikeout);

        WireUpEvents();
    }

    private void WireUpEvents()
    {
        this.Load += MainForm_Load;

        // DataGridView Events
        this.dgvTasks.SelectionChanged += DgvTasks_SelectionChanged;
        this.dgvTasks.RowPrePaint += DgvTasks_RowPrePaint;
        this.dgvTasks.CellFormatting += DgvTasks_CellFormatting;
        this.dgvTasks.ColumnHeaderMouseClick += DgvTasks_ColumnHeaderMouseClick;
        this.dgvTasks.CellDoubleClick += DgvTasks_CellDoubleClick;
        this.dgvTasks.CellClick += DgvTasks_CellClick;
        this.dgvTasks.CellValueChanged += DgvTasks_CellValueChanged;
        this.dgvTasks.CellBeginEdit += DgvTasks_CellBeginEdit;

        // ToolStrip Events
        this.tsbNewTask.Click += TsbNewTask_Click;
        this.tsbEditTask.Click += TsbEditTask_Click;
        this.tsbDeleteTask.Click += TsbDeleteTask_Click;
        this.tsbRefresh.Click += async (s, e) => await LoadTasksAsync();
        this.tsbUserManagement.Click += TsbUserManagement_Click;
        this.tsbAdminDashboard.Click += TsbAdminDashboard_Click;
        this.tsbChangePassword.Click += TsbChangePassword_Click;
        this.tsbSwitchUser.Click += TsbSwitchUser_Click;

        // Filter Events
        this.cmbFilterStatus.SelectedIndexChanged += Filter_Changed;
        this.cmbFilterByUserRelation.SelectedIndexChanged += Filter_Changed;
        this.cmbFilterByAssignedUser.SelectedIndexChanged += Filter_Changed;

        // Pagination Events
        this.btnFirstPage.Click += (s, e) => ChangePage(1);
        this.btnPreviousPage.Click += (s, e) => ChangePage(_currentPage - 1);
        this.btnNextPage.Click += (s, e) => ChangePage(_currentPage + 1);
        this.btnLastPage.Click += (s, e) => ChangePage(_totalPages);
        this.cmbPageSize.SelectedIndexChanged += CmbPageSize_Changed;
        this.txtCurrentPage.KeyDown += TxtCurrentPage_KeyDown;
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        SetWindowTitleWithVersion();
        SetupDataGridView();
        SetupUIPermissions();
        InitializePaginationControls();
        await PopulateFilterDropDownsAsync();
        SetDefaultFiltersForCurrentUser();
        await LoadTasksAsync();
    }

    #region --- Initialization and Setup ---

    private void SetWindowTitleWithVersion()
    {
        try
        {
            var mainAssembly = Assembly.GetExecutingAssembly();
            var version = mainAssembly.GetName().Version;
            var versionString = version != null ? $"V {version.Major}.{version.Minor}.{version.Build}" : "V ?.?.?";
            this.Text = $"待辦事項清單 - [{_currentUser.Username}] - {versionString} Beta";
        }
        catch
        {
            this.Text = $"待辦事項清單 - [{_currentUser.Username}]";
        }
    }

    private void SetupDataGridView()
    {
        dgvTasks.AutoGenerateColumns = false;
        dgvTasks.DataSource = _tasksBindingList;
        dgvTasks.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvTasks.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dgvTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvTasks.RowHeadersVisible = false;
        dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        dgvTasks.Columns.Clear();

        var statusColumn = new DataGridViewComboBoxColumn
        {
            Name = "Status",
            HeaderText = "狀態",
            DataPropertyName = "Status",
            DataSource = Enum.GetValues<TodoStatus>(),
            Width = 100,
            FlatStyle = FlatStyle.Flat,
            SortMode = DataGridViewColumnSortMode.Programmatic
        };
        dgvTasks.Columns.Add(statusColumn);

        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "標題", DataPropertyName = "Title", Width = 300, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Priority", HeaderText = "優先級", DataPropertyName = "Priority", Width = 80, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "DueDate", HeaderText = "到期日", DataPropertyName = "DueDate", Width = 110, DefaultCellStyle = { Format = "yyyy-MM-dd" }, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "AssignedTo", HeaderText = "指派給", DataPropertyName = "AssignedTo", Width = 100, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Creator", HeaderText = "建立者", DataPropertyName = "Creator", Width = 100, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreationDate", HeaderText = "建立日期", DataPropertyName = "CreationDate", Width = 140, DefaultCellStyle = { Format = "yyyy-MM-dd HH:mm" }, ReadOnly = true, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastModifiedDate", HeaderText = "最後更新", DataPropertyName = "LastModifiedDate", Width = 140, DefaultCellStyle = { Format = "yyyy-MM-dd HH:mm" }, ReadOnly = true, SortMode = DataGridViewColumnSortMode.Programmatic });
    }

    private void SetupUIPermissions()
    {
        bool isAdmin = _currentUser.Role == UserRole.Admin;
        tsbUserManagement.Visible = isAdmin;
        tsbAdminDashboard.Visible = isAdmin;
        lblFilterByAssignedUser.Visible = isAdmin;
        cmbFilterByAssignedUser.Visible = isAdmin;
    }

    private void InitializePaginationControls()
    {
        cmbPageSize.Items.AddRange(new object[] { 10, 20, 50, 100 });
        cmbPageSize.SelectedItem = _pageSize;
    }

    private async Task PopulateFilterDropDownsAsync()
    {
        var statusItems = new List<StatusDisplayItem> { new() { Name = "所有狀態", Value = null } };
        foreach (var status in Enum.GetValues<TodoStatus>()) { statusItems.Add(new StatusDisplayItem { Name = status.ToString(), Value = status }); }
        cmbFilterStatus.DataSource = statusItems;
        cmbFilterStatus.DisplayMember = nameof(StatusDisplayItem.Name);
        cmbFilterStatus.ValueMember = nameof(StatusDisplayItem.Value);

        cmbFilterByUserRelation.DataSource = Enum.GetValues<UserTaskFilter>();

        _allUsers = await _userService.GetAllUsersAsync();
        var userFilterItems = new List<UserDisplayItem> { new() { Username = "所有人", Id = 0 } };
        var uniqueUsers = _allUsers.Where(u => u != null && !string.IsNullOrEmpty(u.Username)).GroupBy(u => u.Id).Select(g => g.First());
        foreach (var user in uniqueUsers.OrderBy(u => u.Username)) { userFilterItems.Add(new UserDisplayItem { Username = user.Username, Id = user.Id }); }
        cmbFilterByAssignedUser.DataSource = userFilterItems;
        cmbFilterByAssignedUser.DisplayMember = nameof(UserDisplayItem.Username);
        cmbFilterByAssignedUser.ValueMember = nameof(UserDisplayItem.Id);
    }

    private void SetDefaultFiltersForCurrentUser()
    {
        _isUpdatingUI = true;
        try
        {
            if (cmbFilterStatus.Items.Count > 0) cmbFilterStatus.SelectedIndex = 0;
            if (cmbFilterByAssignedUser.Items.Count > 0) cmbFilterByAssignedUser.SelectedIndex = 0;
            cmbFilterByUserRelation.SelectedItem = (_currentUser.Role == UserRole.Admin) ? UserTaskFilter.All : UserTaskFilter.AssignedToMe;
        }
        finally { _isUpdatingUI = false; }
    }

    #endregion

    #region --- Data Loading and Sorting ---

    private async Task LoadTasksAsync()
    {
        if (!this.IsHandleCreated || cmbFilterStatus.SelectedItem == null || cmbFilterByUserRelation.SelectedItem == null || cmbFilterByAssignedUser.SelectedItem == null) return;

        SetLoadingState(true);
        try
        {
            TodoStatus? statusFilter = (cmbFilterStatus.SelectedItem as StatusDisplayItem)?.Value;
            var userFilter = (UserTaskFilter)cmbFilterByUserRelation.SelectedItem;
            int? assignedToUserIdFilter = (cmbFilterByAssignedUser.SelectedItem as UserDisplayItem)?.Id;
            if (assignedToUserIdFilter == 0) assignedToUserIdFilter = null;

            _totalTasks = await _taskService.GetTaskCountAsync(statusFilter, userFilter, _currentUser.Id, assignedToUserIdFilter);

            _totalPages = (_pageSize > 0) ? (int)Math.Ceiling((double)_totalTasks / _pageSize) : 1;
            if (_totalPages == 0) _totalPages = 1;
            if (_currentPage > _totalPages) _currentPage = _totalPages;

            var tasks = await _taskService.GetAllTasksAsync(statusFilter, userFilter, _currentUser.Id, assignedToUserIdFilter, _currentPage, _pageSize);

            _isUpdatingUI = true;
            _tasksBindingList.Clear();
            tasks.ForEach(task => _tasksBindingList.Add(task));
            _isUpdatingUI = false;

            _sortedColumn = null; // Reset to default sort every time data is loaded.
            ApplySort();
            UpdatePaginationUI();
        }
        catch (Exception ex) { MessageBox.Show($"載入任務時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        finally
        {
            SetLoadingState(false);
            DgvTasks_SelectionChanged(null, EventArgs.Empty);
        }
    }

    private void ApplySort()
    {
        var items = new List<TodoItem>(_tasksBindingList);

        if (_sortedColumn != null)
        {
            var propName = _sortedColumn.DataPropertyName;

            // Handle complex types for sorting
            if (propName == "AssignedTo" || propName == "Creator")
            {
                items = (_sortDirection == ListSortDirection.Ascending)
                    ? items.OrderBy(t => propName == "AssignedTo" ? t.AssignedTo?.Username : t.Creator.Username).ToList()
                    : items.OrderByDescending(t => propName == "AssignedTo" ? t.AssignedTo?.Username : t.Creator.Username).ToList();
            }
            else
            {
                var prop = TypeDescriptor.GetProperties(typeof(TodoItem))[propName];
                if (prop != null)
                {
                    items = (_sortDirection == ListSortDirection.Ascending)
                        ? items.OrderBy(x => prop.GetValue(x)).ToList()
                        : items.OrderByDescending(x => prop.GetValue(x)).ToList();
                }
            }

            foreach (var col in dgvTasks.Columns.Cast<DataGridViewColumn>().Where(c => c != _sortedColumn))
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            _sortedColumn.HeaderCell.SortGlyphDirection = (_sortDirection == ListSortDirection.Ascending) ? SortOrder.Ascending : SortOrder.Descending;
        }
        else // Default sort
        {
            items = items
                .OrderBy(t => t.Status == TodoStatus.InProgress ? 0 : (t.Status == TodoStatus.Pending ? 1 : 2))
                .ThenByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
                .ToList();
        }

        _isUpdatingUI = true;
        _tasksBindingList.Clear();
        items.ForEach(item => _tasksBindingList.Add(item));
        _isUpdatingUI = false;
        _tasksBindingList.ResetBindings();
    }

    #endregion

    #region --- UI State and Event Handlers ---

    private void SetLoadingState(bool isLoading)
    {
        this.UseWaitCursor = isLoading;
        panelFilters.Enabled = !isLoading;
        toolStrip1.Enabled = !isLoading;
        splitContainerMain.Enabled = !isLoading;
        panelPagination.Enabled = !isLoading;
        lblStatus.Text = isLoading ? "正在載入..." : "準備就緒";
    }

    private async void Filter_Changed(object? sender, EventArgs e)
    {
        if (_isUpdatingUI) return;
        _currentPage = 1;
        _sortedColumn = null;
        await LoadTasksAsync();
    }

    private void DgvTasks_SelectionChanged(object? sender, EventArgs e)
    {
        bool isRowSelected = dgvTasks.SelectedRows.Count > 0;
        tsbEditTask.Enabled = isRowSelected;
        tsbDeleteTask.Enabled = isRowSelected;

        if (isRowSelected && dgvTasks.SelectedRows[0].DataBoundItem is TodoItem selectedTask)
            txtCommentsPreview.Text = selectedTask.Comments ?? "(此任務沒有備註)";
        else
            txtCommentsPreview.Text = string.Empty;
    }

    private void DgvTasks_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        var newSortColumn = dgvTasks.Columns[e.ColumnIndex];
        if (newSortColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

        if (_sortedColumn == newSortColumn)
        {
            _sortDirection = (_sortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
        }
        else
        {
            _sortedColumn = newSortColumn;
            _sortDirection = ListSortDirection.Ascending;
        }
        ApplySort();
    }

    private void DgvTasks_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
    {
        if (e.RowIndex < 0 || dgvTasks.Rows.Count <= e.RowIndex || dgvTasks.Rows[e.RowIndex].DataBoundItem is not TodoItem task)
        {
            return;
        }

        var row = dgvTasks.Rows[e.RowIndex];

        Color targetBackColor = SystemColors.Window;
        Color targetForeColor = SystemColors.ControlText;

        // Determine the target style based on task properties
        var now = DateTime.Now;
        if (task.Status == TodoStatus.Completed)
        {
            targetBackColor = Color.Honeydew;
            targetForeColor = Color.DarkGray;
        }
        else if (task.Priority == PriorityLevel.Urgent)
        {
            targetBackColor = Color.Plum;
            targetForeColor = Color.White;
        }
        else if (task.DueDate.HasValue && task.DueDate < now)
        {
            targetBackColor = Color.MistyRose;
            targetForeColor = Color.DarkRed;
        }
        else if (task.DueDate.HasValue && task.DueDate < now.AddDays(2))
        {
            targetBackColor = Color.LightYellow;
            targetForeColor = Color.DarkGoldenrod;
        }

        // --- The most robust fix: Apply style only when it has changed ---
        if (row.DefaultCellStyle.BackColor != targetBackColor)
        {
            row.DefaultCellStyle.BackColor = targetBackColor;
        }
        if (row.DefaultCellStyle.ForeColor != targetForeColor)
        {
            row.DefaultCellStyle.ForeColor = targetForeColor;
        }

        // For now, let's avoid changing the Font object here as it's a common source of flickering.
        // If strikeout/bold is critical, it should be applied carefully in CellFormatting.
        Font expectedFont;
        if (task.Status == TodoStatus.Completed)
        {
            expectedFont = _strikeoutFont;
        }
        else if (task.Priority == PriorityLevel.Urgent || (task.DueDate.HasValue && task.DueDate < DateTime.Now && task.Status != TodoStatus.Completed))
        {
            expectedFont = _boldFont;
        }
        else
        {
            expectedFont = _regularFont;
        }

        Font currentFont = row.DefaultCellStyle.Font ?? dgvTasks.DefaultCellStyle.Font;
        if (!currentFont.Equals(expectedFont))
        {
            row.DefaultCellStyle.Font = expectedFont;
        }
    }

    private void DgvTasks_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvTasks.Rows[e.RowIndex].DataBoundItem is not TodoItem) return;

        string columnName = dgvTasks.Columns[e.ColumnIndex].Name;
        if (columnName == "AssignedTo" && e.Value is User assignedUser) { e.Value = assignedUser.Username; e.FormattingApplied = true; }
        else if (columnName == "Creator" && e.Value is User creatorUser) { e.Value = creatorUser.Username; e.FormattingApplied = true; }
    }

    private void DgvTasks_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && dgvTasks.Columns[e.ColumnIndex].Name == "Status")
        {
            dgvTasks.BeginEdit(true);
            if (dgvTasks.EditingControl is DataGridViewComboBoxEditingControl comboBox) { comboBox.DroppedDown = true; }
        }
    }

    private async void DgvTasks_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_isUpdatingUI || e.RowIndex < 0 || dgvTasks.Columns[e.ColumnIndex].Name != "Status") return;
        if (dgvTasks.Rows[e.RowIndex].DataBoundItem is not TodoItem changedTask) return;

        try
        {
            SetLoadingState(true);
            await _taskService.UpdateTaskAsync(_currentUser, changedTask);
            ApplySort();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"更新任務狀態時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            await LoadTasksAsync();
        }
        finally { SetLoadingState(false); }
    }

    private void DgvTasks_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
    {
        if (dgvTasks.Columns[e.ColumnIndex].Name != "Status") { e.Cancel = true; return; }
        if (dgvTasks.Rows[e.RowIndex].DataBoundItem is TodoItem task)
        {
            bool hasPermission = _currentUser.Role == UserRole.Admin || task.CreatorId == _currentUser.Id || task.AssignedToId == _currentUser.Id;
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

    #endregion

    #region --- Pagination Event Handlers ---

    private void UpdatePaginationUI()
    {
        lblPageInfo.Text = (_totalTasks == 0) ? "沒有符合條件的任務" : $"第 {_currentPage} / {_totalPages} 頁 (共 {_totalTasks} 筆)";

        _isUpdatingUI = true;
        txtCurrentPage.Text = _currentPage.ToString();
        _isUpdatingUI = false;

        btnFirstPage.Enabled = _currentPage > 1;
        btnPreviousPage.Enabled = _currentPage > 1;
        btnNextPage.Enabled = _currentPage < _totalPages;
        btnLastPage.Enabled = _currentPage < _totalPages;
    }

    private async void ChangePage(int newPageNumber)
    {
        if (newPageNumber >= 1 && newPageNumber <= _totalPages && newPageNumber != _currentPage)
        {
            _currentPage = newPageNumber;
            await LoadTasksAsync();
        }
    }

    private async void CmbPageSize_Changed(object? sender, EventArgs e)
    {
        if (_isUpdatingUI) return;
        if (cmbPageSize.SelectedItem is int newSize)
        {
            _pageSize = newSize;
            _currentPage = 1;
            await LoadTasksAsync();
        }
    }

    private void TxtCurrentPage_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            if (int.TryParse(txtCurrentPage.Text, out int pageNumber)) { ChangePage(pageNumber); }
            e.SuppressKeyPress = true;
        }
    }

    #endregion

    #region --- ToolStrip Button Event Handlers ---

    private async void TsbNewTask_Click(object? sender, EventArgs e)
    {
        using var scope = _serviceProvider.CreateScope();
        var taskDialog = scope.ServiceProvider.GetRequiredService<TaskDetailDialog>();
        if (taskDialog.ShowDialog(this) == DialogResult.OK) { await LoadTasksAsync(); }
    }

    private async void TsbEditTask_Click(object? sender, EventArgs e)
    {
        if (dgvTasks.SelectedRows.Count == 0 || dgvTasks.SelectedRows[0].DataBoundItem is not TodoItem selectedTaskInfo) return;

        try
        {
            var taskToEdit = await _taskService.GetTaskByIdAsync(selectedTaskInfo.Id);
            if (taskToEdit == null)
            {
                MessageBox.Show("無法編輯任務，它可能已被其他使用者刪除。請重新整理。", "找不到任務", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                await LoadTasksAsync();
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var taskDialog = scope.ServiceProvider.GetRequiredService<TaskDetailDialog>();
            taskDialog.SetTaskForEdit(taskToEdit);

            if (taskDialog.ShowDialog(this) == DialogResult.OK)
            {
                await LoadTasksAsync();
                lblStatus.Text = "任務已成功更新！";
            }
        }
        catch (Exception ex) { MessageBox.Show($"打開編輯視窗時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private async void TsbDeleteTask_Click(object? sender, EventArgs e)
    {
        if (dgvTasks.SelectedRows.Count == 0 || dgvTasks.SelectedRows[0].DataBoundItem is not TodoItem selectedTask) return;

        var confirmResult = MessageBox.Show($"您確定要刪除任務 '{selectedTask.Title}' 嗎？", "確認刪除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmResult != DialogResult.Yes) return;

        try
        {
            await _taskService.DeleteTaskAsync(_currentUser, selectedTask.Id);
            _tasksBindingList.Remove(selectedTask);
            _totalTasks--;
            UpdatePaginationUI();
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

    private async void TsbChangePassword_Click(object? sender, EventArgs e)
    {
        using (var passwordDialog = new PasswordInputDialog("修改密碼", "請輸入並確認您的新密碼："))
        {
            if (passwordDialog.ShowDialog(this) == DialogResult.OK)
            {
                string? newPassword = passwordDialog.NewPassword;
                if (string.IsNullOrEmpty(newPassword)) return;
                try
                {
                    bool success = await _userService.ResetPasswordAsync(_currentUser.Id, newPassword);
                    if (success) { MessageBox.Show("您的密碼已成功更新。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else { MessageBox.Show("更新密碼失敗，找不到您的使用者帳號。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                catch (Exception ex) { MessageBox.Show($"修改密碼時發生錯誤: {ex.Message}", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
    }

    private void TsbSwitchUser_Click(object? sender, EventArgs e)
    {
        var confirmResult = MessageBox.Show("您確定要登出並切換使用者嗎？", "確認登出", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirmResult == DialogResult.Yes) { Application.Restart(); }
    }

    #endregion
}