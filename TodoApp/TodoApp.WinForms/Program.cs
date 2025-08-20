#nullable enable
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Services;
using TodoApp.Infrastructure.Extensions;
using TodoApp.WinForms.Forms;
using Microsoft.Data.Sqlite;

namespace TodoApp.WinForms;

internal static class Program
{
    internal static readonly CancellationTokenSource AppShutdownTokenSource = new();
    [STAThread]
    static void Main()
    {
        // Before building the host or doing anything else, ensure that any lingering
        // SQLite connections from a previous crash are cleared from the connection pool.
        // This helps release locks on the .db-shm and .db-wal files.
        ClearAllSqlitePools();

        var host = CreateHostBuilder().Build();
        var services = host.Services;

        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (sender, args) => HandleException(args.Exception, "UI Thread");
        AppDomain.CurrentDomain.UnhandledException += (sender, args) => HandleException(args.ExceptionObject as Exception, "Non-UI Thread");

        ApplicationConfiguration.Initialize();

        using (var serviceScope = services.CreateScope())
        {
            var scopedServices = serviceScope.ServiceProvider;
            var loginSuccess = false;
            try
            {
                var dbContext = scopedServices.GetRequiredService<AppDbContext>();
                dbContext.ApplySharedCachePragma();

                loginSuccess = TryAutoLogin(scopedServices);

                if (!loginSuccess)
                {
                    var loginForm = scopedServices.GetRequiredService<LoginForm>();
                    loginSuccess = loginForm.ShowDialog() == DialogResult.OK;
                }

                if (loginSuccess)
                {
                    var mainForm = scopedServices.GetRequiredService<MainForm>();

                    // Wire up the FormClosing event to our single, resolved mainForm instance.
                    mainForm.FormClosing += (s, e) =>
                    {
                        if (!AppShutdownTokenSource.IsCancellationRequested)
                            AppShutdownTokenSource.Cancel();
                    };
                    Application.Run(mainForm);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Application Startup");
            }
            finally
            {
                if (!AppShutdownTokenSource.IsCancellationRequested)
                    AppShutdownTokenSource.Cancel();
                AppShutdownTokenSource.Dispose();
            }
        }
    }

    static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                ConfigureDependencies(context.Configuration, services);
            });
    }

    /// <summary>
    /// Centralized method for configuring all dependency injection services.
    /// </summary>
    private static void ConfigureDependencies(IConfiguration configuration, IServiceCollection services)
    {
        // --- Configuration Models ---
        // Bind the SmtpSettings section from appsettings.json to the SmtpSettings class.
        // This makes it available via IOptions<SmtpSettings> to any service that needs it.
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Fatal: ConnectionString is not configured.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString),
            ServiceLifetime.Transient);

        services.AddSingleton<IUserContext, UserContext>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITaskService, TaskService>();
        services.AddTransient<IAdminDashboardService, AdminDashboardService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<ITaskHistoryService, TaskHistoryService>();

        services.AddTransient<LoginForm>();
        services.AddTransient<MainForm>();
        services.AddTransient<TaskDetailDialog>();
        services.AddTransient<UserManagementDialog>();
        services.AddTransient<AdminDashboardForm>();
        services.AddSingleton<LocalCredentialManager>();
        services.AddSingleton(AppShutdownTokenSource);
    }

    /// <summary>
    /// Handles all unhandled exceptions gracefully.
    /// </summary>
    public static void HandleException(Exception? ex, string source)
    {
        if (ex is null) return;
        try
        {
            var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt");
            var errorDetails =
                $"============================================================\r\n" +
                $"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n" +
                $"Source: {source}\r\n" +
                $"Exception Type: {ex.GetType().FullName}\r\n" +
                $"Message: {ex.Message}\r\n" +
                $"Stack Trace:\r\n{ex}\r\n\r\n";

            File.AppendAllText(logFilePath, errorDetails);
        }
        catch (Exception logEx)
        {
            MessageBox.Show(
                $"A critical error occurred, and the error could not be logged.\n\n" +
                $"Original Error: {ex.Message}\n\n" +
                $"Logging Error: {logEx.Message}",
                "Critical System Failure", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        MessageBox.Show($"應用程式發生未預期的錯誤，即將關閉。\n\n{ex.Message}。", "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

        try { 
            Application.Exit();
            ClearAllSqlitePools();
        }
        catch { 
            Environment.Exit(1); 
        }
    }
    private static bool TryAutoLogin(IServiceProvider services)
    {
        var credentialManager = services.GetRequiredService<LocalCredentialManager>();
        var credentials = credentialManager.TryLoadCredentials();

        if (credentials.HasValue)
        {
            var (username, token) = credentials.Value;
            var userService = services.GetRequiredService<IUserService>();
            var userContext = services.GetRequiredService<IUserContext>();

            var user = userService.AuthenticateByToken(username, token);
            if (user is not null)
            {
                userContext.SetCurrentUser(user);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Clears all connection pools for the Microsoft.Data.Sqlite provider.
    /// This is a robust way to handle "database is locked" errors after an application crash.
    /// </summary>
    private static void ClearAllSqlitePools()
    {
        try
        {
            SqliteConnection.ClearAllPools();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[INFO] Could not clear SQLite connection pools: {ex.Message}");
        }
    }
}