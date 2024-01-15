
using DevExpress.XtraGantt;

namespace WindowsApplication.AvailabilityUserControl
{
    partial class AvailabilityUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AvailabilityUserControl));
            this.ganttControl = new DevExpress.XtraGantt.GanttControl();
            this.SelectedColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.taskNameColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.officeListColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.startDateColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.finishDateColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.ProjectColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.EmployeeColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.resourcesColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.CategoryColumn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
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
            this.ejercicioBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.ejercicioRepositoryItemComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.employeeBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.employeeRepositoryItemComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.ganttViewRibbonPage1 = new DevExpress.XtraGantt.Ribbon.GanttViewRibbonPage();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ganttControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ejercicioRepositoryItemComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeRepositoryItemComboBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ganttControl
            // 
            this.ganttControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ganttControl.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.SelectedColumn,
            this.taskNameColumn,
            this.officeListColumn,
            this.startDateColumn,
            this.finishDateColumn,
            this.ProjectColumn,
            this.EmployeeColumn,
            this.resourcesColumn,
            this.CategoryColumn});
            this.ganttControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ganttControl.FixedLineWidth = 1;
            this.ganttControl.HorzScrollStep = 2;
            this.ganttControl.HorzScrollVisibility = DevExpress.XtraTreeList.ScrollVisibility.Always;
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
            this.ganttControl.OptionsScrollAnnotations.ShowFocusedRow = DevExpress.Utils.DefaultBoolean.False;
            this.ganttControl.OptionsSelection.SelectNodesOnRightClick = true;
            this.ganttControl.OptionsSplitter.OverlayResizeZoneThickness = 3;
            this.ganttControl.OptionsSplitter.SplitterThickness = 0;
            this.ganttControl.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.RowFullFocus;
            this.ganttControl.OptionsView.ShowHorzLines = true;
            this.ganttControl.OptionsView.ShowVertLines = false;
            this.ganttControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.ganttControl.Size = new System.Drawing.Size(1357, 701);
            this.ganttControl.SplitterPosition = 300;
            this.ganttControl.TabIndex = 0;
            this.ganttControl.TimescaleRulerHeight = 0;
            this.ganttControl.TreeLevelWidth = 12;
            this.ganttControl.VertScrollVisibility = DevExpress.XtraTreeList.ScrollVisibility.Always;
            this.ganttControl.CustomTaskScheduling += new DevExpress.XtraGantt.CustomTaskSchedulingEventHandler(this.ganttControl_CustomTaskScheduling);
            this.ganttControl.TaskToolTipShowing += new DevExpress.XtraGantt.GanttTaskToolTipShowingEventHandler(this.ganttControl_TaskToolTipShowing);
            this.ganttControl.NodeCellStyle += new DevExpress.XtraTreeList.GetCustomNodeCellStyleEventHandler(this.ganttControl_NodeCellStyle);
            this.ganttControl.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.ganttControl_FocusedNodeChanged);
            this.ganttControl.CellValueChanging += new DevExpress.XtraTreeList.CellValueChangedEventHandler(this.ganttControl_CellValueChanging);
            this.ganttControl.Load += new System.EventHandler(this.ganttControl_Load);
            this.ganttControl.DoubleClick += new System.EventHandler(this.ganttControl_DoubleClick);
            this.ganttControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ganttControl_MouseMove);
            // 
            // SelectedColumn
            // 
            this.SelectedColumn.Caption = " ";
            this.SelectedColumn.ColumnEdit = this.repositoryItemCheckEdit1;
            this.SelectedColumn.FieldName = "Selected";
            this.SelectedColumn.Fixed = DevExpress.XtraTreeList.Columns.FixedStyle.Left;
            this.SelectedColumn.Name = "SelectedColumn";
            this.SelectedColumn.OptionsColumn.FixedWidth = true;
            this.SelectedColumn.Visible = true;
            this.SelectedColumn.VisibleIndex = 0;
            this.SelectedColumn.Width = 34;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // taskNameColumn
            // 
            this.taskNameColumn.Caption = "Auditor";
            this.taskNameColumn.FieldName = "Name";
            this.taskNameColumn.MinWidth = 16;
            this.taskNameColumn.Name = "taskNameColumn";
            this.taskNameColumn.OptionsColumn.AllowFocus = false;
            this.taskNameColumn.OptionsColumn.ReadOnly = true;
            this.taskNameColumn.Visible = true;
            this.taskNameColumn.VisibleIndex = 1;
            this.taskNameColumn.Width = 165;
            // 
            // officeListColumn
            // 
            this.officeListColumn.Caption = "Delegación";
            this.officeListColumn.FieldName = "BranchOffice";
            this.officeListColumn.Name = "officeListColumn";
            this.officeListColumn.OptionsColumn.AllowEdit = false;
            this.officeListColumn.OptionsColumn.AllowFocus = false;
            this.officeListColumn.OptionsColumn.ReadOnly = true;
            this.officeListColumn.Visible = true;
            this.officeListColumn.VisibleIndex = 3;
            // 
            // startDateColumn
            // 
            this.startDateColumn.Caption = "Start Date";
            this.startDateColumn.FieldName = "StartDate";
            this.startDateColumn.MinWidth = 16;
            this.startDateColumn.Name = "startDateColumn";
            this.startDateColumn.Width = 80;
            // 
            // finishDateColumn
            // 
            this.finishDateColumn.Caption = "Finish Date";
            this.finishDateColumn.FieldName = "FinishDate";
            this.finishDateColumn.MinWidth = 16;
            this.finishDateColumn.Name = "finishDateColumn";
            this.finishDateColumn.Width = 77;
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
            // CategoryColumn
            // 
            this.CategoryColumn.Caption = "Categoría";
            this.CategoryColumn.FieldName = "Category";
            this.CategoryColumn.Name = "CategoryColumn";
            this.CategoryColumn.OptionsColumn.AllowEdit = false;
            this.CategoryColumn.OptionsColumn.AllowFocus = false;
            this.CategoryColumn.OptionsColumn.ReadOnly = true;
            this.CategoryColumn.Visible = true;
            this.CategoryColumn.VisibleIndex = 2;
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
            this.ejercicioBarEditItem,
            this.employeeBarEditItem,
            this.barButtonItem4});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 26;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ganttViewRibbonPage1});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ejercicioRepositoryItemComboBox,
            this.employeeRepositoryItemComboBox});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.ribbonControl1.Size = new System.Drawing.Size(1357, 146);
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
            // 
            // barCheckItem2
            // 
            this.barCheckItem2.Caption = "Por Proyecto";
            this.barCheckItem2.GroupIndex = 1;
            this.barCheckItem2.Id = 13;
            this.barCheckItem2.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCheckItem2.ImageOptions.SvgImage")));
            this.barCheckItem2.Name = "barCheckItem2";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Exportar";
            this.barButtonItem1.Id = 20;
            this.barButtonItem1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem1.ImageOptions.SvgImage")));
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Yo";
            this.barButtonItem2.Id = 21;
            this.barButtonItem2.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem2.ImageOptions.SvgImage")));
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Mi Oficina";
            this.barButtonItem3.Id = 22;
            this.barButtonItem3.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem3.ImageOptions.SvgImage")));
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // ejercicioBarEditItem
            // 
            this.ejercicioBarEditItem.Caption = "Ejercicio";
            this.ejercicioBarEditItem.CaptionToEditorIndent = 8;
            this.ejercicioBarEditItem.Edit = this.ejercicioRepositoryItemComboBox;
            this.ejercicioBarEditItem.EditWidth = 150;
            this.ejercicioBarEditItem.Id = 23;
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
            // employeeBarEditItem
            // 
            this.employeeBarEditItem.Caption = "Auditor";
            this.employeeBarEditItem.CaptionToEditorIndent = 12;
            this.employeeBarEditItem.Edit = this.employeeRepositoryItemComboBox;
            this.employeeBarEditItem.EditWidth = 150;
            this.employeeBarEditItem.Id = 24;
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
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Limpiar Filtros";
            this.barButtonItem4.Id = 25;
            this.barButtonItem4.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem4.ImageOptions.SvgImage")));
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // ganttViewRibbonPage1
            // 
            this.ganttViewRibbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup3,
            this.ribbonPageGroup1});
            this.ganttViewRibbonPage1.Name = "ganttViewRibbonPage1";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Datos";
            this.ribbonPageGroup3.Visible = false;
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem2);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup1.ItemLinks.Add(this.ejercicioBarEditItem, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.employeeBarEditItem);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem4, true);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Filtros";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.labelControl1.Appearance.Options.UseBackColor = true;
            this.labelControl1.Location = new System.Drawing.Point(1116, 70);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Padding = new System.Windows.Forms.Padding(4);
            this.labelControl1.Size = new System.Drawing.Size(71, 21);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "labelControl1";
            this.labelControl1.Visible = false;
            // 
            // AvailabilityUserControl
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.ganttControl);
            this.Controls.Add(this.ribbonControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AvailabilityUserControl";
            this.Size = new System.Drawing.Size(1357, 847);
            ((System.ComponentModel.ISupportInitialize)(this.ganttControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ejercicioRepositoryItemComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeRepositoryItemComboBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GanttControl ganttControl;
        private DevExpress.XtraTreeList.Columns.TreeListColumn taskNameColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn startDateColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn finishDateColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn resourcesColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn EmployeeColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn ProjectColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn SelectedColumn;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn CategoryColumn;
        private DevExpress.XtraTreeList.Columns.TreeListColumn officeListColumn;
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
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private DevExpress.XtraBars.BarCheckItem barCheckItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraGantt.Ribbon.GanttViewRibbonPage ganttViewRibbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarEditItem ejercicioBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ejercicioRepositoryItemComboBox;
        private DevExpress.XtraBars.BarEditItem employeeBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox employeeRepositoryItemComboBox;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
    }
}
