using System.Reflection;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Services;

namespace TodoApp.WinForms.Forms
{
    public partial class LoginForm : Form
    {
        private readonly IUserService _userService;
        private readonly IUserContext _userContext;
        private readonly LocalCredentialManager _credentialManager;
        /// <summary>
        /// Property to hold the authenticated user after a successful login.
        /// The main application can access this property to get the user info.
        /// </summary>
        public User? AuthenticatedUser { get; private set; }

        /// <summary>
        /// Constructor for LoginForm. It receives the IUserService via dependency injection.
        /// </summary>
        /// <param name="userService">The user service implementation.</param>
        public LoginForm(IUserService userService, IUserContext userContext, LocalCredentialManager credentialManager)
        {
            InitializeComponent();
            _userService = userService;
            _userContext = userContext;
            _credentialManager = credentialManager;
            this.btnLogin.Click += BtnLogin_Click;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            SetWindowTitleWithVersion();
        }

        private void SetWindowTitleWithVersion()
        {
            var mainAssembly = Assembly.GetExecutingAssembly();
            var version = mainAssembly.GetName().Version;
            var versionString = version != null ? $"V {version.Major}.{version.Minor}" : "V ?.?";
            this.Text = $"系統登入 TaskManagement App {versionString}";
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

                if (authenticatedUser is not null)
                {
                    _userContext.SetCurrentUser(authenticatedUser);
                    if (chkRememberMe.Checked)
                    {
                        var token = await _userService.GenerateAndStoreLoginTokenAsync(authenticatedUser.Id);
                        if (token is not null)
                            _credentialManager.SaveCredentials(authenticatedUser.Username, token);
                    }
                    else
                    {
                        _credentialManager.ClearCredentials();
                    }
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
                MessageBox.Show($"登入過程中發生未預期的錯誤: {ex.Message}", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            this.UseWaitCursor = isLoading;

            txtUsername.Enabled = !isLoading;
            txtPassword.Enabled = !isLoading;
            btnLogin.Enabled = !isLoading;
            btnCancel.Enabled = !isLoading;

            btnLogin.Text = isLoading ? "登入中..." : "登入(&L)";
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}