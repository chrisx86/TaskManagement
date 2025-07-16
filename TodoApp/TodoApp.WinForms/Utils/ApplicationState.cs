// We need to use the User model from the Core project.
using TodoApp.Core.Models;

// The namespace should match the project and folder structure.
namespace TodoApp.WinForms.Utils;

/// <summary>
/// Provides a global, static context for the application.
/// This class holds application-wide state, such as the currently logged-in user.
/// </summary>
public static class ApplicationState
{
    /// <summary>
    /// Gets or sets the user who is currently logged into the application.
    /// This property is set by the login process and can be accessed from anywhere
    /// in the application to check user identity and permissions.
    /// It is nullable because before a user logs in, there is no current user.
    /// </summary>
    public static User? CurrentUser { get; set; }
}