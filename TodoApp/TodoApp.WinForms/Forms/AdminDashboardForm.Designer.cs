#nullable disable

using System.ComponentModel;

namespace TodoApp.WinForms.Forms
{
    partial class AdminDashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Panel panelFilters;
        private StatusStrip statusStrip1;
        private TreeView tvTasks;
        private TextBox txtSearch;
        private Label lblSearch;
        private ComboBox cmbFilterPriority;
        private Label lblFilterPriority;
        private Button btnClearFilter;
        private ToolStripStatusLabel lblStatus;
        private TableLayoutPanel mainTableLayoutPanel;
        private Panel panelLeft;
        private Panel panelRight;
        private Panel panelStatistics;
        private Panel cardTotalTasks;
        private Label lblTotalTasksValue;
        private Label lblTotalTasksTitle;
        private Panel cardUncompleted;
        private Label lblUncompletedValue;
        private Label lblUncompletedTitle;
        private Panel cardOverdue;
        private Label lblOverdueValue;
        private Label lblOverdueTitle;
        private Panel cardUnassigned;
        private Label lblUnassignedValue;
        private Label lblUnassignedTitle;
        private Panel panelTaskDetails;
        private Label lblDetailTitle;
        private Label lblDetailStatusTitle;
        private Label lblDetailStatus;
        private Label lblDetailPriorityTitle;
        private Label lblDetailPriority;
        private Label lblDetailDueDateTitle;
        private Label lblDetailDueDate;
        private Label lblDetailCreatorTitle;
        private Label lblDetailCreator;
        private Label lblDetailAssignedToTitle;
        private Label lblDetailAssignedTo;
        private Label lblDetailCommentsTitle;
        private RichTextBox txtDetailComments;
        private ToolStrip commentsFormatToolStrip;
        private ToolStripButton tsBtnBold;
        private ToolStripButton tsBtnItalic;
        private ToolStripButton tsBtnUnderline;
        private ToolStripButton tsBtnStrikeout;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsBtnSetColorRed;
        private ToolStripButton tsBtnSetColorBlack;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tsBtnBulletList;
        private ToolStripButton tsBtnIndent;
        private ToolStripButton tsBtnOutdent;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton tsBtnHighlight;
        private ToolStripButton tsBtnClearHighlight;

        private Label lblFilterStatus;
        private ComboBox cmbFilterStatus;
        private CheckBox chkFilterOverdue;
        private Panel panelDetailActions;
        private Button btnDetailDelete;
        private Button btnDetailReassign;
        private Button btnDetailEdit;
        private ContextMenuStrip treeViewContextMenu;
        private ToolStripMenuItem ctxEditTask;
        private ToolStripMenuItem ctxReassignTask;
        private ToolStripMenuItem ctxDeleteTask;
        private Label lblDetailLastModified;
        private Label lblDetailLastModifiedTitle;
        private Label lblDetailCreationDate;
        private Label lblDetailCreationDateTitle;
        private Panel cardRejected;
        private Label lblRejectedValue;
        private Label lblRejectedTitle;
        private Label lblFilterByUser;
        private ComboBox cmbFilterByUser;
        private Panel cardCompleted;
        private Label lblCompletedValue;
        private Label lblCompletedTitle;
        private Button btnSaveComment;
        private Button btnViewHistory;
        private ToolStripButton tsBtnCodeSnippet;
        private ToolStripButton tsBtnMoreColors;

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
            components = new Container();
            panelFilters = new Panel();
            cmbFilterByUser = new ComboBox();
            lblFilterByUser = new Label();
            chkFilterOverdue = new CheckBox();
            cmbFilterStatus = new ComboBox();
            lblFilterStatus = new Label();
            btnClearFilter = new Button();
            cmbFilterPriority = new ComboBox();
            lblFilterPriority = new Label();
            txtSearch = new TextBox();
            lblSearch = new Label();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            tvTasks = new TreeView();
            treeViewContextMenu = new ContextMenuStrip(components);
            ctxEditTask = new ToolStripMenuItem();
            ctxReassignTask = new ToolStripMenuItem();
            ctxDeleteTask = new ToolStripMenuItem();
            mainTableLayoutPanel = new TableLayoutPanel();
            panelLeft = new Panel();
            panelStatistics = new Panel();
            cardCompleted = new Panel();
            lblCompletedValue = new Label();
            lblCompletedTitle = new Label();
            cardRejected = new Panel();
            lblRejectedValue = new Label();
            lblRejectedTitle = new Label();
            cardUnassigned = new Panel();
            lblUnassignedValue = new Label();
            lblUnassignedTitle = new Label();
            cardOverdue = new Panel();
            lblOverdueValue = new Label();
            lblOverdueTitle = new Label();
            cardUncompleted = new Panel();
            lblUncompletedValue = new Label();
            lblUncompletedTitle = new Label();
            cardTotalTasks = new Panel();
            lblTotalTasksValue = new Label();
            lblTotalTasksTitle = new Label();
            panelRight = new Panel();
            panelTaskDetails = new Panel();
            lblDetailLastModified = new Label();
            lblDetailLastModifiedTitle = new Label();
            lblDetailCreationDate = new Label();
            lblDetailCreationDateTitle = new Label();
            panelDetailActions = new Panel();
            btnDetailDelete = new Button();
            btnDetailReassign = new Button();
            btnDetailEdit = new Button();

