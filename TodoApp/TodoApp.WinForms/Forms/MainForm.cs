#nullable enable
using System.Data;
using System.Reflection;
using System.ComponentModel;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Services;
using TodoApp.WinForms.Helpers;
using TodoApp.WinForms.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace TodoApp.WinForms.Forms;

public partial class MainForm : Form
{
    // --- Injected Services & Context ---
    private readonly IServiceProvider _serviceProvider;
    private readonly ITaskService _taskService;
    private readonly IUserService _userService;
    private readonly IUserContext _userContext;
    private readonly IConfiguration _configuration;
    private readonly User _currentUser;
    private readonly LocalCredentialManager _credentialManager;
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

    private System.Windows.Forms.Timer? _filterDebounceTimer;

    private int _hoveredRowIndex = -1;

    public MainForm(
        IServiceProvider serviceProvider,
        ITaskService taskService,
        IUserService userService,
        IUserContext userContext,
        IConfiguration configuration,
        LocalCredentialManager credentialManager)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
        _taskService = taskService;
        _userService = userService;
        _userContext = userContext;
        _configuration = configuration;
        _credentialManager = credentialManager;

        _currentUser = _userContext.CurrentUser
            ?? throw new InvalidOperationException("Cannot open MainForm without a logged-in user.");

        _regularFont = new Font(this.Font, FontStyle.Regular);
        _boldFont = new Font(this.Font, FontStyle.Bold);
        _strikeoutFont = new Font(this.Font, FontStyle.Strikeout);
        _italicFont = new Font(this.Font, FontStyle.Italic);

        WireUpEvents();
        WireUpFormatButtons();
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
        this.tsbRefresh.Click += TsbRefresh_Click;
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

        this.txtSearch.TextChanged += Filter_Changed;

