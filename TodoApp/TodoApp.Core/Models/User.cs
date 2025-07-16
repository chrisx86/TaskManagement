// The namespace should match the project and folder structure.
namespace TodoApp.Core.Models;

/// <summary>
/// Represents a user account in the system.
/// This is an entity that will be mapped to the 'Users' table in the database.
/// </summary>
public class User
{
    /// <summary>
    /// The unique identifier for the user (Primary Key).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The unique username for login. This will be indexed in the database.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The hashed and salted password. 
    /// IMPORTANT: Never store plain text passwords.
    /// </summary>
    public string HashedPassword { get; set; } = string.Empty;

    /// <summary>
    /// The role of the user, determining their permissions.
    /// </summary>
    public UserRole Role { get; set; }
}