#nullable enable
using TodoApp.Core.Models;

namespace TodoApp.Core.Services;

/// <summary>
/// Defines the contract for a service that manages task history records.
/// </summary>
public interface ITaskHistoryService
{
    /// <summary>
    /// Logs a new history event for a task.
    /// </summary>
    /// <param name="todoItemId">The ID of the task that was changed.</param>
    /// <param name="userId">The ID of the user who performed the action.</param>
    /// <param name="action">The type of action performed (e.g., "Create", "Update").</param>
    /// <param name="description">A detailed description of the change.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task LogHistoryAsync(int todoItemId, int userId, string action, string description);

    /// <summary>
    /// Retrieves all history records for a specific task, ordered by the most recent change first.
    /// </summary>
    /// <param name="todoItemId">The ID of the task to retrieve history for.</param>
    /// <returns>A list of TaskHistory objects.</returns>
    Task<List<TaskHistory>> GetHistoryForTaskAsync(int todoItemId);
}