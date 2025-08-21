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
        private Controls.RichTextEditor richTextEditorComments;
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

        private Label lblFilterStatus;
        private ComboBox cmbFilterStatus;
        private CheckBox chkFilterOverdue;
        private Panel panelDetailActions;
        private Button btnDetailDelete;
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDetails;
        private Button btnDetailReassign;

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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AdminDashboardForm));
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
            richTextEditorComments = new Controls.RichTextEditor();
            tableLayoutPanelDetails = new TableLayoutPanel();
            lblDetailStatusTitle = new Label();
            lblDetailStatus = new Label();
            lblDetailPriorityTitle = new Label();
            lblDetailPriority = new Label();
            lblDetailDueDateTitle = new Label();
            lblDetailDueDate = new Label();
            lblDetailCreatorTitle = new Label();
            lblDetailCreator = new Label();
            lblDetailAssignedToTitle = new Label();
            lblDetailAssignedTo = new Label();
            lblDetailCreationDateTitle = new Label();
            lblDetailCreationDate = new Label();
            lblDetailLastModifiedTitle = new Label();
            lblDetailLastModified = new Label();
            panelDetailActions = new Panel();
            btnSaveComment = new Button();
            btnViewHistory = new Button();
            btnDetailEdit = new Button();
            btnDetailDelete = new Button();
            lblDetailTitle = new Label();
            lblDetailCommentsTitle = new Label();

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
            cmbFilterByUser.Location = new Point(105, 122);
            cmbFilterByUser.Name = "cmbFilterByUser";
            cmbFilterByUser.Size = new Size(268, 23);
            cmbFilterByUser.TabIndex = 9;
            // 
            // lblFilterByUser
            // 
            lblFilterByUser.AutoSize = true;
            lblFilterByUser.Location = new Point(17, 125);
            lblFilterByUser.Name = "lblFilterByUser";
            lblFilterByUser.Size = new Size(55, 15);
            lblFilterByUser.TabIndex = 8;
            lblFilterByUser.Text = "使用者：";
            // 
            // chkFilterOverdue
            // 
            chkFilterOverdue.AutoSize = true;
            chkFilterOverdue.Location = new Point(105, 157);
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
            cmbFilterStatus.Location = new Point(105, 90);
            cmbFilterStatus.Name = "cmbFilterStatus";
            cmbFilterStatus.Size = new Size(268, 23);
            cmbFilterStatus.TabIndex = 6;
            // 
            // lblFilterStatus
            // 
            lblFilterStatus.AutoSize = true;
            lblFilterStatus.Location = new Point(17, 93);
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
            cmbFilterPriority.Location = new Point(105, 55);
            cmbFilterPriority.Name = "cmbFilterPriority";
            cmbFilterPriority.Size = new Size(268, 23);
            cmbFilterPriority.TabIndex = 3;
            // 
            // lblFilterPriority
            // 
            lblFilterPriority.AutoSize = true;
            lblFilterPriority.Location = new Point(17, 58);
            lblFilterPriority.Name = "lblFilterPriority";
            lblFilterPriority.Size = new Size(55, 15);
            lblFilterPriority.TabIndex = 2;
            lblFilterPriority.Text = "優先級：";
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.Location = new Point(105, 19);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(268, 23);
            txtSearch.TabIndex = 1;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(17, 22);
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
            panelTaskDetails.Controls.Add(richTextEditorComments);
            panelTaskDetails.Controls.Add(tableLayoutPanelDetails);
            panelTaskDetails.Controls.Add(panelDetailActions);
            panelTaskDetails.Controls.Add(lblDetailTitle);
            panelTaskDetails.Dock = DockStyle.Fill;
            panelTaskDetails.Location = new Point(10, 10);
            panelTaskDetails.Name = "panelTaskDetails";
            panelTaskDetails.Size = new Size(277, 513);
            panelTaskDetails.TabIndex = 0;
            // 
            // richTextEditorComments
            // 
            richTextEditorComments.Dock = DockStyle.Fill;
            richTextEditorComments.Location = new Point(0, 29);
            richTextEditorComments.Name = "richTextEditorComments";
            richTextEditorComments.ReadOnly = false;
            richTextEditorComments.Rtf = resources.GetString("richTextEditorComments.Rtf");
            richTextEditorComments.Size = new Size(277, 444);
            richTextEditorComments.TabIndex = 2;
            // 
            // tableLayoutPanelDetails
            // 
            this.tableLayoutPanelDetails.AutoSize = true;
            this.tableLayoutPanelDetails.ColumnCount = 2;
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailStatusTitle, 0, 0);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailStatus, 1, 0);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailPriorityTitle, 0, 1);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailPriority, 1, 1);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailDueDateTitle, 0, 2);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailDueDate, 1, 2);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailCreatorTitle, 0, 3);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailCreator, 1, 3);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailAssignedToTitle, 0, 4);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailAssignedTo, 1, 4);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailCreationDateTitle, 0, 5);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailCreationDate, 1, 5);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailLastModifiedTitle, 0, 6);
            this.tableLayoutPanelDetails.Controls.Add(this.lblDetailLastModified, 1, 6);

            this.tableLayoutPanelDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelDetails.Location = new System.Drawing.Point(0, 50);
            this.tableLayoutPanelDetails.Name = "tableLayoutPanelDetails";
            tableLayoutPanelDetails.RowCount = 7;
            tableLayoutPanelDetails.RowStyles.Add(new RowStyle());
            tableLayoutPanelDetails.RowStyles.Add(new RowStyle());
            tableLayoutPanelDetails.RowStyles.Add(new RowStyle());
            tableLayoutPanelDetails.RowStyles.Add(new RowStyle());
            tableLayoutPanelDetails.RowStyles.Add(new RowStyle());
            tableLayoutPanelDetails.RowStyles.Add(new RowStyle());
            tableLayoutPanelDetails.RowStyles.Add(new RowStyle());
            tableLayoutPanelDetails.Size = new Size(277, 0);
            tableLayoutPanelDetails.TabIndex = 1;

            this.lblDetailStatusTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDetailStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDetailStatus.AutoSize = true;
            // 
            // lblDetailStatusTitle
            // 
            lblDetailStatusTitle.Anchor = AnchorStyles.Left;
            lblDetailStatusTitle.AutoSize = true;
            lblDetailStatusTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailStatusTitle.Location = new Point(3, 0);
            lblDetailStatusTitle.Name = "lblDetailStatusTitle";
            lblDetailStatusTitle.Size = new Size(31, 15);
            lblDetailStatusTitle.TabIndex = 1;
            lblDetailStatusTitle.Text = "狀態";
            // 
            // lblDetailStatus
            // 
            lblDetailStatus.Anchor = AnchorStyles.Left;
            lblDetailStatus.AutoSize = true;
            lblDetailStatus.Location = new Point(83, 0);
            lblDetailStatus.Name = "lblDetailStatus";
            lblDetailStatus.Size = new Size(12, 15);
            lblDetailStatus.TabIndex = 2;
            lblDetailStatus.Text = "-";
            // 
            // lblDetailPriorityTitle
            // 
            lblDetailPriorityTitle.Anchor = AnchorStyles.Left;
            lblDetailPriorityTitle.AutoSize = true;
            lblDetailPriorityTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailPriorityTitle.Location = new Point(3, 15);
            lblDetailPriorityTitle.Name = "lblDetailPriorityTitle";
            lblDetailPriorityTitle.Size = new Size(43, 15);
            lblDetailPriorityTitle.TabIndex = 3;
            lblDetailPriorityTitle.Text = "優先級";
            // 
            // lblDetailPriority
            // 
            lblDetailPriority.Anchor = AnchorStyles.Left;
            lblDetailPriority.AutoSize = true;
            lblDetailPriority.Location = new Point(83, 15);
            lblDetailPriority.Name = "lblDetailPriority";
            lblDetailPriority.Size = new Size(12, 15);
            lblDetailPriority.TabIndex = 4;
            lblDetailPriority.Text = "-";
            // 
            // lblDetailDueDateTitle
            // 
            lblDetailDueDateTitle.Anchor = AnchorStyles.Left;
            lblDetailDueDateTitle.AutoSize = true;
            lblDetailDueDateTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailDueDateTitle.Location = new Point(3, 30);
            lblDetailDueDateTitle.Name = "lblDetailDueDateTitle";
            lblDetailDueDateTitle.Size = new Size(43, 15);
            lblDetailDueDateTitle.TabIndex = 5;
            lblDetailDueDateTitle.Text = "到期日";
            // 
            // lblDetailDueDate
            // 
            lblDetailDueDate.Anchor = AnchorStyles.Left;
            lblDetailDueDate.AutoSize = true;
            lblDetailDueDate.Location = new Point(83, 30);
            lblDetailDueDate.Name = "lblDetailDueDate";
            lblDetailDueDate.Size = new Size(12, 15);
            lblDetailDueDate.TabIndex = 6;
            lblDetailDueDate.Text = "-";
            // 
            // lblDetailCreatorTitle
            // 
            lblDetailCreatorTitle.Anchor = AnchorStyles.Left;
            lblDetailCreatorTitle.AutoSize = true;
            lblDetailCreatorTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailCreatorTitle.Location = new Point(3, 45);
            lblDetailCreatorTitle.Name = "lblDetailCreatorTitle";
            lblDetailCreatorTitle.Size = new Size(43, 15);
            lblDetailCreatorTitle.TabIndex = 7;
            lblDetailCreatorTitle.Text = "建立者";
            // 
            // lblDetailCreator
            // 
            lblDetailCreator.Anchor = AnchorStyles.Left;
            lblDetailCreator.AutoSize = true;
            lblDetailCreator.Location = new Point(83, 45);
            lblDetailCreator.Name = "lblDetailCreator";
            lblDetailCreator.Size = new Size(12, 15);
            lblDetailCreator.TabIndex = 8;
            lblDetailCreator.Text = "-";
            // 
            // lblDetailAssignedToTitle
            // 
            lblDetailAssignedToTitle.Anchor = AnchorStyles.Left;
            lblDetailAssignedToTitle.AutoSize = true;
            lblDetailAssignedToTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailAssignedToTitle.Location = new Point(3, 60);
            lblDetailAssignedToTitle.Name = "lblDetailAssignedToTitle";
            lblDetailAssignedToTitle.Size = new Size(43, 15);
            lblDetailAssignedToTitle.TabIndex = 9;
            lblDetailAssignedToTitle.Text = "指派給";
            // 
            // lblDetailAssignedTo
            // 
            lblDetailAssignedTo.Anchor = AnchorStyles.Left;
            lblDetailAssignedTo.AutoSize = true;
            lblDetailAssignedTo.Location = new Point(83, 60);
            lblDetailAssignedTo.Name = "lblDetailAssignedTo";
            lblDetailAssignedTo.Size = new Size(12, 15);
            lblDetailAssignedTo.TabIndex = 10;
            lblDetailAssignedTo.Text = "-";
            // 
            // lblDetailCreationDateTitle
            // 
            lblDetailCreationDateTitle.Anchor = AnchorStyles.Left;
            lblDetailCreationDateTitle.AutoSize = true;
            lblDetailCreationDateTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailCreationDateTitle.Location = new Point(3, 75);
            lblDetailCreationDateTitle.Name = "lblDetailCreationDateTitle";
            lblDetailCreationDateTitle.Size = new Size(55, 15);
            lblDetailCreationDateTitle.TabIndex = 13;
            lblDetailCreationDateTitle.Text = "建立時間";
            // 
            // lblDetailCreationDate
            // 
            lblDetailCreationDate.Anchor = AnchorStyles.Left;
            lblDetailCreationDate.AutoSize = true;
            lblDetailCreationDate.Location = new Point(83, 75);
            lblDetailCreationDate.Name = "lblDetailCreationDate";
            lblDetailCreationDate.Size = new Size(12, 15);
            lblDetailCreationDate.TabIndex = 14;
            lblDetailCreationDate.Text = "-";
            // 
            // lblDetailLastModifiedTitle
            // 
            lblDetailLastModifiedTitle.Anchor = AnchorStyles.Left;
            lblDetailLastModifiedTitle.AutoSize = true;
            lblDetailLastModifiedTitle.ForeColor = SystemColors.ControlDarkDark;
            lblDetailLastModifiedTitle.Location = new Point(3, 90);
            lblDetailLastModifiedTitle.Name = "lblDetailLastModifiedTitle";
            lblDetailLastModifiedTitle.Size = new Size(55, 15);
            lblDetailLastModifiedTitle.TabIndex = 15;
            lblDetailLastModifiedTitle.Text = "最後更新";
            // 
            // lblDetailLastModified
            // 
            lblDetailLastModified.Anchor = AnchorStyles.Left;
            lblDetailLastModified.AutoSize = true;
            lblDetailLastModified.Location = new Point(83, 90);
            lblDetailLastModified.Name = "lblDetailLastModified";
            lblDetailLastModified.Size = new Size(12, 15);
            lblDetailLastModified.TabIndex = 16;
            lblDetailLastModified.Text = "-";
            // 
            // panelDetailActions
            // 
            panelDetailActions.Controls.Add(btnSaveComment);
            panelDetailActions.Controls.Add(btnViewHistory);
            panelDetailActions.Controls.Add(btnDetailEdit);
            panelDetailActions.Controls.Add(btnDetailDelete);
            panelDetailActions.Dock = DockStyle.Bottom;
            panelDetailActions.Location = new Point(0, 473);
            panelDetailActions.Name = "panelDetailActions";
            panelDetailActions.Size = new Size(277, 40);
            panelDetailActions.TabIndex = 3;
            // 
            // btnSaveComment
            // 
            btnSaveComment.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveComment.Enabled = true;
            btnSaveComment.Location = new Point(3, 8);
            btnSaveComment.Name = "btnSaveComment";
            btnSaveComment.Size = new Size(75, 23);
            btnSaveComment.TabIndex = 17;
            btnSaveComment.Text = "儲存";
            btnSaveComment.UseVisualStyleBackColor = true;
            // 
            // btnViewHistory
            // 
            btnViewHistory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnViewHistory.Location = new Point(140, 8);
            btnViewHistory.Name = "btnViewHistory";
            btnViewHistory.Size = new Size(75, 23);
            btnViewHistory.TabIndex = 3;
            btnViewHistory.Text = "查看歷史";
            btnViewHistory.UseVisualStyleBackColor = true;
            // 
            // btnDetailEdit
            // 
            btnDetailEdit.Location = new Point(79, 8);
            btnDetailEdit.Name = "btnDetailEdit";
            btnDetailEdit.Size = new Size(60, 23);
            btnDetailEdit.TabIndex = 0;
            btnDetailEdit.Text = "編輯";
            btnDetailEdit.UseVisualStyleBackColor = true;
            // 
            // btnDetailDelete
            // 
            btnDetailDelete.Location = new Point(216, 8);
            btnDetailDelete.Name = "btnDetailDelete";
            btnDetailDelete.Size = new Size(60, 23);
            btnDetailDelete.TabIndex = 2;
            btnDetailDelete.Text = "刪除";
            btnDetailDelete.UseVisualStyleBackColor = true;
            // 
            // lblDetailTitle
            // 
            lblDetailTitle.Dock = DockStyle.Top;
            lblDetailTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDetailTitle.Location = new Point(0, 0);
            lblDetailTitle.Name = "lblDetailTitle";
            lblDetailTitle.Padding = new Padding(0, 0, 0, 5);
            lblDetailTitle.Size = new Size(277, 29);
            lblDetailTitle.TabIndex = 0;
            lblDetailTitle.Text = "請選擇一個任務";
            // 
            // lblDetailCommentsTitle
            // 
            lblDetailCommentsTitle.AutoSize = true;
            lblDetailCommentsTitle.Location = new Point(0, 225);
            lblDetailCommentsTitle.Name = "lblDetailCommentsTitle";
            lblDetailCommentsTitle.Size = new Size(31, 15);
            lblDetailCommentsTitle.TabIndex = 11;
            lblDetailCommentsTitle.Text = "備註";
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
            panelFilters.ResumeLayout(false);
            panelFilters.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            treeViewContextMenu.ResumeLayout(false);
            mainTableLayoutPanel.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            panelStatistics.ResumeLayout(false);
            cardCompleted.ResumeLayout(false);
            cardRejected.ResumeLayout(false);
            cardUnassigned.ResumeLayout(false);
            cardOverdue.ResumeLayout(false);
            cardUncompleted.ResumeLayout(false);
            cardTotalTasks.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            panelTaskDetails.ResumeLayout(false);
            panelTaskDetails.PerformLayout();
            panelDetailActions.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

    }
}