// The namespace should match the project and folder structure.
namespace TodoApp.Core.Models;

/// <summary>
/// Defines the priority levels for a to-do item.
/// Using an enum makes the code more readable and type-safe.
/// </summary>
public enum PriorityLevel
{
    // The default value will be 0.
    Low,

    // This will have a value of 1.
    Medium,

    // This will have a value of 2.
    High
}