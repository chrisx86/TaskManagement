#nullable disable

using Org.BouncyCastle.Pqc.Crypto.Lms;

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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            toolStrip1 = new ToolStrip();
            tsbNewTask = new ToolStripButton();
            tsbEditTask = new ToolStripButton();
            tsbDeleteTask = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            tsbRefresh = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            tsbUserManagement = new ToolStripButton();
            tsbAdminDashboard = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            tsbSwitchUser = new ToolStripButton();
            tsbChangePassword = new ToolStripButton();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            panelFilters = new Panel();
            lblFilterByAssignedUser = new Label();
            cmbFilterByAssignedUser = new ComboBox();
            cmbFilterByUserRelation = new ComboBox();
            lblFilterUser = new Label();
            cmbFilterStatus = new ComboBox();
            lblFilterStatus = new Label();
            txtSearch = new TextBox();
            lblSearch = new Label();
            panelPagination = new Panel();
            lblPageSize = new Label();
            cmbPageSize = new ComboBox();
            btnLastPage = new Button();
            btnNextPage = new Button();
            lblPageInfo = new Label();
            txtCurrentPage = new TextBox();
            btnPreviousPage = new Button();
            btnFirstPage = new Button();
            splitContainerMain = new SplitContainer();
            dgvTasks = new DataGridView();
            btnSaveChanges = new Button();
            txtCommentsPreview = new TextBox();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            panelFilters.SuspendLayout();
            panelPagination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTasks).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { tsbNewTask, tsbEditTask, tsbDeleteTask, toolStripSeparator1, tsbRefresh, toolStripSeparator2, tsbUserManagement, tsbAdminDashboard, toolStripSeparator3, tsbSwitchUser, tsbChangePassword });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1207, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbNewTask
            // 
            tsbNewTask.ImageTransparentColor = Color.Magenta;
            tsbNewTask.Name = "tsbNewTask";
            tsbNewTask.Size = new Size(35, 22);
            tsbNewTask.Text = "新增";
            // 
            // tsbEditTask
            // 
            tsbEditTask.ImageTransparentColor = Color.Magenta;
            tsbEditTask.Name = "tsbEditTask";
            tsbEditTask.Size = new Size(35, 22);
            tsbEditTask.Text = "編輯";
            // 
            // tsbDeleteTask
            // 
            tsbDeleteTask.ImageTransparentColor = Color.Magenta;
            tsbDeleteTask.Name = "tsbDeleteTask";
            tsbDeleteTask.Size = new Size(35, 22);
            tsbDeleteTask.Text = "刪除";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // tsbRefresh
            // 
            tsbRefresh.ImageTransparentColor = Color.Magenta;
            tsbRefresh.Name = "tsbRefresh";
            tsbRefresh.Size = new Size(59, 22);
            tsbRefresh.Text = "重新整理";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // tsbUserManagement
            // 
            tsbUserManagement.ImageTransparentColor = Color.Magenta;
            tsbUserManagement.Name = "tsbUserManagement";
            tsbUserManagement.Size = new Size(71, 22);
            tsbUserManagement.Text = "使用者管理";
            tsbUserManagement.Visible = false;
            // 
            // tsbAdminDashboard
            // 
            tsbAdminDashboard.ImageTransparentColor = Color.Magenta;
            tsbAdminDashboard.Name = "tsbAdminDashboard";
            tsbAdminDashboard.Size = new Size(71, 22);
            tsbAdminDashboard.Text = "管理儀表板";
            tsbAdminDashboard.Visible = false;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 25);
            // 
            // tsbSwitchUser
            // 
            tsbSwitchUser.Alignment = ToolStripItemAlignment.Right;
            tsbSwitchUser.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsbSwitchUser.ImageTransparentColor = Color.Magenta;
            tsbSwitchUser.Name = "tsbSwitchUser";
            tsbSwitchUser.Size = new Size(71, 22);
            tsbSwitchUser.Text = "切換使用者";
            // 
            // tsbChangePassword
            // 
            tsbChangePassword.Alignment = ToolStripItemAlignment.Right;
            tsbChangePassword.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsbChangePassword.ImageTransparentColor = Color.Magenta;
            tsbChangePassword.Name = "tsbChangePassword";
            tsbChangePassword.Size = new Size(59, 22);
            tsbChangePassword.Text = "修改密碼";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip1.Location = new Point(0, 578);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(1207, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(43, 17);
            lblStatus.Text = "Ready";
            // 
            // panelFilters
            // 
            panelFilters.BackColor = SystemColors.Control;
            panelFilters.Controls.Add(lblFilterByAssignedUser);
            panelFilters.Controls.Add(cmbFilterByAssignedUser);
            panelFilters.Controls.Add(cmbFilterByUserRelation);
            panelFilters.Controls.Add(lblFilterUser);
            panelFilters.Controls.Add(cmbFilterStatus);
            panelFilters.Controls.Add(lblFilterStatus);
            panelFilters.Controls.Add(txtSearch);
            panelFilters.Controls.Add(lblSearch);
            panelFilters.Dock = DockStyle.Top;
            panelFilters.Location = new Point(0, 25);
            panelFilters.Margin = new Padding(4, 3, 4, 3);
            panelFilters.Name = "panelFilters";
            panelFilters.Size = new Size(1207, 40);
            panelFilters.TabIndex = 2;
            // 
            // lblFilterByAssignedUser
            // 
            lblFilterByAssignedUser.AutoSize = true;
            lblFilterByAssignedUser.Location = new Point(432, 12);
            lblFilterByAssignedUser.Margin = new Padding(4, 0, 4, 0);
            lblFilterByAssignedUser.Name = "lblFilterByAssignedUser";
            lblFilterByAssignedUser.Size = new Size(55, 15);
            lblFilterByAssignedUser.TabIndex = 5;
            lblFilterByAssignedUser.Text = "使用者：";
            // 
            // cmbFilterByAssignedUser
            // 
            cmbFilterByAssignedUser.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterByAssignedUser.FormattingEnabled = true;
            cmbFilterByAssignedUser.Location = new Point(503, 9);
            cmbFilterByAssignedUser.Margin = new Padding(4, 3, 4, 3);
            cmbFilterByAssignedUser.Name = "cmbFilterByAssignedUser";
            cmbFilterByAssignedUser.Size = new Size(163, 23);
            cmbFilterByAssignedUser.TabIndex = 4;
            // 
            // cmbFilterByUserRelation
            // 
            cmbFilterByUserRelation.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterByUserRelation.FormattingEnabled = true;
            cmbFilterByUserRelation.Location = new Point(257, 9);
            cmbFilterByUserRelation.Margin = new Padding(4, 3, 4, 3);
            cmbFilterByUserRelation.Name = "cmbFilterByUserRelation";
            cmbFilterByUserRelation.Size = new Size(163, 23);
            cmbFilterByUserRelation.TabIndex = 3;
            // 
            // lblFilterUser
            // 
            lblFilterUser.AutoSize = true;
            lblFilterUser.Location = new Point(198, 12);
            lblFilterUser.Margin = new Padding(4, 0, 4, 0);
            lblFilterUser.Name = "lblFilterUser";
            lblFilterUser.Size = new Size(43, 15);
            lblFilterUser.TabIndex = 2;
            lblFilterUser.Text = "篩選：";
            // 
            // cmbFilterStatus
            // 
            cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterStatus.FormattingEnabled = true;
            cmbFilterStatus.Location = new Point(64, 9);
            cmbFilterStatus.Margin = new Padding(4, 3, 4, 3);
            cmbFilterStatus.Name = "cmbFilterStatus";
            cmbFilterStatus.Size = new Size(116, 23);
            cmbFilterStatus.TabIndex = 1;
            // 
            // lblFilterStatus
            // 
            lblFilterStatus.AutoSize = true;
            lblFilterStatus.Location = new Point(14, 12);
            lblFilterStatus.Margin = new Padding(4, 0, 4, 0);
            lblFilterStatus.Name = "lblFilterStatus";
            lblFilterStatus.Size = new Size(43, 15);
            lblFilterStatus.TabIndex = 0;
            lblFilterStatus.Text = "狀態：";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(734, 9);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(200, 23);
            txtSearch.TabIndex = 7;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(690, 12);
            lblSearch.Margin = new Padding(4, 0, 4, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(43, 15);
            lblSearch.TabIndex = 6;
            lblSearch.Text = "搜尋：";
            // 
            // panelPagination
            // 
            panelPagination.Controls.Add(lblPageSize);
            panelPagination.Controls.Add(cmbPageSize);
            panelPagination.Controls.Add(btnLastPage);
            panelPagination.Controls.Add(btnNextPage);
            panelPagination.Controls.Add(lblPageInfo);
            panelPagination.Controls.Add(txtCurrentPage);
            panelPagination.Controls.Add(btnPreviousPage);
            panelPagination.Controls.Add(btnFirstPage);
            panelPagination.Dock = DockStyle.Bottom;
            panelPagination.Location = new Point(0, 533);
            panelPagination.Margin = new Padding(4, 3, 4, 3);
            panelPagination.Name = "panelPagination";
            panelPagination.Size = new Size(1207, 45);
            panelPagination.TabIndex = 4;
            // 
            // lblPageSize
            // 
            lblPageSize.AutoSize = true;
            lblPageSize.Location = new Point(14, 15);
            lblPageSize.Margin = new Padding(4, 0, 4, 0);
            lblPageSize.Name = "lblPageSize";
            lblPageSize.Size = new Size(67, 15);
            lblPageSize.TabIndex = 7;
            lblPageSize.Text = "每頁顯示：";
            // 
            // cmbPageSize
            // 
            cmbPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Location = new Point(99, 12);
            cmbPageSize.Margin = new Padding(4, 3, 4, 3);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(69, 23);
            cmbPageSize.TabIndex = 6;
            // 
            // btnLastPage
            // 
            btnLastPage.Location = new Point(682, 10);
            btnLastPage.Margin = new Padding(4, 3, 4, 3);
            btnLastPage.Name = "btnLastPage";
            btnLastPage.Size = new Size(47, 25);
            btnLastPage.TabIndex = 5;
            btnLastPage.Text = ">|";
            btnLastPage.UseVisualStyleBackColor = true;
            // 
            // btnNextPage
            // 
            btnNextPage.Location = new Point(630, 10);
            btnNextPage.Margin = new Padding(4, 3, 4, 3);
            btnNextPage.Name = "btnNextPage";
            btnNextPage.Size = new Size(47, 25);
            btnNextPage.TabIndex = 4;
            btnNextPage.Text = ">";
            btnNextPage.UseVisualStyleBackColor = true;
            // 
            // lblPageInfo
            // 
            lblPageInfo.AutoSize = true;
            lblPageInfo.Location = new Point(391, 15);
            lblPageInfo.Margin = new Padding(4, 0, 4, 0);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(110, 15);
            lblPageInfo.TabIndex = 3;
            lblPageInfo.Text = "第 1 / 1 頁 (共 0 筆)";
            // 
            // txtCurrentPage
            // 
            txtCurrentPage.Location = new Point(338, 11);
            txtCurrentPage.Margin = new Padding(4, 3, 4, 3);
            txtCurrentPage.Name = "txtCurrentPage";
            txtCurrentPage.Size = new Size(46, 23);
            txtCurrentPage.TabIndex = 2;
            txtCurrentPage.TextAlign = HorizontalAlignment.Center;
            // 
            // btnPreviousPage
            // 
            btnPreviousPage.Location = new Point(286, 10);
            btnPreviousPage.Margin = new Padding(4, 3, 4, 3);
            btnPreviousPage.Name = "btnPreviousPage";
            btnPreviousPage.Size = new Size(47, 25);
            btnPreviousPage.TabIndex = 1;
            btnPreviousPage.Text = "<";
            btnPreviousPage.UseVisualStyleBackColor = true;
            // 
            // btnFirstPage
            // 
            btnFirstPage.Location = new Point(233, 10);
            btnFirstPage.Margin = new Padding(4, 3, 4, 3);
            btnFirstPage.Name = "btnFirstPage";
            btnFirstPage.Size = new Size(47, 25);
            btnFirstPage.TabIndex = 0;
            btnFirstPage.Text = "|<";
            btnFirstPage.UseVisualStyleBackColor = true;
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerMain.Location = new Point(0, 65);
            splitContainerMain.Margin = new Padding(4, 3, 4, 3);
            splitContainerMain.Name = "splitContainerMain";
            splitContainerMain.Orientation = Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(dgvTasks);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(btnSaveChanges);
            splitContainerMain.Panel2.Controls.Add(txtCommentsPreview);
            splitContainerMain.Size = new Size(1207, 468);
            splitContainerMain.SplitterDistance = 358;
            splitContainerMain.TabIndex = 5;
            // 
            // dgvTasks
            // 
            dgvTasks.AllowUserToAddRows = false;
            dgvTasks.AllowUserToDeleteRows = false;
            dgvTasks.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvTasks.DefaultCellStyle = dataGridViewCellStyle1;
            dgvTasks.Dock = DockStyle.Fill;
            dgvTasks.Location = new Point(0, 0);
            dgvTasks.Margin = new Padding(4, 3, 4, 3);
            dgvTasks.MultiSelect = false;
            dgvTasks.Name = "dgvTasks";
            dgvTasks.RowHeadersVisible = false;
            dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTasks.Size = new Size(1207, 358);
            dgvTasks.TabIndex = 3;
            // 
            // btnSaveChanges
            // 
            btnSaveChanges.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveChanges.Enabled = false;
            btnSaveChanges.Location = new Point(1164, 77);
            btnSaveChanges.Margin = new Padding(4, 3, 4, 3);
            btnSaveChanges.Name = "btnSaveChanges";
            btnSaveChanges.Size = new Size(88, 23);
            btnSaveChanges.TabIndex = 1;
            btnSaveChanges.Text = "儲存";
            btnSaveChanges.UseVisualStyleBackColor = true;
            // 
            // txtCommentsPreview
            // 
            txtCommentsPreview.BackColor = SystemColors.Info;
            txtCommentsPreview.Dock = DockStyle.Fill;
            txtCommentsPreview.Font = new Font("Segoe UI", 9.75F);
            txtCommentsPreview.Location = new Point(0, 0);
            txtCommentsPreview.Margin = new Padding(4, 3, 4, 3);
            txtCommentsPreview.Multiline = true;
            txtCommentsPreview.Name = "txtCommentsPreview";
            txtCommentsPreview.ScrollBars = ScrollBars.Vertical;
            txtCommentsPreview.Size = new Size(1207, 106);
            txtCommentsPreview.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1207, 600);
            Controls.Add(splitContainerMain);
            Controls.Add(panelPagination);
            Controls.Add(panelFilters);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(1192, 600);
            Name = "MainForm";
            Text = "Task Management";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panelFilters.ResumeLayout(false);
            panelFilters.PerformLayout();
            panelPagination.ResumeLayout(false);
            panelPagination.PerformLayout();
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            splitContainerMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTasks).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbUserManagement;
        private System.Windows.Forms.ToolStripButton tsbAdminDashboard;
        private System.Windows.Forms.Panel panelPagination;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.TextBox txtCurrentPage;
        private System.Windows.Forms.Button btnPreviousPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Label lblPageSize;
        private System.Windows.Forms.ComboBox cmbPageSize;
        private System.Windows.Forms.Label lblFilterByAssignedUser;
        private System.Windows.Forms.ComboBox cmbFilterByAssignedUser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbChangePassword;
        private System.Windows.Forms.ToolStripButton tsbSwitchUser;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TextBox txtCommentsPreview;
        private System.Windows.Forms.Button btnSaveChanges;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
    }
}