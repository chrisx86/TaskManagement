#nullable enable
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Core.ViewModels;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Comparers;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Infrastructure.Services;

/// <summary>
/// Implements the IAdminDashboardService, providing the concrete logic for gathering dashboard data.
/// </summary>
public class AdminDashboardService : IAdminDashboardService
{
    private readonly AppDbContext _context;

    public AdminDashboardService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gathers all dashboard data in a single, efficient operation.
    /// </summary>
    /// <returns>A DashboardViewModel populated with statistics and grouped tasks.</returns>
    // In AdminDashboardService.cs

    public async Task<DashboardViewModel> GetDashboardDataAsync()
    {
        var now = DateTime.Now;

        var allUsers = await _context.Users.AsNoTracking().ToListAsync();
        var allTasks = await _context.TodoItems
            .Include(t => t.Creator)
            .Include(t => t.AssignedTo)
            .AsNoTracking()
            .ToListAsync();

        var viewModel = new DashboardViewModel
        {
            TotalTaskCount = allTasks.Count,
            UncompletedTaskCount = allTasks.Count(t => t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject),
            OverdueTaskCount = allTasks.Count(t => t.DueDate < now && t.Status != TodoStatus.Completed && t.Status != TodoStatus.Reject),
            UnassignedTaskCount = allTasks.Count(t => t.AssignedToId == null),
            RejectedTaskCount = allTasks.Count(t => t.Status == TodoStatus.Reject)
        };

        // 1. Initialize the dictionary with all real users.
        var tasksByUser = allUsers.ToDictionary(
            user => user,
            user => new List<TodoItem>(),
            new UserEqualityComparer()
        );

        // 2. Create a special "pseudo-user" to hold unassigned tasks.
        //    We give it a unique negative Id to avoid any conflicts with real user Ids.
        var unassignedUserKey = new User { Id = -1, Username = "(待指派的任務)" };

        // 3. Iterate through all tasks and place them in the correct bucket.
        foreach (var task in allTasks)
        {
            if (task.AssignedTo != null)
            {
                if (tasksByUser.TryGetValue(task.AssignedTo, out var taskList))
                    taskList.Add(task);
            }
            else
            {
                if (!tasksByUser.ContainsKey(unassignedUserKey))
                    tasksByUser[unassignedUserKey] = new List<TodoItem>();
                tasksByUser[unassignedUserKey].Add(task);
            }
        }

        // 4. (Optional) For a cleaner UI, remove the unassigned group if it ends up being empty.
        if (tasksByUser.ContainsKey(unassignedUserKey) && !tasksByUser[unassignedUserKey].Any())
        {
            tasksByUser.Remove(unassignedUserKey);
        }

        viewModel.GroupedTasks = tasksByUser;
        return viewModel;
    }
}