#nullable enable
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
    private readonly CancellationTokenSource _appShutdownTokenSource;
    public TaskHistoryService(AppDbContext context, CancellationTokenSource appShutdownTokenSource)
    {
        _context = context;
        _appShutdownTokenSource = appShutdownTokenSource;
    }

    public async Task LogHistoryAsync(int todoItemId, int userId, string action, string description)
    {
        var historyEntry = new TaskHistory
        {
            TodoItemId = todoItemId,
            UserId = userId,
            Action = action,
            ChangeDescription = description,
            ChangeDate = DateTime.Now
        };

        _context.TaskHistories.Add(historyEntry);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ERROR] Failed to log task history: {ex.Message}");
        }
    }

    public async Task<List<TaskHistory>> GetHistoryForTaskAsync(int todoItemId)
    {
        var historyRecords = await _context.TaskHistories
            .AsNoTracking()
            .Where(h => h.TodoItemId == todoItemId)
            .OrderByDescending(h => h.ChangeDate)
            .ToListAsync(_appShutdownTokenSource.Token);

        if (!historyRecords.Any())
            return new List<TaskHistory>();

        var userIds = historyRecords.Select(h => h.UserId).Distinct().ToList();

        var users = await _context.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, _appShutdownTokenSource.Token);

        foreach (var record in historyRecords)
        {
            if (users.TryGetValue(record.UserId, out var user))
                record.User = user;
        }
        return historyRecords;
    }
}