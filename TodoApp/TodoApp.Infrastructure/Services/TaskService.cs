#nullable enable
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Comparers;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Infrastructure.Services;

/// <summary>
/// Implements the ITaskService, providing concrete business logic for task management.
/// </summary>
public class TaskService : ITaskService
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    public TaskService(AppDbContext context, IEmailService emailService, IUserService userService)
    {
        _context = context;
        _emailService = emailService;
        _userService = userService;
    }

    public async Task<TodoItem?> GetTaskByIdAsync(int taskId)
    {
        return await _context.TodoItems
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .AsNoTracking() // Use AsNoTracking for read-only queries
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task<List<TodoItem>> GetAllTasksAsync(
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int currentUserId,
        int? assignedToUserIdFilter,
        int pageNumber,
        int pageSize,
        string? sortColumn,
        bool isAscending)
    {
        var query = BuildFilteredQuery(statusFilter, userFilter, currentUserId, assignedToUserIdFilter);

        IOrderedQueryable<TodoItem> orderedQuery;

        if (!string.IsNullOrEmpty(sortColumn))
        {
            // --- FIXED: Add cases for all sortable columns ---
            switch (sortColumn)
            {
                case "Status":
                    orderedQuery = isAscending ? query.OrderBy(t => t.Status) : query.OrderByDescending(t => t.Status);
                    break;
                case "Title":
                    orderedQuery = isAscending ? query.OrderBy(t => t.Title) : query.OrderByDescending(t => t.Title);
                    break;
                case "Priority":
                    orderedQuery = isAscending ? query.OrderBy(t => t.Priority) : query.OrderByDescending(t => t.Priority);
                    break;
                case "DueDate":
                    orderedQuery = isAscending
                        ? query.OrderBy(t => t.DueDate ?? DateTime.MaxValue)
                        : query.OrderByDescending(t => t.DueDate ?? DateTime.MinValue);
                    break;
                case "AssignedTo":
                    // Include the related entity for sorting
                    orderedQuery = isAscending
                        ? query.Include(t => t.AssignedTo).OrderBy(t => t.AssignedTo.Username)
                        : query.Include(t => t.AssignedTo).OrderByDescending(t => t.AssignedTo.Username);
                    break;
                case "Creator":
                    orderedQuery = isAscending
                        ? query.Include(t => t.Creator).OrderBy(t => t.Creator.Username)
                        : query.Include(t => t.Creator).OrderByDescending(t => t.Creator.Username);
                    break;
                case "CreationDate":
                    orderedQuery = isAscending ? query.OrderBy(t => t.CreationDate) : query.OrderByDescending(t => t.CreationDate);
                    break;
                case "LastModifiedDate":
                    orderedQuery = isAscending ? query.OrderBy(t => t.LastModifiedDate) : query.OrderByDescending(t => t.LastModifiedDate);
                    break;
                default:
                    // Fallback to default sort if column name is unknown
                    orderedQuery = ApplyDefaultSort(query);
                    break;
            }
        }
        else // Apply default sort if no sortColumn is specified
        {
            orderedQuery = ApplyDefaultSort(query);
        }

        // Apply pagination AFTER sorting
        return await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .ToListAsync();
    }

    private IOrderedQueryable<TodoItem> ApplyDefaultSort(IQueryable<TodoItem> query)
    {
        return query
            .OrderBy(t =>
                t.Status == TodoStatus.InProgress ? 0 :
                t.Status == TodoStatus.Pending ? 1 :
                t.Status == TodoStatus.Reject ? 2 :
                t.Status == TodoStatus.Completed ? 3 :
                4 // Default fallback value
            )
            .ThenByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate ?? DateTime.MaxValue);
    }

    public async Task<int> GetTaskCountAsync(
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int currentUserId,
        int? assignedToUserIdFilter)
    {
        var query = BuildFilteredQuery(statusFilter, userFilter, currentUserId, assignedToUserIdFilter);
        return await query.CountAsync();
    }

    public async Task<TodoItem> CreateTaskAsync(User currentUser, string title, string? comments, PriorityLevel priority, DateTime? dueDate, int? assignedToId)
    {
        var now = DateTime.Now;
        var newTask = new TodoItem
        {
            Title = title,
            Comments = comments,
            CreatorId = currentUser.Id,
            Priority = priority,
            DueDate = dueDate?.Date,
            AssignedToId = assignedToId,
            Status = TodoStatus.Pending,
            CreationDate = now,
            LastModifiedDate = now
        };

        _context.TodoItems.Add(newTask);
        await _context.SaveChangesAsync();

        _ = NotifyTaskChangeAsync(newTask, currentUser, "建立");

        return await GetTaskByIdAsync(newTask.Id) ?? newTask;
    }

    public async Task<TodoItem> UpdateTaskAsync(User currentUser, TodoItem taskFromUI)
    {
        var trackedTask = await _context.TodoItems.FindAsync(taskFromUI.Id);
        if (trackedTask is null)
        {
            throw new DbUpdateConcurrencyException($"操作失敗：找不到 ID 為 {taskFromUI.Id} 的任務。");
        }

        if (trackedTask.LastModifiedDate != taskFromUI.LastModifiedDate)
        {
            throw new DbUpdateConcurrencyException("資料已被他人修改，請重新整理後再試。");
        }

        trackedTask.Title = taskFromUI.Title;
        trackedTask.Comments = taskFromUI.Comments;
        trackedTask.Status = taskFromUI.Status;
        trackedTask.Priority = taskFromUI.Priority;
        trackedTask.DueDate = taskFromUI.DueDate;
        trackedTask.AssignedToId = taskFromUI.AssignedToId;

        trackedTask.LastModifiedDate = DateTime.Now;

        try
        {
            await _context.SaveChangesAsync();
            _ = NotifyTaskChangeAsync(trackedTask, currentUser, "更新");
            return trackedTask;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new Exception("儲存時發生並行衝突，請重試。", ex);
        }
    }

    public async Task DeleteTaskAsync(User currentUser, int taskId)
    {
        var taskToDelete = await _context.TodoItems.FindAsync(taskId);
        if (taskToDelete is null) return;

        if (currentUser.Role != UserRole.Admin && taskToDelete.CreatorId != currentUser.Id)
        {
            throw new UnauthorizedAccessException("您沒有權限刪除此任務。");
        }

        var tombstone = new TodoItem
        {
            Id = taskToDelete.Id,
            Title = taskToDelete.Title,
            Status = taskToDelete.Status,
            Priority = taskToDelete.Priority,
            DueDate = taskToDelete.DueDate,
            AssignedToId = taskToDelete.AssignedToId
        };

        _context.TodoItems.Remove(taskToDelete);
        await _context.SaveChangesAsync();
        _ = NotifyTaskChangeTombstoneAsync(tombstone, currentUser, "刪除");
    }

    public async Task<Dictionary<User, List<TodoItem>>> GetTasksGroupedByUserAsync()
    {
        var allUsers = await _context.Users.AsNoTracking().ToListAsync();
        var allTasks = await _context.TodoItems
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .AsNoTracking()
            .ToListAsync();

        var tasksByUser = allUsers.ToDictionary(
            user => user,
            user => new List<TodoItem>(),
            new UserEqualityComparer()
        );

        foreach (var task in allTasks)
        {
            User? owner = task.AssignedTo ?? task.Creator;
            if (owner != null)
            {
                if (tasksByUser.TryGetValue(owner, out var taskList))
                {
                    taskList.Add(task);
                }
            }
        }
        return tasksByUser;
    }

    #region Private Helpers

    private IQueryable<TodoItem> BuildFilteredQuery(
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int currentUserId,
        int? assignedToUserIdFilter)
    {
        var query = _context.TodoItems.AsNoTracking();

        if (statusFilter.HasValue) { query = query.Where(t => t.Status == statusFilter.Value); }

        switch (userFilter)
        {
            case UserTaskFilter.AssignedToMe:
                query = query.Where(t => t.AssignedToId == currentUserId);
                break;
            case UserTaskFilter.CreatedByMe:
                query = query.Where(t => t.CreatorId == currentUserId);
                break;
        }

        if (assignedToUserIdFilter.HasValue) { query = query.Where(t => t.AssignedToId == assignedToUserIdFilter.Value); }

        return query;
    }
    #region Private Notification Helpers

    /// <summary>
    /// Notifies relevant users about a change (Create/Update) to a task.
    /// This method runs in the background ("fire and forget") to not block the main operation.
    /// </summary>
    /// <param name="task">The task that was changed. It should have navigation properties loaded if needed.</param>
    /// <param name="currentUser">The user who performed the action.</param>
    /// <param name="action">The action performed (e.g., "建立", "更新").</param>
    private async Task NotifyTaskChangeAsync(TodoItem task, User currentUser, string action)
    {
        try
        {
            var subject = $"任務通知: '{task.Title}' 已被{action}";
            var body = BuildEmailBody(task, currentUser, action);

            if (currentUser.Role == UserRole.Admin)
            {
                // If an admin makes a change, notify the person the task is assigned to.
                if (task.AssignedTo?.Email != null)
                {
                    await _emailService.SendEmailAsync(task.AssignedTo.Email, subject, body);
                }
            }
            else // A regular user made the change.
            {
                // Notify all administrators.
                var admins = (await _userService.GetAllUsersAsync())
                    .Where(u => u.Role == UserRole.Admin && !string.IsNullOrEmpty(u.Email));

                var notificationTasks = admins.Select(admin =>
                    _emailService.SendEmailAsync(admin.Email!, subject, body)
                );
                await Task.WhenAll(notificationTasks);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ERROR] Failed to send task change notification: {ex.Message}");
        }
    }

    /// <summary>
    /// Notifies relevant users about a task that has been deleted.
    /// </summary>
    /// <param name="deletedTask">The task object right before it was deleted (a "tombstone").</param>
    /// <param name="currentUser">The user who performed the deletion.</param>
    /// <param name="action">The action performed (typically "刪除").</param>
    private async Task NotifyTaskChangeTombstoneAsync(TodoItem deletedTask, User currentUser, string action)
    {
        try
        {
            var subject = $"任務通知: '{deletedTask.Title}' 已被{action}";
            var body = BuildEmailBody(deletedTask, currentUser, action, isDeleted: true);

            if (currentUser.Role == UserRole.Admin)
            {
                if (deletedTask.AssignedToId.HasValue)
                {
                    var assignee = await _context.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == deletedTask.AssignedToId.Value);

                    if (assignee?.Email != null)
                    {
                        await _emailService.SendEmailAsync(assignee.Email, subject, body);
                    }
                }
            }
            else // Regular user deleted the task.
            {
                var admins = (await _userService.GetAllUsersAsync())
                    .Where(u => u.Role == UserRole.Admin && !string.IsNullOrEmpty(u.Email));

                var notificationTasks = admins.Select(admin =>
                    _emailService.SendEmailAsync(admin.Email!, subject, body)
                );
                await Task.WhenAll(notificationTasks);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ERROR] Failed to send task deletion notification: {ex.Message}");
        }
    }

    /// <summary>
    /// A helper method to build a standardized HTML body for the notification email.
    /// </summary>
    /// <param name="task">The task object containing the details.</param>
    /// <param name="performedBy">The user who performed the action.</param>
    /// <param name="action">The string representing the action (e.g., "更新").</param>
    /// <param name="isDeleted">A flag to indicate if the task was deleted, for different formatting.</param>
    /// <returns>A formatted HTML string.</returns>
    private string BuildEmailBody(TodoItem task, User performedBy, string action, bool isDeleted = false)
    {
        var assignedToText = isDeleted
            ? (task.AssignedToId.HasValue ? $"(原指派給 UserID: {task.AssignedToId})" : "(未指派)")
            : (task.AssignedTo?.Username ?? "(未指派)");

        var body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: 'Segoe UI', sans-serif; }}
                    .container {{ border: 1px solid #ddd; padding: 20px; border-radius: 5px; max-width: 600px; }}
                    h3 {{ color: #005A9E; }}
                    ul {{ list-style-type: none; padding-left: 0; }}
                    li {{ margin-bottom: 10px; }}
                    b {{ color: #333; }}
                    i {{ color: #888; font-size: 0.9em; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <p>您好，</p>
                    <p>任務 <b>{System.Web.HttpUtility.HtmlEncode(task.Title)}</b> (ID: {task.Id}) 已被使用者 <b>{performedBy.Username}</b> {action}。</p>
                    <hr>
                    <h3>任務詳情：</h3>
                    <ul>
                        <li><b>狀態:</b> {task.Status}</li>
                        <li><b>優先級:</b> {task.Priority}</li>
                        <li><b>到期日:</b> {task.DueDate?.ToShortDateString() ?? "無"}</li>
                        <li><b>指派給:</b> {assignedToText}</li>
                    </ul>
                    <br>
                    <p><i>這是一封自動通知郵件，請勿直接回覆。</i></p>
                </div>
            </body>
            </html>";

        return body;
    }

    #endregion
    #endregion
}