#nullable enable
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Security;
using TodoApp.Infrastructure.Services;

namespace TodoApp.Infrastructure.Tests.Services;

/// <summary>
/// Contains unit tests for the UserService class.
/// </summary>
[TestFixture]
public class UserServiceTests
{
    private AppDbContext _context;
    private IUserService _userService;

    [SetUp]
    public void Setup()
    {
        // --- Arrange: Create a fresh in-memory database for each test ---
        // Using a unique database name for each test run ensures complete isolation.
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _userService = new UserService(_context);
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up the context after each test.
        _context.Dispose();
    }

    #region AuthenticateAsync Tests

    [Test]
    public async Task AuthenticateAsync_WithValidCredentials_ReturnsUser()
    {
        var username = "testuser";
        var password = "password123";
        _context.Users.Add(new User { Username = username, HashedPassword = PasswordHasher.HashPassword(password), Role = UserRole.User });
        await _context.SaveChangesAsync();

        var result = await _userService.AuthenticateAsync(username, password);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo(username));
        });
    }

    [Test]
    public async Task AuthenticateAsync_WithInvalidPassword_ReturnsNull()
    {
        var username = "testuser";
        _context.Users.Add(new User { Username = username, HashedPassword = PasswordHasher.HashPassword("password123"), Role = UserRole.User });
        await _context.SaveChangesAsync();

        var result = await _userService.AuthenticateAsync(username, "wrongpassword");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AuthenticateAsync_WithNonExistentUser_ReturnsNull()
    {
        var result = await _userService.AuthenticateAsync("nonexistent", "password");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AuthenticateAsync_IsCaseInsensitive_ReturnsUser()
    {
        var username = "TestUser";
        var password = "password123";
        _context.Users.Add(new User { Username = username, HashedPassword = PasswordHasher.HashPassword(password), Role = UserRole.User });
        await _context.SaveChangesAsync();

        var result = await _userService.AuthenticateAsync("testuser", password); // Use lowercase for testing

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo(username));
        });
    }

    #endregion

    #region CreateUserAsync Tests

    [Test]
    public async Task CreateUserAsync_WithValidData_CreatesUserSuccessfully()
    {
        var username = "newuser";
        var password = "newpassword";
        var email = "new@example.com";

        var result = await _userService.CreateUserAsync(username, password, UserRole.User, email);
        var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo(username));
            Assert.That(userInDb, Is.Not.Null);
            Assert.That(userInDb.Email, Is.EqualTo(email));
            Assert.That(_context.Users.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public async Task CreateUserAsync_WithExistingUsername_ThrowsInvalidOperationException()
    {
        var existingUsername = "existinguser";
        _context.Users.Add(new User { Username = existingUsername, HashedPassword = "hash", Role = UserRole.User });
        await _context.SaveChangesAsync();

        // Use Assert.ThrowsAsync to verify that the correct exception is thrown.
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _userService.CreateUserAsync(existingUsername, "password", UserRole.User, null)
        );
    }

    #endregion

    #region DeleteUserAsync & ResetPasswordAsync Tests

    [Test]
    public async Task DeleteUserAsync_WithExistingUser_ReturnsTrueAndRemovesUser()
    {
        var userToDelete = new User { Id = 1, Username = "todelete", HashedPassword = "hash" };
        _context.Users.Add(userToDelete);
        await _context.SaveChangesAsync();

        var result = await _userService.DeleteUserAsync(userToDelete.Id);
        var userInDb = await _context.Users.FindAsync(userToDelete.Id);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(userInDb, Is.Null);
        });
    }

    [Test]
    public async Task DeleteUserAsync_WithNonExistentUser_ReturnsFalse()
    {
        var result = await _userService.DeleteUserAsync(999);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task ResetPasswordAsync_WithExistingUser_UpdatesPasswordSuccessfully()
    {
        var username = "user_to_reset";
        var oldPassword = "old_password";
        var newPassword = "new_secure_password";
        var user = new User { Username = username, HashedPassword = PasswordHasher.HashPassword(oldPassword) };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _userService.ResetPasswordAsync(user.Id, newPassword);

        Assert.That(result, Is.True);

        // Verify by trying to authenticate with the new password.
        var authenticatedUser = await _userService.AuthenticateAsync(username, newPassword);
        Assert.That(authenticatedUser, Is.Not.Null);

        // Verify that authentication with the old password fails.
        var failedAuthUser = await _userService.AuthenticateAsync(username, oldPassword);
        Assert.That(failedAuthUser, Is.Null);
    }

    #endregion
}