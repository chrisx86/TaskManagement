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
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

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
    public async Task<User> CreateUserAsync(string username, string password, UserRole role)
    {
        // 1. Check if a user with the same username already exists.
        if (await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
        {
            // Throw an exception if the username is taken. The UI layer will have to catch this.
            throw new InvalidOperationException($"Username '{username}' already exists.");
        }

        // 2. Hash the new user's password.
        var hashedPassword = PasswordHasher.HashPassword(password);

        // 3. Create a new User entity.
        var newUser = new User
        {
            Username = username,
            HashedPassword = hashedPassword,
            Role = role
        };

        // 4. Add the new user to the context and save changes to the database.
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        // 5. Return the newly created user object (which now has an Id).
        return newUser;
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