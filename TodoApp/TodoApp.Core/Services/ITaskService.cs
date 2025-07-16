#nullable enable
using TodoApp.Core.Models;

namespace TodoApp.Core.Services;
// --- NEW ENUM FOR FILTERING ---
// Defines the filtering options for task status.
public enum TaskStatusFilter { All, Completed, Pending, InProgress }

// --- NEW ENUM FOR FILTERING ---
// Defines the filtering options related to the current user.
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
    /// <summary>
    /// Creates a new to-do item.
    /// </summary>
    /// <param name="title">The title of the new task.</param>
    /// <param name="creatorId">The ID of the user creating the task.</param>
    /// <param name="priority">The priority of the task.</param>
    /// <param name="dueDate">The optional due date of the task.</param>
    /// <param name="assignedToId">The optional ID of the user the task is assigned to.</param>
    /// <returns>A Task that represents the asynchronous operation.
    /// The task result contains the newly created TodoItem object.</returns>
    Task<TodoItem> CreateTaskAsync(string title, string? comments, int creatorId, PriorityLevel priority, DateTime? dueDate, int? assignedToId);

    /// <summary>
    /// Updates an existing to-do item.
    /// The implementation must handle concurrency conflicts.
    /// </summary>
    /// <param name="taskToUpdate">The to-do item object with updated values.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task UpdateTaskAsync(TodoItem taskToUpdate);

    /// <summary>
    /// Deletes a to-do item.
    /// </summary>
    /// <param name="taskId">The ID of the task to delete.</param>
    /// <param name="currentUserId">The ID of the user attempting the deletion (for permission checks).</param>
    /// <param name="isCurrentUserAdmin">A flag indicating if the current user is an admin.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteTaskAsync(int taskId, int currentUserId, bool isCurrentUserAdmin);

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