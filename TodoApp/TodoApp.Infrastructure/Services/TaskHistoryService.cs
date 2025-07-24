#nullable enable
using System.Text;
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Services;

/// <summary>
/// Implements the ITaskHistoryService, providing logic to log and retrieve task audit trails.
/// </summary>
public class TaskHistoryService : ITaskHistoryService
{
    private readonly AppDbContext _context;

    public TaskHistoryService(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task LogHistoryAsync(int todoItemId, int userId, string action, string description)
    {
        var historyEntry = new TaskHistory
        {
            TodoItemId = todoItemId,
            UserId = userId,
            Action = action,
            ChangeDescription = description,
            ChangeDate = DateTime.Now // Using local time as per previous requirement
        };

        _context.TaskHistories.Add(historyEntry);

        // This SaveChanges is often called right after another SaveChanges in TaskService.
        // It's acceptable as DbContext is transient, ensuring a fresh context for each operation.
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<List<TaskHistory>> GetHistoryForTaskAsync(int todoItemId)
    {
        // 1. Fetch all history records for the task.
        var historyRecords = await _context.TaskHistories
            .AsNoTracking()
            .Where(h => h.TodoItemId == todoItemId)
            .OrderByDescending(h => h.ChangeDate)
            .ToListAsync();

        // 2. If there's no history, return an empty list immediately.
        if (!historyRecords.Any())
        {
            return new List<TaskHistory>();
        }

        // 3. Get a distinct list of all user IDs involved in this task's history.
        var userIds = historyRecords.Select(h => h.UserId).Distinct().ToList();

        // 4. Fetch all relevant user objects from the database in a single query.
        var users = await _context.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id); // Create a lookup dictionary for fast access.

        // 5. Populate the 'User' navigation property on each history record.
        foreach (var record in historyRecords)
        {
            if (users.TryGetValue(record.UserId, out var user))
            {
                record.User = user;
            }
        }

        return historyRecords;
    }
}