#nullable enable
using TodoApp.Core.Models;
using TodoApp.Core.Services;

namespace TodoApp.WinForms.Forms;

/// <summary>
/// A dialog that displays the complete audit trail for a specific task.
/// </summary>
public partial class TaskHistoryDialog : Form
{
    private readonly int _todoItemId;
    private readonly string _taskTitle;
    private readonly ITaskHistoryService _historyService;
    private readonly IUserService _userService;

    public TaskHistoryDialog(
        int todoItemId,
        string taskTitle,
        ITaskHistoryService historyService,
        IUserService userService)
    {
        InitializeComponent();
        _todoItemId = todoItemId;
        _taskTitle = taskTitle;
        _historyService = historyService;
        _userService = userService;

        this.Load += OnDialogLoad;
        this.btnClose.Click += (s, e) => this.Close();
    }

    private async void OnDialogLoad(object? sender, System.EventArgs e)
    {
        lblTaskTitle.Text = $"任務 '{_taskTitle}' 的歷史記錄";
        SetupDataGridView();
        await LoadHistoryAsync();
    }

    private void SetupDataGridView()
    {
        dgvHistory.AutoGenerateColumns = false;

        // --- KEY FIX for Word Wrap and Auto-Height ---
        dgvHistory.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        dgvHistory.Columns.Clear();

        // --- Define columns with optimized widths ---
        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ChangeDate",
            HeaderText = "操作時間",
            DataPropertyName = "ChangeDate",
            Width = 100,
            DefaultCellStyle = { Format = "yyyy-MM-dd HH:mm:ss" }
        });

        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Username",
            HeaderText = "操作者",
            DataPropertyName = "User",
            Width = 50
        });

        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Action",
            HeaderText = "操作類型",
            DataPropertyName = "Action",
            Width = 50
        });

        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ChangeDescription",
            HeaderText = "詳細描述",
            DataPropertyName = "ChangeDescription",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            MinimumWidth = 500
        });

        dgvHistory.CellFormatting += DgvHistory_CellFormatting;
    }

    private async Task LoadHistoryAsync()
    {
        try
        {
            this.UseWaitCursor = true;

            var history = await _historyService.GetHistoryForTaskAsync(_todoItemId);

            dgvHistory.DataSource = history;
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"載入歷史記錄時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            this.UseWaitCursor = false;
        }
    }
    private void DgvHistory_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;

        if (dgvHistory.Columns[e.ColumnIndex].Name == "Username")
        {
            if (e.Value is User user)
            {
                e.Value = user.Username;
                e.FormattingApplied = true;
            }
            else
            {
                if (dgvHistory.Rows[e.RowIndex].DataBoundItem is TaskHistory history)
                {
                    e.Value = $"使用者 (ID: {history.UserId})";
                    e.FormattingApplied = true;
                }
            }
        }
    }
}