        this.dgvTasks.CellMouseMove += DgvTasks_CellMouseMove;
        this.dgvTasks.MouseLeave += DgvTasks_MouseLeave;
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
        var versionString = version != null ? $"V {version.Major}.{version.Minor}" : "V ?.?";
        this.Text = $"{_configuration["ProjectName"]} App - Login : [ {_currentUser.Username} ] - {versionString}";
    }

    private void SetupDataGridView()
    {
        typeof(DataGridView).InvokeMember(
            "DoubleBuffered",
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
            null,
            this.dgvTasks,
            new object[] { true }
        );

        dgvTasks.AutoGenerateColumns = false;
        dgvTasks.DataSource = _tasksBindingList;

        dgvTasks.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

        dgvTasks.RowTemplate.Height = 30;

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
            DataSource = Enum.GetValues<PriorityLevel>(),
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

    private void WireUpFormatButtons()
    {
        // --- Font Style ---
        tsBtnBold.Click += (s, e) => txtCommentsPreview.ToggleFontStyle(FontStyle.Bold);
        tsBtnItalic.Click += (s, e) => txtCommentsPreview.ToggleFontStyle(FontStyle.Italic);
        tsBtnUnderline.Click += (s, e) => txtCommentsPreview.ToggleFontStyle(FontStyle.Underline);

        // --- Font Color ---
        tsBtnSetColorRed.Click += (s, e) => txtCommentsPreview.SetSelectionColor(Color.Red);
        tsBtnSetColorBlue.Click += (s, e) => txtCommentsPreview.SetSelectionColor(Color.Blue);
        tsBtnSetColorGreen.Click += (s, e) => txtCommentsPreview.SetSelectionColor(Color.Green);
        tsBtnSetColorBlack.Click += (s, e) => txtCommentsPreview.SetSelectionColor(Color.Black);

        // --- Paragraph ---
        tsBtnBulletList.Click += (s, e) => txtCommentsPreview.ToggleBullet();
        tsBtnIndent.Click += (s, e) => txtCommentsPreview.IncreaseIndent();
        tsBtnOutdent.Click += (s, e) => txtCommentsPreview.DecreaseIndent();

        // --- Highlighting ---
        tsBtnHighlightYellow.Click += (s, e) => txtCommentsPreview.SetSelectionBackColor(Color.Yellow);
        tsBtnHighlightGreen.Click += (s, e) => txtCommentsPreview.SetSelectionBackColor(Color.LightGreen);
        tsBtnClearHighlight.Click += (s, e) => txtCommentsPreview.ClearSelectionBackColor();
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
        foreach (var status in Enum.GetValues<TodoStatus>()) { 
            statusItems.Add(new StatusDisplayItem { Name = status.ToString(), Value = status }); 
        }
        cmbFilterStatus.DataSource = statusItems;
        cmbFilterStatus.DisplayMember = nameof(StatusDisplayItem.Name);
        cmbFilterStatus.ValueMember = nameof(StatusDisplayItem.Value);

        if (_currentUser.Role == UserRole.Admin)
        {
            cmbFilterByUserRelation.DataSource = Enum.GetValues<UserTaskFilter>();
        }
        else
        {
            cmbFilterByUserRelation.DataSource = new[]
            {
                UserTaskFilter.AssignedToMe,
                UserTaskFilter.CreatedByMe
            };
        }
        _allUsers = await _userService.GetAllUsersAsync();
        var userFilterItems = new List<UserDisplayItem> { new() { Username = "所有人", Id = 0 } };
        var uniqueUsers = _allUsers.Where(u => u != null && !string.IsNullOrEmpty(u.Username)).GroupBy(u => u.Id).Select(g => g.First());
        foreach (var user in uniqueUsers.OrderBy(u => u.Username)) { 
            userFilterItems.Add(new UserDisplayItem { Username = user.Username, Id = user.Id }); 
        }
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
    /// <summary>
    /// Safely invalidates a specific row in the DataGridView, preventing out-of-range exceptions.
    /// This method performs boundary checks before calling the underlying InvalidateRow.
    /// </summary>
    /// <param name="rowIndex">The index of the row to invalidate.</param>
    private void SafeInvalidateRow(int rowIndex)
    {
        // If the control handle hasn't been created yet, do nothing.
        if (!dgvTasks.IsHandleCreated) return;

        // Ensure the rowIndex is within the valid range of the current number of rows.
        if (rowIndex >= 0 && rowIndex < dgvTasks.RowCount)
        {
            // Optional: For future thread safety, use Invoke if called from a non-UI thread.
            if (dgvTasks.InvokeRequired)
                dgvTasks.Invoke(() => dgvTasks.InvalidateRow(rowIndex));
            else
                dgvTasks.InvalidateRow(rowIndex);
        }
    }

    private async Task LoadTasksAsync()
    {
        if (!this.IsHandleCreated || IsAnyFilterNull()) return;

        ResetHoverState();

        if (_sortedColumn is null) ClearSortGlyphs();

        SetLoadingState(true);

        try
        {
            var filterData = GetCurrentFilterData();

            var countTask = _taskService.GetTaskCountAsync(
                    _currentUser, filterData.Status, filterData.UserRelation,
                    filterData.AssignedToUser, filterData.SearchKeyword
                );

            var tasksTask = _taskService.GetAllTasksAsync(
                    _currentUser, filterData.Status, filterData.UserRelation,
                    filterData.AssignedToUser, _currentPage, _pageSize,
                    _sortedColumn?.Name, _sortDirection == ListSortDirection.Ascending,
                    filterData.SearchKeyword
                );

            await Task.WhenAll(countTask, tasksTask);
            _totalTasks = await countTask;
            var tasks = await tasksTask;
            await Task.Yield(); // A simple way to ensure we are back on the UI context.
            UpdatePaginationState();
            UpdateBindingList(tasks);
            UpdatePaginationUI();
            UpdateSortGlyphs();
        }
        catch (Exception ex) { 
            MessageBox.Show($"載入任務時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error); 
        }
        finally
        {
            SetLoadingState(false);
            DgvTasks_SelectionChanged(null, EventArgs.Empty);
        }
    }

    private record FilterData(
        TodoStatus? Status,
        UserTaskFilter UserRelation,
        int? AssignedToUser,
        string? SearchKeyword
    );

    private FilterData GetCurrentFilterData()
    {
        var searchKeyword = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(searchKeyword)) searchKeyword = null;
        var statusFilter = (cmbFilterStatus.SelectedItem as StatusDisplayItem)?.Value;
        var userFilter = (UserTaskFilter)cmbFilterByUserRelation.SelectedItem;
        int? assignedToUserIdFilter = (cmbFilterByAssignedUser.SelectedItem as UserDisplayItem)?.Id;
        if (assignedToUserIdFilter == 0) assignedToUserIdFilter = null;

        return new FilterData(statusFilter, userFilter, assignedToUserIdFilter, searchKeyword);
    }

    private void ClearSortGlyphs()
    {
        foreach (DataGridViewColumn column in dgvTasks.Columns)
        {
            column.HeaderCell.SortGlyphDirection = SortOrder.None;
        }
    }

    private bool IsAnyFilterNull()
    {
        return cmbFilterStatus.SelectedItem is null ||
               cmbFilterByUserRelation.SelectedItem is null ||
               cmbFilterByAssignedUser.SelectedItem is null;
    }

    private void ResetHoverState()
    {
        if (_hoveredRowIndex != -1)
        {
            var oldIndex = _hoveredRowIndex;
            _hoveredRowIndex = -1;
            SafeInvalidateRow(oldIndex);
        }
    }

    private void UpdatePaginationState()
    {
        _totalPages = (_pageSize > 0) ? (int)Math.Ceiling((double)_totalTasks / _pageSize) : 1;
        if (_totalPages == 0) _totalPages = 1;
        if (_currentPage > _totalPages) _currentPage = _totalPages;
    }

    private void UpdateBindingList(List<TodoItem> tasks)
    {
        _isUpdatingUI = true;
        try
        {
            _tasksBindingList.Clear();
            tasks.ForEach(task => _tasksBindingList.Add(task));
        }
        finally
        {
            _isUpdatingUI = false;
        }
    }

    private void UpdateSortGlyphs()
    {
        foreach (DataGridViewColumn col in dgvTasks.Columns)
        {
            if (col == _sortedColumn)
                col.HeaderCell.SortGlyphDirection = (_sortDirection == ListSortDirection.Ascending) ? SortOrder.Ascending : SortOrder.Descending;
            else
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
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

        _filterDebounceTimer?.Stop();

        if (_filterDebounceTimer is null)
        {
            _filterDebounceTimer = new System.Windows.Forms.Timer { Interval = 500 };
            _filterDebounceTimer.Tick += async (s, args) =>
            {
                _filterDebounceTimer.Stop();
                _currentPage = 1;
                _sortedColumn = null;
                await LoadTasksAsync();
            };
        }

        _filterDebounceTimer.Start();
    }

    private void DgvTasks_SelectionChanged(object? sender, EventArgs e)
    {
        bool isRowSelected = dgvTasks.SelectedRows.Count > 0;
        tsbEditTask.Enabled = isRowSelected;
        tsbDeleteTask.Enabled = isRowSelected;

        // Reset the save button state.
        btnSaveChanges.Enabled = false;

        if (isRowSelected && dgvTasks.SelectedRows[0].DataBoundItem is TodoItem selectedTask)
        {
            _selectedTaskForEditing = selectedTask;
            // We still need to prevent TextChanged from firing when we set the text.
            _isUpdatingUI = true;
            try
            {
                if (!string.IsNullOrEmpty(selectedTask.Comments))
                {
                    try
                    {
                        // Attempt to load as RTF first.
                        txtCommentsPreview.Rtf = selectedTask.Comments;
                    }
                    catch (ArgumentException)
                    {
                        // If it fails (because it's plain text), fall back to setting the Text property.
                        txtCommentsPreview.Text = selectedTask.Comments;
                    }
                }
                else
                {
                    txtCommentsPreview.Clear();
                }
            }
            finally
            {
                _isUpdatingUI = false;
            }
            txtCommentsPreview.Enabled = true;
            btnSaveChanges.Enabled = false;
        }
        else
        {
            _selectedTaskForEditing = null;
            _isUpdatingUI = true;
            txtCommentsPreview.Clear();
            txtCommentsPreview.Enabled = false;
            btnSaveChanges.Enabled = false;
            _isUpdatingUI = false;
        }
    }

    private void TxtCommentsPreview_TextChanged(object? sender, EventArgs e)
    {
        if (!_isUpdatingUI) btnSaveChanges.Enabled = true;
    }

    private async void DgvTasks_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        var newSortColumn = dgvTasks.Columns[e.ColumnIndex];
        if (newSortColumn.SortMode == DataGridViewColumnSortMode.NotSortable) return;

        if (_sortedColumn == newSortColumn)
            _sortDirection = (_sortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
        else
        {
            if (_sortedColumn is not null) 
                _sortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
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

        var baseStyle = GetRowStyleForTask(task);

        if (e.RowIndex == _hoveredRowIndex)
        {
            var highlightStyle = (DataGridViewCellStyle)baseStyle.Clone();
            highlightStyle.BackColor = Color.FromArgb(220, 235, 252);
            highlightStyle.ForeColor = Color.Black;
            row.DefaultCellStyle = highlightStyle;
        }
        else
        {
            row.DefaultCellStyle = baseStyle;
        }
    }

    private DataGridViewCellStyle GetRowStyleForTask(TodoItem task)
    {
        var overdueStyle = new DataGridViewCellStyle { BackColor = Color.MistyRose, ForeColor = Color.DarkRed, Font = _boldFont };
        var dueSoonStyle = new DataGridViewCellStyle { BackColor = Color.LightYellow, ForeColor = Color.DarkGoldenrod, Font = _regularFont };
        var completedStyle = new DataGridViewCellStyle { BackColor = Color.Honeydew, ForeColor = Color.DarkGray, Font = _strikeoutFont };
        var rejectedStyle = new DataGridViewCellStyle { BackColor = Color.LightGray, ForeColor = Color.Black, Font = _italicFont };
        var urgentStyle = new DataGridViewCellStyle { BackColor = Color.LightPink, ForeColor = Color.DarkRed, Font = _boldFont };
        var defaultStyle = new DataGridViewCellStyle { BackColor = SystemColors.Window, ForeColor = SystemColors.ControlText, Font = _regularFont };

        var now = DateTime.Now;
        var isOverdue = task.DueDate.HasValue && task.DueDate < now.AddDays(-1) && task.Status != TodoStatus.Completed && task.Status != TodoStatus.Reject;
        var isDueSoon = task.DueDate.HasValue && task.DueDate <= now.AddDays(3) && task.Status != TodoStatus.Completed && task.Status != TodoStatus.Reject;

        if (task.Status == TodoStatus.Completed) return completedStyle;
        if (task.Status == TodoStatus.Reject) return rejectedStyle;
        if (task.Priority == PriorityLevel.Urgent) return urgentStyle;
        if (isOverdue) return overdueStyle;
        if (isDueSoon) return dueSoonStyle;

        return defaultStyle;
    }

    private void DgvTasks_CellMouseMove(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (_isUpdatingUI) return;
        // If the mouse is over a valid row and it's a different row than the last one
        if (e.RowIndex >= 0 && e.RowIndex != _hoveredRowIndex)
        {
            var oldIndex = _hoveredRowIndex;
            _hoveredRowIndex = e.RowIndex;

            SafeInvalidateRow(oldIndex);
            SafeInvalidateRow(_hoveredRowIndex);
        }
    }

    private void DgvTasks_MouseLeave(object? sender, EventArgs e)
    {
        if (_isUpdatingUI) return;
        if (_hoveredRowIndex != -1)
        {
            var oldIndex = _hoveredRowIndex;
            _hoveredRowIndex = -1;
            SafeInvalidateRow(oldIndex);
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
                var message = $"Thread Apartment State: {Thread.CurrentThread.GetApartmentState()}";
                System.Diagnostics.Debug.WriteLine(message);
                dgvTasks.BeginEdit(true);
                if (dgvTasks.EditingControl is DataGridViewComboBoxEditingControl comboBox)
                    comboBox.DroppedDown = true;
            }
        }
    }

    private async void DgvTasks_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        var cancellationToken = Program.AppShutdownTokenSource.Token;
        if (_isUpdatingUI || e.RowIndex < 0) return;

        var columnName = dgvTasks.Columns[e.ColumnIndex].Name;
        if (columnName != "Status" && columnName != "Priority") return;

        if (dgvTasks.Rows[e.RowIndex].DataBoundItem is not TodoItem taskToUpdate) return;
        dgvTasks.InvalidateRow(e.RowIndex);
        _ = SaveTaskChangesInBackground(taskToUpdate);
    }

    /// <summary>
    /// Saves changes to a TodoItem in the background without blocking the UI.
    /// Includes error handling and updates the UI with the final state from the server.
    /// </summary>
    private async Task SaveTaskChangesInBackground(TodoItem taskToSave)
    {
        try
        {
            // Call the full update service in the background.
            var updatedTaskFromServer = await _taskService.UpdateTaskAsync(_currentUser, taskToSave);

            // --- Step 3 (Optional but Recommended): Re-sync the UI cache with the server state ---
            // After the server confirms the save, update our local object with the final version
            // from the database (which includes the new LastModifiedDate).
            var indexInList = _tasksBindingList.IndexOf(taskToSave);
            if (indexInList != -1)
            {
                _isUpdatingUI = true;
                _tasksBindingList[indexInList] = updatedTaskFromServer;
                _isUpdatingUI = false;

                // We don't need ResetItem here as InvalidateRow is sufficient for repainting.
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"自動儲存任務 '{taskToSave.Title}' 時發生錯誤: {ex.Message}", "儲存失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // A full reload is the safest way to ensure data consistency after an error.
            await LoadTasksAsync();
        }
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
            var hasPermission = _currentUser.Role == UserRole.Admin || task.CreatorId == _currentUser.Id || task.AssignedToId == _currentUser.Id;
            if (!hasPermission)
            {
                e.Cancel = true;
                lblStatus.Text = "您沒有權限修改此任務。";
            }
        }
    }

    private void DgvTasks_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

        var cell = dgvTasks.Rows[e.RowIndex].Cells[e.ColumnIndex];
        var cellValue = cell.Value?.ToString();

        if (string.IsNullOrEmpty(cellValue)) return;

        using (Graphics g = this.CreateGraphics())
        {
            var textSize = g.MeasureString(cellValue, dgvTasks.Font);

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
        if (dgvTasks.SelectedRows.Count == 0 || dgvTasks.SelectedRows[0].DataBoundItem is not TodoItem selectedTask)
            return;

        var currentRtfContent = txtCommentsPreview.Rtf;

        if (selectedTask.Comments == currentRtfContent)
        {
            btnSaveChanges.Enabled = false;
            return;
        }

        selectedTask.Comments = currentRtfContent;
        lblStatus.Text = "正在儲存備註...";
        btnSaveChanges.Enabled = false;

        // Trigger the actual save operation in the background without awaiting it.
        // The UI thread is now free and the application remains fully responsive.
        _ = SyncCommentChangesToServerAsync(selectedTask);
    }

    /// <summary>
    /// A helper method to handle the asynchronous server-side update for comments.
    /// This runs in the background.
    /// </summary>
    /// <param name="taskToSync">The task object with the updated comments.</param>
    private async Task SyncCommentChangesToServerAsync(TodoItem taskToSync)
    {
        try
        {
            // We now use the dedicated, lightweight service method.
            var updatedTaskFromServer = await _taskService.UpdateTaskCommentsAsync(
                _currentUser,
                taskToSync.Id,
                taskToSync.Comments ?? string.Empty
            );

            // After the server confirms, find the item in our BindingList and
            // replace it with the authoritative version from the server.
            // This updates properties like LastModifiedDate.
            var indexInList = _tasksBindingList.IndexOf(taskToSync);
            if (indexInList != -1)
            {
                _isUpdatingUI = true;
                _tasksBindingList[indexInList] = updatedTaskFromServer;
                _isUpdatingUI = false;

                // We don't need a full ResetItem. A targeted repaint is more efficient.
                SafeInvalidateRow(indexInList);
            }

            // This needs to be invoked to run on the UI thread.
            this.Invoke(() => {
                lblStatus.Text = "備註已成功儲存。";
            });
        }
        catch (Exception ex)
        {
            // If the background save fails, inform the user and trigger a full reload
            // to ensure the UI is consistent with the database.
            this.Invoke(() => {
                MessageBox.Show($"儲存備註時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Trigger a full, safe reload on the UI thread.
                _ = LoadTasksAsync();
            });
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
            return;

        var taskIdToSelect = selectedTaskInfo.Id;
        var pageToRestore = _currentPage;

        try
        {
            var taskToEdit = await _taskService.GetTaskByIdAsync(selectedTaskInfo.Id);
            if (taskToEdit is null)
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

                _currentPage = pageToRestore;
                await LoadTasksAsync();

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
        dgvTasks.ClearSelection();

        foreach (DataGridViewRow row in dgvTasks.Rows)
        {
            if (row.DataBoundItem is TodoItem item && item.Id == taskId)
            {
                row.Selected = true;
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
                    var success = await _userService.ResetPasswordAsync(_currentUser.Id, newPassword);
                    if (success) 
                        MessageBox.Show("您的密碼已成功更新。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else 
                        MessageBox.Show("更新密碼失敗，找不到您的使用者帳號。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex) { 
                    MessageBox.Show($"修改密碼時發生錯誤: {ex.Message}", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                }
            }
        }
    }

    private async void TsbSwitchUser_Click(object? sender, EventArgs e)
    {
        var confirmResult = MessageBox.Show("您確定要登出並切換使用者嗎？", "確認登出", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirmResult == DialogResult.Yes)
        {
            try
            {
                _credentialManager.ClearCredentials();
                if (_currentUser is not null)
                    await _userService.LogoutAsync(_currentUser.Id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Failed to clear credentials during logout: {ex.Message}");
            }
            Application.Restart();
        }
    }

    private async void TsbRefresh_Click(object? sender, EventArgs e)
    {
        lblStatus.Text = "正在重新整理...";
        _sortedColumn = null;
        SetDefaultFiltersForCurrentUser();
        await LoadTasksAsync();
        lblStatus.Text = "資料已重新整理。";
    }
    #endregion
}