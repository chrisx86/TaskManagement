#nullable enable
using System.Data;
using System.Reflection;
using System.ComponentModel;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Services;
using TodoApp.WinForms.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TodoApp.WinForms.Caching;

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
    private List<User> _allUsers = new();
    private readonly TaskDataCache _taskDataCache;

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

        // --- Instantiate the cache in the constructor ---
        // We pass a lambda expression that matches the DataPageProvider delegate's signature.
        // This lambda expression is what the cache will call to get data.
        _taskDataCache = new TaskDataCache(
            async (pageNumber, pageSize) =>
            {
                // This is where we call our actual backend service.
                // All the filter and sort parameters are captured from the MainForm's state.
                var filterData = GetCurrentFilterData();
                return await _taskService.GetAllTasksAsync(
                    _currentUser,
                    filterData.Status,
                    filterData.UserRelation,
                    filterData.AssignedToUser,
                    pageNumber,
                    pageSize,
                    _sortedColumn?.Name,
                    _sortDirection == ListSortDirection.Ascending,
                    filterData.SearchKeyword
                );
            }
        );

        WireUpEvents();
    }

    private void WireUpEvents()
    {
        this.Load += MainForm_Load;

        // DataGridView Events
        this.dgvTasks.SelectionChanged += DgvTasks_SelectionChanged;
        this.dgvTasks.RowPrePaint += DgvTasks_RowPrePaint;
        //this.dgvTasks.CellFormatting += DgvTasks_CellFormatting;
        this.dgvTasks.ColumnHeaderMouseClick += DgvTasks_ColumnHeaderMouseClick;
        this.dgvTasks.CellDoubleClick += DgvTasks_CellDoubleClick;
        this.dgvTasks.CellClick += DgvTasks_CellClick;
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

        this.btnSaveChanges.Click += BtnSaveChanges_Click;

        this.txtSearch.TextChanged += Filter_Changed;

        this.dgvTasks.CellMouseMove += DgvTasks_CellMouseMove;
        this.dgvTasks.MouseLeave += DgvTasks_MouseLeave;

        this.richTextEditorComments.TextChanged += TxtCommentsPreview_TextChanged;
        // --- The core event for Virtual Mode ---
        this.dgvTasks.CellValueNeeded += DgvTasks_CellValueNeeded;
        this.dgvTasks.Scroll += DgvTasks_Scroll;
    }

    /// <summary>
    /// NEW: Handles the Scroll event to pre-fetch data for upcoming rows.
    /// </summary>
    private void DgvTasks_Scroll(object? sender, ScrollEventArgs e)
    {
        // We only care about vertical scrolling.
        if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
        {
            // Get the index of the last visible row.
            int lastVisibleRow = dgvTasks.FirstDisplayedScrollingRowIndex + dgvTasks.DisplayedRowCount(false) - 1;

            // --- Prefetching Logic ---
            // Trigger a fire-and-forget task to load the data for the last visible row.
            // The cache will fetch the entire page containing this row.
            // This warms up the cache for rows the user is about to see.
            if (lastVisibleRow < dgvTasks.RowCount)
            {
                _ = PrefetchDataForRow(lastVisibleRow);
            }
        }
    }

    /// <summary>
    /// NEW: Helper for background data prefetching.
    /// </summary>
    private async Task PrefetchDataForRow(int rowIndex)
    {
        // This call will ensure the page containing the rowIndex is loaded into the cache.
        await _taskDataCache.EnsureItemLoadedAsync(rowIndex);

        // After prefetching, we can invalidate the visible rows to update the "..." placeholders.
        // This is an optional refinement for a smoother experience.
        this.Invoke(() => {
            if (dgvTasks.IsHandleCreated)
            {
                dgvTasks.Invalidate(); // Invalidate the whole grid to repaint visible cells
            }
        });
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        SetWindowTitleWithVersion();
        SetupDataGridViewForVirtualMode();
        SetupUIPermissions();
        InitializePaginationControls();
        await PopulateFilterDropDownsAsync();
        SetDefaultFiltersForCurrentUser();
        await LoadTasksAsync();
    }

    #region --- Initialization and Setup ---

    private void SetupDataGridViewForVirtualMode()
    {
        // --- Performance Optimization ---
        typeof(DataGridView).InvokeMember(
            "DoubleBuffered",
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
            null,
            this.dgvTasks,
            new object[] { true }
        );

        // --- CORE VIRTUAL MODE ACTIVATION ---
        dgvTasks.VirtualMode = true;
        dgvTasks.DataSource = null; // Essential: Disconnect from BindingList

        // --- Standard Appearance & Behavior Settings ---
        dgvTasks.AutoGenerateColumns = false; // Always false
        dgvTasks.AllowUserToAddRows = false;
        dgvTasks.AllowUserToDeleteRows = false;
        dgvTasks.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dgvTasks.RowTemplate.Height = 30;
        dgvTasks.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvTasks.RowHeadersVisible = false;
        dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTasks.MultiSelect = false;

        // --- Column Definitions ---
        dgvTasks.Columns.Clear();
        dgvTasks.Columns.Add(new DataGridViewComboBoxColumn { Name = "Status", HeaderText = "狀態", DataSource = Enum.GetValues<TodoStatus>(), Width = 100, FlatStyle = FlatStyle.Flat, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "標題", Width = 550, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewComboBoxColumn { Name = "Priority", HeaderText = "優先級", DataSource = Enum.GetValues<PriorityLevel>(), Width = 100, FlatStyle = FlatStyle.Flat, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "DueDate", HeaderText = "到期日", Width = 80, DefaultCellStyle = { Format = "yyyy-MM-dd" }, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "AssignedTo", HeaderText = "指派給", Width = 80, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Creator", HeaderText = "建立者", Width = 80, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreationDate", HeaderText = "建立日期", Width = 80, DefaultCellStyle = { Format = "yyyy-MM-dd" }, ReadOnly = true, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastModifiedDate", HeaderText = "最後更新", Width = 120, DefaultCellStyle = { Format = "yyyy-MM-dd HH:mm" }, ReadOnly = true, SortMode = DataGridViewColumnSortMode.Programmatic });
    }

    private void DgvTasks_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
    {
        // Standard guard clause for headers and invalid rows.
        if (e.RowIndex < 0 || e.RowIndex >= dgvTasks.RowCount)
        {
            return;
        }

        // --- Step 1: Synchronously try to get the task object from the cache ---
        // We use TryGetItem because RowPrePaint is a synchronous event and cannot be awaited.
        // If the data isn't in the cache yet, we'll just use the default style for this paint cycle.
        if (!_taskDataCache.TryGetItem(e.RowIndex, out var task) || task is null)
        {
            // If task is not available in cache, do nothing and let it paint with default style.
            return;
        }

        // --- Step 2: The rest of the logic is the same as before ---
        var row = dgvTasks.Rows[e.RowIndex];

        // Determine the base style from business logic (overdue, urgent, etc.)
        var baseStyle = GetRowStyleForTask(task);

        // Determine the final style by applying the hover effect.
        if (e.RowIndex == _hoveredRowIndex)
        {
            var highlightStyle = (DataGridViewCellStyle)baseStyle.Clone();
            highlightStyle.BackColor = Color.FromArgb(220, 235, 252);
            highlightStyle.ForeColor = Color.Black;
            row.DefaultCellStyle = highlightStyle;
        }
        else
        {
            // If this row is not hovered, just apply its base style.
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

    private void SetWindowTitleWithVersion()
    {
        var mainAssembly = Assembly.GetExecutingAssembly();
        var version = mainAssembly.GetName().Version;
        var versionString = version != null ? $"V {version.Major}.{version.Minor}" : "V ?.?";
        this.Text = $"{_configuration["ProjectName"]} App - Login : [ {_currentUser.Username} ] - {versionString}";
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
        cmbPageSize.Items.AddRange(new object[] { 10, 20, 50 });
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

    // --- LoadTasksAsync now only loads the total count ---
    private async Task LoadTasksAsync()
    {
        if (!this.IsHandleCreated || IsAnyFilterNull()) return;

        ResetHoverState();
        if (_sortedColumn is null) ClearSortGlyphs();
        SetLoadingState(true);

        try
        {
            var filterData = GetCurrentFilterData();

            // 1. Fetch ONLY the total count from the database.
            _totalTasks = await _taskService.GetTaskCountAsync(
                _currentUser, filterData.Status, filterData.UserRelation,
                filterData.AssignedToUser, filterData.SearchKeyword
            );

            UpdatePaginationState();
            UpdatePaginationUI();
            // 2. Clear the old cached data and set the new RowCount.
            // This is the trigger for the UI refresh in Virtual Mode.
            _taskDataCache.Clear();
            dgvTasks.RowCount = _totalTasks;

            // 3. Update UI state based on the new total count.
            UpdateSortGlyphs();

            if (_totalTasks > 0)
            {
                // We don't need the result, just to trigger the fetch.
                await _taskDataCache.EnsureItemLoadedAsync(0);

                // After the first page is guaranteed to be in the cache,
                // invalidate the grid to force it to re-request the cell values.
                // It will now find the data in the cache synchronously.
                dgvTasks.Invalidate();
            }
        }
        catch (Exception ex)
        {
            dgvTasks.RowCount = 0;
            MessageBox.Show($"載入任務時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
            // We need to trigger selection changed manually as the DGV is now 'empty'
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
                // --- Synchronous UI State Protection ---
                _taskDataCache.Clear();
                dgvTasks.RowCount = 0; // The key to preventing errors

                _currentPage = 1;
                _sortedColumn = null;

                await LoadTasksAsync();
            };
        }
        _filterDebounceTimer.Start();
    }

    /// <summary>
    /// CORE VIRTUAL MODE HANDLER: Provides data to the DataGridView on demand (Cell by Cell).
    /// </summary>
    private void DgvTasks_CellValueNeeded(object? sender, DataGridViewCellValueEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        // --- PRE-AWAIT GUARD ---
        // This is the first line of defense.
        if (e.RowIndex >= dgvTasks.RowCount) return;

        if (_taskDataCache.TryGetItem(e.RowIndex, out var task) && task != null)
        {
            // --- POST-AWAIT GUARD (THE CRITICAL FIX) ---
            // After the await, the DataGridView's state (RowCount) might have changed.
            // We MUST re-validate the rowIndex against the CURRENT RowCount before proceeding.
            // If the row is no longer valid, we must not attempt to set e.Value.
            if (e.RowIndex >= dgvTasks.RowCount) return;

            // 2. Handle the loading state placeholder.
            if (task is null)
            {
                e.Value = "Loading...";
                return;
            }

            // 3. Map the data object's properties to the correct columns.
            switch (dgvTasks.Columns[e.ColumnIndex].Name)
            {
                case "Status": e.Value = task.Status; break;
                case "Title": e.Value = task.Title; break;
                case "Priority": e.Value = task.Priority; break;
                case "DueDate": e.Value = task.DueDate; break;
                case "AssignedTo": e.Value = task.AssignedTo?.Username; break;
                case "Creator": e.Value = task.Creator?.Username; break;
                case "CreationDate": e.Value = task.CreationDate; break;
                case "LastModifiedDate": e.Value = task.LastModifiedDate; break;
            }
        }
    }

    private async void DgvTasks_SelectionChanged(object? sender, EventArgs e)
    {
        bool isRowSelected = dgvTasks.SelectedRows.Count > 0;
        tsbEditTask.Enabled = isRowSelected;
        tsbDeleteTask.Enabled = isRowSelected;
        btnSaveChanges.Enabled = false; // Reset state

        // Clear previous selection info immediately for better responsiveness.
        _selectedTaskForEditing = null;
        richTextEditorComments.Clear();
        richTextEditorComments.Enabled = false;

        if (isRowSelected)
        {
            // The 'await' operator is now valid inside this async method.
            _selectedTaskForEditing = await _taskDataCache.EnsureItemLoadedAsync(dgvTasks.SelectedRows[0].Index);

            // It's possible for the task to be null if the cache is still loading,
            // so we must handle this gracefully.
            if (_selectedTaskForEditing is not null)
            {
                richTextEditorComments.Rtf = _selectedTaskForEditing.Comments ?? "";
                richTextEditorComments.Enabled = true;
            }
        }
        else
        {
            _selectedTaskForEditing = null;
            richTextEditorComments.Clear();
            richTextEditorComments.Enabled = false;
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
        _taskDataCache.Clear();
        await ApplySortAndReload();
    }

    private async Task ApplySortAndReload()
    {
        _currentPage = 1;
        _taskDataCache.Clear(); // Critical step for sorting in virtual mode
        await LoadTasksAsync();
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

    /// <summary>
    /// Handles in-place editing of ComboBox cells. In Virtual Mode, it fetches the
    /// task from the cache, updates it, and triggers a background save.
    /// </summary>
    private async void DgvTasks_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_isUpdatingUI || e.RowIndex < 0) return;

        var columnName = dgvTasks.Columns[e.ColumnIndex].Name;
        if (columnName != "Status" && columnName != "Priority") return;

        // Fetch the task object from the cache using the row index.
        var taskToUpdate = await _taskDataCache.EnsureItemLoadedAsync(e.RowIndex);
        if (taskToUpdate is null) return;

        var newValue = dgvTasks.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

        // Manually update the in-memory object based on the cell's new value.
        if (columnName == "Status" && newValue is TodoStatus newStatus)
            taskToUpdate.Status = newStatus;
        else if (columnName == "Priority" && newValue is PriorityLevel newPriority)
            taskToUpdate.Priority = newPriority;
        else
            return; // Exit if the value is not of the expected type

        // Invalidate the row immediately for visual feedback (color change).
        SafeInvalidateRow(e.RowIndex);

        // Fire-and-forget the background save.
        _ = SaveTaskChangesInBackground(taskToUpdate, e.RowIndex);
    }

    /// <summary>
    /// Saves changes to a TodoItem in the background without blocking the UI.
    /// This version is adapted for Virtual Mode with TaskDataCache.
    /// </summary>
    private async Task SaveTaskChangesInBackground(TodoItem taskToSave, int rowIndex)
    {
        try
        {
            // Call the full update service in the background.
            // This persists the changes (e.g., new Status or Priority) to the database.
            var updatedTaskFromServer = await _taskService.UpdateTaskAsync(_currentUser, taskToSave);

            // --- Step 1: Invalidate the cache for the updated item ---
            // This ensures that the next time this row is requested, the cache will
            // fetch the fresh data (with the new LastModifiedDate) from the server.
            _taskDataCache.InvalidateItem(rowIndex);

            // --- Step 2: Trigger a repaint of the specific row ---
            // This will cause CellValueNeeded and RowPrePaint to fire again for this row,
            // displaying the latest data from the now-updated cache.
            SafeInvalidateRow(rowIndex);
        }
        catch (Exception ex)
        {
            // If the background save fails, inform the user and trigger a full reload
            // to ensure the UI is consistent with the database.
            this.Invoke(async () => {
                MessageBox.Show($"自動儲存任務 '{taskToSave.Title}' 時發生錯誤: {ex.Message}", "儲存失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // A full reload is the safest way to ensure data consistency after an error.
                await LoadTasksAsync();
            });
        }
    }

    private async void DgvTasks_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
    {
        var columnName = dgvTasks.Columns[e.ColumnIndex].Name;
        if (columnName != "Status" && columnName != "Priority")
        {
            e.Cancel = true;
            return;
        }

        // Fetch the task asynchronously to check permissions.
        var task = await _taskDataCache.EnsureItemLoadedAsync(e.RowIndex);
        if (task is not null)
        {
            var hasPermission = _currentUser.Role == UserRole.Admin || task.CreatorId == _currentUser.Id || task.AssignedToId == _currentUser.Id;
            if (!hasPermission)
            {
                e.Cancel = true;
                lblStatus.Text = "您沒有權限修改此任務。";
            }
        }
        else
        {
            // If task is not yet loaded, cancel editing for now.
            e.Cancel = true;
        }
    }

    private async void DgvTasks_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

        var task = await _taskDataCache.EnsureItemLoadedAsync(e.RowIndex);
        if (task == null) return;

        var cell = dgvTasks.Rows[e.RowIndex].Cells[e.ColumnIndex];
        var cellValue = cell.Value?.ToString(); // Value is already formatted by CellValueNeeded

        if (string.IsNullOrEmpty(cellValue)) return;

        // This logic can be simplified as we don't need Graphics object in many cases.
        e.ToolTipText = cellValue;
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

    private void BtnSaveChanges_Click(object? sender, EventArgs e)
    {
        if (_selectedTaskForEditing is null || dgvTasks.SelectedRows.Count == 0) return;

        // --- Store the rowIndex at the moment the save is initiated ---
        var rowIndex = dgvTasks.SelectedRows[0].Index;
        var taskToSave = _selectedTaskForEditing;

        var currentRtfContent = richTextEditorComments.Rtf;
        if (taskToSave.Comments == currentRtfContent)
        {
            btnSaveChanges.Enabled = false;
            return;
        }

        // --- Optimistic UI Update ---
        taskToSave.Comments = currentRtfContent;
        lblStatus.Text = "正在儲存備註...";
        btnSaveChanges.Enabled = false;

        // --- Fire-and-Forget, passing BOTH the task and its original rowIndex ---
        _ = SyncCommentChangesToServerAsync(taskToSave, rowIndex);
    }

    /// <summary>
    /// A helper method to handle the asynchronous server-side update for comments.
    /// This runs in the background and is decoupled from the current UI selection.
    /// </summary>
    /// <param name="taskToSync">The task object with the updated comments.</param>
    /// <param name="rowIndex">The original row index of the task when the save was initiated.</param>
    private async Task SyncCommentChangesToServerAsync(TodoItem taskToSync, int rowIndex)
    {
        try
        {
            // --- Step 1: Call the server to persist the changes ---
            var updatedTaskFromServer = await _taskService.UpdateTaskCommentsAsync(
                _currentUser,
                taskToSync.Id,
                taskToSync.Comments ?? string.Empty
            );

            // --- Step 2: Directly update the cache with the authoritative server version ---
            // This is simpler and more direct than invalidating then re-fetching.
            _taskDataCache.UpdateItem(rowIndex, updatedTaskFromServer);

            // --- Step 3: Trigger a repaint to show the new LastModifiedDate ---
            // This call is now safe because it uses the original, unchanging rowIndex.
            SafeInvalidateRow(rowIndex);

            // Update the status on the UI thread.
            this.Invoke(() => {
                lblStatus.Text = "備註已成功儲存。";
            });
        }
        catch (Exception ex)
        {
            this.Invoke(async () => {
                MessageBox.Show($"儲存備註時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // On error, a full reload is the safest way to ensure data consistency.
                _taskDataCache.Clear();
                await LoadTasksAsync();
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
        if (dgvTasks.SelectedRows.Count == 0) return;

        var selectedRowIndex = dgvTasks.SelectedRows[0].Index;

        try
        {
            var taskToEdit = await _taskDataCache.EnsureItemLoadedAsync(selectedRowIndex);

            if (taskToEdit is null)
            {
                MessageBox.Show("無法獲取任務資料，它可能已被刪除。請重新整理。", "找不到任務", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                await LoadTasksAsync();
                return;
            }

            // --- Step 3: Open the dialog and handle the result ---
            using var scope = _serviceProvider.CreateScope();
            var taskDialog = scope.ServiceProvider.GetRequiredService<TaskDetailDialog>();
            taskDialog.SetTaskForEdit(taskToEdit);

            if (taskDialog.ShowDialog(this) == DialogResult.OK)
            {
                lblStatus.Text = "任務已成功更新，正在重新整理...";
                _taskDataCache.Clear();
                await LoadTasksAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"打開編輯視窗時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void TsbDeleteTask_Click(object? sender, EventArgs e)
    {
        // Ensure a row is selected before proceeding.
        if (dgvTasks.SelectedRows.Count == 0)
        {
            MessageBox.Show("請先選擇一個要刪除的任務。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Store the original selected row index BEFORE any async operations or reloads.
        var originalRowIndex = dgvTasks.SelectedRows[0].Index;

        // Get the task object from the cache to display its title in the confirmation box.
        var selectedTask = await _taskDataCache.EnsureItemLoadedAsync(originalRowIndex);

        if (selectedTask is null)
        {
            // This can happen if the cache is out of sync. A refresh is a good recovery action.
            MessageBox.Show("無法獲取任務資料，請重新整理後再試。", "操作失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TsbRefresh_Click(sender, e);
            return;
        }

        // --- Confirmation Dialog with Traditional Chinese text ---
        var confirmResult = MessageBox.Show(
            $"您確定要永久刪除任務 '{selectedTask.Title}' 嗎？\n\n此操作無法復原。",
            "確認刪除",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2); // Default focus on "No"

        if (confirmResult != DialogResult.Yes) return;

        try
        {
            // Perform the delete operation.
            await _taskService.DeleteTaskAsync(_currentUser, selectedTask.Id);

            // Invalidate the cache and reload the grid.
            _taskDataCache.Clear();
            await LoadTasksAsync();

            // After reloading, try to select the nearest available row.
            if (dgvTasks.RowCount > 0)
            {
                var newIndexToSelect = Math.Min(originalRowIndex, dgvTasks.RowCount - 1);

                dgvTasks.ClearSelection();
                dgvTasks.Rows[newIndexToSelect].Selected = true;
                dgvTasks.FirstDisplayedScrollingRowIndex = newIndexToSelect;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"刪除任務失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // On failure, a full reload is the safest way to ensure consistency.
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
        //_sortedColumn = null;
        //SetDefaultFiltersForCurrentUser();
        await LoadTasksAsync();
        lblStatus.Text = "資料已重新整理。";
    }
    #endregion
}