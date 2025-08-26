#nullable enable
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Services;

namespace TodoApp.Tests.NUnit;

/// <summary>
/// Contains unit tests for the TaskService class using the NUnit framework.
/// </summary>
[TestFixture]
public class TaskServiceTests
{
    // These fields will be re-initialized for each test by the [SetUp] method.
    private AppDbContext _context = null!;
    private ITaskService _taskService = null!;
    private Mock<IEmailService> _mockEmailService = null!;
    private Mock<IUserService> _mockUserService = null!;
    private Mock<ITaskHistoryService> _mockHistoryService = null!;

    private User _adminUser = null!;
    private User _normalUser1 = null!;
    private User _normalUser2 = null!;

    /// <summary>
    /// NUnit's equivalent of [TestInitialize]. This method is run before each test.
    /// It sets up a fresh in-memory database and service instances for each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        // 1. Setup In-Memory Database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        // 2. Seed Data
        _adminUser = new User { Id = 1, Username = "admin", Role = UserRole.Admin };
        _normalUser1 = new User { Id = 2, Username = "user1", Role = UserRole.User };
        _normalUser2 = new User { Id = 3, Username = "user2", Role = UserRole.User };

        _context.Users.AddRange(_adminUser, _normalUser1, _normalUser2);

        _context.TodoItems.AddRange(new List<TodoItem>
        {
            new() { Id = 1, Title = "User1's own task", CreatorId = 2, AssignedToId = 2, Status = TodoStatus.Pending },
            new() { Id = 2, Title = "Task for User2", CreatorId = 2, AssignedToId = 3, Status = TodoStatus.InProgress },
            new() { Id = 3, Title = "Admin task for User1", CreatorId = 1, AssignedToId = 2, Status = TodoStatus.Completed, Priority = PriorityLevel.High },
            new() { Id = 4, Title = "Unassigned urgent task", CreatorId = 1, AssignedToId = null, Status = TodoStatus.Pending, Priority = PriorityLevel.Urgent }
        });
        _context.SaveChanges();

        // 3. Setup Mocks and Service Instance
        _mockEmailService = new Mock<IEmailService>();
        _mockUserService = new Mock<IUserService>();
        _mockHistoryService = new Mock<ITaskHistoryService>();

