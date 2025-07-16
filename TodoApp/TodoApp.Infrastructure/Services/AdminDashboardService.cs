#nullable enable
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Core.ViewModels;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Comparers;
using TodoStatus = TodoApp.Core.Models.TodoStatus;

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

        // --- STEP 3: FIXED - A robust grouping logic. ---

        // 3a. Initialize a dictionary with all users, each with an empty list of tasks.
        //     We use a custom User equality comparer to handle dictionary keys correctly.
        var tasksByUser = new Dictionary<User, List<TodoItem>>(new UserEqualityComparer());
        foreach (var user in allUsers)
        {
            tasksByUser[user] = new List<TodoItem>();
        }

        // 3b. Iterate through all tasks and place them in the correct user's "bucket".
        //     The primary owner is the assignee. If not assigned, it's the creator.
        foreach (var task in allTasks)
        {
            User? owner = null;
            if (task.AssignedTo != null)
            {
                owner = task.AssignedTo;
            }
            else if (task.Creator != null)
            {
                // Fallback to creator if not assigned
                owner = task.Creator;
            }

            if (owner != null && tasksByUser.ContainsKey(owner))
            {
                tasksByUser[owner].Add(task);
            }
        }

        viewModel.GroupedTasks = tasksByUser;

        return viewModel;
    }
}