#nullable enable
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Core.Models;

public class TodoItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    // A new field to store discussion, notes, or updates about the task.
    // Can be null if there are no comments.
    public string? Comments { get; set; }

    // The Status field now directly supports Pending, InProgress, and Completed.
    public TodoStatus Status { get; set; }

    public PriorityLevel Priority { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? DueDate { get; set; }

    // Foreign Keys and Navigation Properties
    public int CreatorId { get; set; }
    public virtual User Creator { get; set; } = null!;

    public int? AssignedToId { get; set; }
    public virtual User? AssignedTo { get; set; }

    [ConcurrencyCheck]
    public DateTime LastModifiedDate { get; set; }
}