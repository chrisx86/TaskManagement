#nullable disable

namespace TodoApp.WinForms.Controls
{
    partial class RichTextEditor
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ToolStrip commentsFormatToolStrip;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.ToolStripButton tsBtnBold;
        private System.Windows.Forms.ToolStripButton tsBtnItalic;
        private System.Windows.Forms.ToolStripButton tsBtnUnderline;
        private System.Windows.Forms.ToolStripButton tsBtnStrikeout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsBtnSetColorRed;
        private System.Windows.Forms.ToolStripButton tsBtnSetColorBlack;
        private System.Windows.Forms.ToolStripButton tsBtnMoreColors;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsBtnBulletList;
        private System.Windows.Forms.ToolStripButton tsBtnIndent;
        private System.Windows.Forms.ToolStripButton tsBtnOutdent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsBtnHighlight;
        private System.Windows.Forms.ToolStripButton tsBtnClearHighlight;
        private System.Windows.Forms.ToolStripButton tsBtnCodeSnippet;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.commentsFormatToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsBtnBold = new System.Windows.Forms.ToolStripButton();
            this.tsBtnItalic = new System.Windows.Forms.ToolStripButton();
            this.tsBtnUnderline = new System.Windows.Forms.ToolStripButton();
            this.tsBtnStrikeout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnSetColorRed = new System.Windows.Forms.ToolStripButton();
            this.tsBtnSetColorBlack = new System.Windows.Forms.ToolStripButton();
            this.tsBtnMoreColors = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnBulletList = new System.Windows.Forms.ToolStripButton();
            this.tsBtnIndent = new System.Windows.Forms.ToolStripButton();
            this.tsBtnOutdent = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnHighlight = new System.Windows.Forms.ToolStripButton();
            this.tsBtnClearHighlight = new System.Windows.Forms.ToolStripButton();
            this.tsBtnCodeSnippet = new System.Windows.Forms.ToolStripButton();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.commentsFormatToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // commentsFormatToolStrip
            // 
            this.commentsFormatToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.commentsFormatToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsBtnBold,
                this.tsBtnItalic,
                this.tsBtnUnderline,
                this.tsBtnStrikeout,
                this.toolStripSeparator1,
                this.tsBtnSetColorRed,
                this.tsBtnSetColorBlack,
                this.tsBtnMoreColors,
                this.toolStripSeparator2,
                this.tsBtnHighlight,
                this.tsBtnClearHighlight,
                this.tsBtnCodeSnippet,
                this.toolStripSeparator3,
                this.tsBtnBulletList,
                this.tsBtnIndent,
                this.tsBtnOutdent,
            });
            this.commentsFormatToolStrip.Location = new System.Drawing.Point(0, 0);
            this.commentsFormatToolStrip.Name = "commentsFormatToolStrip";
            this.commentsFormatToolStrip.Size = new System.Drawing.Size(500, 25);
            this.commentsFormatToolStrip.TabIndex = 0;
            this.commentsFormatToolStrip.Text = "toolStrip1";
            // 
            // tsBtnBold
            // 
            this.tsBtnBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnBold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tsBtnBold.Name = "tsBtnBold";
            this.tsBtnBold.Size = new System.Drawing.Size(23, 22);
            this.tsBtnBold.Text = "B";
            this.tsBtnBold.ToolTipText = "粗體";
            // 
            // tsBtnItalic
            // 
            this.tsBtnItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnItalic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.tsBtnItalic.Name = "tsBtnItalic";
            this.tsBtnItalic.Size = new System.Drawing.Size(23, 22);
            this.tsBtnItalic.Text = "I";
            this.tsBtnItalic.ToolTipText = "斜體";
            // 
            // tsBtnUnderline
            // 
            this.tsBtnUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnUnderline.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.tsBtnUnderline.Name = "tsBtnUnderline";
            this.tsBtnUnderline.Size = new System.Drawing.Size(23, 22);
            this.tsBtnUnderline.Text = "U";
            this.tsBtnUnderline.ToolTipText = "底線";
            // 
            // tsBtnStrikeout
            // 
            this.tsBtnStrikeout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnStrikeout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Strikeout);
            this.tsBtnStrikeout.Name = "tsBtnStrikeout";
            this.tsBtnStrikeout.Size = new System.Drawing.Size(23, 22);
            this.tsBtnStrikeout.Text = "S";
            this.tsBtnStrikeout.ToolTipText = "刪除線";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBtnSetColorRed
            // 
            this.tsBtnSetColorRed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnSetColorRed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tsBtnSetColorRed.ForeColor = System.Drawing.Color.Red;
            this.tsBtnSetColorRed.Name = "tsBtnSetColorRed";
            this.tsBtnSetColorRed.Size = new System.Drawing.Size(23, 22);
            this.tsBtnSetColorRed.Text = "A";
            this.tsBtnSetColorRed.ToolTipText = "紅色文字";
            // 
            // tsBtnSetColorBlack
            // 
            this.tsBtnSetColorBlack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnSetColorBlack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tsBtnSetColorBlack.ForeColor = System.Drawing.Color.Black;
            this.tsBtnSetColorBlack.Name = "tsBtnSetColorBlack";
            this.tsBtnSetColorBlack.Size = new System.Drawing.Size(23, 22);
            this.tsBtnSetColorBlack.Text = "A";
            this.tsBtnSetColorBlack.ToolTipText = "黑色文字 (預設)";
            // 
            // tsBtnMoreColors
            // 
            this.tsBtnMoreColors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnMoreColors.Name = "tsBtnMoreColors";
            this.tsBtnMoreColors.Size = new System.Drawing.Size(23, 22);
            this.tsBtnMoreColors.Text = "🎨";
            this.tsBtnMoreColors.ToolTipText = "更多顏色...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBtnBulletList
            // 
            this.tsBtnBulletList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnBulletList.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.tsBtnBulletList.Name = "tsBtnBulletList";
            this.tsBtnBulletList.Size = new System.Drawing.Size(23, 22);
            this.tsBtnBulletList.Text = "•";
            this.tsBtnBulletList.ToolTipText = "項目符號";
            // 
            // tsBtnIndent
            // 
            this.tsBtnIndent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnIndent.Name = "tsBtnIndent";
            this.tsBtnIndent.Size = new System.Drawing.Size(23, 22);
            this.tsBtnIndent.Text = "➡";
            this.tsBtnIndent.ToolTipText = "增加縮排";
            // 
            // tsBtnOutdent
            // 
            this.tsBtnOutdent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnOutdent.Name = "tsBtnOutdent";
            this.tsBtnOutdent.Size = new System.Drawing.Size(23, 22);
            this.tsBtnOutdent.Text = "⬅";
            this.tsBtnOutdent.ToolTipText = "減少縮排";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBtnHighlight
            // 
            this.tsBtnHighlight.BackColor = System.Drawing.Color.Yellow;
            this.tsBtnHighlight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnHighlight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tsBtnHighlight.Name = "tsBtnHighlight";
            this.tsBtnHighlight.Size = new System.Drawing.Size(23, 22);
            this.tsBtnHighlight.Text = "H";
            this.tsBtnHighlight.ToolTipText = "設定底色 (螢光筆)";
            // 
            // tsBtnClearHighlight
            // 
            this.tsBtnClearHighlight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnClearHighlight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Strikeout);
            this.tsBtnClearHighlight.Name = "tsBtnClearHighlight";
            this.tsBtnClearHighlight.Size = new System.Drawing.Size(23, 22);
            this.tsBtnClearHighlight.Text = "H";
            this.tsBtnClearHighlight.ToolTipText = "清除底色標示";
            // 
            // tsBtnCodeSnippet
            // 
            this.tsBtnCodeSnippet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnCodeSnippet.Name = "tsBtnCodeSnippet";
            this.tsBtnCodeSnippet.Size = new System.Drawing.Size(27, 22);
            this.tsBtnCodeSnippet.Text = "{;}";
            this.tsBtnCodeSnippet.ToolTipText = "程式碼片段樣式";
            // 
            // richTextBox
            // 
            this.richTextBox.AcceptsTab = true;
            this.richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox.Location = new System.Drawing.Point(0, 25);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(500, 275);
            this.richTextBox.TabIndex = 1;
            this.richTextBox.Text = "";
            // 
            // RichTextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.commentsFormatToolStrip);
            this.Name = "RichTextEditor";
            this.Size = new System.Drawing.Size(500, 300);
            this.commentsFormatToolStrip.ResumeLayout(false);
            this.commentsFormatToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

    }
}