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
    public async Task<DashboardViewModel> GetDashboardDataAsync()
    {
        var now = DateTime.UtcNow;

        // --- STEP 1: Fetch all users and all tasks in two efficient queries. ---
        var allUsers = await _context.Users.AsNoTracking().ToListAsync();
        var allTasks = await _context.TodoItems
            .Include(t => t.Creator) // Include is still needed for details panel later
            .Include(t => t.AssignedTo)
            .AsNoTracking()
            .ToListAsync();

        // --- STEP 2: Perform calculations in-memory (this part is likely correct). ---
        var viewModel = new DashboardViewModel
        {
            TotalTaskCount = allTasks.Count,
            UncompletedTaskCount = allTasks.Count(t => t.Status != TodoStatus.Completed),
            OverdueTaskCount = allTasks.Count(t => t.DueDate < now && t.Status != TodoStatus.Completed),
            UnassignedTaskCount = allTasks.Count(t => t.AssignedToId == null)
        };

        // 3a. Initialize a dictionary with all users, each with an empty list of tasks.
        //     We use a custom User equality comparer to handle dictionary keys correctly.
        var tasksByUser = allUsers.ToDictionary(
            user => user,
            user => new List<TodoItem>(),
            new UserEqualityComparer()
        );
        var unassignedUserKey = new User { Id = -1, Username = "(未指派的任務)" };
        tasksByUser[unassignedUserKey] = new List<TodoItem>();

        foreach (var task in allTasks)
        {
            if (task.AssignedTo != null)
            {
                // Task is assigned, find the correct user bucket.
                if (tasksByUser.TryGetValue(task.AssignedTo, out var taskList))
                {
                    taskList.Add(task);
                }
            }
            else
            {
                tasksByUser[unassignedUserKey].Add(task);
            }
        }

        // Remove the unassigned group if it has no tasks, for a cleaner UI.
        if (!tasksByUser[unassignedUserKey].Any())
        {
            tasksByUser.Remove(unassignedUserKey);
        }

        viewModel.GroupedTasks = tasksByUser;
        return viewModel;
    }
}