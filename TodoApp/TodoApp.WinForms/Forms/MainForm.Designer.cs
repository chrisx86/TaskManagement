#nullable disable

namespace TodoApp.WinForms.Forms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNewTask = new System.Windows.Forms.ToolStripButton();
            this.tsbEditTask = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteTask = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUserManagement = new System.Windows.Forms.ToolStripButton();
            this.tsbAdminDashboard = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbChangePassword = new System.Windows.Forms.ToolStripButton();
            this.tsbSwitchUser = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.lblFilterByAssignedUser = new System.Windows.Forms.Label();
            this.cmbFilterByAssignedUser = new System.Windows.Forms.ComboBox();
            this.cmbFilterByUserRelation = new System.Windows.Forms.ComboBox();
            this.lblFilterUser = new System.Windows.Forms.Label();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.dgvTasks = new System.Windows.Forms.DataGridView();
            this.txtCommentsPreview = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panelFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).BeginInit();
            this.SuspendLayout();

            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewTask,
            this.tsbEditTask,
            this.tsbDeleteTask,
            this.toolStripSeparator1,
            this.tsbRefresh,
            this.toolStripSeparator2,
            this.tsbUserManagement,
            this.tsbAdminDashboard,
            this.toolStripSeparator3,
            this.tsbChangePassword,
            this.tsbSwitchUser});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1008, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNewTask
            // 
            this.tsbNewTask.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewTask.Image")));
            this.tsbNewTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewTask.Name = "tsbNewTask";
            this.tsbNewTask.Size = new System.Drawing.Size(59, 24);
            this.tsbNewTask.Text = "新增";
            // 
            // tsbEditTask
            // 
            this.tsbEditTask.Image = ((System.Drawing.Image)(resources.GetObject("tsbEditTask.Image")));
            this.tsbEditTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditTask.Name = "tsbEditTask";
            this.tsbEditTask.Size = new System.Drawing.Size(59, 24);
            this.tsbEditTask.Text = "編輯";
            // 
            // tsbDeleteTask
            // 
            this.tsbDeleteTask.Image = ((System.Drawing.Image)(resources.GetObject("tsbDeleteTask.Image")));
            this.tsbDeleteTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteTask.Name = "tsbDeleteTask";
            this.tsbDeleteTask.Size = new System.Drawing.Size(59, 24);
            this.tsbDeleteTask.Text = "刪除";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsbRefresh.Image")));
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(83, 24);
            this.tsbRefresh.Text = "重新整理";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbUserManagement
            // 
            this.tsbUserManagement.Image = ((System.Drawing.Image)(resources.GetObject("tsbUserManagement.Image")));
            this.tsbUserManagement.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUserManagement.Name = "tsbUserManagement";
            this.tsbUserManagement.Size = new System.Drawing.Size(95, 24);
            this.tsbUserManagement.Text = "使用者管理";
            this.tsbUserManagement.Visible = false;
            // 
            // tsbAdminDashboard
            // 
            this.tsbAdminDashboard.Image = ((System.Drawing.Image)(resources.GetObject("tsbAdminDashboard.Image")));
            this.tsbAdminDashboard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAdminDashboard.Name = "tsbAdminDashboard";
            this.tsbAdminDashboard.Size = new System.Drawing.Size(95, 24);
            this.tsbAdminDashboard.Text = "管理儀表板";
            this.tsbAdminDashboard.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 659);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(43, 17);
            this.lblStatus.Text = "Ready";
            // 
            // panelFilters
            // 
            this.panelFilters.BackColor = System.Drawing.SystemColors.Control;
            this.panelFilters.Controls.Add(this.lblFilterByAssignedUser);
            this.panelFilters.Controls.Add(this.cmbFilterByAssignedUser);
            this.panelFilters.Controls.Add(this.cmbFilterByUserRelation);
            this.panelFilters.Controls.Add(this.lblFilterUser);
            this.panelFilters.Controls.Add(this.cmbFilterStatus);
            this.panelFilters.Controls.Add(this.lblFilterStatus);
            // --- THIS IS THE CRUCIAL FIX for the layout ---
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top; // Ensure it docks to the top
            this.panelFilters.Location = new System.Drawing.Point(0, 27); // Position it below the ToolStrip
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new System.Drawing.Size(1184, 40); // Give it a fixed height
            this.panelFilters.TabIndex = 2;
            this.panelFilters.Visible = true; // Ensure it's visible
            // 
            // lblFilterByAssignedUser
            // 
            this.lblFilterByAssignedUser.AutoSize = true;
            this.lblFilterByAssignedUser.Location = new System.Drawing.Point(370, 12);
            this.lblFilterByAssignedUser.Name = "lblFilterByAssignedUser";
            this.lblFilterByAssignedUser.Size = new System.Drawing.Size(55, 15);
            this.lblFilterByAssignedUser.TabIndex = 5;
            this.lblFilterByAssignedUser.Text = "指派給：";
            // 
            // cmbFilterByAssignedUser
            // 
            this.cmbFilterByAssignedUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterByAssignedUser.FormattingEnabled = true;
            this.cmbFilterByAssignedUser.Location = new System.Drawing.Point(431, 9);
            this.cmbFilterByAssignedUser.Name = "cmbFilterByAssignedUser";
            this.cmbFilterByAssignedUser.Size = new System.Drawing.Size(140, 23);
            this.cmbFilterByAssignedUser.TabIndex = 4;
            // 
            // cmbFilterByUserRelation
            // 
            this.cmbFilterByUserRelation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterByUserRelation.FormattingEnabled = true;
            this.cmbFilterByUserRelation.Location = new System.Drawing.Point(220, 9);
            this.cmbFilterByUserRelation.Name = "cmbFilterByUserRelation";
            this.cmbFilterByUserRelation.Size = new System.Drawing.Size(140, 23);
            this.cmbFilterByUserRelation.TabIndex = 3;
            // 
            // lblFilterUser
            // 
            this.lblFilterUser.AutoSize = true;
            this.lblFilterUser.Location = new System.Drawing.Point(170, 12);
            this.lblFilterUser.Name = "lblFilterUser";
            this.lblFilterUser.Size = new System.Drawing.Size(43, 15);
            this.lblFilterUser.TabIndex = 2;
            this.lblFilterUser.Text = "篩選：";
            // 
            // cmbFilterStatus
            // 
            this.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStatus.FormattingEnabled = true;
            this.cmbFilterStatus.Location = new System.Drawing.Point(55, 9);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(100, 23);
            this.cmbFilterStatus.TabIndex = 1;
            // 
            // lblFilterStatus
            // 
            this.lblFilterStatus.AutoSize = true;
            this.lblFilterStatus.Location = new System.Drawing.Point(12, 12);
            this.lblFilterStatus.Name = "lblFilterStatus";
            this.lblFilterStatus.Size = new System.Drawing.Size(43, 15);
            this.lblFilterStatus.TabIndex = 0;
            this.lblFilterStatus.Text = "狀態：";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 67);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.dgvTasks);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.txtCommentsPreview);
            this.splitContainerMain.Size = new System.Drawing.Size(1008, 592);
            this.splitContainerMain.SplitterDistance = 450;
            this.splitContainerMain.TabIndex = 3;
            // 
            // dgvTasks
            // 
            this.dgvTasks.AllowUserToAddRows = false;
            this.dgvTasks.AllowUserToDeleteRows = false;
            this.dgvTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTasks.Location = new System.Drawing.Point(0, 67);
            this.dgvTasks.MultiSelect = false;
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.RowHeadersWidth = 51;
            this.dgvTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTasks.Size = new System.Drawing.Size(1008, 450);
            this.dgvTasks.TabIndex = 0;
            // 
            // txtCommentsPreview
            // 
            this.txtCommentsPreview.BackColor = System.Drawing.SystemColors.Info;
            this.txtCommentsPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCommentsPreview.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommentsPreview.Location = new System.Drawing.Point(0, 0);
            this.txtCommentsPreview.Multiline = true;
            this.txtCommentsPreview.Name = "txtCommentsPreview";
            this.txtCommentsPreview.ReadOnly = true;
            this.txtCommentsPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCommentsPreview.Size = new System.Drawing.Size(1008, 138);
            this.txtCommentsPreview.TabIndex = 0;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbChangePassword
            // 
            this.tsbChangePassword.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbChangePassword.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbChangePassword.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbChangePassword.Name = "tsbChangePassword";
            this.tsbChangePassword.Size = new System.Drawing.Size(62, 24);
            this.tsbChangePassword.Text = "修改密碼";
            // 
            // tsbSwitchUser
            // 
            this.tsbSwitchUser.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbSwitchUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSwitchUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSwitchUser.Name = "tsbSwitchUser";
            this.tsbSwitchUser.Size = new System.Drawing.Size(74, 24);
            this.tsbSwitchUser.Text = "切換使用者";

            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 681);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.panelFilters);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "待辦事項清單";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNewTask;
        private System.Windows.Forms.ToolStripButton tsbEditTask;
        private System.Windows.Forms.ToolStripButton tsbDeleteTask;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.ComboBox cmbFilterByUserRelation;
        private System.Windows.Forms.Label lblFilterUser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbUserManagement;
        private System.Windows.Forms.ToolStripButton tsbAdminDashboard;
        private System.Windows.Forms.Label lblFilterByAssignedUser;
        private System.Windows.Forms.ComboBox cmbFilterByAssignedUser;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.TextBox txtCommentsPreview;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbChangePassword;
        private System.Windows.Forms.ToolStripButton tsbSwitchUser;
    }
}