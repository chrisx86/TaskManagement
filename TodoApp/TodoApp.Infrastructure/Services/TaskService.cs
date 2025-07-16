#nullable enable
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Services;

/// <summary>
/// Implements the ITaskService interface, providing concrete business logic for task management.
/// </summary>
public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<TodoItem?> GetTaskByIdAsync(int taskId)
    {
        // Find the task and include all necessary related data for editing.
        return await _context.TodoItems
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    /// <summary>
    /// Retrieves a list of to-do items based on specified filter criteria.
    /// </summary>
    public async Task<List<TodoItem>> GetAllTasksAsync(
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int currentUserId,
        int? assignedToUserIdFilter,
        int pageNumber,
        int pageSize)
    {
        var query = BuildFilteredQuery(statusFilter, userFilter, currentUserId, assignedToUserIdFilter);

        // Apply ordering BEFORE pagination
        var orderedQuery = query
            .OrderBy(t => t.Status == TodoStatus.InProgress ? 0 : (t.Status == TodoStatus.Pending ? 1 : 2))
            .ThenByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate ?? DateTime.MaxValue);

        // Apply pagination. EF Core will translate this to efficient SQL.
        return await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .ToListAsync();
    }

    public async Task<int> GetTaskCountAsync(
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int currentUserId,
        int? assignedToUserIdFilter)
    {
        var query = BuildFilteredQuery(statusFilter, userFilter, currentUserId, assignedToUserIdFilter);
        // CountAsync is highly optimized at the database level.
        return await query.CountAsync();
    }

    /// <summary>
    /// Creates a new to-do item.
    /// </summary>
    public async Task<TodoItem> CreateTaskAsync(string title, string? comments, int creatorId, PriorityLevel priority, DateTime? dueDate, int? assignedToId)
    {
        var newTask = new TodoItem
        {
            Title = title,
            Comments = comments,
            CreatorId = creatorId,
            Priority = priority,
            DueDate = dueDate,
            AssignedToId = assignedToId,
            Status = TodoStatus.Pending,
            CreationDate = DateTime.UtcNow
        };

        _context.TodoItems.Add(newTask);
        await _context.SaveChangesAsync();

        return await _context.TodoItems
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .AsNoTracking()
            .FirstAsync(t => t.Id == newTask.Id);
    }

    /// <summary>
    /// Updates an existing to-do item, handling concurrency.
    /// </summary>
    // --- The parameter name here is 'taskToUpdate' ---
    public async Task UpdateTaskAsync(TodoItem taskToUpdate)
    {
        // Use the "Find then Update" pattern for disconnected entities

        // 1. Find the existing entity in the database using its primary key.
        var trackedTask = await _context.TodoItems.FindAsync(taskToUpdate.Id);
        if (trackedTask != null)
        {
            // When using SetValues, EF Core's change tracker is smart enough to know
            // that the Timestamp is a concurrency token and should be included in the
            // WHERE clause of the UPDATE statement.
            _context.Entry(trackedTask).CurrentValues.SetValues(taskToUpdate);
            await _context.SaveChangesAsync();
        }
        else
        {
            // --- FIXED: Also use the correct parameter name here for the error message ---
            throw new Exception($"操作失敗：找不到 ID 為 {taskToUpdate.Id} 的任務，它可能已被刪除。");
        }
    }

    /// <summary>
    /// Deletes a to-do item after checking permissions.
    /// </summary>
    public async Task DeleteTaskAsync(int taskId, int currentUserId, bool isCurrentUserAdmin)
    {
        var task = await _context.TodoItems.FindAsync(taskId);
        if (task == null)
        {
            // If the task is already deleted, we can just return.
            return;
        }

        // --- Permission Check ---
        if (!isCurrentUserAdmin && task.CreatorId != currentUserId)
        {
            throw new UnauthorizedAccessException("您沒有權限刪除此任務，只有任務建立者或管理員可以刪除。");
        }

        _context.TodoItems.Remove(task);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Gets all tasks grouped by user, optimized for the admin dashboard.
    /// </summary>
    public async Task<Dictionary<User, List<TodoItem>>> GetTasksGroupedByUserAsync()
    {
        // Fetch all users that have any relation to tasks (creator or assignee)
        // This is more efficient than loading all users.
        var relevantUserIds = await _context.TodoItems
            .Select(t => t.CreatorId)
            .Union(_context.TodoItems.Where(t => t.AssignedToId.HasValue).Select(t => t.AssignedToId!.Value))
            .Distinct()
            .ToListAsync();

        var relevantUsers = await _context.Users
            .Where(u => relevantUserIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id);

        // Fetch all tasks and group them in memory.
        var allTasks = await _context.TodoItems
            .AsNoTracking()
            .ToListAsync();

        var tasksByUser = relevantUsers.Values.ToDictionary(user => user, user => new List<TodoItem>());

        foreach (var task in allTasks)
        {
            // The task belongs to the user it is assigned to.
            if (task.AssignedToId.HasValue && relevantUsers.TryGetValue(task.AssignedToId.Value, out var assignee))
            {
                if (!tasksByUser.ContainsKey(assignee))
                {
                    tasksByUser[assignee] = new List<TodoItem>();
                }
                tasksByUser[assignee].Add(task);
            }
            // If not assigned, it belongs to the creator.
            else if (relevantUsers.TryGetValue(task.CreatorId, out var creator))
            {
                if (!tasksByUser.ContainsKey(creator))
                {
                    tasksByUser[creator] = new List<TodoItem>();
                }
                tasksByUser[creator].Add(task);
            }
        }

        return tasksByUser;
    }

    // A private helper method to build the base query with filters
    private IQueryable<TodoItem> BuildFilteredQuery(
        TodoStatus? statusFilter, // Use the alias
        UserTaskFilter userFilter,
        int currentUserId,
        int? assignedToUserIdFilter)
    {
        var query = _context.TodoItems.AsNoTracking();

        if (statusFilter.HasValue)
        {
            query = query.Where(t => t.Status == statusFilter.Value);
        }

        switch (userFilter)
        {
            case UserTaskFilter.AssignedToMe:
                query = query.Where(t => t.AssignedToId == currentUserId);
                break;
            case UserTaskFilter.CreatedByMe:
                query = query.Where(t => t.CreatorId == currentUserId);
                break;
        }

        if (assignedToUserIdFilter.HasValue)
            query = query.Where(t => t.AssignedToId == assignedToUserIdFilter.Value);

        return query;
    }
}