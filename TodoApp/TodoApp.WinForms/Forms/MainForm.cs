#nullable enable
using System.Data;
using System.Reflection;
using System.ComponentModel;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.WinForms.ViewModels;
using Microsoft.Extensions.DependencyInjection;

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
    private readonly Font _italicFont;

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
    private TodoItem? _selectedTaskForEditing = null;

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
        _italicFont = new Font(this.Font, FontStyle.Italic);

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
        this.dgvTasks.CellToolTipTextNeeded += DgvTasks_CellToolTipTextNeeded;

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

        this.txtCommentsPreview.TextChanged += TxtCommentsPreview_TextChanged;
        this.btnSaveChanges.Click += BtnSaveChanges_Click;
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
        var mainAssembly = Assembly.GetExecutingAssembly();
        var version = mainAssembly.GetName().Version;
        var versionString = version != null ? $"V {version.Major}.{version.Minor}.{version.Build}" : "V ?.?.?";
        this.Text = $"Task Management App - Login : [ {_currentUser.Username} ] - {versionString} Beta";
    }

    private void SetupDataGridView()
    {
        // --- PERFORMANCE OPTIMIZATION ---
        // Use Double-Buffering to reduce flicker during repaint.
        // This requires setting the property via reflection as it's protected.
        typeof(DataGridView).InvokeMember(
            "DoubleBuffered",
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
            null,
            this.dgvTasks,
            new object[] { true }
        );

        dgvTasks.AutoGenerateColumns = false;
        dgvTasks.DataSource = _tasksBindingList;

        // --- CRITICAL FIX: Disable automatic row sizing ---
        // Change from AllCells to None. We will manage row height manually if needed,
        // or use a ToolTip to show full text. This is the single biggest performance gain.
        dgvTasks.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

        // Set a reasonable, fixed row height.
        dgvTasks.RowTemplate.Height = 30; // Adjust as needed

        // Keep WrapMode for individual cells, but it won't affect row height anymore.
        // Text will wrap if the cell is tall enough (e.g., if manually resized).
        dgvTasks.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

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

        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "標題", DataPropertyName = "Title", Width = 550, SortMode = DataGridViewColumnSortMode.Programmatic });
        var priorityColumn = new DataGridViewComboBoxColumn
        {
            Name = "Priority",
            HeaderText = "優先級",
            DataPropertyName = "Priority",
            DataSource = Enum.GetValues<PriorityLevel>(), // Populate with enum values
            Width = 100,
            FlatStyle = FlatStyle.Flat,
            SortMode = DataGridViewColumnSortMode.Programmatic
        };
        dgvTasks.Columns.Add(priorityColumn);
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "DueDate", HeaderText = "到期日", DataPropertyName = "DueDate", Width = 80, DefaultCellStyle = { Format = "yyyy-MM-dd" }, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "AssignedTo", HeaderText = "指派給", DataPropertyName = "AssignedTo", Width = 80, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Creator", HeaderText = "建立者", DataPropertyName = "Creator", Width = 80, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreationDate", HeaderText = "建立日期", DataPropertyName = "CreationDate", Width = 80, DefaultCellStyle = { Format = "yyyy-MM-dd" }, ReadOnly = true, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastModifiedDate", HeaderText = "最後更新", DataPropertyName = "LastModifiedDate", Width = 120, DefaultCellStyle = { Format = "yyyy-MM-dd HH:mm" }, ReadOnly = true, SortMode = DataGridViewColumnSortMode.Programmatic });
    }

    private void SetupUIPermissions()
    {
        var isAdmin = _currentUser.Role == UserRole.Admin;
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

            var tasks = await _taskService.GetAllTasksAsync(
                statusFilter, userFilter, _currentUser.Id, assignedToUserIdFilter,
                _currentPage, _pageSize,
                _sortedColumn?.Name, // Pass the column name, or null for default sort
                _sortDirection == ListSortDirection.Ascending // Pass the direction
            );

            _tasksBindingList.Clear();
            tasks.ForEach(task => _tasksBindingList.Add(task));
            UpdatePaginationUI();
            UpdateSortGlyphs();
        }
        catch (Exception ex) { MessageBox.Show($"載入任務時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        finally
        {
            SetLoadingState(false);
            DgvTasks_SelectionChanged(null, EventArgs.Empty);
        }
    }

    private async void ApplySort()
    {
        _currentPage = 1; // Reset to first page when sorting changes
        await LoadTasksAsync();
    }

    private void UpdateSortGlyphs()
    {
        foreach (DataGridViewColumn col in dgvTasks.Columns)
        {
            if (col == _sortedColumn)
            {
                col.HeaderCell.SortGlyphDirection = (_sortDirection == ListSortDirection.Ascending) ? SortOrder.Ascending : SortOrder.Descending;
            }
            else
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }
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
        _sortedColumn = null; // When filters change, always reset to default sort
        await LoadTasksAsync();
    }

    private void DgvTasks_SelectionChanged(object? sender, EventArgs e)
    {
        var isRowSelected = dgvTasks.SelectedRows.Count > 0;
        tsbEditTask.Enabled = isRowSelected;
        tsbDeleteTask.Enabled = isRowSelected;

        if (isRowSelected && dgvTasks.SelectedRows[0].DataBoundItem is TodoItem selectedTask)
        {
            _selectedTaskForEditing = selectedTask; // Store the selected task

            // Prevent the TextChanged event from firing while we are setting the text.
            _isUpdatingUI = true;
            txtCommentsPreview.Text = selectedTask.Comments ?? "";
            txtCommentsPreview.Enabled = true;
            _isUpdatingUI = false;

            // After setting text, reset the save button to disabled.
            btnSaveChanges.Enabled = false;
        }
        else
        {
            _selectedTaskForEditing = null;

            _isUpdatingUI = true;
            txtCommentsPreview.Clear();
            txtCommentsPreview.Enabled = false;
            _isUpdatingUI = false;

            btnSaveChanges.Enabled = false;
        }
    }

    private void TxtCommentsPreview_TextChanged(object? sender, EventArgs e)
    {
        // Only enable the button if the change was made by the user, not by code.
        if (!_isUpdatingUI) btnSaveChanges.Enabled = true;
    }

    private async void DgvTasks_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        var newSortColumn = dgvTasks.Columns[e.ColumnIndex];
        if (newSortColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

        if (_sortedColumn == newSortColumn)
        {
            _sortDirection = (_sortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
        }
        else
        {
            if (_sortedColumn != null) { _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None; }
            _sortedColumn = newSortColumn;
            _sortDirection = ListSortDirection.Ascending;
        }
        await ApplySortAndReload();
    }

    private async Task ApplySortAndReload()
    {
        _currentPage = 1;
        await LoadTasksAsync();
    }

    private void DgvTasks_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
    {
        if (e.RowIndex < 0 || dgvTasks.Rows.Count <= e.RowIndex || dgvTasks.Rows[e.RowIndex].DataBoundItem is not TodoItem task)
            return;

        var row = dgvTasks.Rows[e.RowIndex];

        Color targetBackColor = SystemColors.Window;
        Color targetForeColor = SystemColors.ControlText;
        Font targetFont = _regularFont;

        var now = DateTime.Now;
        var isOverdue = task.DueDate.HasValue && task.DueDate < now && task.Status != TodoStatus.Completed && task.Status != TodoStatus.Reject;
        var isDueSoon = task.DueDate.HasValue && task.DueDate >= now && task.DueDate < now.AddDays(2) && task.Status != TodoStatus.Completed && task.Status != TodoStatus.Reject;
        
        if (task.Status == TodoStatus.Completed)
        {
            targetBackColor = Color.Honeydew;
            targetForeColor = Color.DarkGray;
            targetFont = _strikeoutFont;
        }
        else if (task.Status == TodoStatus.Reject)
        {
            targetBackColor = Color.LightGray;
            targetForeColor = Color.Black;
            targetFont = _italicFont;
        }
        else if (task.Priority == PriorityLevel.Urgent)
        {
            targetBackColor = Color.LightPink;
            targetForeColor = Color.DarkRed;
            targetFont = _boldFont;
        }
        else if (isOverdue)
        {
            targetBackColor = Color.MistyRose;
            targetForeColor = Color.DarkRed;
            targetFont = _boldFont;
        }
        else if (isDueSoon)
        {
            targetBackColor = Color.LightYellow;
            targetForeColor = Color.DarkGoldenrod;
        }

        var currentStyle = row.DefaultCellStyle;
        if (currentStyle.BackColor != targetBackColor) currentStyle.BackColor = targetBackColor;
        if (currentStyle.ForeColor != targetForeColor) currentStyle.ForeColor = targetForeColor;

        Font currentFont = currentStyle.Font ?? dgvTasks.DefaultCellStyle.Font;
        if (!currentFont.Equals(targetFont))
        {
            currentStyle.Font = targetFont;
        }
    }

    private void DgvTasks_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvTasks.Rows[e.RowIndex].DataBoundItem is not TodoItem) return;

        var columnName = dgvTasks.Columns[e.ColumnIndex].Name;
        if (columnName == "AssignedTo" && e.Value is User assignedUser) { e.Value = assignedUser.Username; e.FormattingApplied = true; }
        else if (columnName == "Creator" && e.Value is User creatorUser) { e.Value = creatorUser.Username; e.FormattingApplied = true; }
    }

    private void DgvTasks_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            var columnName = dgvTasks.Columns[e.ColumnIndex].Name;
            if (columnName == "Status" || columnName == "Priority")
            {
                dgvTasks.BeginEdit(true);
                if (dgvTasks.EditingControl is DataGridViewComboBoxEditingControl comboBox)
                {
                    comboBox.DroppedDown = true;
                }
            }
        }
    }

    private async void DgvTasks_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_isUpdatingUI || e.RowIndex < 0) return;

        var columnName = dgvTasks.Columns[e.ColumnIndex].Name;
        if (columnName != "Status" && columnName != "Priority") return;

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
        var columnName = dgvTasks.Columns[e.ColumnIndex].Name;
        if (columnName != "Status" && columnName != "Priority")
        {
            e.Cancel = true;
            return;
        }

        if (dgvTasks.Rows[e.RowIndex].DataBoundItem is TodoItem task)
        {
            // Use the same permission logic for both Status and Priority
            bool hasPermission = _currentUser.Role == UserRole.Admin || task.CreatorId == _currentUser.Id || task.AssignedToId == _currentUser.Id;
            if (!hasPermission)
            {
                e.Cancel = true;
                lblStatus.Text = "您沒有權限修改此任務。";
            }
        }
    }

    private void DgvTasks_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        // Ignore headers and invalid rows
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

        // Get the cell and its value
        var cell = dgvTasks.Rows[e.RowIndex].Cells[e.ColumnIndex];
        var cellValue = cell.Value?.ToString();

        if (string.IsNullOrEmpty(cellValue)) return;

        // Use the graphics object to measure the size of the text
        using (Graphics g = this.CreateGraphics())
        {
            // Get the size required to display the full text
            var textSize = g.MeasureString(cellValue, dgvTasks.Font);

            // If the required width is greater than the cell's width, show the tooltip.
            if (textSize.Width > cell.Size.Width) e.ToolTipText = cellValue;
        }
    }

    private void DgvTasks_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) TsbEditTask_Click(sender, e);
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

    private async void BtnSaveChanges_Click(object? sender, EventArgs e)
    {
        if (_selectedTaskForEditing == null) return;

        // Check if the comments have actually changed.
        if (_selectedTaskForEditing.Comments == txtCommentsPreview.Text.Trim())
        {
            btnSaveChanges.Enabled = false;
            return;
        }

        try
        {
            SetLoadingState(true);

            _selectedTaskForEditing.Comments = txtCommentsPreview.Text.Trim();

            var updatedTask = await _taskService.UpdateTaskAsync(_currentUser, _selectedTaskForEditing);

            // Replace our stale, in-memory object with the fresh one from the database.
            _selectedTaskForEditing = updatedTask;

            var indexInList = -1;
            for (int i = 0; i < _tasksBindingList.Count; i++)
            {
                if (_tasksBindingList[i].Id == updatedTask.Id)
                {
                    indexInList = i;
                    break;
                }
            }

            if (indexInList != -1)
            {
                // Update the object in the BindingList to ensure the grid has the latest data.
                // Temporarily disable events to prevent unwanted side effects.
                _isUpdatingUI = true;
                _tasksBindingList[indexInList] = updatedTask;
                _isUpdatingUI = false;

                // Calling ResetItem is more efficient than InvalidateRow or ResetBindings in this case.
                _tasksBindingList.ResetItem(indexInList);
            }
            else
            {
                // Fallback to a full reload if the item wasn't found (should be rare).
                await LoadTasksAsync();
            }

            btnSaveChanges.Enabled = false;
            lblStatus.Text = "備註已成功儲存。";

            // InvalidateRow is still a good, efficient way to repaint.
            // The underlying data is now fresh.
            if (dgvTasks.SelectedRows.Count > 0)
            {
                dgvTasks.InvalidateRow(dgvTasks.SelectedRows[0].Index);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"儲存備註時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
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
        if (dgvTasks.SelectedRows.Count == 0 || dgvTasks.SelectedRows[0].DataBoundItem is not TodoItem selectedTaskInfo)
        {
            return;
        }

        // --- STEP 1: Remember the ID and the current page of the task being edited. ---
        var taskIdToSelect = selectedTaskInfo.Id;
        var pageToRestore = _currentPage;

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
                lblStatus.Text = "任務已成功更新，正在重新整理...";

                // --- STEP 2: Perform the full reload on the *same page*. ---
                _currentPage = pageToRestore; // Ensure we load the same page
                await LoadTasksAsync();

                // --- STEP 3: Restore the selection after the grid has been reloaded. ---
                SelectTaskInDataGridView(taskIdToSelect);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"打開編輯視窗時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SelectTaskInDataGridView(int taskId)
    {
        // Clear previous selections to avoid confusion.
        dgvTasks.ClearSelection();

        // Iterate through the rows to find the one with the matching task ID.
        foreach (DataGridViewRow row in dgvTasks.Rows)
        {
            if (row.DataBoundItem is TodoItem item && item.Id == taskId)
            {
                row.Selected = true;

                // Scroll the grid to make the selected row visible.
                // This is especially important in a paged view.
                dgvTasks.FirstDisplayedScrollingRowIndex = row.Index;

                return;
            }
        }
    }

    private async void TsbDeleteTask_Click(object? sender, EventArgs e)
    {
        if (dgvTasks.SelectedRows.Count == 0 || dgvTasks.SelectedRows[0].DataBoundItem is not TodoItem selectedTask) return;

        var originalRowIndex = dgvTasks.SelectedRows[0].Index;

        var confirmResult = MessageBox.Show($"您確定要刪除任務 '{selectedTask.Title}' 嗎？", "確認刪除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmResult != DialogResult.Yes) return;

        try
        {
            await _taskService.DeleteTaskAsync(_currentUser, selectedTask.Id);

            await LoadTasksAsync();

            if (dgvTasks.Rows.Count > 0)
            {
                var newIndex = Math.Min(originalRowIndex, dgvTasks.Rows.Count - 1);
                dgvTasks.Rows[newIndex].Selected = true;
                dgvTasks.FirstDisplayedScrollingRowIndex = newIndex;
            }
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