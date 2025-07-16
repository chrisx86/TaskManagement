#nullable enable
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoStatus = TodoApp.Core.Models.TodoStatus;
using Microsoft.VisualBasic;
using TodoApp.WinForms.ViewModels;
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
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        this.Text = $"�ݿ�ƶ��M�� - [{_currentUser.Username}]";

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
            HeaderText = "���A",
            DataPropertyName = "Status",
            DataSource = Enum.GetValues<TodoStatus>(),
            Width = 100,
            FlatStyle = FlatStyle.Flat,
            SortMode = DataGridViewColumnSortMode.Programmatic // Enable manual sorting
        };
        dgvTasks.Columns.Add(statusColumn);

        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "���D", DataPropertyName = "Title", Width = 300, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Priority", HeaderText = "�u����", DataPropertyName = "Priority", Width = 80, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "DueDate", HeaderText = "�����", DataPropertyName = "DueDate", Width = 110, DefaultCellStyle = { Format = "yyyy-MM-dd" }, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "AssignedTo", HeaderText = "������", DataPropertyName = "AssignedTo", Width = 100, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Creator", HeaderText = "�إߪ�", DataPropertyName = "Creator", Width = 100, SortMode = DataGridViewColumnSortMode.Programmatic });
        dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Comments", HeaderText = "�Ƶ�", DataPropertyName = "Comments", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.Programmatic });
    }
    #region --- NEW User Action Event Handlers ---

    // --- Requirement 1: Change Password ---
    private async void TsbChangePassword_Click(object? sender, EventArgs e)
    {
        // Use a simple InputBox for password entry. For higher security, a custom dialog is better.
        string newPassword = Interaction.InputBox("�п�J�z���s�K�X�G", "�ק�K�X", "");

        // Check if the user cancelled or entered an empty password.
        if (string.IsNullOrWhiteSpace(newPassword))
        {
            // If user clicks "Cancel", InputBox returns an empty string.
            // We can silently exit without showing a message.
            return;
        }

        string confirmPassword = Interaction.InputBox("�ЦA����J�z���s�K�X�H�i��T�{�G", "�T�{�K�X", "");
        if (newPassword != confirmPassword)
        {
            MessageBox.Show("�⦸��J���K�X���@�P�A�Э��աC", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            var success = await _userService.ResetPasswordAsync(_currentUser.Id, newPassword);

            if (success)
            {
                MessageBox.Show("�z���K�X�w���\��s�C", "���\", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // This case should be rare, but it's good to handle it.
                MessageBox.Show("��s�K�X���ѡA�䤣��z���ϥΪ̱b���C", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"�ק�K�X�ɵo�Ϳ��~: {ex.Message}", "�t�ο��~", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // --- Requirement 2: Switch User ---
    private void TsbSwitchUser_Click(object? sender, EventArgs e)
    {
        // Show a confirmation dialog before logging out.
        var confirmResult = MessageBox.Show(
            "�z�T�w�n�n�X�ä����ϥΪ̶ܡH",
            "�T�{�n�X",
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
        if (e.RowIndex < 0) return;

        // Use a guard clause to exit if the row's DataBoundItem is not a TodoItem.
        if (dgvTasks.Rows[e.RowIndex].DataBoundItem is not TodoItem task) return;

        // --- Apply row style first ---
        var rowStyle = dgvTasks.Rows[e.RowIndex].DefaultCellStyle;
        // ... (The conditional coloring logic remains the same) ...

        // --- Format specific cells ---
        string columnName = dgvTasks.Columns[e.ColumnIndex].Name;

        // IMPORTANT: Only set the value if it's different to prevent recursion.
        // This is a key fix to prevent flickering.
        if (columnName == "Status")
        {
            if (e.Value?.ToString() != task.Status.ToString())
            {
                e.Value = task.Status.ToString();
            }
        }
        else if (columnName == "AssignedTo" && e.Value is User assignedUser)
        {
            if (e.Value?.ToString() != assignedUser.Username)
            {
                e.Value = assignedUser.Username;
                e.FormattingApplied = true;
            }
        }
        else if (columnName == "Creator")
        {
            if (e.Value is User creatorUser)
            {
                e.Value = creatorUser.Username;
                e.FormattingApplied = true;
            }
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
            new StatusDisplayItem { Name = "�Ҧ����A", Value = null }
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
            new UserDisplayItem { Username = "�Ҧ��H", Id = null }
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
        if (!this.IsHandleCreated ||
            cmbFilterStatus.SelectedItem == null ||
            cmbFilterByUserRelation.SelectedItem == null ||
            cmbFilterByAssignedUser.SelectedItem == null)
        {
            // If any condition is not met, simply exit the method.
            // This prevents NullReferenceException and unnecessary processing.
            return;
        }
        SetLoadingState(true);
        try
        {
            TodoStatus? statusFilter = (cmbFilterStatus.SelectedItem as StatusDisplayItem)?.Value;
            var userFilter = (UserTaskFilter)cmbFilterByUserRelation.SelectedItem; // This line is now safe.
            int? assignedToUserIdFilter = (cmbFilterByAssignedUser.SelectedItem as UserDisplayItem)?.Id;

            var tasks = await _taskService.GetAllTasksAsync(statusFilter, userFilter, _currentUser.Id, assignedToUserIdFilter);
            var sortedTasks = tasks
                .OrderBy(t => t.Status == TodoStatus.InProgress ? 0 : (t.Status == TodoStatus.Pending ? 1 : 2))
                .ThenByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
                .ToList();

            _tasksBindingList.Clear();
            sortedTasks.ForEach(task => _tasksBindingList.Add(task));

            lblStatus.Text = $"�@ {_tasksBindingList.Count} �ӥ���";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"���J���Ȯɵo�Ϳ��~: {ex.Message}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
            DgvTasks_SelectionChanged(null, EventArgs.Empty);
        }
    }

    private void SetLoadingState(bool isLoading)
    {
        this.UseWaitCursor = isLoading;
        panelFilters.Enabled = !isLoading;
        toolStrip1.Enabled = !isLoading;
        splitContainerMain.Enabled = !isLoading;
        lblStatus.Text = isLoading ? "���b���J..." : "�N��";
    }

    private async void Filter_Changed(object? sender, EventArgs e)
    {
        // If the UI is being updated by code, do nothing.
        if (_isUpdatingUI) return;

        // When the user manually changes a filter, reload the tasks.
        await LoadTasksAsync();
    }

    #region --- Event Handlers ---

    private void SetDefaultFiltersForCurrentUser()
    {
        _isUpdatingUI = true;
        try
        {
            // Instead of setting SelectedValue to null, set the index to 0.
            // This is safer because we know the "All" item is always the first one.
            if (cmbFilterStatus.Items.Count > 0)
            {
                cmbFilterStatus.SelectedIndex = 0; // Select "�Ҧ����A"
            }

            if (cmbFilterByAssignedUser.Items.Count > 0)
            {
                cmbFilterByAssignedUser.SelectedIndex = 0; // Select "�Ҧ��H"
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
            txtCommentsPreview.Text = selectedTask.Comments ?? "�����ȨS���Ƶ��C";
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
            lblStatus.Text = $"���� '{changedTask.Title}' �����A�w��s�C";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"��s���Ȫ��A����: {ex.Message}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                lblStatus.Text = "�z�S���v���ק惡���Ȫ����A�C";
            }
        }
    }

    private void DgvTasks_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) { TsbEditTask_Click(sender, e); }
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

        var confirmResult = MessageBox.Show($"�z�T�w�n�R������ '{selectedTask.Title}' �ܡH", "�T�{�R��", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirmResult != DialogResult.Yes) return;

        try
        {
            await _taskService.DeleteTaskAsync(selectedTask.Id, _currentUser.Id, _currentUser.Role == UserRole.Admin);
            _tasksBindingList.Remove(selectedTask);
            lblStatus.Text = "���Ȥw���\�R���I";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"�R�����ȥ���: {ex.Message}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
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