#nullable enable
using Moq;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Tests.Services;

/// <summary>
/// Contains unit tests for the TaskService class.
/// </summary>
[TestFixture]
public class TaskServiceTests
{
    // --- System Under Test (SUT) and its dependencies ---
    private AppDbContext _context;
    private ITaskService _taskService;

    // --- Mocks for external dependencies ---
    private Mock<IEmailService> _mockEmailService;
    private Mock<IUserService> _mockUserService;

    // --- Test Data ---
    private User _adminUser;
    private User _regularUser;

    [SetUp]
    public void Setup()
    {
        // 1. Create a fresh in-memory database for each test.
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        // 2. Create mock instances for the services TaskService depends on.
        _mockEmailService = new Mock<IEmailService>();
        _mockUserService = new Mock<IUserService>();

        // 3. Instantiate the TaskService with the real DbContext and mocked services.
        _taskService = new TaskService(_context, _mockEmailService.Object, _mockUserService.Object);

        // 4. Seed initial user data for tests.
        _adminUser = new User { Id = 1, Username = "admin", Role = UserRole.Admin, Email = "admin@example.com" };
        _regularUser = new User { Id = 2, Username = "user", Role = UserRole.User, Email = "user@example.com" };
        _context.Users.AddRange(_adminUser, _regularUser);
        _context.SaveChanges();
    }

    [TearDown]
    public void Teardown()
    {
        _context.Dispose();
    }

    #region CreateTaskAsync Tests

    [Test]
    public async Task CreateTaskAsync_WhenCalled_CreatesTaskInDatabase()
    {
        var createdTask = await _taskService.CreateTaskAsync(
            _adminUser, "New Test Task", "Some comments", PriorityLevel.High, DateTime.Now, _regularUser.Id
        );

        var taskInDb = await _context.TodoItems.FindAsync(createdTask.Id);
        Assert.Multiple(() =>
        {
            Assert.That(taskInDb, Is.Not.Null);
            Assert.That(taskInDb.Title, Is.EqualTo("New Test Task"));
            Assert.That(taskInDb.CreatorId, Is.EqualTo(_adminUser.Id));
        });
    }

    [Test]
    public async Task CreateTaskAsync_WhenCreatedByRegularUser_NotifiesAllAdmins()
    {
        // Setup the mock UserService to return a list containing one admin when called.
        var admins = new List<User> { _adminUser };
        _mockUserService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(admins);

        await _taskService.CreateTaskAsync(
            _regularUser, "User Task", null, PriorityLevel.Low, null, null
        );

        // Verify that SendEmailAsync was called exactly once, with the admin's email.
        _mockEmailService.Verify(
            s => s.SendEmailAsync(
                _adminUser.Email,
                It.IsAny<string>(),
                It.IsAny<string>()
            ),
            Times.Once
        );
    }

    [Test]
    public async Task CreateTaskAsync_WhenCreatedByAdmin_NotifiesAssignee()
    {
        await _taskService.CreateTaskAsync(
            _adminUser, "Admin Task", null, PriorityLevel.Medium, null, _regularUser.Id
        );

        // Verify that SendEmailAsync was called exactly once, with the assignee's (regularUser) email.
        _mockEmailService.Verify(
            s => s.SendEmailAsync(
                _regularUser.Email,
                It.IsAny<string>(),
                It.IsAny<string>()
            ),
            Times.Once
        );
    }

    #endregion

    #region UpdateTaskAsync Tests

    [Test]
    public async Task UpdateTaskAsync_WhenConcurrencyTokenMismatches_ThrowsDbUpdateConcurrencyException()
    {
        var initialTask = new TodoItem
        {
            Title = "Initial Version",
            CreatorId = _adminUser.Id,
            LastModifiedDate = DateTime.Now
        };
        _context.TodoItems.Add(initialTask);
        await _context.SaveChangesAsync();

        // 2. Capture the original timestamp BEFORE it's updated in the database.
        //    This represents the state of the data when the first user started editing.
        var staleTimestamp = initialTask.LastModifiedDate;

        // 3. Simulate another user updating the task in the database.
        //    We fetch it again to ensure we're working with the tracked entity.
        var taskInDb = await _context.TodoItems.FindAsync(initialTask.Id);
        Assert.That(taskInDb, Is.Not.Null, "Task should exist in DB.");

        taskInDb!.LastModifiedDate = DateTime.Now.AddSeconds(5); // Update to a new timestamp
        await _context.SaveChangesAsync();

        // 4. Create a "stale" object that represents the data from the first user's perspective.
        //    It has the old, stale timestamp.
        var staleTaskFromUI = new TodoItem
        {
            Id = initialTask.Id,
            Title = "An Attempted Stale Update",
            LastModifiedDate = staleTimestamp // Use the captured old timestamp
        };

        // Verify that calling Update with the stale object throws the correct exception.
        Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            await _taskService.UpdateTaskAsync(_adminUser, staleTaskFromUI)
        );
    }

    #endregion

    #region DeleteTaskAsync Tests

    [Test]
    public async Task DeleteTaskAsync_ByUserWithoutPermission_ThrowsUnauthorizedAccessException()
    {
        var task = new TodoItem { Title = "Admin's Task", CreatorId = _adminUser.Id };
        _context.TodoItems.Add(task);
        await _context.SaveChangesAsync();

        // A regular user tries to delete it.
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await _taskService.DeleteTaskAsync(_regularUser, task.Id)
        );
    }

    #endregion

    #region Pagination Tests

    [Test]
    public async Task GetAllTasksAsync_WithPagination_ReturnsCorrectPageAndCount()
    {
        // Create 25 tasks
        for (int i = 1; i <= 25; i++)
        {
            _context.TodoItems.Add(new TodoItem { Title = $"Task {i}", CreatorId = _adminUser.Id });
        }
        await _context.SaveChangesAsync();

        // Act: Request the second page, with a page size of 10.
        var pagedTasks = await _taskService.GetAllTasksAsync(null, UserTaskFilter.All, _adminUser.Id, null, 2, 10);
        var totalCount = await _taskService.GetTaskCountAsync(null, UserTaskFilter.All, _adminUser.Id, null);

        Assert.Multiple(() =>
        {
            Assert.That(totalCount, Is.EqualTo(25));
            Assert.That(pagedTasks.Count, Is.EqualTo(10));
            // Assuming a stable order by Id, the first item on the second page should be "Task 11".
            Assert.That(pagedTasks.First().Title, Is.EqualTo("Task 11"));
        });
    }

    #endregion
}