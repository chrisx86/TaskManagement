using TodoApp.Core.Models;

public class TodoItemViewModel
{
    private readonly TodoItem _model;

    public TodoItemViewModel(TodoItem model)
    {
        _model = model;
    }

    // Expose all properties needed by the grid
    public int Id => _model.Id;
    public string Title => _model.Title;
    public TodoStatus Status { get => _model.Status; set => _model.Status = value; }
    public PriorityLevel Priority => _model.Priority;
    public DateTime? DueDate => _model.DueDate;
    public string? Comments => _model.Comments;

    // --- FLATTENED PROPERTIES FOR BINDING ---
    public string CreatorUsername => _model.Creator?.Username ?? "N/A";
    public string AssignedToUsername => _model.AssignedTo?.Username ?? string.Empty; // Empty for unassigned
}