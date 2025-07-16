#nullable enable
using TodoApp.Core.Models;

namespace TodoApp.Core.Services;

/// <summary>
/// Defines a contract for accessing the current user's context.
/// </summary>
public interface IUserContext
{
    User? CurrentUser { get; }
    void SetCurrentUser(User user);
}