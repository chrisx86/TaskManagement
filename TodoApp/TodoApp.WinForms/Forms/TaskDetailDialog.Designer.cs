#nullable disable
namespace TodoApp.WinForms.Forms
{
    partial class TaskDetailDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblPriority;
        private ComboBox cmbPriority;
        private Label lblDueDate;
        private DateTimePicker dtpDueDate;
        private Label lblAssignedTo;
        private ComboBox cmbAssignedTo;
        private Button btnSave;
        private Button btnCancel;
        private Label lblComments;
        private RichTextBox txtComments;
        private ToolStrip commentsFormatToolStrip;
        private ToolStripButton tsBtnBold;
        private ToolStripButton tsBtnItalic;
        private ToolStripButton tsBtnUnderline;
        private ToolStripButton tsBtnSetColorRed;
        private ToolStripButton tsBtnSetColorBlack;
        private ToolStripButton tsBtnBulletList;
        private ToolStripButton tsBtnIndent;
        private ToolStripButton tsBtnOutdent;
        private ToolStripButton tsBtnClearHighlight;
        private Label lblStatus;
        private ComboBox cmbStatus;
        private ErrorProvider errorProvider1;
        private ToolStripButton tsBtnCodeSnippet;
        private ToolStripButton tsBtnMoreColors;
        private ToolStripButton tsBtnHighlight;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            txtTitle = new TextBox();
            lblStatus = new Label();
            cmbStatus = new ComboBox();
            lblPriority = new Label();
            cmbPriority = new ComboBox();
            lblDueDate = new Label();
            dtpDueDate = new DateTimePicker();
            lblAssignedTo = new Label();
            cmbAssignedTo = new ComboBox();
            lblComments = new Label();
            txtComments = new RichTextBox();
            commentsFormatToolStrip = new ToolStrip();
            tsBtnBold = new ToolStripButton();
            tsBtnItalic = new ToolStripButton();
            tsBtnUnderline = new ToolStripButton();
            tsBtnSetColorRed = new ToolStripButton();
            tsBtnSetColorBlack = new ToolStripButton();
            tsBtnBulletList = new ToolStripButton();
            tsBtnIndent = new ToolStripButton();
            tsBtnOutdent = new ToolStripButton();
            tsBtnClearHighlight = new ToolStripButton();
            btnSave = new Button();
            btnCancel = new Button();
            errorProvider1 = new ErrorProvider(components);
            this.tsBtnCodeSnippet = new System.Windows.Forms.ToolStripButton();
            this.tsBtnMoreColors = new System.Windows.Forms.ToolStripButton();
            this.tsBtnHighlight = new System.Windows.Forms.ToolStripButton();

            commentsFormatToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(49, 15);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "標題(&T):";
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(120, 22);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(444, 23);
            txtTitle.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(30, 65);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(49, 15);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "狀態(&S):";
            // 
            // cmbStatus
            // 
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Location = new Point(120, 62);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(140, 23);
            cmbStatus.TabIndex = 3;
            // 
            // lblPriority
            // 
            lblPriority.AutoSize = true;
            lblPriority.Location = new Point(30, 100);
            lblPriority.Name = "lblPriority";
            lblPriority.Size = new Size(61, 15);
            lblPriority.TabIndex = 4;
            lblPriority.Text = "優先級(&P):";
            // 
            // cmbPriority
            // 
            cmbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPriority.FormattingEnabled = true;
            cmbPriority.Location = new Point(120, 97);
            cmbPriority.Name = "cmbPriority";
            cmbPriority.Size = new Size(140, 23);
            cmbPriority.TabIndex = 5;
            // 
            // lblDueDate
            // 
            lblDueDate.AutoSize = true;
            lblDueDate.Location = new Point(30, 135);
            lblDueDate.Name = "lblDueDate";
            lblDueDate.Size = new Size(63, 15);
            lblDueDate.TabIndex = 6;
            lblDueDate.Text = "到期日(&D):";
            // 
            // dtpDueDate
            // 
            dtpDueDate.CustomFormat = "yyyy-MM-dd";
            dtpDueDate.Format = DateTimePickerFormat.Custom;
            dtpDueDate.Location = new Point(120, 132);
            dtpDueDate.Name = "dtpDueDate";
            dtpDueDate.ShowCheckBox = true;
            dtpDueDate.Size = new Size(140, 23);
            dtpDueDate.TabIndex = 7;
            // 
            // lblAssignedTo
            // 
            lblAssignedTo.AutoSize = true;
            lblAssignedTo.Location = new Point(30, 170);
            lblAssignedTo.Name = "lblAssignedTo";
            lblAssignedTo.Size = new Size(62, 15);
            lblAssignedTo.TabIndex = 8;
            lblAssignedTo.Text = "指派給(&A):";
            // 
            // cmbAssignedTo
            // 
            cmbAssignedTo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAssignedTo.FormattingEnabled = true;
            cmbAssignedTo.Location = new Point(120, 167);
            cmbAssignedTo.Name = "cmbAssignedTo";
            cmbAssignedTo.Size = new Size(140, 23);
            cmbAssignedTo.TabIndex = 9;
            // 
            // lblComments
            // 
            lblComments.AutoSize = true;
            lblComments.Location = new Point(30, 205);
            lblComments.Name = "lblComments";
            lblComments.Size = new Size(52, 15);
            lblComments.TabIndex = 10;
            lblComments.Text = "備註(&O):";
            // 
            // txtComments
            // 
            txtComments.AcceptsTab = true;
            txtComments.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtComments.Location = new Point(120, 237);
            txtComments.Name = "txtComments";
            txtComments.Size = new Size(444, 144);
            txtComments.TabIndex = 12;
            txtComments.Text = "";
            // 
            // commentsFormatToolStrip
            // 
            commentsFormatToolStrip.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            commentsFormatToolStrip.Dock = DockStyle.None;
            commentsFormatToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            commentsFormatToolStrip.Items.AddRange(new ToolStripItem[] { tsBtnBold, tsBtnItalic, tsBtnUnderline, tsBtnCodeSnippet, tsBtnSetColorRed, tsBtnSetColorBlack, tsBtnMoreColors, tsBtnHighlight, tsBtnClearHighlight, tsBtnBulletList, tsBtnIndent, tsBtnOutdent });
            commentsFormatToolStrip.Location = new Point(120, 202);
            commentsFormatToolStrip.Name = "commentsFormatToolStrip";
            commentsFormatToolStrip.Size = new Size(400, 28);
            commentsFormatToolStrip.TabIndex = 11;
            commentsFormatToolStrip.Text = "toolStrip1";
            // 
            // tsBtnHighlight
            // 
            this.tsBtnHighlight.BackColor = System.Drawing.Color.Yellow;
            this.tsBtnHighlight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnHighlight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tsBtnHighlight.Name = "tsBtnHighlight";
            this.tsBtnHighlight.Size = new System.Drawing.Size(23, 22);
            this.tsBtnHighlight.Text = "H"; // "H" for Highlight
            this.tsBtnHighlight.ToolTipText = "設定底色 (螢光筆)";
            // 
            // tsBtnCodeSnippet
            // 
            this.tsBtnCodeSnippet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnCodeSnippet.Name = "tsBtnCodeSnippet";
            this.tsBtnCodeSnippet.Size = new System.Drawing.Size(23, 22);
            this.tsBtnCodeSnippet.Text = "{;}"; // A simple text icon representing code
            this.tsBtnCodeSnippet.ToolTipText = "程式碼片段樣式";

