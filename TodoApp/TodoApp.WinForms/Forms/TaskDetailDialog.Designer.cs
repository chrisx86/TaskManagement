#nullable disable

namespace TodoApp.WinForms.Forms
{
    partial class TaskDetailDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            txtComments = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            errorProvider1 = new ErrorProvider(components);
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
            txtComments.AcceptsReturn = true;
            txtComments.Location = new Point(120, 202);
            txtComments.Multiline = true;
            txtComments.Name = "txtComments";
            txtComments.ScrollBars = ScrollBars.Vertical;
            txtComments.Size = new Size(444, 120);
            txtComments.TabIndex = 11;
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
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblPriority;
        private System.Windows.Forms.ComboBox cmbPriority;
        private System.Windows.Forms.Label lblDueDate;
        private System.Windows.Forms.DateTimePicker dtpDueDate;
        private System.Windows.Forms.Label lblAssignedTo;
        private System.Windows.Forms.ComboBox cmbAssignedTo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblComments;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}