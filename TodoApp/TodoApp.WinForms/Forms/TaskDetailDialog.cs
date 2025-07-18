#nullable enable
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.WinForms.ViewModels;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cmp;

namespace TodoApp.WinForms.Forms;

/// <summary>
/// A dialog form for creating a new task or editing an existing one.
/// </summary>
public partial class TaskDetailDialog : Form
{
    private readonly IUserService _userService;
    private readonly ITaskService _taskService;
    private readonly IUserContext _userContext;

    private TodoItem? _editingTask;
    private bool _isUpdatingUI;

    public TaskDetailDialog(IUserService userService, ITaskService taskService, IUserContext userContext)
    {
        InitializeComponent();

        _userService = userService;
        _taskService = taskService;
        _userContext = userContext;

        this.Load += OnFormLoad;
        this.btnSave.Click += OnSaveButtonClick;
    }

    /// <summary>
    /// Sets the dialog to "Edit" mode by providing the task to be edited.
    /// This method should be called before showing the dialog for editing an existing task.
    /// </summary>
    /// <param name="taskToEdit">The task object to be edited.</param>
    public void SetTaskForEdit(TodoItem taskToEdit)
    {
        _editingTask = taskToEdit;
    }

    private async void OnFormLoad(object? sender, EventArgs e)
    {
        try
        {
            this.Enabled = false;

            await PopulateComboBoxesAsync();

            if (_editingTask != null)
            {
                this.Text = "編輯任務";
                PopulateControlsFromTask(_editingTask);
            }
            else
            {
                this.Text = "新增任務";
                SetDefaultsForCreateMode();
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

    private async void OnSaveButtonClick(object? sender, EventArgs e)
    {
        if (!TryValidateInput(out var validationError))
        {
            MessageBox.Show(validationError, "驗證錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var currentUser = _userContext.CurrentUser;
            if (currentUser is null)
            {
                MessageBox.Show("無法獲取當前使用者資訊，請重新登入。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_editingTask is null) // CREATE MODE
            {
                var newTask = new TodoItem();
                UpdateTaskFromControls(newTask);

                await _taskService.CreateTaskAsync(
                    currentUser,
                    newTask.Title,
                    newTask.Comments,
                    newTask.Priority,
                    newTask.DueDate,
                    newTask.AssignedToId
                );
            }
            else // EDIT MODE
            {
                UpdateTaskFromControls(_editingTask);
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

    private async Task PopulateComboBoxesAsync()
    {
        cmbStatus.DataSource = Enum.GetValues<TodoStatus>();

        cmbPriority.DataSource = Enum.GetValues<PriorityLevel>();

        var allUsers = await _userService.GetAllUsersAsync();
        var userDataSource = new List<UserDisplayItem> { new() { Id = 0, Username = "(未指派)" } };
        var uniqueUsers = allUsers
            .Where(u => u != null && !string.IsNullOrEmpty(u.Username))
            .GroupBy(u => u.Id)
            .Select(g => g.First());

        foreach (var user in uniqueUsers.OrderBy(u => u.Username))
        {
            userDataSource.Add(new UserDisplayItem { Id = user.Id, Username = user.Username });
        }

        cmbAssignedTo.DataSource = userDataSource;
        cmbAssignedTo.DisplayMember = nameof(UserDisplayItem.Username);
        cmbAssignedTo.ValueMember = nameof(UserDisplayItem.Id);
    }

    private void PopulateControlsFromTask(TodoItem task)
    {
        _isUpdatingUI = true;
        try
        {
            txtTitle.Text = task.Title;
            txtComments.Text = task.Comments;

            cmbStatus.SelectedItem = task.Status;

            cmbPriority.SelectedItem = task.Priority;
            cmbAssignedTo.SelectedValue = task.AssignedToId ?? 0;

            if (task.DueDate.HasValue)
            {
                dtpDueDate.Checked = true;
                dtpDueDate.Value = task.DueDate.Value.ToLocalTime();
            }
            else { dtpDueDate.Checked = false; }
        }
        finally { _isUpdatingUI = false; }
    }

    private void SetDefaultsForCreateMode()
    {
        cmbPriority.SelectedItem = PriorityLevel.Medium;
        dtpDueDate.Checked = false;
    }

    private void UpdateTaskFromControls(TodoItem task)
    {
        task.Title = txtTitle.Text.Trim();
        task.Comments = txtComments.Text.Trim();
        task.Status = (TodoStatus)cmbStatus.SelectedItem;
        task.Priority = (PriorityLevel)cmbPriority.SelectedItem;
        task.DueDate = dtpDueDate.Checked ? dtpDueDate.Value.Date : null;
        task.AssignedToId = (cmbAssignedTo.SelectedValue is int selectedId && selectedId > 0) ? selectedId : null;
    }

    private bool TryValidateInput(out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(txtTitle.Text))
        {
            errorMessage = "標題欄位為必填。";
            txtTitle.Focus();
            return false;
        }

        if (cmbPriority.SelectedItem is not PriorityLevel)
        {
            errorMessage = "請選擇一個有效的優先級。";
            cmbPriority.Focus();
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}