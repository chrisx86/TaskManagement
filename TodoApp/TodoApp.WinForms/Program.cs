#nullable enable
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoApp.Core.Services;
using TodoApp.WinForms.Forms;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Services;

namespace TodoApp.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        // Use the modern hosting model which handles application lifecycle gracefully.
        var host = CreateHostBuilder().Build();

        // The service provider should be retrieved from the host.
        var services = host.Services;

        // --- Global Exception Handling Setup ---
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (sender, args) => HandleException(args.Exception, "UI Thread");
        AppDomain.CurrentDomain.UnhandledException += (sender, args) => HandleException(args.ExceptionObject as Exception, "Non-UI Thread");

        ApplicationConfiguration.Initialize();

        // --- Clean Application Startup Flow ---
        // We use a using statement to ensure the scope is properly disposed.
        using (var serviceScope = services.CreateScope())
        {
            var scopedServices = serviceScope.ServiceProvider;
            try
            {
                // Resolve the login form from the scope.
                var loginForm = scopedServices.GetRequiredService<LoginForm>();

                // Show the login form. If login is successful...
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // ...the main form is resolved and run.
                    // The IUserContext already holds the user info, no need for ApplicationState.
                    Application.Run(scopedServices.GetRequiredService<MainForm>());
                }
            }
            catch (Exception ex)
            {
                // This catch block handles critical DI or startup failures.
                HandleException(ex, "Application Startup");
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
                // --- Configure all dependencies here ---
                ConfigureDependencies(context.Configuration, services);
            });
    }

    // --- NEW: Centralized dependency configuration method ---
    private static void ConfigureDependencies(IConfiguration configuration, IServiceCollection services)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Fatal: ConnectionString is not configured.");
        }

        // DbContext
        services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString),
                ServiceLifetime.Transient);
        // Core Services
        services.AddSingleton<IUserContext, UserContext>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITaskService, TaskService>();
        services.AddTransient<IAdminDashboardService, AdminDashboardService>();

        // Forms (all forms that are resolved via DI)
        services.AddTransient<LoginForm>();
        services.AddTransient<MainForm>();
        services.AddTransient<TaskDetailDialog>();
        services.AddTransient<UserManagementDialog>();
        services.AddTransient<AdminDashboardForm>();
    }

    // --- MODIFIED: HandleException should try to close gracefully first ---
    private static void HandleException(Exception? ex, string source)
    {
        if (ex == null) return;

        // Log the error (your existing logic is fine)
        // ... File.AppendAllText(...) ...

        const string friendlyMessage = "應用程式發生未預期的錯誤，即將關閉。\n\n" +
                                       "詳細資訊已記錄供開發團隊參考。";
        MessageBox.Show(friendlyMessage, "系統錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // --- Try to exit gracefully first ---
        // This gives the application a chance to clean up resources.
        try
        {
            Application.Exit();
        }
        catch
        {
            // If graceful exit fails, use the hammer.
            Environment.Exit(1);
        }
    }
}