            // 
            // tsBtnMoreColors
            // 
            this.tsBtnMoreColors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnMoreColors.Name = "tsBtnMoreColors";
            this.tsBtnMoreColors.Size = new System.Drawing.Size(23, 22);
            this.tsBtnMoreColors.Text = "🎨"; // Using a palette emoji as a visual cue
            this.tsBtnMoreColors.ToolTipText = "更多顏色...";
            // 
            // tsBtnBold
            // 
            tsBtnBold.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnBold.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            tsBtnBold.Name = "tsBtnBold";
            tsBtnBold.Size = new Size(23, 25);
            tsBtnBold.Text = "B";
            tsBtnBold.ToolTipText = "粗體";
            // 
            // tsBtnItalic
            // 
            tsBtnItalic.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnItalic.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            tsBtnItalic.Name = "tsBtnItalic";
            tsBtnItalic.Size = new Size(23, 25);
            tsBtnItalic.Text = "I";
            tsBtnItalic.ToolTipText = "斜體";
            // 
            // tsBtnUnderline
            // 
            tsBtnUnderline.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnUnderline.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            tsBtnUnderline.Name = "tsBtnUnderline";
            tsBtnUnderline.Size = new Size(23, 25);
            tsBtnUnderline.Text = "U";
            tsBtnUnderline.ToolTipText = "底線";
            // 
            // tsBtnSetColorRed
            // 
            tsBtnSetColorRed.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnSetColorRed.ForeColor = Color.Red;
            tsBtnSetColorRed.Name = "tsBtnSetColorRed";
            tsBtnSetColorRed.Size = new Size(23, 25);
            tsBtnSetColorRed.Text = "A";
            tsBtnSetColorRed.ToolTipText = "紅色文字";
            // 
            // tsBtnSetColorBlack
            // 
            tsBtnSetColorBlack.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnSetColorBlack.ForeColor = Color.Black;
            tsBtnSetColorBlack.Name = "tsBtnSetColorBlack";
            tsBtnSetColorBlack.Size = new Size(23, 25);
            tsBtnSetColorBlack.Text = "A";
            tsBtnSetColorBlack.ToolTipText = "黑色文字 (預設)";
            // 
            // tsBtnBulletList
            // 
            tsBtnBulletList.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnBulletList.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tsBtnBulletList.Name = "tsBtnBulletList";
            tsBtnBulletList.Size = new Size(23, 25);
            tsBtnBulletList.Text = "•";
            tsBtnBulletList.ToolTipText = "項目符號";
            // 
            // tsBtnIndent
            // 
            tsBtnIndent.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnIndent.Font = new Font("Segoe UI", 9F);
            tsBtnIndent.Name = "tsBtnIndent";
            tsBtnIndent.Size = new Size(23, 25);
            tsBtnIndent.Text = "→";
            tsBtnIndent.ToolTipText = "增加縮排";
            // 
            // tsBtnOutdent
            // 
            tsBtnOutdent.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnOutdent.Font = new Font("Segoe UI", 9F);
            tsBtnOutdent.Name = "tsBtnOutdent";
            tsBtnOutdent.Size = new Size(23, 25);
            tsBtnOutdent.Text = "←";
            tsBtnOutdent.ToolTipText = "減少縮排";
            // 
            // tsBtnClearHighlight
            // 
            tsBtnClearHighlight.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnClearHighlight.Font = new Font("Segoe UI", 9F, FontStyle.Strikeout);
            tsBtnClearHighlight.Name = "tsBtnClearHighlight";
            tsBtnClearHighlight.Size = new Size(23, 25);
            tsBtnClearHighlight.Text = "C";
            tsBtnClearHighlight.ToolTipText = "清除底色標示";
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(408, 387);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 13;
            btnSave.Text = "儲存(&S)";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(489, 387);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 14;
            btnCancel.Text = "取消(&C)";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // TaskDetailDialog
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(596, 440);
            Controls.Add(cmbStatus);
            Controls.Add(lblStatus);
            Controls.Add(commentsFormatToolStrip);
            Controls.Add(txtComments);
            Controls.Add(lblComments);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(cmbAssignedTo);
            Controls.Add(lblAssignedTo);
            Controls.Add(dtpDueDate);
            Controls.Add(lblDueDate);
            Controls.Add(cmbPriority);
            Controls.Add(lblPriority);
            Controls.Add(txtTitle);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TaskDetailDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "任務詳細資料";
            commentsFormatToolStrip.ResumeLayout(false);
            commentsFormatToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

    }
}