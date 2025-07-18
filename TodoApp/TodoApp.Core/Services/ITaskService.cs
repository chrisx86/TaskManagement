#nullable enable
using TodoApp.Core.Models;

namespace TodoApp.Core.Services;

public enum TaskStatusFilter { All, Completed, Pending, InProgress, Reject }

public enum UserTaskFilter { All, AssignedToMe, CreatedByMe }
/// <summary>
/// Defines the contract for to-do item related business logic services.
/// This interface will be implemented by a class in the Infrastructure layer.
/// </summary>
public interface ITaskService
{
    Task<List<TodoItem>> GetAllTasksAsync(
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int currentUserId,
        int? assignedToUserIdFilter,
        int pageNumber,
        int pageSize
    );

    Task<int> GetTaskCountAsync(
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int currentUserId,
        int? assignedToUserIdFilter
    );
    Task<TodoItem> CreateTaskAsync(User currentUser, string title, string? comments, PriorityLevel priority, DateTime? dueDate, int? assignedToId);

    Task<TodoItem> UpdateTaskAsync(User currentUser, TodoItem taskToUpdate);

    Task DeleteTaskAsync(User currentUser, int taskId);

    /// <summary>
    /// Gets all tasks grouped by user for the admin dashboard.
    /// </summary>
    /// <returns>
    /// A Task that represents the asynchronous operation.
    /// The task result contains a dictionary where the key is the User and the value is a list of their TodoItems.
    /// </returns>
    Task<Dictionary<User, List<TodoItem>>> GetTasksGroupedByUserAsync();

    Task<TodoItem?> GetTaskByIdAsync(int taskId);
}