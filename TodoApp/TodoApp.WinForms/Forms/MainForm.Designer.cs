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
            this.panelCommentsContainer = new System.Windows.Forms.Panel();
            this.commentsFormatToolStrip = new System.Windows.Forms.ToolStrip();
            this.txtCommentsPreview = new System.Windows.Forms.RichTextBox();

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
            txtCommentsPreview = new RichTextBox();

            this.tsBtnBold = new System.Windows.Forms.ToolStripButton();
            this.tsBtnItalic = new System.Windows.Forms.ToolStripButton();
            this.tsBtnUnderline = new System.Windows.Forms.ToolStripButton();
            this.tsBtnStrikeout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnSetColorRed = new System.Windows.Forms.ToolStripButton();
            this.tsBtnSetColorBlue = new System.Windows.Forms.ToolStripButton();
            this.tsBtnSetColorGreen = new System.Windows.Forms.ToolStripButton();
            this.tsBtnSetColorBlack = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnBulletList = new System.Windows.Forms.ToolStripButton();
            this.tsBtnIndent = new System.Windows.Forms.ToolStripButton();
            this.tsBtnOutdent = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnHighlightYellow = new System.Windows.Forms.ToolStripButton();
            this.tsBtnHighlightGreen = new System.Windows.Forms.ToolStripButton();
            this.tsBtnClearHighlight = new System.Windows.Forms.ToolStripButton();

            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            panelFilters.SuspendLayout();
            panelPagination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.Panel2.Controls.Clear();
            this.splitContainerMain.Panel2.Controls.Add(this.panelCommentsContainer);
            splitContainerMain.SuspendLayout();
            this.panelCommentsContainer.SuspendLayout();
            this.commentsFormatToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTasks).BeginInit();
            SuspendLayout();

            // 
            // panelCommentsContainer
            // 
            this.panelCommentsContainer.Controls.Add(this.btnSaveChanges);
            this.panelCommentsContainer.Controls.Add(this.txtCommentsPreview);
            this.panelCommentsContainer.Controls.Add(this.commentsFormatToolStrip);
            this.panelCommentsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCommentsContainer.Location = new System.Drawing.Point(0, 0);
            this.panelCommentsContainer.Name = "panelCommentsContainer";
            this.panelCommentsContainer.Size = new System.Drawing.Size(1207, 114);
            this.panelCommentsContainer.TabIndex = 0;

            // 
            // commentsFormatToolStrip
            // 
            this.commentsFormatToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.commentsFormatToolStrip.Location = new System.Drawing.Point(0, 0);
            this.commentsFormatToolStrip.Name = "commentsFormatToolStrip";
            this.commentsFormatToolStrip.Size = new System.Drawing.Size(1207, 25);
            this.commentsFormatToolStrip.TabIndex = 2;
            this.commentsFormatToolStrip.Text = "toolStrip2";
            this.commentsFormatToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsBtnBold,
                this.tsBtnItalic,
                this.tsBtnUnderline,
                this.tsBtnStrikeout,
                this.toolStripSeparator1,
                this.tsBtnSetColorRed,
                this.tsBtnSetColorBlue,
                this.tsBtnSetColorGreen,
                this.tsBtnSetColorBlack,
                this.toolStripSeparator2,
                this.tsBtnBulletList,
                this.tsBtnIndent,
                this.tsBtnOutdent,
                this.toolStripSeparator3,
                this.tsBtnHighlightYellow,
                this.tsBtnHighlightGreen,
                this.tsBtnClearHighlight
            });
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
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 65);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(dgvTasks);
            // 
            // splitContainerMain.Panel2
            // 
            // The bottom panel should contain our new container.
            this.splitContainerMain.Panel2.Controls.Add(this.panelCommentsContainer);
            this.splitContainerMain.Size = new System.Drawing.Size(1207, 468);
            this.splitContainerMain.SplitterDistance = 350; // Adjusted splitter distance
            this.splitContainerMain.TabIndex = 5;
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
            this.btnSaveChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveChanges.Enabled = false;
            this.btnSaveChanges.Location = new System.Drawing.Point(1112, 84);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(88, 27);
            this.btnSaveChanges.TabIndex = 0;
            this.btnSaveChanges.Text = "儲存備註";
            this.btnSaveChanges.UseVisualStyleBackColor = true;
            // 
            // txtCommentsPreview
            // 
            this.txtCommentsPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommentsPreview.BackColor = System.Drawing.SystemColors.Info;
            this.txtCommentsPreview.Location = new System.Drawing.Point(3, 28);
            this.txtCommentsPreview.Name = "txtCommentsPreview";
            this.txtCommentsPreview.Size = new System.Drawing.Size(1201, 52);
            this.txtCommentsPreview.TabIndex = 1;
            this.txtCommentsPreview.Text = "";
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveChanges.Enabled = false;
            this.btnSaveChanges.Location = new System.Drawing.Point(1115, 79);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(88, 23);
            this.btnSaveChanges.TabIndex = 3;
            this.btnSaveChanges.Text = "儲存備註";
            this.btnSaveChanges.UseVisualStyleBackColor = true;
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
            // Resume layout
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panelFilters.ResumeLayout(false);
            panelFilters.PerformLayout();
            panelPagination.ResumeLayout(false);
            panelPagination.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.panelCommentsContainer.ResumeLayout(false);
            this.panelCommentsContainer.PerformLayout();
            this.commentsFormatToolStrip.ResumeLayout(false);
            this.commentsFormatToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private RichTextBox txtCommentsPreview;
        private Panel panelCommentsContainer;
        private System.Windows.Forms.ToolStrip commentsFormatToolStrip;
        private System.Windows.Forms.ToolStripButton tsBtnBold;
        private System.Windows.Forms.ToolStripButton tsBtnItalic;
        private System.Windows.Forms.ToolStripButton tsBtnUnderline;
        private System.Windows.Forms.ToolStripButton tsBtnStrikeout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsBtnSetColorRed;
        private System.Windows.Forms.ToolStripButton tsBtnSetColorBlue;
        private System.Windows.Forms.ToolStripButton tsBtnSetColorGreen;
        private System.Windows.Forms.ToolStripButton tsBtnSetColorBlack;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsBtnBulletList;
        private System.Windows.Forms.ToolStripButton tsBtnIndent;
        private System.Windows.Forms.ToolStripButton tsBtnOutdent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsBtnHighlightYellow;
        private System.Windows.Forms.ToolStripButton tsBtnHighlightGreen;
        private System.Windows.Forms.ToolStripButton tsBtnClearHighlight;

        private ToolStrip toolStrip1;
        private ToolStripButton tsbNewTask;
        private ToolStripButton tsbEditTask;
        private ToolStripButton tsbDeleteTask;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsbRefresh;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatus;
        private Panel panelFilters;
        private ComboBox cmbFilterStatus;
        private Label lblFilterStatus;
        private ComboBox cmbFilterByUserRelation;
        private Label lblFilterUser;
        private DataGridView dgvTasks;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tsbUserManagement;
        private ToolStripButton tsbAdminDashboard;
        private Panel panelPagination;
        private Button btnLastPage;
        private Button btnNextPage;
        private Label lblPageInfo;
        private TextBox txtCurrentPage;
        private Button btnPreviousPage;
        private Button btnFirstPage;
        private Label lblPageSize;
        private ComboBox cmbPageSize;
        private Label lblFilterByAssignedUser;
        private ComboBox cmbFilterByAssignedUser;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton tsbChangePassword;
        private ToolStripButton tsbSwitchUser;
        private SplitContainer splitContainerMain;
        private Button btnSaveChanges;
        private Label lblSearch;
        private TextBox txtSearch;
    }
}