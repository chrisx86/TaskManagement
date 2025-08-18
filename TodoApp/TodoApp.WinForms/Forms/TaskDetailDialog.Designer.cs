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
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsBtnSetColorRed;
        private ToolStripButton tsBtnSetColorBlue;
        private ToolStripButton tsBtnSetColorGreen;
        private ToolStripButton tsBtnSetColorBlack;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tsBtnBulletList;
        private ToolStripButton tsBtnIndent;
        private ToolStripButton tsBtnOutdent;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton tsBtnHighlightYellow;
        private ToolStripButton tsBtnHighlightGreen;
        private ToolStripButton tsBtnClearHighlight;
        private Label lblStatus;
        private ComboBox cmbStatus;
        private ErrorProvider errorProvider1;
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
            toolStripSeparator1 = new ToolStripSeparator();
            tsBtnSetColorRed = new ToolStripButton();
            tsBtnSetColorBlack = new ToolStripButton();
            this.tsBtnSetColorBlue = new System.Windows.Forms.ToolStripButton();
            this.tsBtnSetColorGreen = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            tsBtnBulletList = new ToolStripButton();
            this.tsBtnIndent = new System.Windows.Forms.ToolStripButton();
            this.tsBtnOutdent = new System.Windows.Forms.ToolStripButton();

            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnHighlightYellow = new System.Windows.Forms.ToolStripButton();
            this.tsBtnHighlightGreen = new System.Windows.Forms.ToolStripButton();
            this.tsBtnClearHighlight = new System.Windows.Forms.ToolStripButton();
            btnSave = new Button();
            btnCancel = new Button();
            errorProvider1 = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            commentsFormatToolStrip.SuspendLayout();
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
            txtComments.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            txtComments.Location = new Point(120, 237);
            txtComments.Multiline = true;
            txtComments.Name = "txtComments";
            txtComments.Size = new Size(444, 120);
            txtComments.TabIndex = 12;
            // 
            // commentsFormatToolStrip
            // 
            commentsFormatToolStrip.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            commentsFormatToolStrip.Dock = DockStyle.None;
            commentsFormatToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            commentsFormatToolStrip.Items.AddRange(new ToolStripItem[] {
                tsBtnBold,
                tsBtnItalic,
                tsBtnUnderline,
                toolStripSeparator1,
                tsBtnSetColorRed,
                tsBtnSetColorBlue,
                tsBtnSetColorGreen,
                tsBtnSetColorBlack,
                toolStripSeparator2,
                tsBtnBulletList,
                tsBtnIndent,
                tsBtnOutdent,
                toolStripSeparator3,
                tsBtnHighlightYellow,
                tsBtnHighlightGreen,
                tsBtnClearHighlight
            });
            commentsFormatToolStrip.Location = new Point(120, 202);
            commentsFormatToolStrip.Name = "commentsFormatToolStrip";
            commentsFormatToolStrip.Size = new Size(444, 25);
            commentsFormatToolStrip.TabIndex = 11;
            commentsFormatToolStrip.Text = "toolStrip1";
            // 
            // tsBtnBold
            // 
            tsBtnBold.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnBold.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            tsBtnBold.Name = "tsBtnBold";
            tsBtnBold.Size = new Size(23, 22);
            tsBtnBold.Text = "B";
            tsBtnBold.ToolTipText = "粗體";
            // 
            // tsBtnItalic
            // 
            tsBtnItalic.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnItalic.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            tsBtnItalic.Name = "tsBtnItalic";
            tsBtnItalic.Size = new Size(23, 22);
            tsBtnItalic.Text = "I";
            tsBtnItalic.ToolTipText = "斜體";
            // 
            // tsBtnUnderline
            // 
            tsBtnUnderline.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnUnderline.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            tsBtnUnderline.Name = "tsBtnUnderline";
            tsBtnUnderline.Size = new Size(23, 22);
            tsBtnUnderline.Text = "U";
            tsBtnUnderline.ToolTipText = "底線";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // tsBtnSetColorRed
            // 
            tsBtnSetColorRed.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnSetColorRed.ForeColor = Color.Red;
            tsBtnSetColorRed.Name = "tsBtnSetColorRed";
            tsBtnSetColorRed.Size = new Size(23, 22);
            tsBtnSetColorRed.Text = "A";
            tsBtnSetColorRed.ToolTipText = "紅色文字";
            // 
            // tsBtnSetColorBlack
            // 
            tsBtnSetColorBlack.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnSetColorBlack.ForeColor = Color.Black;
            tsBtnSetColorBlack.Name = "tsBtnSetColorBlack";
            tsBtnSetColorBlack.Size = new Size(23, 22);
            tsBtnSetColorBlack.Text = "A";
            tsBtnSetColorBlack.ToolTipText = "黑色文字 (預設)";
            // tsBtnSetColorBlue
            tsBtnSetColorBlue.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsBtnSetColorBlue.ForeColor = Color.Blue;
            tsBtnSetColorBlue.Name = "tsBtnSetColorBlue";
            tsBtnSetColorBlue.Size = new Size(23, 22);
            tsBtnSetColorBlue.Text = "A";
            tsBtnSetColorBlue.ToolTipText = "藍色文字";
            // 
            // tsBtnSetColorGreen
            // 
            this.tsBtnSetColorGreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnSetColorGreen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tsBtnSetColorGreen.ForeColor = System.Drawing.Color.Green;
            this.tsBtnSetColorGreen.Name = "tsBtnSetColorGreen";
            this.tsBtnSetColorGreen.Size = new System.Drawing.Size(23, 22);
            this.tsBtnSetColorGreen.Text = "A";
            this.tsBtnSetColorGreen.ToolTipText = "綠色文字";
            //
            // toolStripSeparator2
            //
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // tsBtnBulletList
            // 
            this.tsBtnBulletList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnBulletList.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point); // Use a larger font for better visibility
            this.tsBtnBulletList.Name = "tsBtnBulletList";
            this.tsBtnBulletList.Size = new System.Drawing.Size(23, 22);
            this.tsBtnBulletList.Text = "•"; // Using a solid bullet character (Alt+7 on numpad)
            this.tsBtnBulletList.ToolTipText = "項目符號";

            // 
            // tsBtnIndent
            // 
            this.tsBtnIndent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnIndent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tsBtnIndent.Name = "tsBtnIndent";
            this.tsBtnIndent.Size = new System.Drawing.Size(23, 22);
            this.tsBtnIndent.Text = "→"; // Using right arrow character
            this.tsBtnIndent.ToolTipText = "增加縮排";
            // 
            // tsBtnOutdent
            // 
            this.tsBtnOutdent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnOutdent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tsBtnOutdent.Name = "tsBtnOutdent";
            this.tsBtnOutdent.Size = new System.Drawing.Size(23, 22);
            this.tsBtnOutdent.Text = "←"; // Using left arrow character
            this.tsBtnOutdent.ToolTipText = "減少縮排";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBtnHighlightYellow
            // 
            this.tsBtnHighlightYellow.BackColor = System.Drawing.Color.Yellow;
            this.tsBtnHighlightYellow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image; // Show as a color block
            this.tsBtnHighlightYellow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnHighlightYellow.Name = "tsBtnHighlightYellow";
            this.tsBtnHighlightYellow.Size = new System.Drawing.Size(23, 22);
            this.tsBtnHighlightYellow.Text = "Yellow Highlight";
            this.tsBtnHighlightYellow.ToolTipText = "黃色螢光筆";
            // 
            // tsBtnHighlightGreen
            // 
            this.tsBtnHighlightGreen.BackColor = System.Drawing.Color.LightGreen;
            this.tsBtnHighlightGreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnHighlightGreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnHighlightGreen.Name = "tsBtnHighlightGreen";
            this.tsBtnHighlightGreen.Size = new System.Drawing.Size(23, 22);
            this.tsBtnHighlightGreen.Text = "Green Highlight";
            this.tsBtnHighlightGreen.ToolTipText = "綠色螢光筆";
            // 
            // tsBtnClearHighlight
            // 
            this.tsBtnClearHighlight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnClearHighlight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Strikeout);
            this.tsBtnClearHighlight.Name = "tsBtnClearHighlight";
            this.tsBtnClearHighlight.Size = new System.Drawing.Size(23, 22);
            this.tsBtnClearHighlight.Text = "C";
            this.tsBtnClearHighlight.ToolTipText = "清除底色標示";

            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(408, 336);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 12;
            btnSave.Text = "儲存(&S)";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(489, 336);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 13;
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
            ClientSize = new Size(596, 381);
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