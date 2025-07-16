#nullable enable
using TodoApp.Core.Models;

namespace TodoApp.Core.ViewModels;

/// <summary>
/// Represents the data transfer object (DTO) / ViewModel for the Admin Dashboard.
/// This single class encapsulates all the data needed to render the entire dashboard UI,
/// promoting a clean data flow from the service layer to the presentation layer.
/// </summary>
public class DashboardViewModel
{
    // ===================================================================
    // Global Statistic Cards Data
    // These properties map directly to the KPI cards at the top of the dashboard.
    // ===================================================================

    /// <summary>
    /// The total number of tasks in the entire system.
    /// </summary>
    public int TotalTaskCount { get; set; }

    /// <summary>
    /// The number of tasks that are not yet completed (i.e., in 'Pending' or 'InProgress' status).
    /// </summary>
    public int UncompletedTaskCount { get; set; }

    /// <summary>
    /// The number of uncompleted tasks whose due date has passed.
    /// </summary>
    public int OverdueTaskCount { get; set; }

    /// <summary>
    /// The number of tasks that are not assigned to any user.
    /// </summary>
    public int UnassignedTaskCount { get; set; }

    // ===================================================================
    // Main Content Data
    // This property provides the data for the central TreeView.
    // ===================================================================

    /// <summary>
    /// A dictionary containing all tasks, grouped by the user they are primarily associated with.
    /// The key is the User object, and the value is the list of their associated tasks.
    /// </summary>
    /// <remarks>
    /// Using a non-nullable 'List' and initializing it ensures that we don't need to perform
    /// null checks when iterating over the dictionary values in the UI layer.
    /// </remarks>
    public Dictionary<User, List<TodoItem>> GroupedTasks { get; set; } = new();
}