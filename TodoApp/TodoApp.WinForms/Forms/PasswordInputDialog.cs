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

        // Set dialog title and prompt text dynamically.
        this.Text = title;
        this.lblPrompt.Text = prompt; // Assuming you have a main prompt label lblPrompt

        // Wire up events
        this.btnOk.Click += BtnOk_Click;
        this.btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        errorProvider1.Clear(); // Clear previous errors

        string newPassword = txtNewPassword.Text;
        string confirmPassword = txtConfirmPassword.Text;

        // Validation
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

        // If validation passes:
        this.NewPassword = newPassword;
        this.DialogResult = DialogResult.OK;
        this.Close();
    }
}