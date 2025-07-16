using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// Core and Infrastructure using statements
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Services;
using TodoApp.WinForms.Forms;
using TodoApp.WinForms.Utils;

namespace TodoApp.WinForms
{
    internal static class Program
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ===================================================================
            // Task 8.1: Global Error Handling Implementation
            // This setup must be at the very beginning of the Main method.
            // ===================================================================
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Catches exceptions from the main UI thread.
            Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);

            // Catches exceptions from non-UI threads.
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
            // ===================================================================

            // Your existing DI and application startup logic continues here.
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            ApplicationConfiguration.Initialize();

            // --- APPLICATION STARTUP LOGIC ---
            try
            {
                var loginForm = ServiceProvider.GetRequiredService<LoginForm>();

                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    ApplicationState.CurrentUser = loginForm.AuthenticatedUser;
                    Application.Run(ServiceProvider.GetRequiredService<MainForm>());
                }
            }
            catch (Exception ex)
            {
                // This catch block will handle errors that might occur during startup,
                // before the main message loop has even started.
                HandleException(ex, "Application Startup");
            }
        }


        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                // Step 1: Add configuration sources. This tells the host to read the JSON file.
                .ConfigureAppConfiguration((context, builder) =>
                {
                    // Sets the base path for finding the config file to the application's current directory.
                    builder.SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) => {
                    // Step 2: Use the configuration to set up services.

                    // Read the connection string from the "ConnectionStrings" section of appsettings.json.
                    string? connectionString = context.Configuration.GetConnectionString("DefaultConnection");

                    // A robust check to ensure the connection string exists in the config file.
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        MessageBox.Show(
                            "Database connection string 'DefaultConnection' could not be found in appsettings.json.",
                            "Configuration Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        // Stop the application if configuration is missing.
                        throw new InvalidOperationException("Fatal: ConnectionString is not configured.");
                    }
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseSqlite(connectionString);

                        // --- ADD THESE LINES FOR DETAILED LOGGING ---
                        // This will log EF Core's activity to the debug output window.
                        options.LogTo(message => System.Diagnostics.Debug.WriteLine(message),
                                      Microsoft.Extensions.Logging.LogLevel.Information);
                        // This enables sensitive data logging, which includes connection strings and parameter values.
                        // Be careful using this in production. It's great for debugging.
                        options.EnableSensitiveDataLogging();
                    });
                    services.AddSingleton<IUserContext, UserContext>();
                    services.AddTransient<IUserService, UserService>();
                    services.AddTransient<ITaskService, TaskService>();
                    // We use AddTransient because the service itself is lightweight and doesn't hold state.
                    // Each time the dashboard is opened, it can get a fresh service instance.
                    services.AddTransient<IAdminDashboardService, AdminDashboardService>();
                    // --- FORMS REGISTRATION ---
                    // Register all forms that will be resolved by the DI container
                    services.AddTransient<LoginForm>();
                    services.AddTransient<MainForm>();
                    services.AddTransient<TaskDetailDialog>();
                    services.AddTransient<UserManagementDialog>();
                    services.AddTransient<AdminDashboardForm>();
                });
        }

        // --- NEW ERROR HANDLING METHODS ---

        /// <summary>
        /// Handles exceptions thrown on the UI thread.
        /// </summary>
        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception, "UI Thread Exception");
        }

        /// <summary>
        /// Handles exceptions thrown on non-UI threads.
        /// </summary>
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception, "Non-UI Thread Exception");
        }

        /// <summary>
        /// The core logic for logging the exception and showing a user-friendly message.
        /// </summary>
        /// <param name="ex">The exception that was caught.</param>
        /// <param name="source">The source of the exception.</param>
        private static void HandleException(Exception? ex, string source)
        {
            if (ex == null) return;

            // Step 1: Log the detailed exception
            try
            {
                var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt");
                var errorDetails =
                    $"============================================================\r\n" +
                    $"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n" +
                    $"Source: {source}\r\n" +
                    $"Exception Type: {ex.GetType().FullName}\r\n" +
                    $"Message: {ex.Message}\r\n" +
                    $"Stack Trace:\r\n{ex}\r\n" + // .ToString() includes inner exceptions
                    $"============================================================\r\n\r\n";

                File.AppendAllText(logFilePath, errorDetails);
            }
            catch (Exception logEx)
            {
                // If logging fails, show a more critical error message.
                MessageBox.Show(
                    $"A critical error occurred, and the error could not be logged.\n\n" +
                    $"Original Error: {ex.Message}\n\n" +
                    $"Logging Error: {logEx.Message}",
                    "Critical System Failure",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
            }

            // Step 2: Show a friendly message to the user
            var friendlyMessage = "An unexpected error has occurred in the application.\n\n" +
                                           "The details have been logged for the development team.\n" +
                                           "The application will now close to prevent data corruption.";

            MessageBox.Show(friendlyMessage, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Step 3: Terminate the application
            // It's generally unsafe to continue after an unhandled exception.
            Environment.Exit(1);
        }
    }
}