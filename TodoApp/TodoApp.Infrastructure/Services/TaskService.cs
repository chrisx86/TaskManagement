#nullable enable
using System.Text;
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
    private readonly CancellationTokenSource _appShutdownTokenSource;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;
    private readonly ITaskHistoryService _historyService;

    public TaskService(AppDbContext context, IEmailService emailService, IUserService userService, ITaskHistoryService historyService,
        CancellationTokenSource appShutdownTokenSource)
    {
        _context = context;
        _emailService = emailService;
        _userService = userService;
        _historyService = historyService;
        _appShutdownTokenSource = appShutdownTokenSource;
    }

    public async Task<TodoItem?> GetTaskByIdAsync(int taskId)
    {
        return await _context.TodoItems
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == taskId, _appShutdownTokenSource.Token);
    }

    public async Task<List<TodoItem>> GetAllTasksAsync(
        User currentUser,
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int? assignedToUserIdFilter,
        int pageNumber,
        int pageSize,
        string? sortColumn,
        bool isAscending,
        string? searchKeyword)
    {
        var query = BuildFilteredQuery(currentUser, statusFilter, userFilter, assignedToUserIdFilter, searchKeyword);

        IOrderedQueryable<TodoItem> orderedQuery;

        if (!string.IsNullOrEmpty(sortColumn))
        {
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
                    orderedQuery = ApplyDefaultSort(query);
                    break;
            }
        }
        else
        {
            orderedQuery = ApplyDefaultSort(query);
        }

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
        User currentUser,
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int? assignedToUserIdFilter,
        string? searchKeyword)
    {
        var query = BuildFilteredQuery(currentUser, statusFilter, userFilter, assignedToUserIdFilter, searchKeyword);
        return await query.CountAsync();
    }

    public async Task<TodoItem> CreateTaskAsync(User currentUser, string title, string? comments, TodoStatus status, PriorityLevel priority, DateTime? dueDate, int? assignedToId)
    {
        var now = DateTime.Now;
        var newTask = new TodoItem
        {
            Title = title,
            Comments = comments,
            CreatorId = currentUser.Id,
            Status = status,
            Priority = priority,
            DueDate = dueDate?.Date,
            AssignedToId = assignedToId,
            CreationDate = now,
            LastModifiedDate = now
        };

        _context.TodoItems.Add(newTask);
        await _context.SaveChangesAsync(_appShutdownTokenSource.Token);
        _ = _historyService.LogHistoryAsync(newTask.Id, currentUser.Id, "Create", "任務已建立。");
        _ = NotifyTaskChangeAsync(newTask, currentUser, "建立");

        return await GetTaskByIdAsync(newTask.Id) ?? newTask;
    }

    /// <summary>
    /// Updates an existing to-do item based on the provided data.
    /// This implementation follows the "Last Write Wins" strategy.
    /// </summary>
    /// <param name="currentUser">The user performing the update.</param>
    /// <param name="taskFromUI">A TodoItem object containing the new values from the UI.</param>
    /// <returns>The updated and saved TodoItem entity, complete with navigation properties.</returns>
    public async Task<TodoItem> UpdateTaskAsync(User currentUser, TodoItem taskFromUI)
    {
        var trackedTask = await _context.TodoItems
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == taskFromUI.Id, _appShutdownTokenSource.Token);

        if (trackedTask is null)
            throw new InvalidOperationException($"操作失敗：找不到 ID 為 {taskFromUI.Id} 的任務，它可能已被刪除。");

        var changeDescription = BuildChangeDescription(originalTask: trackedTask, newTask: taskFromUI);

        //_context.Entry(trackedTask).CurrentValues.SetValues(taskFromUI);

        // --- Manual Property Mapping ---
        trackedTask.Title = taskFromUI.Title;
        trackedTask.Status = taskFromUI.Status;
        trackedTask.Priority = taskFromUI.Priority;
        trackedTask.DueDate = taskFromUI.DueDate;
        trackedTask.Comments = taskFromUI.Comments;
        trackedTask.AssignedToId = taskFromUI.AssignedToId;
        // CreatorId is usually not updatable, so we don't map it.

        trackedTask.LastModifiedDate = DateTime.Now;

        try
        {
            await _context.SaveChangesAsync(_appShutdownTokenSource.Token);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Handle concurrency exception specifically if needed
            throw new Exception("資料己被他人修改，請重新整理後再試。", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("儲存資料時發生錯誤。", ex);
        }

        if (!string.IsNullOrEmpty(changeDescription))
            _ = _historyService.LogHistoryAsync(trackedTask.Id, currentUser.Id, "Update", changeDescription);

        _ = NotifyTaskChangeAsync(trackedTask, currentUser, "更新");

        return trackedTask;
    }

    /// <summary>
    /// Compares two TodoItem objects and builds a human-readable string describing the differences.
    /// </summary>
    /// <param name="originalTask">The task object representing the state BEFORE the update.</param>
    /// <param name="newTask">The task object containing the new, updated values.</param>
    /// <returns>A string detailing all changes, or an empty string if no significant changes were detected.</returns>
    private string BuildChangeDescription(TodoItem originalTask, TodoItem newTask)
    {
        var descriptionBuilder = new StringBuilder();

        if (!string.Equals(originalTask.Title, newTask.Title))
            descriptionBuilder.AppendLine("標題已修改。");

        if (!string.Equals(originalTask.Comments, newTask.Comments))
        {
            if (string.IsNullOrEmpty(originalTask.Comments))
                descriptionBuilder.AppendLine("已新增備註。");
            else if (string.IsNullOrEmpty(newTask.Comments))
                descriptionBuilder.AppendLine("備註已被清空。");
            else
                descriptionBuilder.AppendLine("備註已修改。");
        }

        if (originalTask.Status != newTask.Status)
            descriptionBuilder.AppendLine($"狀態從 '{originalTask.Status}' 變更為 '{newTask.Status}'。");

        if (originalTask.Priority != newTask.Priority)
            descriptionBuilder.AppendLine($"優先級從 '{originalTask.Priority}' 變更為 '{newTask.Priority}'。");

        if (originalTask.DueDate != newTask.DueDate)
        {
            var originalDate = originalTask.DueDate?.ToString("yyyy-MM-dd") ?? "無";
            var currentDate = newTask.DueDate?.ToString("yyyy-MM-dd") ?? "無";
            descriptionBuilder.AppendLine($"到期日從 '{originalDate}' 變更為 '{currentDate}'。");
        }

        if (originalTask.AssignedToId != newTask.AssignedToId)
        {
            var originalAssigneeId = originalTask.AssignedToId?.ToString() ?? "(未指派)";
            var newAssigneeId = newTask.AssignedToId?.ToString() ?? "(未指派)";
            descriptionBuilder.AppendLine($"指派對象 ID 已從 '{originalAssigneeId}' 變更為 '{newAssigneeId}'。");
        }

        return descriptionBuilder.ToString().Trim();
    }

    public async Task<TodoItem> UpdateTaskCommentsAsync(User currentUser, int taskId, string newComments)
    {
        var trackedTask = await _context.TodoItems
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == taskId, _appShutdownTokenSource.Token);

        if (trackedTask is null)
            throw new DbUpdateConcurrencyException($"操作失敗：找不到 ID 為 {taskId} 的任務。");

        trackedTask.Comments = newComments;
        trackedTask.LastModifiedDate = DateTime.Now;

        try
        {
            await _context.SaveChangesAsync(_appShutdownTokenSource.Token);

            _ = _historyService.LogHistoryAsync(taskId, currentUser.Id, "Update", "備註已修改。");
            _ = NotifyTaskChangeAsync(trackedTask, currentUser, "更新");

            return trackedTask;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new Exception("資料已被他人修改，請重新整理後再試。", ex);
        }
    }

    public async Task DeleteTaskAsync(User currentUser, int taskId)
    {
        var taskToDelete = await _context.TodoItems.FindAsync(taskId);
        if (taskToDelete is null) return;

        if (currentUser.Role != UserRole.Admin && taskToDelete.CreatorId != currentUser.Id)
            throw new UnauthorizedAccessException("您沒有權限刪除此任務。");

        var tombstone = new TodoItem { Id = taskToDelete.Id, Title = taskToDelete.Title };

        _context.TodoItems.Remove(taskToDelete);
        await _context.SaveChangesAsync(_appShutdownTokenSource.Token);
        _ = _historyService.LogHistoryAsync(tombstone.Id, currentUser.Id, "Delete", $"任務 '{tombstone.Title}' 已被刪除。");
        _ = NotifyTaskChangeTombstoneAsync(tombstone, currentUser, "刪除");
    }

    public async Task<Dictionary<User, List<TodoItem>>> GetTasksGroupedByUserAsync()
    {
        var allUsers = await _context.Users.AsNoTracking().ToListAsync();
        var allTasks = await _context.TodoItems
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .AsNoTracking()
            .ToListAsync(_appShutdownTokenSource.Token);

        var tasksByUser = allUsers.ToDictionary(
            user => user,
            user => new List<TodoItem>(),
            new UserEqualityComparer()
        );

        foreach (var task in allTasks)
        {
            User? owner = task.AssignedTo ?? task.Creator;
            if (owner is not null)
            {
                if (tasksByUser.TryGetValue(owner, out var taskList))
                    taskList.Add(task);
            }
        }
        return tasksByUser;
    }

    #region Private Helpers

    private IQueryable<TodoItem> BuildFilteredQuery(
        User currentUser,
        TodoStatus? statusFilter,
        UserTaskFilter userFilter,
        int? assignedToUserIdFilter,
        string? searchKeyword)
    {
        var query = _context.TodoItems.AsNoTracking();
        if (statusFilter.HasValue) { query = query.Where(t => t.Status == statusFilter.Value); }
        if (!string.IsNullOrEmpty(searchKeyword))
        {
            var keyword = searchKeyword.ToLower();
            query = query.Where(t =>
                (t.Title != null && t.Title.ToLower().Contains(keyword)) ||
                (t.Comments != null && t.Comments.ToLower().Contains(keyword))
            );
        }
        if (currentUser.Role == UserRole.Admin)
        {
            switch (userFilter)
            {
                case UserTaskFilter.AssignedToMe:
                    query = query.Where(t => t.AssignedToId == currentUser.Id);
                    break;
                case UserTaskFilter.CreatedByMe:
                    query = query.Where(t => t.CreatorId == currentUser.Id);
                    break;
            }

            if (assignedToUserIdFilter.HasValue && assignedToUserIdFilter > 0)
                query = query.Where(t => t.AssignedToId == assignedToUserIdFilter.Value);
        }
        else
        {
            query = query.Where(t => t.AssignedToId == currentUser.Id || t.CreatorId == currentUser.Id);
            switch (userFilter)
            {
                case UserTaskFilter.AssignedToMe:
                    query = query.Where(t => t.AssignedToId == currentUser.Id);
                    break;
                case UserTaskFilter.CreatedByMe:
                    query = query.Where(t => t.CreatorId == currentUser.Id);
                    break;
            }
        }
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
        return;
        try
        {
            if (_appShutdownTokenSource.IsCancellationRequested) return;
            var subject = $"任務通知: '{task.Title}' 已被{action}";
            var body = BuildEmailBody(task, currentUser, action);

            if (currentUser.Role == UserRole.Admin)
            {
                // If an admin makes a change, notify the person the task is assigned to.
                if (task.AssignedTo?.Email is not null)
                    await _emailService.SendEmailAsync(task.AssignedTo.Email, subject, body);
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
        return;
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