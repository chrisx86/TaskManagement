// We need access to DbContext, models, interfaces, and security tools.
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Security;

// The namespace should match the project and folder structure.
namespace TodoApp.Infrastructure.Services;

/// <summary>
/// Implements the IUserService interface, providing concrete business logic for user management.
/// </summary>
public class UserService : IUserService
{
    // A private field to hold the database context, injected via the constructor.
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
        // 1. Find the user by username. The search is case-insensitive for better usability.
        var user = await _context.Users
            .FirstOrDefaultAsync(u => EF.Functions.Like(u.Username, username));
        // 2. If user is not found, authentication fails.
        if (user == null)
        {
            return null;
        }

        // 3. Verify the provided password against the stored hash.
        if (!PasswordHasher.VerifyPassword(password, user.HashedPassword))
        {
            // Password does not match.
            return null;
        }

        // 4. Authentication successful. Return the user object.
        return user;
    }

    /// <summary>
    /// Retrieves a list of all users in the system.
    /// </summary>
    public async Task<List<User>> GetAllUsersAsync()
    {
        // Simply query the Users table and return all entries as a list.
        // AsNoTracking() is a performance optimization for read-only queries.
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
            Email = email // Assign the email
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }

    public async Task<bool> UpdateUserAsync(User userToUpdate)
    {
        var trackedUser = await _context.Users.FindAsync(userToUpdate.Id);
        if (trackedUser == null) return false;

        // We only allow updating Role and Email in this method.
        // Username and Password changes are handled by other methods.
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
        if (user == null)
        {
            // If user is not found, the operation did not succeed.
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        // If we reach here, the operation was successful.
        return true;
    }

    /// <summary>
    /// Resets a user's password.
    /// </summary>
    public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
    {
        // 1. Find the user.
        var user = await _context.Users.FindAsync(userId);

        // 2. If user exists, update their password.
        if (user != null)
        {
            // 3. Hash the new password.
            user.HashedPassword = PasswordHasher.HashPassword(newPassword);

            // 4. Mark the user entity as modified and save changes.
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
        // If user is not found, an exception could be thrown, but for now we do nothing.
    }
}