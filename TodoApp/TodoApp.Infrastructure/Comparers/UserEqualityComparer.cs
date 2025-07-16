#nullable enable
using System.Diagnostics.CodeAnalysis;
using TodoApp.Core.Models;

namespace TodoApp.Infrastructure.Comparers;

/// <summary>
/// Provides a mechanism to compare two User objects based on their Id property.
/// This is crucial for using User objects as keys in dictionaries or for operations like Distinct().
/// </summary>
public class UserEqualityComparer : IEqualityComparer<User>
{
    /// <summary>
    /// Determines whether two User objects are equal by comparing their Ids.
    /// </summary>
    public bool Equals(User? x, User? y)
    {
        // If both are the same instance or both are null, they are equal.
        if (ReferenceEquals(x, y)) return true;

        // If one is null, but not both, they are not equal.
        if (x is null || y is null) return false;

        // The core logic: two users are considered equal if their Ids are the same.
        return x.Id == y.Id;
    }

    /// <summary>
    /// Returns a hash code for a User object, based on its Id.
    /// </summary>
    /// <remarks>
    /// It's critical that if Equals(x, y) is true, then GetHashCode(x) must equal GetHashCode(y).
    /// Basing the hash code on the Id ensures this contract is met.
    /// </remarks>
    public int GetHashCode([DisallowNull] User obj)
    {
        // The hash code should be based on the same property used for equality comparison.
        return obj.Id.GetHashCode();
    }
}