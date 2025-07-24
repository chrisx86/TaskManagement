using Microsoft.EntityFrameworkCore;
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
        if (user == null) return null;
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
}