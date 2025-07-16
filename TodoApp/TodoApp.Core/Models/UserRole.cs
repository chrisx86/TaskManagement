// The namespace should match the project and folder structure.
namespace TodoApp.Core.Models;

/// <summary>
/// Defines the roles a user can have within the system,
/// which will determine their permissions.
/// </summary>
public enum UserRole
{
    // Standard user with limited permissions.
    User,

    // Administrator with full permissions.
    Admin
}