#nullable enable
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.WinForms.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.WinForms.Forms;

public partial class TaskDetailDialog : Form
{
    private readonly IUserService _userService;
    private readonly ITaskService _taskService;
    private readonly IUserContext _userContext;

    // This field holds the task being edited. If it's null, we are in "Create" mode.
    private TodoItem? _editingTask;
    private List<User> _allUsers = new();

    // A flag to prevent events from firing during programmatic UI updates.
    private bool _isUpdatingUI = false;

    public TaskDetailDialog(IUserService userService, ITaskService taskService, IUserContext userContext)
    {
        InitializeComponent();

        _userService = userService;
        _taskService = taskService;
        _userContext = userContext;

        this.Load += TaskDialog_Load;
        this.btnSave.Click += BtnSave_Click;
    }

    private async void TaskDialog_Load(object? sender, EventArgs e)
    {
        try
        {
            this.Enabled = false; // Disable form during async load for better UX

            // --- STEP 1: Populate all data sources first ---
            _allUsers = await _userService.GetAllUsersAsync();
            PopulateComboBoxes();

            // --- STEP 2: Check the mode and populate UI controls at the correct time ---
            if (_editingTask != null)
            {
                // We are in EDIT mode. Populate all controls from the _editingTask object.
                this.Text = "編輯任務";
                PopulateControlsFromTask(_editingTask);
            }
            else
            {
                // We are in CREATE mode. Set default values.
                this.Text = "新增任務";
                cmbPriority.SelectedItem = PriorityLevel.Medium;
                dtpDueDate.Checked = false;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"載入對話框時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }
        finally
        {
            this.Enabled = true;
        }
    }

    private void PopulateComboBoxes()
    {
        // Priority ComboBox (no change needed)
        cmbPriority.DataSource = Enum.GetValues<PriorityLevel>();

        // --- FIXED: Avoid using a null value in the ValueMember for the ComboBox ---
        // We will use a sentinel value (e.g., 0) to represent "Unassigned".

        var userDataSource = new List<UserDisplayItem>
        {
            // Use a non-null placeholder value for the "Unassigned" option.
            // 0 is a safe choice as real user Ids from the database start at 1.
            new UserDisplayItem { Id = 0, Username = "(未指派)" }
        };

        // This part remains the same
        var uniqueUsersById = new Dictionary<int, string>();
        foreach (var user in _allUsers.Where(u => u != null && !string.IsNullOrEmpty(u.Username)))
        {
            uniqueUsersById.TryAdd(user.Id, user.Username);
        }
        foreach (var userPair in uniqueUsersById.OrderBy(p => p.Value))
        {
            userDataSource.Add(new UserDisplayItem { Id = userPair.Key, Username = userPair.Value });
        }

        cmbAssignedTo.DataSource = userDataSource;
        cmbAssignedTo.DisplayMember = nameof(UserDisplayItem.Username);
        // The ValueMember is now of type int, not int?
        cmbAssignedTo.ValueMember = nameof(UserDisplayItem.Id);
    }


    /// <summary>
    /// Sets the dialog to "Edit" mode by providing the task to be edited.
    /// Its only job is to set the state. UI population happens in the Load event.
    /// </summary>
    public void SetTaskForEdit(TodoItem taskToEdit)
    {
        _editingTask = taskToEdit;
    }

    /// <summary>
    /// Helper method to populate all UI controls from a given TodoItem.
    /// </summary>
    private void PopulateControlsFromTask(TodoItem task)
    {
        _isUpdatingUI = true; // Prevent event handlers from firing
        try
        {
            txtTitle.Text = task.Title;
            txtComments.Text = task.Comments;

            // These assignments are now safe because the ComboBox DataSources are already populated.
            cmbPriority.SelectedItem = task.Priority;
            cmbAssignedTo.SelectedValue = task.AssignedToId ?? 0;
            if (task.DueDate.HasValue)
            {
                dtpDueDate.Checked = true;
                // Ensure to convert from UTC (database) to Local time for display
                dtpDueDate.Value = task.DueDate.Value.ToLocalTime();
            }
            else
            {
                dtpDueDate.Checked = false;
            }
        }
        finally
        {
            _isUpdatingUI = false; // Always re-enable events
        }
    }

    private async void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTitle.Text))
        {
            MessageBox.Show("標題欄位為必填。", "驗證錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtTitle.Focus();
            return;
        }

        // Safely get values from ComboBoxes before the try block.
        if (cmbPriority.SelectedItem is not PriorityLevel priority)
        {
            MessageBox.Show("請選擇一個有效的優先級。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        int? assignedToId = null;
        if (cmbAssignedTo.SelectedValue is int selectedId && selectedId > 0)
        {
            assignedToId = selectedId;
        }
        DateTime? dueDate = null;
        if (dtpDueDate.Checked)
        {
            dueDate = dtpDueDate.Value.Date;
        }
        try
        {
            var currentUser = _userContext.CurrentUser;
            if (currentUser == null)
            {
                MessageBox.Show("無法獲取當前使用者資訊，請重新登入。");
                return;
            }

            if (_editingTask == null) // CREATE MODE
            {
                await _taskService.CreateTaskAsync(
                    currentUser,
                    txtTitle.Text.Trim(),
                    txtComments.Text.Trim(),
                    priority,
                    dueDate,
                    assignedToId
                );
            }
            else
            {
                // --- EDIT MODE ---
                _editingTask.Title = txtTitle.Text.Trim();
                _editingTask.Comments = txtComments.Text.Trim();
                _editingTask.Priority = priority;
                _editingTask.AssignedToId = assignedToId;
                _editingTask.DueDate = dueDate;

                await _taskService.UpdateTaskAsync(currentUser, _editingTask);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (DbUpdateConcurrencyException)
        {
            MessageBox.Show("儲存失敗！此任務已被他人修改或刪除。", "並行衝突", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"儲存任務時發生未預期的錯誤: {ex.Message}", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}