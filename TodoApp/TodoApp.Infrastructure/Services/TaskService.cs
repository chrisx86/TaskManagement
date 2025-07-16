#nullable enable
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
// Using alias to resolve ambiguity with System.Threading.Tasks.TaskStatus
using TodoStatus = TodoApp.Core.Models.TodoStatus;

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

    /// <summary>
    /// Retrieves a list of to-do items based on specified filter criteria.
    /// </summary>
    public async Task<List<TodoItem>> GetAllTasksAsync(
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int currentUserId,
        int? assignedToUserIdFilter)
    {
        var query = _context.TodoItems
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .AsNoTracking();

        // Apply Status Filter
        if (statusFilter.HasValue)
        {
            query = query.Where(t => t.Status == statusFilter.Value);
        }

        // Apply general User Filter (My Tasks, etc.)
        switch (userFilter)
        {
            case UserTaskFilter.AssignedToMe:
                query = query.Where(t => t.AssignedToId == currentUserId);
                break;
            case UserTaskFilter.CreatedByMe:
                query = query.Where(t => t.CreatorId == currentUserId);
                break;
        }

        // Apply specific 'Assigned To User' Filter
        if (assignedToUserIdFilter.HasValue)
        {
            query = query.Where(t => t.AssignedToId == assignedToUserIdFilter.Value);
        }

        return await query.OrderByDescending(t => t.CreationDate).ToListAsync();
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
            // 2. Copy the updated values from the UI object to the tracked entity.
            // --- FIXED: Use the correct parameter name 'taskToUpdate' ---
            // 修正：使用正確的參數名稱 'taskToUpdate'
            _context.Entry(trackedTask).CurrentValues.SetValues(taskToUpdate);

            try
            {
                // 3. Save the changes.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                if (await entry.GetDatabaseValuesAsync() == null)
                {
                    throw new Exception("操作失敗：此任務已被其他使用者刪除。", ex);
                }
                throw new Exception("資料已被他人修改，請重新整理後再試。", ex);
            }
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
}