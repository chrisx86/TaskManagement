#nullable enable
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.WinForms.ViewModels;
namespace TodoApp.WinForms.Forms;

public partial class TaskDetailDialog : Form
{
    private readonly IUserService _userService;
    private readonly ITaskService _taskService;
    private readonly IUserContext _userContext;

    // This field holds the task being edited. If it's null, we are in "Create" mode.
    private TodoItem? _editingTask;
    private List<User> _allUsers = new();

    // The DI container will inject these services.
    public TaskDetailDialog(IUserService userService, ITaskService taskService, IUserContext userContext)
    {
        InitializeComponent();

        _userService = userService;
        _taskService = taskService; // Injected for future use if needed, e.g., validation.
        _userContext = userContext;

        this.Load += TaskDetailDialog_Load;
        this.btnSave.Click += BtnSave_Click;
        // Let the Cancel button on the form handle closing.
    }

    private async void TaskDetailDialog_Load(object? sender, EventArgs e)
    {
        try
        {
            this.Enabled = false; // Disable form during async load
            _allUsers = await _userService.GetAllUsersAsync();
            PopulateComboBoxes();

            if (_editingTask == null)
            {
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
        // Priority ComboBox
        cmbPriority.DataSource = Enum.GetValues<PriorityLevel>();

        // --- FIXED: A much safer way to bind a nullable foreign key ---
        // We use a custom class/struct or a list of KeyValuePair to avoid dictionary issues.
        // Here we use a list of a simple custom object for clarity.
        var userDataSource = new List<UserDisplayItem>
        {
            new UserDisplayItem { Id = null, Username = "(未指派)" }
        };

        var validUsers = _allUsers
            .Where(u => !string.IsNullOrEmpty(u.Username))
            .OrderBy(u => u.Username);

        foreach (var user in validUsers)
        {
            userDataSource.Add(new UserDisplayItem { Id = user.Id, Username = user.Username });
        }

        cmbAssignedTo.DataSource = userDataSource;
        cmbAssignedTo.DisplayMember = nameof(UserDisplayItem.Username);
        cmbAssignedTo.ValueMember = nameof(UserDisplayItem.Id);
    }

    /// <summary>
    /// This is the public method for MainForm to call when a task needs to be edited.
    /// It switches the dialog to "Edit" mode.
    /// </summary>
    /// <param name="taskToEdit">The task object from MainForm's DataGridView.</param>
    public void SetTaskForEdit(TodoItem taskToEdit)
    {
        _editingTask = taskToEdit;
        this.Text = "編輯任務";

        txtTitle.Text = _editingTask.Title;
        txtComments.Text = _editingTask.Comments;
        cmbPriority.SelectedItem = _editingTask.Priority;
        cmbAssignedTo.SelectedValue = _editingTask.AssignedToId;

        if (_editingTask.DueDate.HasValue)
        {
            dtpDueDate.Checked = true;
            dtpDueDate.Value = _editingTask.DueDate.Value.ToLocalTime();
        }
        else
        {
            dtpDueDate.Checked = false;
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

        try
        {
            if (_editingTask == null)
            {
                await _taskService.CreateTaskAsync(
                    txtTitle.Text.Trim(),
                    txtComments.Text.Trim(),
                    _userContext.CurrentUser!.Id,
                    (PriorityLevel)cmbPriority.SelectedValue,
                    dtpDueDate.Checked ? dtpDueDate.Value.ToUniversalTime() : null,
                    (int?)cmbAssignedTo.SelectedValue > 0 ? (int?)cmbAssignedTo.SelectedValue : null
                );
            }
            else
            {
                _editingTask.Title = txtTitle.Text.Trim();
                _editingTask.Comments = txtComments.Text.Trim();
                _editingTask.Priority = (PriorityLevel)cmbPriority.SelectedValue;
                _editingTask.AssignedToId = (int?)cmbAssignedTo.SelectedValue;
                _editingTask.DueDate = dtpDueDate.Checked ? dtpDueDate.Value.ToUniversalTime() : null;

                await _taskService.UpdateTaskAsync(_editingTask);
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