#nullable enable
namespace TodoApp.WinForms.Helpers;

/// <summary>
/// Provides extension methods for RichTextBox to simplify text formatting operations.
/// This centralizes the formatting logic for reuse across different forms.
/// </summary>
public static class RichTextBoxFormatHelper
{
    private const int IndentSize = 20;

    /// <summary>
    /// Toggles the specified font style (e.g., Bold, Italic) for the selected text.
    /// </summary>
    public static void ToggleFontStyle(this RichTextBox rtb, FontStyle style)
    {
        if (rtb.SelectionFont == null) return;

        // Use XOR to toggle the style bit.
        var newStyle = rtb.SelectionFont.Style ^ style;
        rtb.SelectionFont = new Font(rtb.SelectionFont, newStyle);
    }

    /// <summary>
    /// Sets the foreground color for the selected text.
    /// </summary>
    public static void SetSelectionColor(this RichTextBox rtb, Color color)
    {
        rtb.SelectionColor = color;
    }

    /// <summary>
    /// Sets the background color (highlight) for the selected text.
    /// </summary>
    public static void SetSelectionBackColor(this RichTextBox rtb, Color color)
    {
        rtb.SelectionBackColor = color;
    }

    /// <summary>
    /// Clears the background color (highlight) for the selected text.
    /// </summary>
    public static void ClearSelectionBackColor(this RichTextBox rtb)
    {
        rtb.SelectionBackColor = rtb.BackColor;
    }

    /// <summary>
    /// Toggles bullet points for the selected paragraph(s).
    /// </summary>
    public static void ToggleBullet(this RichTextBox rtb)
    {
        rtb.SelectionBullet = !rtb.SelectionBullet;
    }

    /// <summary>
    /// Increases the indentation for the selected paragraph(s).
    /// </summary>
    public static void IncreaseIndent(this RichTextBox rtb)
    {
        rtb.SelectionIndent += IndentSize;
    }

    /// <summary>
    /// Decreases the indentation for the selected paragraph(s).
    /// </summary>
    public static void DecreaseIndent(this RichTextBox rtb)
    {
        rtb.SelectionIndent = Math.Max(0, rtb.SelectionIndent - IndentSize);
    }
}