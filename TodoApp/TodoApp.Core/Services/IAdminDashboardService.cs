#nullable enable
using System.Threading.Tasks;
using TodoApp.Core.ViewModels;

namespace TodoApp.Core.Services;

/// <summary>
/// Defines the contract for a service that provides data specifically for the Admin Dashboard.
/// This service encapsulates the business logic required to gather and process dashboard-specific information.
/// </summary>
public interface IAdminDashboardService
{
    /// <summary>
    /// Asynchronously retrieves all the necessary data required to render the Admin Dashboard.
    /// This includes global statistics and tasks grouped by user.
    /// </summary>
    /// <returns>
    /// A Task that represents the asynchronous operation. 
    /// The task result contains a <see cref="DashboardViewModel"/> object, which is a single,
    /// comprehensive data model for the entire dashboard view.
    /// </returns>
    Task<DashboardViewModel> GetDashboardDataAsync();
}