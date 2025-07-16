#nullable enable
using TodoApp.Core.Models;

namespace TodoApp.WinForms.ViewModels;

/// <summary>
/// A ViewModel for displaying TodoStatus options in a ComboBox.
/// </summary>
public class StatusDisplayItem
{
    public string Name { get; set; } = string.Empty;
    public TodoStatus? Value { get; set; }
}