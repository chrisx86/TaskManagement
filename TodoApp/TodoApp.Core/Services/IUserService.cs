using TodoApp.Core.Models;

namespace TodoApp.Core.Services;

/// <summary>
/// Defines the contract for user-related business logic services.
/// This interface will be implemented by a class in the Infrastructure layer.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Authenticates a user based on their username and password.
    /// </summary>
    /// <param name="username">The user's username.</param>
    /// <param name="password">The user's plain text password.</param>
    /// <returns>
    /// A Task that represents the asynchronous operation.
    /// The task result contains the User object if authentication is successful; otherwise, null.
    /// </returns>
    Task<User?> AuthenticateAsync(string username, string password);

    /// <summary>
    /// Retrieves a list of all users in the system.
    /// This is typically used for populating dropdowns (e.g., 'Assign To').
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation. 
    /// The task result contains a list of all User objects.</returns>
    Task<List<User>> GetAllUsersAsync();

    /// <summary>
    /// Creates a new user in the system. (Admin only)
    /// </summary>
    /// <param name="username">The username for the new user.</param>
    /// <param name="password">The plain text password for the new user.</param>
    /// <param name="role">The role assigned to the new user.</param>
    /// <returns>A Task that represents the asynchronous operation.
    /// The task result contains the newly created User object.</returns>
    Task<User> CreateUserAsync(string username, string password, UserRole role, string? email);

    /// <summary>
    /// Deletes a user from the system. (Admin only)
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task<bool> DeleteUserAsync(int userId);

    /// <summary>
    /// Updates an existing user's details (e.g., Role, Email).
    /// </summary>
    /// <param name="userToUpdate">The user object with updated values.</param>
    /// <returns>A Task that represents the asynchronous operation. 
    /// The task result contains true if the user was found and updated; otherwise, false.</returns>
    Task<bool> UpdateUserAsync(User userToUpdate);

    /// <summary>
    /// Resets a user's password. (Admin only)
    /// </summary>
    /// <param name="userId">The ID of the user whose password will be reset.</param>
    /// <param name="newPassword">The new plain text password.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task<bool> ResetPasswordAsync(int userId, string newPassword);

    /// <summary>
    /// Generates a new long-lived login token for a user, hashes it, and stores it in the database.
    /// </summary>
    /// <param name="userId">The ID of the user to generate a token for.</param>
    /// <returns>A Task that represents the asynchronous operation. 
    /// The task result contains the raw, unhashed login token string to be stored on the client.</returns>
    Task<string?> GenerateAndStoreLoginTokenAsync(int userId);

    /// <summary>
    /// Authenticates a user based on their username and a long-lived login token.
    /// </summary>
    /// <param name="username">The user's username.</param>
    /// <param name="token">The raw, unhashed login token provided by the client.</param>
    /// <returns>A Task that represents the asynchronous operation.
    /// The task result contains the User object if authentication is successful; otherwise, null.</returns>
    User? AuthenticateByToken(string username, string token);

    /// <summary>
    /// Invalidates any active long-lived login token for a specific user.
    /// This is a crucial security step for a logout or "switch user" feature.
    /// </summary>
    /// <param name="userId">The ID of the user to log out.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task LogoutAsync(int userId);
}