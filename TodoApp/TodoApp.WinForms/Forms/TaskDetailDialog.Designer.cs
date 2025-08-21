#nullable disable
namespace TodoApp.WinForms.Forms
{
    partial class TaskDetailDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Controls.RichTextEditor richTextEditorComments;

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
            btnSave = new Button();
            btnCancel = new Button();
            errorProvider1 = new ErrorProvider(components);
            this.richTextEditorComments = new Controls.RichTextEditor();

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
            // richTextEditorComments (Our new User Control)
            // 
            this.richTextEditorComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextEditorComments.Location = new System.Drawing.Point(120, 202);
            this.richTextEditorComments.Name = "richTextEditorComments";
            this.richTextEditorComments.ReadOnly = false;
            this.richTextEditorComments.Rtf = "{\\rtf1\\ansi\\ansicpg950\\deff0\\nouicompat\\deflang1028{\\fonttbl{\\f0\\fnil\\fcharset136 Microsoft JhengHei UI;}}\r\n{\\*\\generator Riched20 10.0.19041}\\viewkind4\\uc1 \r\n\\pard\\f0\\fs18\\lang1028\\par\r\n}\r\n";
            this.richTextEditorComments.Size = new System.Drawing.Size(444, 179);
            this.richTextEditorComments.TabIndex = 11;
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
            Controls.Add(lblComments);
            Controls.Add(this.richTextEditorComments);
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
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}