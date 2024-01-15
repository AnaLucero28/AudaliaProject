using DevExpress.Utils;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraGantt;
using DevExpress.XtraLayout;

namespace WindowsApplication
{
    partial class GanttUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GanttUserControl));
            DevExpress.Utils.Animation.PushTransition pushTransition1 = new DevExpress.Utils.Animation.PushTransition();
            this.ganttControl = new DevExpress.XtraGantt.GanttControl();
            this.taskNameColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.officeTreeListColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.startDateColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.finishDateColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.ProjectColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.EmployeeColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.resourcesColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.taxYearColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.categoryListColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.ganttAllowResizeBarCheckItem1 = new DevExpress.XtraGantt.Ribbon.GanttAllowResizeBarCheckItem();
            this.ganttPanelVisibilityBarSubItem1 = new DevExpress.XtraGantt.Ribbon.GanttPanelVisibilityBarSubItem();
            this.ganttPanelVisibilityDefaultBarCheckItem1 = new DevExpress.XtraGantt.Ribbon.GanttPanelVisibilityDefaultBarCheckItem();
            this.ganttPanelVisibilityBothBarCheckItem1 = new DevExpress.XtraGantt.Ribbon.GanttPanelVisibilityBothBarCheckItem();
            this.ganttPanelVisibilityChartBarCheckItem1 = new DevExpress.XtraGantt.Ribbon.GanttPanelVisibilityChartBarCheckItem();
            this.ganttPanelVisibilityTreeBarCheckItem1 = new DevExpress.XtraGantt.Ribbon.GanttPanelVisibilityTreeBarCheckItem();
            this.ganttFixedPanelBarSubItem1 = new DevExpress.XtraGantt.Ribbon.GanttFixedPanelBarSubItem();
            this.ganttFixedPanelDefaultBarCheckItem1 = new DevExpress.XtraGantt.Ribbon.GanttFixedPanelDefaultBarCheckItem();
            this.ganttFixedPanelNoneBarCheckItem1 = new DevExpress.XtraGantt.Ribbon.GanttFixedPanelNoneBarCheckItem();
            this.ganttFixedPanelChartBarCheckItem1 = new DevExpress.XtraGantt.Ribbon.GanttFixedPanelChartBarCheckItem();
            this.ganttFixedPanelTreeBarCheckItem1 = new DevExpress.XtraGantt.Ribbon.GanttFixedPanelTreeBarCheckItem();
            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            this.barCheckItem2 = new DevExpress.XtraBars.BarCheckItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.projectBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.projectRepositoryItemComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.employeeBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.employeeRepositoryItemComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItem3 = new DevExpress.XtraBars.BarCheckItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
            this.ejercicioBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.ejercicioRepositoryItemComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.monthBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.monthRepositoryItemComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.ganttViewRibbonPage1 = new DevExpress.XtraGantt.Ribbon.GanttViewRibbonPage();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ganttSplitViewRibbonPageGroup1 = new DevExpress.XtraGantt.Ribbon.GanttSplitViewRibbonPageGroup();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ganttBarController = new DevExpress.XtraGantt.Ribbon.GanttBarController();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.workspaceManager1 = new DevExpress.Utils.WorkspaceManager(this.components);
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            ((System.ComponentModel.ISupportInitialize)(this.ganttControl)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectRepositoryItemComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeRepositoryItemComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ejercicioRepositoryItemComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.monthRepositoryItemComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ganttBarController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ganttControl
            // 
            this.ganttControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ganttControl.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.taskNameColumn,
            this.officeTreeListColumn,
            this.startDateColumn,
            this.finishDateColumn,
            this.ProjectColumn,
            this.EmployeeColumn,
            this.resourcesColumn,
            this.taxYearColumn,
            this.categoryListColumn});
            this.ganttControl.ContextMenuStrip = this.contextMenuStrip1;
            this.ganttControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ganttControl.FixedLineWidth = 1;
            this.ganttControl.HorzScrollStep = 2;
            this.ganttControl.Location = new System.Drawing.Point(0, 146);
            this.ganttControl.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ganttControl.Margin = new System.Windows.Forms.Padding(2);
            this.ganttControl.MinWidth = 16;
            this.ganttControl.Name = "ganttControl";
            this.ganttControl.OptionsCustomization.AllowModifyDependencies = DevExpress.Utils.DefaultBoolean.True;
            this.ganttControl.OptionsCustomization.AllowModifyProgress = DevExpress.Utils.DefaultBoolean.True;
            this.ganttControl.OptionsCustomization.AllowModifyTasks = DevExpress.Utils.DefaultBoolean.True;
            this.ganttControl.OptionsMainTimeRuler.Count = 7;
            this.ganttControl.OptionsMainTimeRuler.MaxUnit = DevExpress.XtraGantt.GanttTimescaleUnit.Day;
            this.ganttControl.OptionsMainTimeRuler.MinUnit = DevExpress.XtraGantt.GanttTimescaleUnit.Day;
            this.ganttControl.OptionsScrollAnnotations.ShowFocusedRow = DevExpress.Utils.DefaultBoolean.True;
            this.ganttControl.OptionsSelection.SelectNodesOnRightClick = true;
            this.ganttControl.OptionsSplitter.OverlayResizeZoneThickness = 3;
            this.ganttControl.OptionsSplitter.SplitterThickness = 0;
            this.ganttControl.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.RowFullFocus;
            this.ganttControl.OptionsView.ShowHorzLines = true;
            this.ganttControl.OptionsView.ShowVertLines = false;
            this.ganttControl.Size = new System.Drawing.Size(1410, 679);
            this.ganttControl.SplitterPosition = 400;
            this.ganttControl.TabIndex = 0;
            this.ganttControl.TimescaleRulerHeight = 0;
            this.ganttControl.TreeLevelWidth = 12;
            this.ganttControl.TaskMoved += new DevExpress.XtraGantt.TaskMovedEventHandler(this.ganttControl_TaskMoved);
            this.ganttControl.TaskFinishDateModified += new DevExpress.XtraGantt.TaskFinishModifiedEventHandler(this.ganttControl_TaskFinishDateModified);
            this.ganttControl.CustomTaskScheduling += new DevExpress.XtraGantt.CustomTaskSchedulingEventHandler(this.ganttControl_CustomTaskScheduling);
            this.ganttControl.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.ganttControl_FocusedNodeChanged);
            this.ganttControl.CustomColumnDisplayText += new DevExpress.XtraTreeList.CustomColumnDisplayTextEventHandler(this.ganttControl_CustomColumnDisplayText);
            this.ganttControl.DoubleClick += new System.EventHandler(this.ganttControl_DoubleClick);
            // 
            // taskNameColumn
            // 
            this.taskNameColumn.Caption = "Nombre";
            this.taskNameColumn.FieldName = "Name";
            this.taskNameColumn.MinWidth = 16;
            this.taskNameColumn.Name = "taskNameColumn";
            this.taskNameColumn.OptionsColumn.AllowEdit = false;
            this.taskNameColumn.OptionsColumn.AllowFocus = false;
            this.taskNameColumn.OptionsColumn.ReadOnly = true;
            this.taskNameColumn.Visible = true;
            this.taskNameColumn.VisibleIndex = 0;
            this.taskNameColumn.Width = 200;
            // 
            // officeTreeListColumn
            // 
            this.officeTreeListColumn.Caption = "Delegación";
            this.officeTreeListColumn.FieldName = "BranchOffice";
            this.officeTreeListColumn.Name = "officeTreeListColumn";
            this.officeTreeListColumn.OptionsColumn.AllowEdit = false;
            this.officeTreeListColumn.OptionsColumn.AllowFocus = false;
            this.officeTreeListColumn.OptionsColumn.ReadOnly = true;
            this.officeTreeListColumn.Visible = true;
            this.officeTreeListColumn.VisibleIndex = 3;
            this.officeTreeListColumn.Width = 65;
            // 
            // startDateColumn
            // 
            this.startDateColumn.Caption = "Fecha de inicio";
            this.startDateColumn.FieldName = "StartDate";
            this.startDateColumn.MinWidth = 16;
            this.startDateColumn.Name = "startDateColumn";
            this.startDateColumn.OptionsColumn.AllowEdit = false;
            this.startDateColumn.OptionsColumn.AllowFocus = false;
            this.startDateColumn.OptionsColumn.ReadOnly = true;
            this.startDateColumn.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.startDateColumn.Visible = true;
            this.startDateColumn.VisibleIndex = 1;
            this.startDateColumn.Width = 69;
            // 
            // finishDateColumn
            // 
            this.finishDateColumn.Caption = "Fecha de finalización";
            this.finishDateColumn.FieldName = "FinishDate";
            this.finishDateColumn.MinWidth = 16;
            this.finishDateColumn.Name = "finishDateColumn";
            this.finishDateColumn.OptionsColumn.AllowEdit = false;
            this.finishDateColumn.OptionsColumn.AllowFocus = false;
            this.finishDateColumn.OptionsColumn.ReadOnly = true;
            this.finishDateColumn.Visible = true;
            this.finishDateColumn.VisibleIndex = 2;
            this.finishDateColumn.Width = 66;
            // 
            // ProjectColumn
            // 
            this.ProjectColumn.Caption = "Proyecto";
            this.ProjectColumn.FieldName = "Project";
            this.ProjectColumn.Name = "ProjectColumn";
            // 
            // EmployeeColumn
            // 
            this.EmployeeColumn.Caption = "Auditor";
            this.EmployeeColumn.FieldName = "Employee";
            this.EmployeeColumn.Name = "EmployeeColumn";
            // 
            // resourcesColumn
            // 
            this.resourcesColumn.Caption = "Resources";
            this.resourcesColumn.FieldName = "Resources";
            this.resourcesColumn.MinWidth = 16;
            this.resourcesColumn.Name = "resourcesColumn";
            this.resourcesColumn.Width = 50;
            // 
            // taxYearColumn
            // 
            this.taxYearColumn.Caption = "Ejercicio";
            this.taxYearColumn.FieldName = "TaxYear";
            this.taxYearColumn.Name = "taxYearColumn";
            this.taxYearColumn.OptionsColumn.AllowEdit = false;
            this.taxYearColumn.OptionsColumn.AllowFocus = false;
            this.taxYearColumn.OptionsColumn.ReadOnly = true;
            // 
            // categoryListColumn
            // 
            this.categoryListColumn.Caption = "Categoría";
            this.categoryListColumn.FieldName = "Category";
            this.categoryListColumn.Name = "categoryListColumn";
            this.categoryListColumn.Visible = true;
            this.categoryListColumn.VisibleIndex = 4;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(115, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem1.Text = "Filtrar";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(114, 22);
            this.toolStripMenuItem2.Text = "Asignar";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.AutoHideEmptyItems = true;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.ganttAllowResizeBarCheckItem1,
            this.ganttPanelVisibilityBarSubItem1,
            this.ganttPanelVisibilityDefaultBarCheckItem1,
            this.ganttPanelVisibilityBothBarCheckItem1,
            this.ganttPanelVisibilityChartBarCheckItem1,
            this.ganttPanelVisibilityTreeBarCheckItem1,
            this.ganttFixedPanelBarSubItem1,
            this.ganttFixedPanelDefaultBarCheckItem1,
            this.ganttFixedPanelNoneBarCheckItem1,
            this.ganttFixedPanelChartBarCheckItem1,
            this.ganttFixedPanelTreeBarCheckItem1,
            this.barCheckItem1,
            this.barCheckItem2,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.projectBarEditItem,
            this.employeeBarEditItem,
            this.barButtonItem5,
            this.barCheckItem3,
            this.barButtonItem6,
            this.barButtonItem7,
            this.barButtonItem8,
            this.barButtonItem9,
            this.ejercicioBarEditItem,
            this.monthBarEditItem});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 38;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ganttViewRibbonPage1});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.projectRepositoryItemComboBox,
            this.employeeRepositoryItemComboBox,
            this.ejercicioRepositoryItemComboBox,
            this.repositoryItemComboBox1,
            this.monthRepositoryItemComboBox});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.ribbonControl1.Size = new System.Drawing.Size(1410, 146);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
            // 
            // ganttAllowResizeBarCheckItem1
            // 
            this.ganttAllowResizeBarCheckItem1.Id = 1;
            this.ganttAllowResizeBarCheckItem1.Name = "ganttAllowResizeBarCheckItem1";
            // 
            // ganttPanelVisibilityBarSubItem1
            // 
            this.ganttPanelVisibilityBarSubItem1.Id = 2;
            this.ganttPanelVisibilityBarSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ganttPanelVisibilityDefaultBarCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.ganttPanelVisibilityBothBarCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.ganttPanelVisibilityChartBarCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.ganttPanelVisibilityTreeBarCheckItem1)});
            this.ganttPanelVisibilityBarSubItem1.Name = "ganttPanelVisibilityBarSubItem1";
            // 
            // ganttPanelVisibilityDefaultBarCheckItem1
            // 
            this.ganttPanelVisibilityDefaultBarCheckItem1.Id = 3;
            this.ganttPanelVisibilityDefaultBarCheckItem1.Name = "ganttPanelVisibilityDefaultBarCheckItem1";
            // 
            // ganttPanelVisibilityBothBarCheckItem1
            // 
            this.ganttPanelVisibilityBothBarCheckItem1.Id = 4;
            this.ganttPanelVisibilityBothBarCheckItem1.Name = "ganttPanelVisibilityBothBarCheckItem1";
            // 
            // ganttPanelVisibilityChartBarCheckItem1
            // 
            this.ganttPanelVisibilityChartBarCheckItem1.Id = 5;
            this.ganttPanelVisibilityChartBarCheckItem1.Name = "ganttPanelVisibilityChartBarCheckItem1";
            // 
            // ganttPanelVisibilityTreeBarCheckItem1
            // 
            this.ganttPanelVisibilityTreeBarCheckItem1.Id = 6;
            this.ganttPanelVisibilityTreeBarCheckItem1.Name = "ganttPanelVisibilityTreeBarCheckItem1";
            // 
            // ganttFixedPanelBarSubItem1
            // 
            this.ganttFixedPanelBarSubItem1.Id = 7;
            this.ganttFixedPanelBarSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ganttFixedPanelDefaultBarCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.ganttFixedPanelNoneBarCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.ganttFixedPanelChartBarCheckItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.ganttFixedPanelTreeBarCheckItem1)});
            this.ganttFixedPanelBarSubItem1.Name = "ganttFixedPanelBarSubItem1";
            // 
            // ganttFixedPanelDefaultBarCheckItem1
            // 
            this.ganttFixedPanelDefaultBarCheckItem1.Id = 8;
            this.ganttFixedPanelDefaultBarCheckItem1.Name = "ganttFixedPanelDefaultBarCheckItem1";
            // 
            // ganttFixedPanelNoneBarCheckItem1
            // 
            this.ganttFixedPanelNoneBarCheckItem1.Id = 9;
            this.ganttFixedPanelNoneBarCheckItem1.Name = "ganttFixedPanelNoneBarCheckItem1";
            // 
            // ganttFixedPanelChartBarCheckItem1
            // 
            this.ganttFixedPanelChartBarCheckItem1.Id = 10;
            this.ganttFixedPanelChartBarCheckItem1.Name = "ganttFixedPanelChartBarCheckItem1";
            // 
            // ganttFixedPanelTreeBarCheckItem1
            // 
            this.ganttFixedPanelTreeBarCheckItem1.Id = 11;
            this.ganttFixedPanelTreeBarCheckItem1.Name = "ganttFixedPanelTreeBarCheckItem1";
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.BindableChecked = true;
            this.barCheckItem1.Caption = "Por Auditor";
            this.barCheckItem1.Checked = true;
            this.barCheckItem1.GroupIndex = 1;
            this.barCheckItem1.Id = 12;
            this.barCheckItem1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCheckItem1.ImageOptions.SvgImage")));
            this.barCheckItem1.Name = "barCheckItem1";
            this.barCheckItem1.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem1_CheckedChanged);
            // 
            // barCheckItem2
            // 
            this.barCheckItem2.Caption = "Por Proyecto";
            this.barCheckItem2.GroupIndex = 1;
            this.barCheckItem2.Id = 13;
            this.barCheckItem2.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCheckItem2.ImageOptions.SvgImage")));
            this.barCheckItem2.Name = "barCheckItem2";
            this.barCheckItem2.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem2_CheckedChanged);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Nuevo Auditor";
            this.barButtonItem1.Id = 16;
            this.barButtonItem1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem1.ImageOptions.SvgImage")));
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Nuevo Proyecto";
            this.barButtonItem2.Id = 17;
            this.barButtonItem2.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem2.ImageOptions.SvgImage")));
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Guardar";
            this.barButtonItem3.Id = 18;
            this.barButtonItem3.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem3.ImageOptions.SvgImage")));
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Deshacer todo";
            this.barButtonItem4.Id = 19;
            this.barButtonItem4.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem4.ImageOptions.SvgImage")));
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // projectBarEditItem
            // 
            this.projectBarEditItem.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.projectBarEditItem.Caption = "Proyecto";
            this.projectBarEditItem.CaptionAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.projectBarEditItem.CaptionToEditorIndent = 4;
            this.projectBarEditItem.Edit = this.projectRepositoryItemComboBox;
            this.projectBarEditItem.EditWidth = 200;
            this.projectBarEditItem.Id = 20;
            this.projectBarEditItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("projectBarEditItem.ImageOptions.SvgImage")));
            this.projectBarEditItem.Name = "projectBarEditItem";
            // 
            // projectRepositoryItemComboBox
            // 
            this.projectRepositoryItemComboBox.AutoHeight = false;
            this.projectRepositoryItemComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.projectRepositoryItemComboBox.Name = "projectRepositoryItemComboBox";
            this.projectRepositoryItemComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.projectRepositoryItemComboBox.SelectedIndexChanged += new System.EventHandler(this.projectRepositoryItemComboBox_SelectedIndexChanged);
            // 
            // employeeBarEditItem
            // 
            this.employeeBarEditItem.Caption = "Auditor";
            this.employeeBarEditItem.CaptionToEditorIndent = 12;
            this.employeeBarEditItem.Edit = this.employeeRepositoryItemComboBox;
            this.employeeBarEditItem.EditWidth = 200;
            this.employeeBarEditItem.Id = 21;
            this.employeeBarEditItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("employeeBarEditItem.ImageOptions.SvgImage")));
            this.employeeBarEditItem.Name = "employeeBarEditItem";
            // 
            // employeeRepositoryItemComboBox
            // 
            this.employeeRepositoryItemComboBox.AutoHeight = false;
            this.employeeRepositoryItemComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.employeeRepositoryItemComboBox.Name = "employeeRepositoryItemComboBox";
            this.employeeRepositoryItemComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.employeeRepositoryItemComboBox.SelectedIndexChanged += new System.EventHandler(this.employeeRepositoryItemComboBox_SelectedIndexChanged);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "Refrescar";
            this.barButtonItem5.Id = 23;
            this.barButtonItem5.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem5.ImageOptions.SvgImage")));
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barCheckItem3
            // 
            this.barCheckItem3.BindableChecked = true;
            this.barCheckItem3.Caption = "Mostrar Auditores sin asignar";
            this.barCheckItem3.Checked = true;
            this.barCheckItem3.Id = 25;
            this.barCheckItem3.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCheckItem3.ImageOptions.SvgImage")));
            this.barCheckItem3.Name = "barCheckItem3";
            this.barCheckItem3.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem3_CheckedChanged);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "Limpiar Filtros";
            this.barButtonItem6.Id = 28;
            this.barButtonItem6.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem6.ImageOptions.SvgImage")));
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "Yo";
            this.barButtonItem7.Id = 29;
            this.barButtonItem7.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem7.ImageOptions.SvgImage")));
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem7_ItemClick);
            // 
            // barButtonItem8
            // 
            this.barButtonItem8.Caption = "Este Mes";
            this.barButtonItem8.Id = 30;
            this.barButtonItem8.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem8.ImageOptions.SvgImage")));
            this.barButtonItem8.Name = "barButtonItem8";
            this.barButtonItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem8_ItemClick);
            // 
            // barButtonItem9
            // 
            this.barButtonItem9.Caption = "Mi Oficina";
            this.barButtonItem9.Id = 31;
            this.barButtonItem9.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem9.ImageOptions.SvgImage")));
            this.barButtonItem9.Name = "barButtonItem9";
            this.barButtonItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem9_ItemClick);
            // 
            // ejercicioBarEditItem
            // 
            this.ejercicioBarEditItem.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.ejercicioBarEditItem.Caption = "Ejercicio";
            this.ejercicioBarEditItem.CaptionAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.ejercicioBarEditItem.CaptionToEditorIndent = 8;
            this.ejercicioBarEditItem.Edit = this.ejercicioRepositoryItemComboBox;
            this.ejercicioBarEditItem.EditWidth = 150;
            this.ejercicioBarEditItem.Id = 33;
            this.ejercicioBarEditItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("ejercicioBarEditItem.ImageOptions.SvgImage")));
            this.ejercicioBarEditItem.Name = "ejercicioBarEditItem";
            // 
            // ejercicioRepositoryItemComboBox
            // 
            this.ejercicioRepositoryItemComboBox.AutoHeight = false;
            this.ejercicioRepositoryItemComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ejercicioRepositoryItemComboBox.Name = "ejercicioRepositoryItemComboBox";
            this.ejercicioRepositoryItemComboBox.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ejercicioRepositoryItemComboBox.SelectedIndexChanged += new System.EventHandler(this.ejercicioRepositoryItemComboBox_SelectedIndexChanged);
            // 
            // monthBarEditItem
            // 
            this.monthBarEditItem.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.monthBarEditItem.Caption = "Mes";
            this.monthBarEditItem.CaptionAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.monthBarEditItem.CaptionToEditorIndent = 28;
            this.monthBarEditItem.Edit = this.monthRepositoryItemComboBox;
            this.monthBarEditItem.EditWidth = 150;
            this.monthBarEditItem.Id = 37;
            this.monthBarEditItem.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("monthBarEditItem.ImageOptions.SvgImage")));
            this.monthBarEditItem.Name = "monthBarEditItem";
            // 
            // monthRepositoryItemComboBox
            // 
            this.monthRepositoryItemComboBox.AutoHeight = false;
            this.monthRepositoryItemComboBox.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.monthRepositoryItemComboBox.Name = "monthRepositoryItemComboBox";
            this.monthRepositoryItemComboBox.SelectedIndexChanged += new System.EventHandler(this.monthRepositoryItemComboBox_SelectedIndexChanged);
            // 
            // ganttViewRibbonPage1
            // 
            this.ganttViewRibbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup3,
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ganttSplitViewRibbonPageGroup1});
            this.ganttViewRibbonPage1.Name = "ganttViewRibbonPage1";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem4);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem2, true);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Datos";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barCheckItem2);
            this.ribbonPageGroup1.ItemLinks.Add(this.barCheckItem1);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem5);
            this.ribbonPageGroup1.ItemLinks.Add(this.barCheckItem3);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Vista";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem7);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem9);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem8, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.ejercicioBarEditItem, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.monthBarEditItem);
            this.ribbonPageGroup2.ItemLinks.Add(this.employeeBarEditItem, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.projectBarEditItem);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem6, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Filtros";
            // 
            // ganttSplitViewRibbonPageGroup1
            // 
            this.ganttSplitViewRibbonPageGroup1.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False;
            this.ganttSplitViewRibbonPageGroup1.ItemLinks.Add(this.ganttAllowResizeBarCheckItem1);
            this.ganttSplitViewRibbonPageGroup1.ItemLinks.Add(this.ganttPanelVisibilityBarSubItem1);
            this.ganttSplitViewRibbonPageGroup1.ItemLinks.Add(this.ganttFixedPanelBarSubItem1);
            this.ganttSplitViewRibbonPageGroup1.Name = "ganttSplitViewRibbonPageGroup1";
            this.ganttSplitViewRibbonPageGroup1.Visible = false;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 825);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1410, 22);
            // 
            // ganttBarController
            // 
            this.ganttBarController.BarItems.Add(this.ganttAllowResizeBarCheckItem1);
            this.ganttBarController.BarItems.Add(this.ganttPanelVisibilityDefaultBarCheckItem1);
            this.ganttBarController.BarItems.Add(this.ganttPanelVisibilityBothBarCheckItem1);
            this.ganttBarController.BarItems.Add(this.ganttPanelVisibilityChartBarCheckItem1);
            this.ganttBarController.BarItems.Add(this.ganttPanelVisibilityTreeBarCheckItem1);
            this.ganttBarController.BarItems.Add(this.ganttPanelVisibilityBarSubItem1);
            this.ganttBarController.BarItems.Add(this.ganttFixedPanelDefaultBarCheckItem1);
            this.ganttBarController.BarItems.Add(this.ganttFixedPanelNoneBarCheckItem1);
            this.ganttBarController.BarItems.Add(this.ganttFixedPanelChartBarCheckItem1);
            this.ganttBarController.BarItems.Add(this.ganttFixedPanelTreeBarCheckItem1);
            this.ganttBarController.BarItems.Add(this.ganttFixedPanelBarSubItem1);
            this.ganttBarController.Control = this.ganttControl;
            // 
            // workspaceManager1
            // 
            this.workspaceManager1.TargetControl = this;
            this.workspaceManager1.TransitionType = pushTransition1;
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "Ejercicio";
            this.barEditItem1.CaptionAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.barEditItem1.CaptionToEditorIndent = 8;
            this.barEditItem1.Edit = this.ejercicioRepositoryItemComboBox;
            this.barEditItem1.EditWidth = 50;
            this.barEditItem1.Id = 33;
            this.barEditItem1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barEditItem1.ImageOptions.SvgImage")));
            this.barEditItem1.Name = "barEditItem1";
            // 
            // GanttUserControl
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ganttControl);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GanttUserControl";
            this.Size = new System.Drawing.Size(1410, 847);
            ((System.ComponentModel.ISupportInitialize)(this.ganttControl)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectRepositoryItemComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeRepositoryItemComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ejercicioRepositoryItemComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.monthRepositoryItemComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ganttBarController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private GanttControl ganttControl;
        private DevExpress.XtraTreeList.Columns.TreeListColumn taskNameColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn startDateColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn finishDateColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn resourcesColumn;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraGantt.Ribbon.GanttAllowResizeBarCheckItem ganttAllowResizeBarCheckItem1;
        private DevExpress.XtraGantt.Ribbon.GanttPanelVisibilityBarSubItem ganttPanelVisibilityBarSubItem1;
        private DevExpress.XtraGantt.Ribbon.GanttPanelVisibilityDefaultBarCheckItem ganttPanelVisibilityDefaultBarCheckItem1;
        private DevExpress.XtraGantt.Ribbon.GanttPanelVisibilityBothBarCheckItem ganttPanelVisibilityBothBarCheckItem1;
        private DevExpress.XtraGantt.Ribbon.GanttPanelVisibilityChartBarCheckItem ganttPanelVisibilityChartBarCheckItem1;
        private DevExpress.XtraGantt.Ribbon.GanttPanelVisibilityTreeBarCheckItem ganttPanelVisibilityTreeBarCheckItem1;
        private DevExpress.XtraGantt.Ribbon.GanttFixedPanelBarSubItem ganttFixedPanelBarSubItem1;
        private DevExpress.XtraGantt.Ribbon.GanttFixedPanelDefaultBarCheckItem ganttFixedPanelDefaultBarCheckItem1;
        private DevExpress.XtraGantt.Ribbon.GanttFixedPanelNoneBarCheckItem ganttFixedPanelNoneBarCheckItem1;
        private DevExpress.XtraGantt.Ribbon.GanttFixedPanelChartBarCheckItem ganttFixedPanelChartBarCheckItem1;
        private DevExpress.XtraGantt.Ribbon.GanttFixedPanelTreeBarCheckItem ganttFixedPanelTreeBarCheckItem1;
        private DevExpress.XtraGantt.Ribbon.GanttViewRibbonPage ganttViewRibbonPage1;
        private DevExpress.XtraGantt.Ribbon.GanttSplitViewRibbonPageGroup ganttSplitViewRibbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraGantt.Ribbon.GanttBarController ganttBarController;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private DevExpress.XtraBars.BarCheckItem barCheckItem2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn EmployeeColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn ProjectColumn;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.XtraBars.BarEditItem projectBarEditItem;
        private DevExpress.XtraBars.BarEditItem employeeBarEditItem;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn officeTreeListColumn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarCheckItem barCheckItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox projectRepositoryItemComboBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox employeeRepositoryItemComboBox;        
        private DevExpress.XtraBars.BarEditItem ejercicioBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ejercicioRepositoryItemComboBox;
        private DevExpress.XtraTreeList.Columns.TreeListColumn taxYearColumn;
        private WorkspaceManager workspaceManager1;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarEditItem monthBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox monthRepositoryItemComboBox;
        private DevExpress.XtraTreeList.Columns.TreeListColumn categoryListColumn;
    }
}
