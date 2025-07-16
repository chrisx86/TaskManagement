#nullable enable

namespace TodoApp.WinForms.ViewModels;

/// <summary>
/// A ViewModel used for displaying user information in UI controls like ComboBoxes.
/// This decouples the UI from the core User model and handles display-specific logic (e.g., for 'unassigned').
/// </summary>
public class UserDisplayItem
{
    // The underlying value for the ComboBox item. Null represents 'unassigned'.
    public int? Id { get; set; }

    // The text to be displayed in the ComboBox item.
    public string Username { get; set; } = string.Empty;

    // Optional: Override ToString for easier debugging or simple data binding scenarios.
    public override string ToString()
    {
        return Username;
    }
}