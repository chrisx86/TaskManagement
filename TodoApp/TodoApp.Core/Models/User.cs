using System.ComponentModel.DataAnnotations;

namespace TodoApp.Core.Models;

/// <summary>
/// Represents a user account in the system.
/// </summary>
public class User
{
    /// <summary>
    /// The unique identifier for the user (Primary Key).
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The unique username for login. This will be indexed in the database.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The hashed and salted password. 
    /// IMPORTANT: Never store plain text passwords.
    /// </summary>
    [Required]
    public string HashedPassword { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Email { get; set; }

    /// <summary>
    /// The role of the user, determining their permissions.
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// The timestamp of the user's last successful login.
    /// </summary>
    public DateTime LoginTime { get; set; }

    /// <summary>
    /// Stores the hashed version of the long-lived login token.
    /// This is nullable because a user may not have an active "Remember Me" session.
    /// </summary>
    public string? LoginTokenHash { get; set; }

    /// <summary>
    /// Stores the expiration date and time for the current login token.
    /// This is nullable for the same reason as the token hash.
    /// </summary>
    public DateTime? LoginTokenExpiryDate { get; set; }

    // --- Navigation properties (assuming they exist, otherwise can be added) ---
    public virtual ICollection<TodoItem> CreatedItems { get; set; } = new List<TodoItem>();
    public virtual ICollection<TodoItem> AssignedItems { get; set; } = new List<TodoItem>();
}