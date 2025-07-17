#nullable disable

namespace TodoApp.WinForms.Forms
{
    partial class AdminDashboardForm
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
            this.components = new System.ComponentModel.Container();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.cmbFilterByUser = new System.Windows.Forms.ComboBox();
            this.lblFilterByUser = new System.Windows.Forms.Label();
            this.chkFilterOverdue = new System.Windows.Forms.CheckBox();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.cmbFilterPriority = new System.Windows.Forms.ComboBox();
            this.lblFilterPriority = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tvTasks = new System.Windows.Forms.TreeView();
            this.treeViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxEditTask = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxReassignTask = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDeleteTask = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelStatistics = new System.Windows.Forms.Panel();
            this.cardUnassigned = new System.Windows.Forms.Panel();
            this.lblUnassignedValue = new System.Windows.Forms.Label();
            this.lblUnassignedTitle = new System.Windows.Forms.Label();
            this.cardOverdue = new System.Windows.Forms.Panel();
            this.lblOverdueValue = new System.Windows.Forms.Label();
            this.lblOverdueTitle = new System.Windows.Forms.Label();
            this.cardUncompleted = new System.Windows.Forms.Panel();
            this.lblUncompletedValue = new System.Windows.Forms.Label();
            this.lblUncompletedTitle = new System.Windows.Forms.Label();
            this.cardTotalTasks = new System.Windows.Forms.Panel();
            this.lblTotalTasksValue = new System.Windows.Forms.Label();
            this.lblTotalTasksTitle = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelTaskDetails = new System.Windows.Forms.Panel();
            this.lblDetailLastModified = new System.Windows.Forms.Label();
            this.lblDetailLastModifiedTitle = new System.Windows.Forms.Label();
            this.lblDetailCreationDate = new System.Windows.Forms.Label();
            this.lblDetailCreationDateTitle = new System.Windows.Forms.Label();
            this.panelDetailActions = new System.Windows.Forms.Panel();
            this.btnDetailDelete = new System.Windows.Forms.Button();
            this.btnDetailReassign = new System.Windows.Forms.Button();
            this.btnDetailEdit = new System.Windows.Forms.Button();
            this.txtDetailComments = new System.Windows.Forms.TextBox();
            this.lblDetailCommentsTitle = new System.Windows.Forms.Label();
            this.lblDetailAssignedTo = new System.Windows.Forms.Label();
            this.lblDetailAssignedToTitle = new System.Windows.Forms.Label();
            this.lblDetailCreator = new System.Windows.Forms.Label();
            this.lblDetailCreatorTitle = new System.Windows.Forms.Label();
            this.lblDetailDueDate = new System.Windows.Forms.Label();
            this.lblDetailDueDateTitle = new System.Windows.Forms.Label();
            this.lblDetailPriority = new System.Windows.Forms.Label();
            this.lblDetailPriorityTitle = new System.Windows.Forms.Label();
            this.lblDetailStatus = new System.Windows.Forms.Label();
            this.lblDetailStatusTitle = new System.Windows.Forms.Label();
            this.lblDetailTitle = new System.Windows.Forms.Label();
            this.panelFilters.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.treeViewContextMenu.SuspendLayout();
            this.mainTableLayoutPanel.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelStatistics.SuspendLayout();
            this.cardUnassigned.SuspendLayout();
            this.cardOverdue.SuspendLayout();
            this.cardUncompleted.SuspendLayout();
            this.cardTotalTasks.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelTaskDetails.SuspendLayout();
            this.panelDetailActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFilters
            // 
            this.panelFilters.Controls.Add(this.cmbFilterByUser);
            this.panelFilters.Controls.Add(this.lblFilterByUser);
            this.panelFilters.Controls.Add(this.chkFilterOverdue);
            this.panelFilters.Controls.Add(this.cmbFilterStatus);
            this.panelFilters.Controls.Add(this.lblFilterStatus);
            this.panelFilters.Controls.Add(this.btnClearFilter);
            this.panelFilters.Controls.Add(this.cmbFilterPriority);
            this.panelFilters.Controls.Add(this.lblFilterPriority);
            this.panelFilters.Controls.Add(this.txtSearch);
            this.panelFilters.Controls.Add(this.lblSearch);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 140);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(5);
            this.panelFilters.Size = new System.Drawing.Size(296, 210);
            this.panelFilters.TabIndex = 0;
            // 
            // cmbFilterByUser
            // 
            this.cmbFilterByUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFilterByUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterByUser.FormattingEnabled = true;
            this.cmbFilterByUser.Location = new System.Drawing.Point(100, 50);
            this.cmbFilterByUser.Name = "cmbFilterByUser";
            this.cmbFilterByUser.Size = new System.Drawing.Size(184, 23);
            this.cmbFilterByUser.TabIndex = 9;
            // 
            // lblFilterByUser
            // 
            this.lblFilterByUser.AutoSize = true;
            this.lblFilterByUser.Location = new System.Drawing.Point(12, 53);
            this.lblFilterByUser.Name = "lblFilterByUser";
            this.lblFilterByUser.Size = new System.Drawing.Size(55, 15);
            this.lblFilterByUser.TabIndex = 8;
            this.lblFilterByUser.Text = "使用者：";
            // 
            // chkFilterOverdue
            // 
            this.chkFilterOverdue.AutoSize = true;
            this.chkFilterOverdue.Location = new System.Drawing.Point(100, 155);
            this.chkFilterOverdue.Name = "chkFilterOverdue";
            this.chkFilterOverdue.Size = new System.Drawing.Size(110, 19);
            this.chkFilterOverdue.TabIndex = 7;
            this.chkFilterOverdue.Text = "僅顯示逾期任務";
            this.chkFilterOverdue.UseVisualStyleBackColor = true;
            // 
            // cmbFilterStatus
            // 
            this.cmbFilterStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStatus.FormattingEnabled = true;
            this.cmbFilterStatus.Location = new System.Drawing.Point(100, 120);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(184, 23);
            this.cmbFilterStatus.TabIndex = 6;
            // 
            // lblFilterStatus
            // 
            this.lblFilterStatus.AutoSize = true;
            this.lblFilterStatus.Location = new System.Drawing.Point(12, 123);
            this.lblFilterStatus.Name = "lblFilterStatus";
            this.lblFilterStatus.Size = new System.Drawing.Size(43, 15);
            this.lblFilterStatus.TabIndex = 5;
            this.lblFilterStatus.Text = "狀態：";
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Location = new System.Drawing.Point(100, 180);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(75, 23);
            this.btnClearFilter.TabIndex = 4;
            this.btnClearFilter.Text = "清除篩選";
            this.btnClearFilter.UseVisualStyleBackColor = true;
            // 
            // cmbFilterPriority
            // 
            this.cmbFilterPriority.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFilterPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterPriority.FormattingEnabled = true;
            this.cmbFilterPriority.Location = new System.Drawing.Point(100, 85);
            this.cmbFilterPriority.Name = "cmbFilterPriority";
            this.cmbFilterPriority.Size = new System.Drawing.Size(184, 23);
            this.cmbFilterPriority.TabIndex = 3;
            // 
            // lblFilterPriority
            // 
            this.lblFilterPriority.AutoSize = true;
            this.lblFilterPriority.Location = new System.Drawing.Point(12, 88);
            this.lblFilterPriority.Name = "lblFilterPriority";
            this.lblFilterPriority.Size = new System.Drawing.Size(59, 15);
            this.lblFilterPriority.TabIndex = 2;
            this.lblFilterPriority.Text = "優先級：";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(100, 14);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(184, 23);
            this.txtSearch.TabIndex = 1;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(12, 17);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(43, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "搜尋：";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1184, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(58, 17);
            this.lblStatus.Text = "準備就緒";
            // 
            // tvTasks
            // 
            this.tvTasks.ContextMenuStrip = this.treeViewContextMenu;
            this.tvTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTasks.FullRowSelect = true;
            this.tvTasks.HideSelection = false;
            this.tvTasks.Location = new System.Drawing.Point(305, 3);
            this.tvTasks.Name = "tvTasks";
            this.tvTasks.ShowLines = true;
            this.tvTasks.Size = new System.Drawing.Size(586, 533);
            this.tvTasks.TabIndex = 0;
            // 
            // treeViewContextMenu
            // 
            this.treeViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxEditTask,
            this.ctxReassignTask,
            this.ctxDeleteTask});
            this.treeViewContextMenu.Name = "treeViewContextMenu";
            this.treeViewContextMenu.Size = new System.Drawing.Size(137, 70);
            // 
            // ctxEditTask
            // 
            this.ctxEditTask.Name = "ctxEditTask";
            this.ctxEditTask.Size = new System.Drawing.Size(136, 22);
            this.ctxEditTask.Text = "編輯任務(&E)";
            // 
            // ctxReassignTask
            // 
            this.ctxReassignTask.Name = "ctxReassignTask";
            this.ctxReassignTask.Size = new System.Drawing.Size(136, 22);
            this.ctxReassignTask.Text = "重新指派(&R)";
            // 
            // ctxDeleteTask
            // 
            this.ctxDeleteTask.Name = "ctxDeleteTask";
            this.ctxDeleteTask.Size = new System.Drawing.Size(136, 22);
            this.ctxDeleteTask.Text = "刪除任務(&D)";
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 3;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.5F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.5F));
            this.mainTableLayoutPanel.Controls.Add(this.tvTasks, 1, 0);
            this.mainTableLayoutPanel.Controls.Add(this.panelLeft, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.panelRight, 2, 0);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 1;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(1184, 539);
            this.mainTableLayoutPanel.TabIndex = 2;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.panelFilters);
            this.panelLeft.Controls.Add(this.panelStatistics);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(3, 3);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(296, 533);
            this.panelLeft.TabIndex = 1;
            // 
            // panelStatistics
            // 
            this.panelStatistics.Controls.Add(this.cardUnassigned);
            this.panelStatistics.Controls.Add(this.cardOverdue);
            this.panelStatistics.Controls.Add(this.cardUncompleted);
            this.panelStatistics.Controls.Add(this.cardTotalTasks);
            this.panelStatistics.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatistics.Location = new System.Drawing.Point(0, 0);
            this.panelStatistics.Name = "panelStatistics";
            this.panelStatistics.Padding = new System.Windows.Forms.Padding(5);
            this.panelStatistics.Size = new System.Drawing.Size(296, 140);
            this.panelStatistics.TabIndex = 1;
            // 
            // cardUnassigned
            // 
            this.cardUnassigned.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardUnassigned.Controls.Add(this.lblUnassignedValue);
            this.cardUnassigned.Controls.Add(this.lblUnassignedTitle);
            this.cardUnassigned.Location = new System.Drawing.Point(155, 75);
            this.cardUnassigned.Name = "cardUnassigned";
            this.cardUnassigned.Size = new System.Drawing.Size(130, 55);
            this.cardUnassigned.TabIndex = 3;
            // 
            // lblUnassignedValue
            // 
            this.lblUnassignedValue.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblUnassignedValue.ForeColor = System.Drawing.Color.Orange;
            this.lblUnassignedValue.Location = new System.Drawing.Point(3, 20);
            this.lblUnassignedValue.Name = "lblUnassignedValue";
            this.lblUnassignedValue.Size = new System.Drawing.Size(122, 25);
            this.lblUnassignedValue.Text = "0";
            this.lblUnassignedValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUnassignedTitle
            // 
            this.lblUnassignedTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUnassignedTitle.Location = new System.Drawing.Point(0, 0);
            this.lblUnassignedTitle.Name = "lblUnassignedTitle";
            this.lblUnassignedTitle.Size = new System.Drawing.Size(128, 20);
            this.lblUnassignedTitle.Text = "未指派任務";
            this.lblUnassignedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardOverdue
            // 
            this.cardOverdue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardOverdue.Controls.Add(this.lblOverdueValue);
            this.cardOverdue.Controls.Add(this.lblOverdueTitle);
            this.cardOverdue.Location = new System.Drawing.Point(10, 75);
            this.cardOverdue.Name = "cardOverdue";
            this.cardOverdue.Size = new System.Drawing.Size(130, 55);
            this.cardOverdue.TabIndex = 2;
            // 
            // lblOverdueValue
            // 
            this.lblOverdueValue.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblOverdueValue.ForeColor = System.Drawing.Color.Red;
            this.lblOverdueValue.Location = new System.Drawing.Point(3, 20);
            this.lblOverdueValue.Name = "lblOverdueValue";
            this.lblOverdueValue.Size = new System.Drawing.Size(122, 25);
            this.lblOverdueValue.Text = "0";
            this.lblOverdueValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOverdueTitle
            // 
            this.lblOverdueTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOverdueTitle.Location = new System.Drawing.Point(0, 0);
            this.lblOverdueTitle.Name = "lblOverdueTitle";
            this.lblOverdueTitle.Size = new System.Drawing.Size(128, 20);
            this.lblOverdueTitle.Text = "已逾期任務";
            this.lblOverdueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardUncompleted
            // 
            this.cardUncompleted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardUncompleted.Controls.Add(this.lblUncompletedValue);
            this.cardUncompleted.Controls.Add(this.lblUncompletedTitle);
            this.cardUncompleted.Location = new System.Drawing.Point(155, 10);
            this.cardUncompleted.Name = "cardUncompleted";
            this.cardUncompleted.Size = new System.Drawing.Size(130, 55);
            this.cardUncompleted.TabIndex = 1;
            // 
            // lblUncompletedValue
            // 
            this.lblUncompletedValue.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblUncompletedValue.Location = new System.Drawing.Point(3, 20);
            this.lblUncompletedValue.Name = "lblUncompletedValue";
            this.lblUncompletedValue.Size = new System.Drawing.Size(122, 25);
            this.lblUncompletedValue.Text = "0";
            this.lblUncompletedValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUncompletedTitle
            // 
            this.lblUncompletedTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUncompletedTitle.Location = new System.Drawing.Point(0, 0);
            this.lblUncompletedTitle.Name = "lblUncompletedTitle";
            this.lblUncompletedTitle.Size = new System.Drawing.Size(128, 20);
            this.lblUncompletedTitle.Text = "未完成任務";
            this.lblUncompletedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardTotalTasks
            // 
            this.cardTotalTasks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotalTasks.Controls.Add(this.lblTotalTasksValue);
            this.cardTotalTasks.Controls.Add(this.lblTotalTasksTitle);
            this.cardTotalTasks.Location = new System.Drawing.Point(10, 10);
            this.cardTotalTasks.Name = "cardTotalTasks";
            this.cardTotalTasks.Size = new System.Drawing.Size(130, 55);
            this.cardTotalTasks.TabIndex = 0;
            // 
            // lblTotalTasksValue
            // 
            this.lblTotalTasksValue.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTotalTasksValue.Location = new System.Drawing.Point(3, 20);
            this.lblTotalTasksValue.Name = "lblTotalTasksValue";
            this.lblTotalTasksValue.Size = new System.Drawing.Size(122, 25);
            this.lblTotalTasksValue.Text = "0";
            this.lblTotalTasksValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalTasksTitle
            // 
            this.lblTotalTasksTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalTasksTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTotalTasksTitle.Name = "lblTotalTasksTitle";
            this.lblTotalTasksTitle.Size = new System.Drawing.Size(128, 20);
            this.lblTotalTasksTitle.Text = "總任務數";
            this.lblTotalTasksTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.SystemColors.Window;
            this.panelRight.Controls.Add(this.panelTaskDetails);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(897, 3);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(10);
            this.panelRight.Size = new System.Drawing.Size(284, 533);
            this.panelRight.TabIndex = 2;
            // 
            // panelTaskDetails
            // 
            this.panelTaskDetails.Controls.Add(this.lblDetailLastModified);
            this.panelTaskDetails.Controls.Add(this.lblDetailLastModifiedTitle);
            this.panelTaskDetails.Controls.Add(this.lblDetailCreationDate);
            this.panelTaskDetails.Controls.Add(this.lblDetailCreationDateTitle);
            this.panelTaskDetails.Controls.Add(this.panelDetailActions);
            this.panelTaskDetails.Controls.Add(this.txtDetailComments);
            this.panelTaskDetails.Controls.Add(this.lblDetailCommentsTitle);
            this.panelTaskDetails.Controls.Add(this.lblDetailAssignedTo);
            this.panelTaskDetails.Controls.Add(this.lblDetailAssignedToTitle);
            this.panelTaskDetails.Controls.Add(this.lblDetailCreator);
            this.panelTaskDetails.Controls.Add(this.lblDetailCreatorTitle);
            this.panelTaskDetails.Controls.Add(this.lblDetailDueDate);
            this.panelTaskDetails.Controls.Add(this.lblDetailDueDateTitle);
            this.panelTaskDetails.Controls.Add(this.lblDetailPriority);
            this.panelTaskDetails.Controls.Add(this.lblDetailPriorityTitle);
            this.panelTaskDetails.Controls.Add(this.lblDetailStatus);
            this.panelTaskDetails.Controls.Add(this.lblDetailStatusTitle);
            this.panelTaskDetails.Controls.Add(this.lblDetailTitle);
            this.panelTaskDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTaskDetails.Location = new System.Drawing.Point(10, 10);
            this.panelTaskDetails.Name = "panelTaskDetails";
            this.panelTaskDetails.Size = new System.Drawing.Size(264, 513);
            this.panelTaskDetails.TabIndex = 0;
            this.panelTaskDetails.Visible = false;
            // 
            // lblDetailLastModified
            // 
            this.lblDetailLastModified.AutoSize = true;
            this.lblDetailLastModified.Location = new System.Drawing.Point(70, 200);
            this.lblDetailLastModified.Name = "lblDetailLastModified";
            this.lblDetailLastModified.Size = new System.Drawing.Size(12, 15);
            this.lblDetailLastModified.TabIndex = 16;
            this.lblDetailLastModified.Text = "-";
            // 
            // lblDetailLastModifiedTitle
            // 
            this.lblDetailLastModifiedTitle.AutoSize = true;
            this.lblDetailLastModifiedTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDetailLastModifiedTitle.Location = new System.Drawing.Point(0, 200);
            this.lblDetailLastModifiedTitle.Name = "lblDetailLastModifiedTitle";
            this.lblDetailLastModifiedTitle.Size = new System.Drawing.Size(55, 15);
            this.lblDetailLastModifiedTitle.TabIndex = 15;
            this.lblDetailLastModifiedTitle.Text = "最後更新";
            // 
            // lblDetailCreationDate
            // 
            this.lblDetailCreationDate.AutoSize = true;
            this.lblDetailCreationDate.Location = new System.Drawing.Point(70, 175);
            this.lblDetailCreationDate.Name = "lblDetailCreationDate";
            this.lblDetailCreationDate.Size = new System.Drawing.Size(12, 15);
            this.lblDetailCreationDate.TabIndex = 14;
            this.lblDetailCreationDate.Text = "-";
            // 
            // lblDetailCreationDateTitle
            // 
            this.lblDetailCreationDateTitle.AutoSize = true;
            this.lblDetailCreationDateTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDetailCreationDateTitle.Location = new System.Drawing.Point(0, 175);
            this.lblDetailCreationDateTitle.Name = "lblDetailCreationDateTitle";
            this.lblDetailCreationDateTitle.Size = new System.Drawing.Size(55, 15);
            this.lblDetailCreationDateTitle.TabIndex = 13;
            this.lblDetailCreationDateTitle.Text = "建立時間";
            // 
            // panelDetailActions
            // 
            this.panelDetailActions.Controls.Add(this.btnDetailDelete);
            this.panelDetailActions.Controls.Add(this.btnDetailReassign);
            this.panelDetailActions.Controls.Add(this.btnDetailEdit);
            this.panelDetailActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDetailActions.Location = new System.Drawing.Point(0, 473);
            this.panelDetailActions.Name = "panelDetailActions";
            this.panelDetailActions.Size = new System.Drawing.Size(264, 40);
            this.panelDetailActions.TabIndex = 13;
            // 
            // btnDetailDelete
            // 
            this.btnDetailDelete.Location = new System.Drawing.Point(175, 8);
            this.btnDetailDelete.Name = "btnDetailDelete";
            this.btnDetailDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDetailDelete.TabIndex = 2;
            this.btnDetailDelete.Text = "刪除";
            this.btnDetailDelete.UseVisualStyleBackColor = true;
            // 
            // btnDetailReassign
            // 
            this.btnDetailReassign.Location = new System.Drawing.Point(88, 8);
            this.btnDetailReassign.Name = "btnDetailReassign";
            this.btnDetailReassign.Size = new System.Drawing.Size(75, 23);
            this.btnDetailReassign.TabIndex = 1;
            this.btnDetailReassign.Text = "重新指派";
            this.btnDetailReassign.UseVisualStyleBackColor = true;
            // 
            // btnDetailEdit
            // 
            this.btnDetailEdit.Location = new System.Drawing.Point(0, 8);
            this.btnDetailEdit.Name = "btnDetailEdit";
            this.btnDetailEdit.Size = new System.Drawing.Size(75, 23);
            this.btnDetailEdit.TabIndex = 0;
            this.btnDetailEdit.Text = "編輯";
            this.btnDetailEdit.UseVisualStyleBackColor = true;
            // 
            // txtDetailComments
            // 
            this.txtDetailComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDetailComments.BackColor = System.Drawing.SystemColors.Window;
            this.txtDetailComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetailComments.Location = new System.Drawing.Point(0, 248);
            this.txtDetailComments.Multiline = true;
            this.txtDetailComments.Name = "txtDetailComments";
            this.txtDetailComments.ReadOnly = true;
            this.txtDetailComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetailComments.Size = new System.Drawing.Size(264, 219);
            this.txtDetailComments.TabIndex = 12;
            // 
            // lblDetailCommentsTitle
            // 
            this.lblDetailCommentsTitle.AutoSize = true;
            this.lblDetailCommentsTitle.Location = new System.Drawing.Point(0, 230);
            this.lblDetailCommentsTitle.Name = "lblDetailCommentsTitle";
            this.lblDetailCommentsTitle.Size = new System.Drawing.Size(31, 15);
            this.lblDetailCommentsTitle.TabIndex = 11;
            this.lblDetailCommentsTitle.Text = "備註";
            // 
            // lblDetailAssignedTo
            // 
            this.lblDetailAssignedTo.AutoSize = true;
            this.lblDetailAssignedTo.Location = new System.Drawing.Point(70, 150);
            this.lblDetailAssignedTo.Name = "lblDetailAssignedTo";
            this.lblDetailAssignedTo.Size = new System.Drawing.Size(12, 15);
            this.lblDetailAssignedTo.TabIndex = 10;
            this.lblDetailAssignedTo.Text = "-";
            // 
            // lblDetailAssignedToTitle
            // 
            this.lblDetailAssignedToTitle.AutoSize = true;
            this.lblDetailAssignedToTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDetailAssignedToTitle.Location = new System.Drawing.Point(0, 150);
            this.lblDetailAssignedToTitle.Name = "lblDetailAssignedToTitle";
            this.lblDetailAssignedToTitle.Size = new System.Drawing.Size(43, 15);
            this.lblDetailAssignedToTitle.TabIndex = 9;
            this.lblDetailAssignedToTitle.Text = "指派給";
            // 
            // lblDetailCreator
            // 
            this.lblDetailCreator.AutoSize = true;
            this.lblDetailCreator.Location = new System.Drawing.Point(70, 125);
            this.lblDetailCreator.Name = "lblDetailCreator";
            this.lblDetailCreator.Size = new System.Drawing.Size(12, 15);
            this.lblDetailCreator.TabIndex = 8;
            this.lblDetailCreator.Text = "-";
            // 
            // lblDetailCreatorTitle
            // 
            this.lblDetailCreatorTitle.AutoSize = true;
            this.lblDetailCreatorTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDetailCreatorTitle.Location = new System.Drawing.Point(0, 125);
            this.lblDetailCreatorTitle.Name = "lblDetailCreatorTitle";
            this.lblDetailCreatorTitle.Size = new System.Drawing.Size(43, 15);
            this.lblDetailCreatorTitle.TabIndex = 7;
            this.lblDetailCreatorTitle.Text = "建立者";
            // 
            // lblDetailDueDate
            // 
            this.lblDetailDueDate.AutoSize = true;
            this.lblDetailDueDate.Location = new System.Drawing.Point(70, 100);
            this.lblDetailDueDate.Name = "lblDetailDueDate";
            this.lblDetailDueDate.Size = new System.Drawing.Size(12, 15);
            this.lblDetailDueDate.TabIndex = 6;
            this.lblDetailDueDate.Text = "-";
            // 
            // lblDetailDueDateTitle
            // 
            this.lblDetailDueDateTitle.AutoSize = true;
            this.lblDetailDueDateTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDetailDueDateTitle.Location = new System.Drawing.Point(0, 100);
            this.lblDetailDueDateTitle.Name = "lblDetailDueDateTitle";
            this.lblDetailDueDateTitle.Size = new System.Drawing.Size(43, 15);
            this.lblDetailDueDateTitle.TabIndex = 5;
            this.lblDetailDueDateTitle.Text = "到期日";
            // 
            // lblDetailPriority
            // 
            this.lblDetailPriority.AutoSize = true;
            this.lblDetailPriority.Location = new System.Drawing.Point(70, 75);
            this.lblDetailPriority.Name = "lblDetailPriority";
            this.lblDetailPriority.Size = new System.Drawing.Size(12, 15);
            this.lblDetailPriority.TabIndex = 4;
            this.lblDetailPriority.Text = "-";
            // 
            // lblDetailPriorityTitle
            // 
            this.lblDetailPriorityTitle.AutoSize = true;
            this.lblDetailPriorityTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDetailPriorityTitle.Location = new System.Drawing.Point(0, 75);
            this.lblDetailPriorityTitle.Name = "lblDetailPriorityTitle";
            this.lblDetailPriorityTitle.Size = new System.Drawing.Size(43, 15);
            this.lblDetailPriorityTitle.TabIndex = 3;
            this.lblDetailPriorityTitle.Text = "優先級";
            // 
            // lblDetailStatus
            // 
            this.lblDetailStatus.AutoSize = true;
            this.lblDetailStatus.Location = new System.Drawing.Point(70, 50);
            this.lblDetailStatus.Name = "lblDetailStatus";
            this.lblDetailStatus.Size = new System.Drawing.Size(12, 15);
            this.lblDetailStatus.TabIndex = 2;
            this.lblDetailStatus.Text = "-";
            // 
            // lblDetailStatusTitle
            // 
            this.lblDetailStatusTitle.AutoSize = true;
            this.lblDetailStatusTitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblDetailStatusTitle.Location = new System.Drawing.Point(0, 50);
            this.lblDetailStatusTitle.Name = "lblDetailStatusTitle";
            this.lblDetailStatusTitle.Size = new System.Drawing.Size(31, 15);
            this.lblDetailStatusTitle.TabIndex = 1;
            this.lblDetailStatusTitle.Text = "狀態";
            // 
            // lblDetailTitle
            // 
            this.lblDetailTitle.AutoEllipsis = true;
            this.lblDetailTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDetailTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDetailTitle.Location = new System.Drawing.Point(0, 0);
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Size = new System.Drawing.Size(264, 40);
            this.lblDetailTitle.TabIndex = 0;
            this.lblDetailTitle.Text = "請選擇一個任務以查看詳情";
            // 
            // AdminDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 561);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Controls.Add(this.statusStrip1);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "AdminDashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "管理員儀表板";
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.treeViewContextMenu.ResumeLayout(false);
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelStatistics.ResumeLayout(false);
            this.cardUnassigned.ResumeLayout(false);
            this.cardOverdue.ResumeLayout(false);
            this.cardUncompleted.ResumeLayout(false);
            this.cardTotalTasks.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelTaskDetails.ResumeLayout(false);
            this.panelTaskDetails.PerformLayout();
            this.panelDetailActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TreeView tvTasks;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.ComboBox cmbFilterPriority;
        private System.Windows.Forms.Label lblFilterByUser;
        private System.Windows.Forms.ComboBox cmbFilterByUser;
        private System.Windows.Forms.Label lblFilterPriority;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelStatistics;
        private System.Windows.Forms.Panel cardTotalTasks;
        private System.Windows.Forms.Label lblTotalTasksValue;
        private System.Windows.Forms.Label lblTotalTasksTitle;
        private System.Windows.Forms.Panel cardUncompleted;
        private System.Windows.Forms.Label lblUncompletedValue;
        private System.Windows.Forms.Label lblUncompletedTitle;
        private System.Windows.Forms.Panel cardOverdue;
        private System.Windows.Forms.Label lblOverdueValue;
        private System.Windows.Forms.Label lblOverdueTitle;
        private System.Windows.Forms.Panel cardUnassigned;
        private System.Windows.Forms.Label lblUnassignedValue;
        private System.Windows.Forms.Label lblUnassignedTitle;
        private System.Windows.Forms.Panel panelTaskDetails;
        private System.Windows.Forms.Label lblDetailTitle;
        private System.Windows.Forms.Label lblDetailStatusTitle;
        private System.Windows.Forms.Label lblDetailStatus;
        private System.Windows.Forms.Label lblDetailPriorityTitle;
        private System.Windows.Forms.Label lblDetailPriority;
        private System.Windows.Forms.Label lblDetailDueDateTitle;
        private System.Windows.Forms.Label lblDetailDueDate;
        private System.Windows.Forms.Label lblDetailCreatorTitle;
        private System.Windows.Forms.Label lblDetailCreator;
        private System.Windows.Forms.Label lblDetailAssignedToTitle;
        private System.Windows.Forms.Label lblDetailAssignedTo;
        private System.Windows.Forms.Label lblDetailCommentsTitle;
        private System.Windows.Forms.TextBox txtDetailComments;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.CheckBox chkFilterOverdue;
        private System.Windows.Forms.Panel panelDetailActions;
        private System.Windows.Forms.Button btnDetailDelete;
        private System.Windows.Forms.Button btnDetailReassign;
        private System.Windows.Forms.Button btnDetailEdit;
        private System.Windows.Forms.ContextMenuStrip treeViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ctxEditTask;
        private System.Windows.Forms.ToolStripMenuItem ctxReassignTask;
        private System.Windows.Forms.ToolStripMenuItem ctxDeleteTask;
        private System.Windows.Forms.Label lblDetailLastModified;
        private System.Windows.Forms.Label lblDetailLastModifiedTitle;
        private System.Windows.Forms.Label lblDetailCreationDate;
        private System.Windows.Forms.Label lblDetailCreationDateTitle;
    }
}