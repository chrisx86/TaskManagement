#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Core.Models;

/// <summary>
/// Represents a single audit trail entry for a change made to a TodoItem.
/// This entity is designed to be an immutable log.
/// </summary>
public class TaskHistory
{
    // Using long for the primary key as history records can grow very large over time.
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The ID of the TodoItem that this history record belongs to.
    /// This is not a foreign key to enforce database-level independence.
    /// </summary>
    [Required]
    public int TodoItemId { get; set; }

    /// <summary>
    /// The ID of the User who performed the action.
    /// Not a foreign key, to allow user deletion without affecting history.
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// The action performed, e.g., "Create", "Update", "Delete".
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// A detailed description of the changes made.
    /// E.g., "Status changed from Pending to InProgress. Priority changed from Low to High."
    /// </summary>
    [Required]
    public string ChangeDescription { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp when the change occurred, stored in UTC.
    /// </summary>
    [Required]
    public DateTime ChangeDate { get; set; }

    // --- Navigation Properties (for application-level use) ---
    // These properties will NOT be configured as foreign keys in the DbContext,
    // making them purely for convenience in application logic where joins are needed.
    [ForeignKey(nameof(TodoItemId))]
    public virtual TodoItem? TodoItem { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
}