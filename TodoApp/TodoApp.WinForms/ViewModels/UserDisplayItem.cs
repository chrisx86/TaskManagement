#nullable enable

namespace TodoApp.WinForms.ViewModels;

/// <summary>
/// A ViewModel used for displaying user information in UI controls like ComboBoxes.
/// This decouples the UI from the core User model and handles display-specific logic (e.g., for 'unassigned').
/// </summary>
public class UserDisplayItem
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
}