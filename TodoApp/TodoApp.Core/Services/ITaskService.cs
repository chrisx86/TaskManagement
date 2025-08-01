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
        User currentUser,
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int? assignedToUserIdFilter,
        int pageNumber,
        int pageSize,
        string? sortColumn,
        bool isAscending,
        string? searchKeyword
    );

    Task<int> GetTaskCountAsync(
        User currentUser,
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int? assignedToUserIdFilter,
        string? searchKeyword
    );
    Task<TodoItem> CreateTaskAsync(User currentUser, string title, string? comments, TodoStatus status, PriorityLevel priority, DateTime? dueDate, int? assignedToId);

    Task<TodoItem> UpdateTaskAsync(User currentUser, TodoItem taskToUpdate);
    /// <summary>
    /// Updates only the 'Comments' field of a specific task.
    /// </summary>
    /// <param name="currentUser">The user performing the action.</param>
    /// <param name="taskId">The ID of the task to update.</param>
    /// <param name="newComments">The new comments text.</param>
    /// <returns>The updated TodoItem with the new LastModifiedDate.</returns>
    Task<TodoItem> UpdateTaskCommentsAsync(User currentUser, int taskId, string newComments);
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