using TodoApp.Core.Models;
using TodoApp.Core.Services;

namespace TodoApp.WinForms.Forms
{
    public partial class LoginForm : Form
    {
        // A private field to hold the user service, injected via the constructor.
        private readonly IUserService _userService;
        // --- STEP 1: Add a field for IUserContext ---
        private readonly IUserContext _userContext;
        /// <summary>
        /// Property to hold the authenticated user after a successful login.
        /// The main application can access this property to get the user info.
        /// </summary>
        public User? AuthenticatedUser { get; private set; }

        /// <summary>
        /// Constructor for LoginForm. It receives the IUserService via dependency injection.
        /// </summary>
        /// <param name="userService">The user service implementation.</param>
        public LoginForm(IUserService userService, IUserContext userContext)
        {
            InitializeComponent();
            _userService = userService;
            _userContext = userContext;
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("使用者名稱和密碼為必填欄位。", "登入失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetLoadingState(true);
            try
            {
                var authenticatedUser = await _userService.AuthenticateAsync(username, password);

                if (authenticatedUser != null)
                {
                    _userContext.SetCurrentUser(authenticatedUser);

                    this.AuthenticatedUser = authenticatedUser;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("使用者名稱或密碼錯誤。", "登入失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle potential exceptions from the service layer (e.g., database connection error).
                MessageBox.Show($"登入過程中發生未預期的錯誤: {ex.Message}", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                ToggleControls(true);
            }
        }
        private void SetLoadingState(bool isLoading)
        {
            this.UseWaitCursor = isLoading;
            txtUsername.Enabled = !isLoading;
            txtPassword.Enabled = !isLoading;
            btnLogin.Enabled = !isLoading;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // Set the DialogResult to Cancel and close the form.
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// A helper method to enable or disable controls during async operations
        /// to prevent user from clicking multiple times.
        /// </summary>
        private void ToggleControls(bool isEnabled)
        {
            txtUsername.Enabled = isEnabled;
            txtPassword.Enabled = isEnabled;
            btnLogin.Enabled = isEnabled;
            btnCancel.Enabled = isEnabled;
            // Show a waiting cursor when controls are disabled.
            this.Cursor = isEnabled ? Cursors.Default : Cursors.WaitCursor;
        }
    }
}