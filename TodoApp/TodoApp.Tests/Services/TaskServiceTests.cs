#nullable enable
using Moq;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.ComponentModel;

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
    private Mock<ITaskHistoryService> _mockHistoryService;

    // --- Test Data ---
    private User _adminUser;
    private User _regularUser;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        _mockEmailService = new Mock<IEmailService>();
        _mockUserService = new Mock<IUserService>();
        _mockHistoryService = new Mock<ITaskHistoryService>();

        _taskService = new TaskService(_context, _mockEmailService.Object, _mockUserService.Object, _mockHistoryService.Object);

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
        var admins = new List<User> { _adminUser };
        _mockUserService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(admins);

        await _taskService.CreateTaskAsync(
            _regularUser, "User Task", null, PriorityLevel.Low, null, null
        );

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

        var staleTimestamp = initialTask.LastModifiedDate;

        var taskInDb = await _context.TodoItems.FindAsync(initialTask.Id);
        Assert.That(taskInDb, Is.Not.Null, "Task should exist in DB.");

        taskInDb!.LastModifiedDate = DateTime.Now.AddSeconds(5);
        await _context.SaveChangesAsync();

        var staleTaskFromUI = new TodoItem
        {
            Id = initialTask.Id,
            Title = "An Attempted Stale Update",
            LastModifiedDate = staleTimestamp
        };

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

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await _taskService.DeleteTaskAsync(_regularUser, task.Id)
        );
    }

    #endregion

    #region Pagination Tests

    [Test]
    public async Task GetAllTasksAsync_WithPagination_ReturnsCorrectPageAndCount()
    {
        for (int i = 1; i <= 25; i++)
        {
            _context.TodoItems.Add(new TodoItem { Title = $"Task {i}", CreatorId = _adminUser.Id, AssignedTo = _adminUser });
        }
        await _context.SaveChangesAsync();

        var pagedTasks = await _taskService.GetAllTasksAsync(_adminUser, null, UserTaskFilter.All, null, 2, 10, null, true, null);
        var totalCount = await _taskService.GetTaskCountAsync(_adminUser, null, UserTaskFilter.All, _adminUser.Id, null);

        Assert.Multiple(() =>
        {
            Assert.That(totalCount, Is.EqualTo(25));
            Assert.That(pagedTasks.Count, Is.EqualTo(10));
            Assert.That(pagedTasks.First().Title, Is.EqualTo("Task 11"));
        });
    }

    #endregion
}