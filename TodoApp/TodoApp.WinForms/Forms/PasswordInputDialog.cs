#nullable enable
namespace TodoApp.WinForms.Forms;

public partial class PasswordInputDialog : Form
{
    /// <summary>
    /// Gets the new password entered by the user.
    /// This will only have a value if the user clicks OK and validation passes.
    /// </summary>
    public string? NewPassword { get; private set; }

    public PasswordInputDialog(string title, string prompt)
    {
        InitializeComponent();

        this.Text = title;
        this.lblPrompt.Text = prompt;

        this.btnOk.Click += BtnOk_Click;
        this.btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        errorProvider1.Clear();

        var newPassword = txtNewPassword.Text;
        var confirmPassword = txtConfirmPassword.Text;

        if (string.IsNullOrWhiteSpace(newPassword))
        {
            errorProvider1.SetError(txtNewPassword, "新密碼不能為空。");
            return;
        }

        if (newPassword != confirmPassword)
        {
            errorProvider1.SetError(txtConfirmPassword, "兩次輸入的密碼不一致。");
            txtConfirmPassword.Clear();
            txtConfirmPassword.Focus();
            return;
        }

        this.NewPassword = newPassword;
        this.DialogResult = DialogResult.OK;
        this.Close();
    }
}