        _taskService = new TaskService(
            _context,
            _mockEmailService.Object,
            _mockUserService.Object,
            _mockHistoryService.Object,
            new CancellationTokenSource()
        );
    }

    /// <summary>
    /// NUnit's equivalent of [TestCleanup]. This method is run after each test.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetAllTasksAsync_AdminWithNoFilters_ReturnsAllTasks()
    {
        // Arrange

        // Act
        var tasks = await _taskService.GetAllTasksAsync(_adminUser, null, UserTaskFilter.All, null, 1, 10, null, true, null);

        // Assert
        Assert.That(tasks, Is.Not.Null);
        Assert.That(tasks.Count, Is.EqualTo(4));
    }

    [Test]
    public async Task GetAllTasksAsync_NormalUserWithNoFilters_ReturnsOnlyRelatedTasks()
    {
        // Arrange

        // Act
        var tasks = await _taskService.GetAllTasksAsync(_normalUser1, null, UserTaskFilter.All, null, 1, 10, null, true, null);

        // Assert
        Assert.That(tasks, Is.Not.Null);
        Assert.That(tasks.Count, Is.EqualTo(3));
        Assert.That(tasks.All(t => t.CreatorId == _normalUser1.Id || t.AssignedToId == _normalUser1.Id), Is.True);
        Assert.That(tasks.Any(t => t.Id == 4), Is.False, "Should not see the unassigned admin task.");
    }

    [Test]
    public async Task GetAllTasksAsync_AdminFiltersByAssignedUser_ReturnsCorrectTasks()
    {
        // Arrange

        // Act
        var tasks = await _taskService.GetAllTasksAsync(_adminUser, null, UserTaskFilter.All, _normalUser2.Id, 1, 10, null, true, null);

        // Assert
        Assert.That(tasks, Is.Not.Null);
        Assert.That(tasks.Count, Is.EqualTo(1));
        Assert.That(tasks.First().Id, Is.EqualTo(2));
    }

    [Test]
    public async Task GetTaskCountAsync_WithSearchKeyword_ReturnsCorrectCount()
    {
        // Arrange

        // Act
        var count = await _taskService.GetTaskCountAsync(_adminUser, null, UserTaskFilter.All, null, "User1");

        // Assert
        Assert.That(count, Is.EqualTo(2));
    }

    [Test]
    public async Task CreateTaskAsync_ValidData_CreatesTaskAndLogsHistory()
    {
        // Arrange
        var title = "New test task";
        var currentUser = _normalUser1;
        var assignedUser = _normalUser2;

        // We must provide all the required arguments in the correct order as defined
        // by the ITaskService.CreateTaskAsync method signature.
        var createdTask = await _taskService.CreateTaskAsync(
            currentUser,                   // User currentUser
            title,                         // string title
            "Test comments",               // string? comments
            TodoStatus.Pending,            // TodoStatus status
            PriorityLevel.High,            // PriorityLevel priority
            DateTime.Now.AddDays(7),       // DateTime? dueDate
            assignedUser.Id                // int? assignedToId
        );

        // Assert
        Assert.That(createdTask, Is.Not.Null);
        Assert.That(createdTask.Title, Is.EqualTo(title));
        Assert.That(createdTask.CreatorId, Is.EqualTo(currentUser.Id));
        Assert.That(createdTask.AssignedToId, Is.EqualTo(assignedUser.Id));
        Assert.That(createdTask.Status, Is.EqualTo(TodoStatus.Pending));
        Assert.That(createdTask.Priority, Is.EqualTo(PriorityLevel.High));

        var taskInDb = await _context.TodoItems.FindAsync(createdTask.Id);
        Assert.That(taskInDb, Is.Not.Null);

        _mockHistoryService.Verify(h => h.LogHistoryAsync(
            It.IsAny<int>(),
            currentUser.Id,
            "Create",
            It.IsAny<string>()),
            Times.Once);
    }

    [Test]
    public async Task DeleteTaskAsync_AdminDeletesAnotherUsersTask_Succeeds()
    {
        // Arrange
        var taskIdToDelete = 1;

        // Act
        await _taskService.DeleteTaskAsync(_adminUser, taskIdToDelete);

        // Assert
        var taskInDb = await _context.TodoItems.FindAsync(taskIdToDelete);
        Assert.That(taskInDb, Is.Null);
    }

    [Test]
    public void DeleteTaskAsync_UserDeletesUnrelatedTask_ThrowsException()
    {
        // Arrange
        var taskIdToDelete = 1;

        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
        {
            await _taskService.DeleteTaskAsync(_normalUser2, taskIdToDelete);
        });
    }

    [Test]
    public async Task UpdateTaskAsync_UserUpdatesOwnTask_SucceedsAndLogsHistory()
    {
        // Arrange
        var originalTaskId = 1; // user1's own task

        // --- THIS IS THE KEY FIX ---
        // 1. Fetch the original state of the task from the database.
        //    Do not modify this object. It represents the state before the user's edit.
        var originalTask = await _context.TodoItems.AsNoTracking().FirstOrDefaultAsync(t => t.Id == originalTaskId);
        Assert.That(originalTask, Is.Not.Null, "Precondition failed: Original task not found.");

        // 2. Create a NEW object that represents the data coming from the UI.
        //    This simulates the user changing values in a dialog and clicking "Save".
        var taskFromUI = new TodoItem
        {
            Id = originalTask.Id,
            Title = "Updated Title", // New value
            Status = TodoStatus.InProgress, // New value
            Priority = originalTask.Priority, // Unchanged value
            Comments = originalTask.Comments, // Unchanged value
            CreatorId = originalTask.CreatorId, // Unchanged value
            AssignedToId = originalTask.AssignedToId, // Unchanged value
            DueDate = originalTask.DueDate, // Unchanged value
            CreationDate = originalTask.CreationDate, // Unchanged value
            LastModifiedDate = originalTask.LastModifiedDate // This will be ignored by SetValues
        };

        // Act
        // Pass the new UI object to the service.
        var updatedTask = await _taskService.UpdateTaskAsync(_normalUser1, taskFromUI);

        // Assert
        Assert.That(updatedTask, Is.Not.Null);
        Assert.That(updatedTask.Title, Is.EqualTo("Updated Title"));
        Assert.That(updatedTask.Status, Is.EqualTo(TodoStatus.InProgress));

        // Now, the verification should pass because a change was detected.
        _mockHistoryService.Verify(h => h.LogHistoryAsync(
            taskFromUI.Id,
            _normalUser1.Id,
            "Update",
            It.Is<string>(s => s.Contains("狀態") && s.Contains("標題"))), // Check for Chinese keywords
            Times.Once);
    }
}