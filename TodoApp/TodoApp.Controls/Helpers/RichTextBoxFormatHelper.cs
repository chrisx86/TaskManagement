#nullable enable
namespace TodoApp.Controls;

/// <summary>
/// Provides extension methods for RichTextBox to simplify text formatting operations.
/// This centralizes the formatting logic for reuse across different forms.
/// </summary>
public static class RichTextBoxFormatHelper
{
    private const int IndentSize = 10;
    // 
    // --- Style 0: Classic Dark (Visual Studio Code) ---
    private const string CodeFontFamily = "Consolas";
    private static readonly Color CodeBackColor = Color.FromArgb(45, 45, 48);
    private static readonly Color CodeForeColor = Color.FromArgb(210, 210, 210);

    // --- Style A: Classic Light (GitHub Style) --- x
    //private const string CodeFontFamily = "Consolas";
    //// A very light gray, almost off-white background.
    //private static readonly Color CodeBackColor = Color.FromArgb(246, 248, 250);
    //// A dark, slightly soft black for the text.
    //private static readonly Color CodeForeColor = Color.FromArgb(36, 41, 47);

    // --- Style B: High-Contrast Blue ---
    //private const string CodeFontFamily = "Consolas";
    //// A deep, rich blue background.
    //private static readonly Color CodeBackColor = Color.FromArgb(1, 36, 86);
    //// A bright, off-white text color for high contrast.
    //private static readonly Color CodeForeColor = Color.FromArgb(230, 230, 230);

    // --- Style C: Soft Sepia/Paper ---
    //private const string CodeFontFamily = "Courier New"; // Courier New fits the "typewriter" feel
    //// A soft, creamy beige background.
    //private static readonly Color CodeBackColor = Color.FromArgb(253, 246, 227);
    //// A dark brown text color, softer than pure black.
    //private static readonly Color CodeForeColor = Color.FromArgb(88, 80, 68);

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

    /// <summary>
    /// Applies a pre-defined "code snippet" style to the selected text.
    /// This uses a monospace font and a distinct background color.
    /// </summary>
    public static void ApplyCodeSnippetStyle(this RichTextBox rtb)
    {
        // Use a try-catch to handle cases where the primary font is not installed.
        Font codeFont;
        try
        {
            codeFont = new Font("Consolas", rtb.Font.Size);
        }
        catch
        {
            codeFont = new Font("Courier New", rtb.Font.Size);
        }

        rtb.SuspendLayout();

        rtb.SelectionFont = codeFont;
        rtb.SelectionColor = Color.FromArgb(210, 210, 210); // A light gray text color
        rtb.SelectionBackColor = Color.FromArgb(45, 45, 48);   // A dark background, similar to VS Code

        rtb.ResumeLayout();
    }

    /// <summary>
    /// Opens a ColorDialog to allow the user to choose a custom color for the selected text.
    /// </summary>
    public static void ShowTextColorPicker(this RichTextBox rtb)
    {
        using (var colorDialog = new ColorDialog())
        {
            colorDialog.Color = rtb.SelectionColor;
            // Show the dialog and apply the color if the user clicks OK.
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                rtb.SelectionColor = colorDialog.Color;
            }
        }
    }

    /// <summary>
    /// Toggles a pre-defined "code snippet" style for the selected text.
    /// If the selection is already in code style, it reverts to the default style.
    /// </summary>
    public static void ToggleCodeSnippetStyle(this RichTextBox rtb)
    {
        if (rtb.SelectionLength == 0 && rtb.SelectionFont == null) return;

        // --- Step 1: Check if the current selection is ALREADY in code style ---
        // We check the font family and background color of the first character in the selection.
        bool isCurrentlyCodeStyle =
            rtb.SelectionFont?.FontFamily.Name == CodeFontFamily &&
            rtb.SelectionBackColor == CodeBackColor;

        rtb.SuspendLayout();

        if (isCurrentlyCodeStyle)
        {
            // --- Step 2a: If it is code style, REVERT to the default style ---
            // Revert to the RichTextBox's default font and colors.
            rtb.SelectionFont = rtb.Font;
            rtb.SelectionColor = rtb.ForeColor;
            rtb.SelectionBackColor = rtb.BackColor;
        }
        else
        {
            // --- Step 2b: If it's not code style, APPLY the code style ---
            Font codeFont;
            try
            {
                codeFont = new Font(CodeFontFamily, rtb.Font.Size);
            }
            catch
            {
                codeFont = new Font("Courier New", rtb.Font.Size); // Fallback
            }

            rtb.SelectionFont = codeFont;
            rtb.SelectionColor = CodeForeColor;
            rtb.SelectionBackColor = CodeBackColor;
        }

        rtb.ResumeLayout();
    }

    /// <summary>
    /// Opens a ColorDialog to allow the user to choose a custom background color (highlight) for the selected text.
    /// </summary>
    public static void ShowBackColorPicker(this RichTextBox rtb)
    {
        using (var colorDialog = new ColorDialog())
        {
            // Set the dialog's initial color to the current selection's background color.
            // If the current BackColor is the default, start with Yellow for a better UX.
            colorDialog.Color = (rtb.SelectionBackColor == rtb.BackColor)
                ? Color.Yellow
                : rtb.SelectionBackColor;

            // Show the dialog and apply the color if the user clicks OK.
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                rtb.SelectionBackColor = colorDialog.Color;
            }
        }
    }

}