            txtDetailComments = new RichTextBox();
            commentsFormatToolStrip = new ToolStrip();
            tsBtnBold = new ToolStripButton();
            tsBtnItalic = new ToolStripButton();
            tsBtnUnderline = new ToolStripButton();
            tsBtnStrikeout = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            tsBtnSetColorRed = new ToolStripButton();
            tsBtnSetColorBlack = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            tsBtnBulletList = new ToolStripButton();
            tsBtnIndent = new ToolStripButton();
            tsBtnOutdent = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            tsBtnHighlight = new ToolStripButton();
            tsBtnClearHighlight = new ToolStripButton();

            lblDetailCommentsTitle = new Label();
            lblDetailAssignedTo = new Label();
            lblDetailAssignedToTitle = new Label();
            lblDetailCreator = new Label();
            lblDetailCreatorTitle = new Label();
            lblDetailDueDate = new Label();
            lblDetailDueDateTitle = new Label();
            lblDetailPriority = new Label();
            lblDetailPriorityTitle = new Label();
            lblDetailStatus = new Label();
            lblDetailStatusTitle = new Label();
            lblDetailTitle = new Label();
            btnSaveComment = new Button();
            btnViewHistory = new Button();
            tsBtnCodeSnippet = new ToolStripButton();
            tsBtnMoreColors = new ToolStripButton();

