using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Security;

namespace TodoApp.Infrastructure.Services;

/// <summary>
/// Implements the IUserService interface, providing concrete business logic for user management.
/// </summary>
public class UserService : IUserService
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor for UserService. It receives the AppDbContext via dependency injection.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Authenticates a user based on their username and password.
    /// </summary>
    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => EF.Functions.Like(u.Username, username));
        if (user is null) return null;
        if (!PasswordHasher.VerifyPassword(password, user.HashedPassword)) return null;
        return user;
    }

    /// <summary>
    /// Retrieves a list of all users in the system.
    /// </summary>
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    public async Task<User> CreateUserAsync(string username, string password, UserRole role, string? email)
    {
        if (await _context.Users.AnyAsync(u => EF.Functions.Like(u.Username, username)))
        {
            throw new InvalidOperationException($"Username '{username}' already exists.");
        }

        var newUser = new User
        {
            Username = username,
            HashedPassword = PasswordHasher.HashPassword(password),
            Role = role,
            Email = email
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }

    public async Task<bool> UpdateUserAsync(User userToUpdate)
    {
        var trackedUser = await _context.Users.FindAsync(userToUpdate.Id);
        if (trackedUser is null) return false;

        trackedUser.Username = userToUpdate.Username;
        trackedUser.Role = userToUpdate.Role;
        trackedUser.Email = userToUpdate.Email;

        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Deletes a user from the system.
    /// </summary>
    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user is null) return false;
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Resets a user's password.
    /// </summary>
    public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user is not null)
        {
            user.HashedPassword = PasswordHasher.HashPassword(newPassword);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public async Task<string?> GenerateAndStoreLoginTokenAsync(int userId)
    {
        // This query must be tracked to allow updates.
        var user = await _context.Users.FindAsync(userId);
        if (user is null) return null;

        // 1. Generate a cryptographically secure, random token.
        var rawTokenBytes = RandomNumberGenerator.GetBytes(64);
        var rawToken = Convert.ToBase64String(rawTokenBytes);

        // 2. Hash the token for secure storage in the database.
        user.LoginTokenHash = PasswordHasher.HashPassword(rawToken);

        // 3. Set the expiration date (e.g., 30 days from now).
        user.LoginTokenExpiryDate = DateTime.Now.AddDays(30);

        await _context.SaveChangesAsync();

        // 4. Return the raw, unhashed token to the client. The client never sees the hash.
        return rawToken;
    }

    /// <summary>
    /// Authenticates a user based on their username and a long-lived login token,
    /// and slides the token's expiration date upon success.
    /// </summary>
    /// <param name="username">The user's username.</param>
    /// <param name="token">The raw, unhashed login token provided by the client.</param>
    /// <returns>A Task that represents the asynchronous operation.
    /// The task result contains the User object if authentication is successful; otherwise, null.</returns>
    public async Task<User?> AuthenticateByTokenAsync(string username, string token)
    {
        // --- Step 1: Find the user. ---
        // IMPORTANT: This query MUST be tracked to allow for updates. Do NOT use AsNoTracking().
        var user = await _context.Users
            .FirstOrDefaultAsync(u => EF.Functions.Like(u.Username, username));

        // --- Step 2: Perform all validation checks in a single block. ---
        if (user == null ||
            string.IsNullOrEmpty(user.LoginTokenHash) ||
            user.LoginTokenExpiryDate < DateTime.Now ||
            !PasswordHasher.VerifyPassword(token, user.LoginTokenHash))
        {
            // If any check fails (user not found, no token, token expired, or token mismatch),
            // authentication fails.
            return null;
        }

        // --- Step 3: SLIDING EXPIRATION LOGIC ---
        // If all checks passed, authentication is successful.
        // We now "slide" the expiration date forward.
        user.LoginTokenExpiryDate = DateTime.Now.AddDays(30);

        // Also update the last login time for consistency.
        user.LoginTime = DateTime.Now;

        try
        {
            // Save the updated expiration date and login time to the database.
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // If updating the expiry date fails, we should log it for diagnostics.
            // However, we should NOT prevent the user from logging in, as the primary
            // authentication was successful.
            System.Diagnostics.Debug.WriteLine($"[WARNING] Failed to slide token expiration for user '{username}': {ex.Message}");
        }
        return user;
    }
}