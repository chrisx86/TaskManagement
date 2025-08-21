#nullable enable
using System.ComponentModel;

namespace TodoApp.Controls;

/// <summary>
/// A reusable user control that encapsulates a RichTextBox with a formatting ToolStrip.
/// </summary>
public partial class RichTextEditor : UserControl
{
    // --- Step 3a: Define a public event ---
    [Category("Action")]
    [Description("Occurs when the text in the RichTextBox is changed.")]
    public event EventHandler? TextChanged;

    [Category("Behavior")]
    [Description("Determines if the format toolbar automatically hides when focus is lost.")]
    [DefaultValue(true)]
    public bool AutoHideToolbar { get; set; } = true;

    public RichTextEditor()
    {
        InitializeComponent();
        WireUpFormattingEvents();
        //this.richTextBox.Enter += (s, e) => { this.commentsFormatToolStrip.Visible = true; };
        //this.richTextBox.Leave += (s, e) => { this.commentsFormatToolStrip.Visible = false; };
        this.richTextBox.TextChanged += (s, e) =>
        {
            this.TextChanged?.Invoke(this, e);
        };
    }

    private void RichTextBox_Leave(object? sender, EventArgs e)
    {
        if (this.AutoHideToolbar)
        {
            this.commentsFormatToolStrip.Visible = false;
        }
    }

    [Category("Appearance")]
    [Description("The text content of the control.")]
    public override string Text
    {
        get => richTextBox.Text;
        set => richTextBox.Text = value;
    }

    [Category("Appearance")]
    [Description("The text content of the control in Rich Text Format.")]
    public string Rtf
    {
        get => richTextBox.Rtf;
        set
        {
            try { richTextBox.Rtf = value; }
            catch { richTextBox.Text = value; } // Fallback for plain text
        }
    }

    /// <summary>
    /// Gets the length of the text in the control.
    /// </summary>
    [Browsable(false)] // Hide this property from the designer's property grid
    public int TextLength
    {
        get => richTextBox.TextLength;
    }

    // You can expose other properties like ReadOnly, etc. in the same way.
    [Category("Behavior")]
    public bool ReadOnly
    {
        get => richTextBox.ReadOnly;
        set => richTextBox.ReadOnly = value;
    }

    /// <summary>
    /// Wires up the Click events for all ToolStripButtons to their corresponding
    /// formatting actions, which are defined as extension methods in RichTextBoxFormatHelper.
    /// </summary>
    private void WireUpFormattingEvents()
    {
        // --- Group 1: Font Style ---
        tsBtnBold.Click += (s, e) => richTextBox.ToggleFontStyle(FontStyle.Bold);
        tsBtnItalic.Click += (s, e) => richTextBox.ToggleFontStyle(FontStyle.Italic);
        tsBtnUnderline.Click += (s, e) => richTextBox.ToggleFontStyle(FontStyle.Underline);
        tsBtnStrikeout.Click += (s, e) => richTextBox.ToggleFontStyle(FontStyle.Strikeout);

        // --- Group 2: Font Color ---
        tsBtnSetColorRed.Click += (s, e) => richTextBox.SetSelectionColor(Color.Red);
        tsBtnSetColorBlack.Click += (s, e) => richTextBox.SetSelectionColor(Color.Black);
        tsBtnMoreColors.Click += (s, e) => richTextBox.ShowTextColorPicker();

        // --- Group 3: Paragraph Formatting ---
        tsBtnBulletList.Click += (s, e) => richTextBox.ToggleBullet();
        tsBtnIndent.Click += (s, e) => richTextBox.IncreaseIndent();
        tsBtnOutdent.Click += (s, e) => richTextBox.DecreaseIndent();

        // --- Group 4: Highlighting ---
        tsBtnHighlight.Click += (s, e) => richTextBox.ShowBackColorPicker();
        tsBtnClearHighlight.Click += (s, e) => richTextBox.ClearSelectionBackColor();

        // --- Group 5: Code Snippet ---
        tsBtnCodeSnippet.Click += (s, e) => richTextBox.ToggleCodeSnippetStyle();

    }

    public void Clear()
    {
        richTextBox.Clear();
    }
}