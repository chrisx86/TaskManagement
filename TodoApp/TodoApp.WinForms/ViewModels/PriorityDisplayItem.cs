#nullable enable
using TodoApp.Core.Models;

namespace TodoApp.WinForms.ViewModels;

/// <summary>
/// Helper class for binding Priority filter ComboBox.
/// </summary>
public class PriorityDisplayItem
{
    public string Name { get; set; } = string.Empty;
    public PriorityLevel? Value { get; set; }
}