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

    public string? Comments { get; set; }

    public TodoStatus Status { get; set; }

    public PriorityLevel Priority { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? DueDate { get; set; }

    public int CreatorId { get; set; }
    public virtual User Creator { get; set; } = null!;

    public int? AssignedToId { get; set; }
    public virtual User? AssignedTo { get; set; }

    public DateTime LastModifiedDate { get; set; }
}