            SuspendLayout();
            // 
            // panelFilters
            // 
            panelFilters.Controls.Add(cmbFilterByUser);
            panelFilters.Controls.Add(lblFilterByUser);
            panelFilters.Controls.Add(chkFilterOverdue);
            panelFilters.Controls.Add(cmbFilterStatus);
            panelFilters.Controls.Add(lblFilterStatus);
            panelFilters.Controls.Add(btnClearFilter);
            panelFilters.Controls.Add(cmbFilterPriority);
            panelFilters.Controls.Add(lblFilterPriority);
            panelFilters.Controls.Add(txtSearch);
            panelFilters.Controls.Add(lblSearch);
            panelFilters.Dock = DockStyle.Top;
            panelFilters.Location = new Point(0, 200);
            panelFilters.Name = "panelFilters";
            panelFilters.Padding = new Padding(5);
            panelFilters.Size = new Size(295, 215);
            panelFilters.TabIndex = 0;
            // 
            // cmbFilterByUser
            // 
            cmbFilterByUser.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbFilterByUser.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterByUser.FormattingEnabled = true;
            cmbFilterByUser.Location = new Point(100, 117);
            cmbFilterByUser.Name = "cmbFilterByUser";
            cmbFilterByUser.Size = new Size(183, 23);
            cmbFilterByUser.TabIndex = 9;
            // 
            // lblFilterByUser
            // 
            lblFilterByUser.AutoSize = true;
            lblFilterByUser.Location = new Point(12, 120);
            lblFilterByUser.Name = "lblFilterByUser";
            lblFilterByUser.Size = new Size(55, 15);
            lblFilterByUser.TabIndex = 8;
            lblFilterByUser.Text = "使用者：";
            // 
            // chkFilterOverdue
            // 
            chkFilterOverdue.AutoSize = true;
            chkFilterOverdue.Location = new Point(100, 152);
            chkFilterOverdue.Name = "chkFilterOverdue";
            chkFilterOverdue.Size = new Size(110, 19);
            chkFilterOverdue.TabIndex = 7;
            chkFilterOverdue.Text = "僅顯示逾期任務";
            chkFilterOverdue.UseVisualStyleBackColor = true;
            // 
            // cmbFilterStatus
            // 
            cmbFilterStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterStatus.FormattingEnabled = true;
            cmbFilterStatus.Location = new Point(100, 85);
            cmbFilterStatus.Name = "cmbFilterStatus";
            cmbFilterStatus.Size = new Size(183, 23);
            cmbFilterStatus.TabIndex = 6;
            // 
            // lblFilterStatus
            // 
            lblFilterStatus.AutoSize = true;
            lblFilterStatus.Location = new Point(12, 88);
            lblFilterStatus.Name = "lblFilterStatus";
            lblFilterStatus.Size = new Size(43, 15);
            lblFilterStatus.TabIndex = 5;
            lblFilterStatus.Text = "狀態：";
            // 
            // btnClearFilter
            // 
            btnClearFilter.Location = new Point(100, 177);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(75, 23);
            btnClearFilter.TabIndex = 4;
            btnClearFilter.Text = "清除篩選";
            btnClearFilter.UseVisualStyleBackColor = true;
            // 
            // cmbFilterPriority
            // 
            cmbFilterPriority.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbFilterPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterPriority.FormattingEnabled = true;
            cmbFilterPriority.Location = new Point(100, 50);
            cmbFilterPriority.Name = "cmbFilterPriority";
            cmbFilterPriority.Size = new Size(183, 23);
            cmbFilterPriority.TabIndex = 3;
            // 
            // lblFilterPriority
            // 
            lblFilterPriority.AutoSize = true;
            lblFilterPriority.Location = new Point(12, 53);
            lblFilterPriority.Name = "lblFilterPriority";
            lblFilterPriority.Size = new Size(55, 15);
            lblFilterPriority.TabIndex = 2;
            lblFilterPriority.Text = "優先級：";
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.Location = new Point(100, 14);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(183, 23);
            txtSearch.TabIndex = 1;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(12, 17);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(43, 15);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "搜尋：";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip1.Location = new Point(0, 539);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1184, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(55, 17);
            lblStatus.Text = "準備就緒";
            // 
            // tvTasks
            // 
            tvTasks.ContextMenuStrip = treeViewContextMenu;
            tvTasks.Dock = DockStyle.Fill;
            tvTasks.FullRowSelect = true;
            tvTasks.HideSelection = false;
            tvTasks.Location = new Point(304, 3);
            tvTasks.Name = "tvTasks";
            tvTasks.Size = new Size(574, 533);
            tvTasks.TabIndex = 0;
            // 
            // treeViewContextMenu
            // 
            treeViewContextMenu.Items.AddRange(new ToolStripItem[] { ctxEditTask, ctxReassignTask, ctxDeleteTask });
            treeViewContextMenu.Name = "treeViewContextMenu";
            treeViewContextMenu.Size = new Size(140, 70);
            // 
            // ctxEditTask
            // 
            ctxEditTask.Name = "ctxEditTask";
            ctxEditTask.Size = new Size(139, 22);
            ctxEditTask.Text = "編輯任務(&E)";
            // 
            // ctxReassignTask
            // 
            ctxReassignTask.Name = "ctxReassignTask";
            ctxReassignTask.Size = new Size(139, 22);
            ctxReassignTask.Text = "重新指派(&R)";
            // 
            // ctxDeleteTask
            // 
            ctxDeleteTask.Name = "ctxDeleteTask";
            ctxDeleteTask.Size = new Size(139, 22);
            ctxDeleteTask.Text = "刪除任務(&D)";
            // 
            // mainTableLayoutPanel
            // 
            mainTableLayoutPanel.ColumnCount = 3;
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.5F));
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49F));
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.5F));
            mainTableLayoutPanel.Controls.Add(tvTasks, 1, 0);
            mainTableLayoutPanel.Controls.Add(panelLeft, 0, 0);
            mainTableLayoutPanel.Controls.Add(panelRight, 2, 0);
            mainTableLayoutPanel.Dock = DockStyle.Fill;
            mainTableLayoutPanel.Location = new Point(0, 0);
            mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            mainTableLayoutPanel.RowCount = 1;
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainTableLayoutPanel.Size = new Size(1184, 539);
            mainTableLayoutPanel.TabIndex = 2;
            // 
            // panelLeft
            // 
            panelLeft.Controls.Add(panelFilters);
            panelLeft.Controls.Add(panelStatistics);
            panelLeft.Dock = DockStyle.Fill;
            panelLeft.Location = new Point(3, 3);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(295, 533);
            panelLeft.TabIndex = 1;
            // 
            // panelStatistics
            // 
            panelStatistics.Controls.Add(cardCompleted);
            panelStatistics.Controls.Add(cardRejected);
            panelStatistics.Controls.Add(cardUnassigned);
            panelStatistics.Controls.Add(cardOverdue);
            panelStatistics.Controls.Add(cardUncompleted);
            panelStatistics.Controls.Add(cardTotalTasks);
            panelStatistics.Dock = DockStyle.Top;
            panelStatistics.Location = new Point(0, 0);
            panelStatistics.Name = "panelStatistics";
            panelStatistics.Padding = new Padding(5);
            panelStatistics.Size = new Size(295, 200);
            panelStatistics.TabIndex = 1;
            // 
            // cardCompleted
            // 
            cardCompleted.BorderStyle = BorderStyle.FixedSingle;
            cardCompleted.Controls.Add(lblCompletedValue);
            cardCompleted.Controls.Add(lblCompletedTitle);
            cardCompleted.Location = new Point(10, 73);
            cardCompleted.Name = "cardCompleted";
            cardCompleted.Size = new Size(130, 55);
            cardCompleted.TabIndex = 5;
            // 
            // lblCompletedValue
            // 
            lblCompletedValue.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblCompletedValue.ForeColor = Color.Green;
            lblCompletedValue.Location = new Point(3, 20);
            lblCompletedValue.Name = "lblCompletedValue";
            lblCompletedValue.Size = new Size(122, 25);
            lblCompletedValue.TabIndex = 0;
            lblCompletedValue.Text = "0";
            lblCompletedValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCompletedTitle
            // 
            lblCompletedTitle.Dock = DockStyle.Top;
            lblCompletedTitle.Location = new Point(0, 0);
            lblCompletedTitle.Name = "lblCompletedTitle";
            lblCompletedTitle.Size = new Size(128, 20);
            lblCompletedTitle.TabIndex = 1;
            lblCompletedTitle.Text = "已完成任務";
            lblCompletedTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardRejected
            // 
            cardRejected.BorderStyle = BorderStyle.FixedSingle;
            cardRejected.Controls.Add(lblRejectedValue);
            cardRejected.Controls.Add(lblRejectedTitle);
            cardRejected.Location = new Point(154, 136);
            cardRejected.Name = "cardRejected";
            cardRejected.Size = new Size(130, 55);
            cardRejected.TabIndex = 4;
            // 
            // lblRejectedValue
            // 
            lblRejectedValue.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblRejectedValue.ForeColor = SystemColors.ControlDarkDark;
            lblRejectedValue.Location = new Point(3, 20);
            lblRejectedValue.Name = "lblRejectedValue";
            lblRejectedValue.Size = new Size(122, 25);
            lblRejectedValue.TabIndex = 0;
            lblRejectedValue.Text = "0";
            lblRejectedValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRejectedTitle
            // 
            lblRejectedTitle.Dock = DockStyle.Top;
            lblRejectedTitle.Location = new Point(0, 0);
            lblRejectedTitle.Name = "lblRejectedTitle";
            lblRejectedTitle.Size = new Size(128, 20);
            lblRejectedTitle.TabIndex = 1;
            lblRejectedTitle.Text = "已拒絕任務";
            lblRejectedTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardUnassigned
            // 
            cardUnassigned.BorderStyle = BorderStyle.FixedSingle;
            cardUnassigned.Controls.Add(lblUnassignedValue);
            cardUnassigned.Controls.Add(lblUnassignedTitle);
            cardUnassigned.Location = new Point(10, 136);
            cardUnassigned.Name = "cardUnassigned";
            cardUnassigned.Size = new Size(130, 55);
            cardUnassigned.TabIndex = 3;
            // 
            // lblUnassignedValue
            // 
            lblUnassignedValue.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblUnassignedValue.ForeColor = Color.Orange;
            lblUnassignedValue.Location = new Point(3, 20);
            lblUnassignedValue.Name = "lblUnassignedValue";
            lblUnassignedValue.Size = new Size(122, 25);
            lblUnassignedValue.TabIndex = 0;
            lblUnassignedValue.Text = "0";
            lblUnassignedValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUnassignedTitle
            // 
            lblUnassignedTitle.Dock = DockStyle.Top;
            lblUnassignedTitle.Location = new Point(0, 0);
            lblUnassignedTitle.Name = "lblUnassignedTitle";
            lblUnassignedTitle.Size = new Size(128, 20);
            lblUnassignedTitle.TabIndex = 1;
            lblUnassignedTitle.Text = "待指派任務";
            lblUnassignedTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardOverdue
            // 
            cardOverdue.BorderStyle = BorderStyle.FixedSingle;
            cardOverdue.Controls.Add(lblOverdueValue);
            cardOverdue.Controls.Add(lblOverdueTitle);
            cardOverdue.Location = new Point(154, 9);
            cardOverdue.Name = "cardOverdue";
            cardOverdue.Size = new Size(130, 55);
            cardOverdue.TabIndex = 2;
            // 
            // lblOverdueValue
            // 
            lblOverdueValue.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblOverdueValue.ForeColor = Color.Red;
            lblOverdueValue.Location = new Point(3, 20);
            lblOverdueValue.Name = "lblOverdueValue";
            lblOverdueValue.Size = new Size(122, 25);
            lblOverdueValue.TabIndex = 0;
            lblOverdueValue.Text = "0";
            lblOverdueValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOverdueTitle
            // 
            lblOverdueTitle.Dock = DockStyle.Top;
            lblOverdueTitle.Location = new Point(0, 0);
            lblOverdueTitle.Name = "lblOverdueTitle";
            lblOverdueTitle.Size = new Size(128, 20);
            lblOverdueTitle.TabIndex = 1;
            lblOverdueTitle.Text = "已逾期任務";
            lblOverdueTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardUncompleted
            // 
            cardUncompleted.BorderStyle = BorderStyle.FixedSingle;
            cardUncompleted.Controls.Add(lblUncompletedValue);
            cardUncompleted.Controls.Add(lblUncompletedTitle);
            cardUncompleted.Location = new Point(154, 73);
            cardUncompleted.Name = "cardUncompleted";
            cardUncompleted.Size = new Size(130, 55);
            cardUncompleted.TabIndex = 1;
            // 
            // lblUncompletedValue
            // 
            lblUncompletedValue.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblUncompletedValue.Location = new Point(3, 20);
            lblUncompletedValue.Name = "lblUncompletedValue";
            lblUncompletedValue.Size = new Size(122, 25);
            lblUncompletedValue.TabIndex = 0;
            lblUncompletedValue.Text = "0";
            lblUncompletedValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUncompletedTitle
            // 
            lblUncompletedTitle.Dock = DockStyle.Top;
            lblUncompletedTitle.Location = new Point(0, 0);
            lblUncompletedTitle.Name = "lblUncompletedTitle";
            lblUncompletedTitle.Size = new Size(128, 20);
            lblUncompletedTitle.TabIndex = 1;
            lblUncompletedTitle.Text = "待完成任務";
            lblUncompletedTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cardTotalTasks
            // 
            cardTotalTasks.BorderStyle = BorderStyle.FixedSingle;
            cardTotalTasks.Controls.Add(lblTotalTasksValue);
            cardTotalTasks.Controls.Add(lblTotalTasksTitle);
            cardTotalTasks.Location = new Point(10, 10);
            cardTotalTasks.Name = "cardTotalTasks";
            cardTotalTasks.Size = new Size(130, 55);
            cardTotalTasks.TabIndex = 0;
            // 
            // lblTotalTasksValue
            // 
            lblTotalTasksValue.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblTotalTasksValue.Location = new Point(3, 20);
            lblTotalTasksValue.Name = "lblTotalTasksValue";
            lblTotalTasksValue.Size = new Size(122, 25);
            lblTotalTasksValue.TabIndex = 0;
            lblTotalTasksValue.Text = "0";
            lblTotalTasksValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalTasksTitle
            // 
            lblTotalTasksTitle.Dock = DockStyle.Top;
            lblTotalTasksTitle.Location = new Point(0, 0);
            lblTotalTasksTitle.Name = "lblTotalTasksTitle";
            lblTotalTasksTitle.Size = new Size(128, 20);
            lblTotalTasksTitle.TabIndex = 1;
            lblTotalTasksTitle.Text = "總任務數";
            lblTotalTasksTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelRight
            // 
            panelRight.BackColor = SystemColors.Window;
            panelRight.Controls.Add(panelTaskDetails);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(884, 3);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(10);
            panelRight.Size = new Size(297, 533);
            panelRight.TabIndex = 2;
            // 
            // panelTaskDetails
            // 
            panelTaskDetails.Controls.Add(this.commentsFormatToolStrip);
            panelTaskDetails.Controls.Add(btnSaveComment);
            panelTaskDetails.Controls.Add(lblDetailLastModified);
            panelTaskDetails.Controls.Add(lblDetailLastModifiedTitle);
            panelTaskDetails.Controls.Add(lblDetailCreationDate);
            panelTaskDetails.Controls.Add(lblDetailCreationDateTitle);
            panelTaskDetails.Controls.Add(panelDetailActions);
            panelTaskDetails.Controls.Add(txtDetailComments);
            panelTaskDetails.Controls.Add(lblDetailCommentsTitle);
            panelTaskDetails.Controls.Add(lblDetailAssignedTo);
            panelTaskDetails.Controls.Add(lblDetailAssignedToTitle);
            panelTaskDetails.Controls.Add(lblDetailCreator);
            panelTaskDetails.Controls.Add(lblDetailCreatorTitle);
            panelTaskDetails.Controls.Add(lblDetailDueDate);
            panelTaskDetails.Controls.Add(lblDetailDueDateTitle);
            panelTaskDetails.Controls.Add(lblDetailPriority);
            panelTaskDetails.Controls.Add(lblDetailPriorityTitle);
            panelTaskDetails.Controls.Add(lblDetailStatus);
            panelTaskDetails.Controls.Add(lblDetailStatusTitle);
            panelTaskDetails.Controls.Add(lblDetailTitle);
            panelTaskDetails.Dock = DockStyle.Fill;
            panelTaskDetails.Location = new Point(10, 10);
            panelTaskDetails.Name = "panelTaskDetails";
            panelTaskDetails.Size = new System.Drawing.Size(277, 513);
            panelTaskDetails.TabIndex = 0;
            panelTaskDetails.Visible = true;
            // 
            // lblDetailLastModified
            // 
            lblDetailLastModified.AutoSize = true;
            lblDetailLastModified.Location = new Point(70, 200);
            lblDetailLastModified.Name = "lblDetailLastModified";
            lblDetailLastModified.Size = new Size(12, 15);
            lblDetailLastModified.TabIndex = 16;
            lblDetailLastModified.Text = "-";
            // 
            // lblDetailLastModifiedTitle
            // 
            lblDetailLastModifiedTitle.AutoSize = true;
            lblDetailLastModifiedTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailLastModifiedTitle.Location = new Point(0, 200);
            lblDetailLastModifiedTitle.Name = "lblDetailLastModifiedTitle";
            lblDetailLastModifiedTitle.Size = new Size(55, 15);
            lblDetailLastModifiedTitle.TabIndex = 15;
            lblDetailLastModifiedTitle.Text = "最後更新";
            // 
            // lblDetailCreationDate
            // 
            lblDetailCreationDate.AutoSize = true;
            lblDetailCreationDate.Location = new Point(70, 175);
            lblDetailCreationDate.Name = "lblDetailCreationDate";
            lblDetailCreationDate.Size = new Size(12, 15);
            lblDetailCreationDate.TabIndex = 14;
            lblDetailCreationDate.Text = "-";
            // 
            // lblDetailCreationDateTitle
            // 
            lblDetailCreationDateTitle.AutoSize = true;
            lblDetailCreationDateTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailCreationDateTitle.Location = new Point(0, 175);
            lblDetailCreationDateTitle.Name = "lblDetailCreationDateTitle";
            lblDetailCreationDateTitle.Size = new Size(55, 15);
            lblDetailCreationDateTitle.TabIndex = 13;
            lblDetailCreationDateTitle.Text = "建立時間";
            // 
            // panelDetailActions
            // 
            panelDetailActions.Controls.Add(btnDetailDelete);
            panelDetailActions.Controls.Add(btnDetailReassign);
            panelDetailActions.Controls.Add(btnDetailEdit);
            panelDetailActions.Controls.Add(btnViewHistory);
            panelDetailActions.Dock = DockStyle.Bottom;
            panelDetailActions.Location = new Point(0, 473);
            panelDetailActions.Name = "panelDetailActions";
            panelDetailActions.Size = new Size(277, 40);
            panelDetailActions.TabIndex = 13;
            // 
            // btnSaveComment
            // 
            this.btnSaveComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveComment.Enabled = false;
            this.btnSaveComment.Location = new System.Drawing.Point(199, 444);
            this.btnSaveComment.Name = "btnSaveComment";
            this.btnSaveComment.Size = new System.Drawing.Size(75, 23);
            this.btnSaveComment.TabIndex = 17;
            this.btnSaveComment.Text = "儲存備註";
            this.btnSaveComment.UseVisualStyleBackColor = true;
            // 
            // btnDetailDelete
            // 
            btnDetailDelete.Location = new Point(0, 8);
            btnDetailDelete.Name = "btnDetailDelete";
            btnDetailDelete.Size = new Size(60, 23);
            btnDetailDelete.TabIndex = 2;
            btnDetailDelete.Text = "刪除";
            btnDetailDelete.UseVisualStyleBackColor = true;
            // 
            // btnDetailReassign
            // 
            btnDetailReassign.Location = new Point(66, 8);
            btnDetailReassign.Name = "btnDetailReassign";
            btnDetailReassign.Size = new Size(75, 23);
            btnDetailReassign.TabIndex = 1;
            btnDetailReassign.Text = "重新指派";
            btnDetailReassign.UseVisualStyleBackColor = true;
            // 
            // btnDetailEdit
            // 
            btnDetailEdit.Location = new Point(147, 8);
            btnDetailEdit.Name = "btnDetailEdit";
            btnDetailEdit.Size = new Size(60, 23);
            btnDetailEdit.TabIndex = 0;
            btnDetailEdit.Text = "編輯";
            btnDetailEdit.UseVisualStyleBackColor = true;
            // 
            // btnViewHistory
            // 
            btnViewHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnViewHistory.Location = new System.Drawing.Point(213, 8);
            btnViewHistory.Name = "btnViewHistory";
            btnViewHistory.Size = new System.Drawing.Size(75, 23);
            btnViewHistory.TabIndex = 3; // Ensure correct tab order
            btnViewHistory.Text = "查看歷史";
            btnViewHistory.UseVisualStyleBackColor = true;
            // 
            // txtDetailComments
            // 
            txtDetailComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            txtDetailComments.BackColor = System.Drawing.SystemColors.Window;
            txtDetailComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtDetailComments.Location = new System.Drawing.Point(0, 273);
            txtDetailComments.Name = "txtDetailComments";
            txtDetailComments.Size = new System.Drawing.Size(277, 165);
            txtDetailComments.TabIndex = 12;
            txtDetailComments.Text = "";
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
            // tsBtnStrikeout
            // 
            tsBtnStrikeout.Name = "tsBtnStrikeout";
            tsBtnStrikeout.Size = new Size(23, 25);
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
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
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
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 25);
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
            // lblDetailCommentsTitle
            // 
            lblDetailCommentsTitle.AutoSize = true;
            lblDetailCommentsTitle.Location = new System.Drawing.Point(0, 225);
            lblDetailCommentsTitle.Name = "lblDetailCommentsTitle";
            lblDetailCommentsTitle.Size = new Size(31, 15);
            lblDetailCommentsTitle.TabIndex = 11;
            lblDetailCommentsTitle.Text = "備註";
            // 
            // commentsFormatToolStrip
            // 
            commentsFormatToolStrip.Items.Clear();
            commentsFormatToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            commentsFormatToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            commentsFormatToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            // Position it correctly above the RichTextBox
            commentsFormatToolStrip.Location = new System.Drawing.Point(0, 245);
            commentsFormatToolStrip.Name = "commentsFormatToolStrip";
            commentsFormatToolStrip.Size = new System.Drawing.Size(277, 25);
            commentsFormatToolStrip.TabIndex = 18;
            commentsFormatToolStrip.Items.AddRange(new ToolStripItem[] { tsBtnBold, tsBtnItalic, tsBtnUnderline, tsBtnCodeSnippet, tsBtnSetColorRed, tsBtnSetColorBlack, tsBtnMoreColors, tsBtnHighlight, tsBtnClearHighlight, tsBtnBulletList, tsBtnIndent, tsBtnOutdent });
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
            // lblDetailAssignedTo
            // 
            lblDetailAssignedTo.AutoSize = true;
            lblDetailAssignedTo.Location = new Point(70, 150);
            lblDetailAssignedTo.Name = "lblDetailAssignedTo";
            lblDetailAssignedTo.Size = new Size(12, 15);
            lblDetailAssignedTo.TabIndex = 10;
            lblDetailAssignedTo.Text = "-";
            // 
            // lblDetailAssignedToTitle
            // 
            lblDetailAssignedToTitle.AutoSize = true;
            lblDetailAssignedToTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailAssignedToTitle.Location = new Point(0, 150);
            lblDetailAssignedToTitle.Name = "lblDetailAssignedToTitle";
            lblDetailAssignedToTitle.Size = new Size(43, 15);
            lblDetailAssignedToTitle.TabIndex = 9;
            lblDetailAssignedToTitle.Text = "指派給";
            // 
            // lblDetailCreator
            // 
            lblDetailCreator.AutoSize = true;
            lblDetailCreator.Location = new Point(70, 125);
            lblDetailCreator.Name = "lblDetailCreator";
            lblDetailCreator.Size = new Size(12, 15);
            lblDetailCreator.TabIndex = 8;
            lblDetailCreator.Text = "-";
            // 
            // lblDetailCreatorTitle
            // 
            lblDetailCreatorTitle.AutoSize = true;
            lblDetailCreatorTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailCreatorTitle.Location = new Point(0, 125);
            lblDetailCreatorTitle.Name = "lblDetailCreatorTitle";
            lblDetailCreatorTitle.Size = new Size(43, 15);
            lblDetailCreatorTitle.TabIndex = 7;
            lblDetailCreatorTitle.Text = "建立者";
            // 
            // lblDetailDueDate
            // 
            lblDetailDueDate.AutoSize = true;
            lblDetailDueDate.Location = new Point(70, 100);
            lblDetailDueDate.Name = "lblDetailDueDate";
            lblDetailDueDate.Size = new Size(12, 15);
            lblDetailDueDate.TabIndex = 6;
            lblDetailDueDate.Text = "-";
            // 
            // lblDetailDueDateTitle
            // 
            lblDetailDueDateTitle.AutoSize = true;
            lblDetailDueDateTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailDueDateTitle.Location = new Point(0, 100);
            lblDetailDueDateTitle.Name = "lblDetailDueDateTitle";
            lblDetailDueDateTitle.Size = new Size(43, 15);
            lblDetailDueDateTitle.TabIndex = 5;
            lblDetailDueDateTitle.Text = "到期日";
            // 
            // lblDetailPriority
            // 
            lblDetailPriority.AutoSize = true;
            lblDetailPriority.Location = new Point(70, 75);
            lblDetailPriority.Name = "lblDetailPriority";
            lblDetailPriority.Size = new Size(12, 15);
            lblDetailPriority.TabIndex = 4;
            lblDetailPriority.Text = "-";
            // 
            // lblDetailPriorityTitle
            // 
            lblDetailPriorityTitle.AutoSize = true;
            lblDetailPriorityTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailPriorityTitle.Location = new Point(0, 75);
            lblDetailPriorityTitle.Name = "lblDetailPriorityTitle";
            lblDetailPriorityTitle.Size = new Size(43, 15);
            lblDetailPriorityTitle.TabIndex = 3;
            lblDetailPriorityTitle.Text = "優先級";
            // 
            // lblDetailStatus
            // 
            lblDetailStatus.AutoSize = true;
            lblDetailStatus.Location = new Point(70, 50);
            lblDetailStatus.Name = "lblDetailStatus";
            lblDetailStatus.Size = new Size(12, 15);
            lblDetailStatus.TabIndex = 2;
            lblDetailStatus.Text = "-";
            // 
            // lblDetailStatusTitle
            // 
            lblDetailStatusTitle.AutoSize = true;
            lblDetailStatusTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailStatusTitle.Location = new Point(0, 50);
            lblDetailStatusTitle.Name = "lblDetailStatusTitle";
            lblDetailStatusTitle.Size = new Size(31, 15);
            lblDetailStatusTitle.TabIndex = 1;
            lblDetailStatusTitle.Text = "狀態";
            // 
            // lblDetailTitle
            // 
            lblDetailTitle.AutoEllipsis = true;
            lblDetailTitle.Dock = DockStyle.Top;
            lblDetailTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDetailTitle.Location = new Point(0, 0);
            lblDetailTitle.Name = "lblDetailTitle";
            lblDetailTitle.Size = new Size(277, 40);
            lblDetailTitle.TabIndex = 0;
            lblDetailTitle.Text = "請選擇一個任務以查看詳情";
            // 
            // AdminDashboardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 561);
            Controls.Add(mainTableLayoutPanel);
            Controls.Add(statusStrip1);
            MinimumSize = new Size(900, 600);
            Name = "AdminDashboardForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "管理員儀表板";

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


    }
}