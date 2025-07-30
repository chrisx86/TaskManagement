#nullable enable
using TodoApp.Core.Models;

namespace TodoApp.WinForms.Forms;

public partial class UserSelectionDialog : Form
{
    /// <summary>
    /// Gets the user that was selected in the dialog.
    /// Will be null if the user cancels or no user is selected.
    /// </summary>
    public User? SelectedUser { get; private set; }

    public UserSelectionDialog(List<User> users)
    {
        InitializeComponent();

        lstUsers.DataSource = users;
        lstUsers.DisplayMember = "Username";

        this.btnOk.Click += BtnOk_Click;
        this.btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
        this.lstUsers.DoubleClick += (s, e) => BtnOk_Click(s, e);
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        if (lstUsers.SelectedItem is User selectedUser)
        {
            this.SelectedUser = selectedUser;
            this.DialogResult = DialogResult.OK;
        }
        else
        {
            MessageBox.Show("請選擇一位使用者。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}