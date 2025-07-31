using TodoApp.Core.Models;

namespace TodoApp.WinForms.ViewModels;

public class PriorityDisplayItem
{
    public string Name { get; set; } = string.Empty;
    public PriorityLevel? Value { get; set; }
}