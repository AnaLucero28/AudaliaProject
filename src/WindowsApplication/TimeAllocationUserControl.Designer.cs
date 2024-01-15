
namespace WindowsApplication
{
    partial class TimeAllocationUserControl
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeAllocationUserControl));
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.bandedGridView = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
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
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.ganttViewRibbonPage1 = new DevExpress.XtraGantt.Ribbon.GanttViewRibbonPage();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.Location = new System.Drawing.Point(0, 146);
            this.gridControl.MainView = this.bandedGridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSpinEdit1});
            this.gridControl.Size = new System.Drawing.Size(686, 339);
            this.gridControl.TabIndex = 0;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridView});
            // 
            // bandedGridView
            // 
            this.bandedGridView.Appearance.BandPanel.ForeColor = System.Drawing.Color.Black;
            this.bandedGridView.Appearance.BandPanel.Options.UseForeColor = true;
            this.bandedGridView.Appearance.GroupFooter.BackColor = System.Drawing.Color.White;
            this.bandedGridView.Appearance.GroupFooter.BackColor2 = System.Drawing.Color.White;
            this.bandedGridView.Appearance.GroupFooter.BorderColor = System.Drawing.Color.White;
            this.bandedGridView.Appearance.GroupFooter.Options.UseBackColor = true;
            this.bandedGridView.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.bandedGridView.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.bandedGridView.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.bandedGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.bandedGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridView.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.bandedGridView.GridControl = this.gridControl;
            this.bandedGridView.Name = "bandedGridView";
            this.bandedGridView.OptionsSelection.MultiSelect = true;
            this.bandedGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.bandedGridView.OptionsView.ShowIndicator = false;
            this.bandedGridView.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.bandedGridView_RowCellClick);
            this.bandedGridView.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.bandedGridView_CustomDrawCell);
            this.bandedGridView.CustomDrawRowFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.bandedGridView_CustomDrawRowFooterCell);
            this.bandedGridView.CustomDrawRowFooter += new DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventHandler(this.bandedGridView_CustomDrawRowFooter);
            this.bandedGridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.bandedGridView_RowCellStyle);
            this.bandedGridView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.bandedGridView_ShowingEditor);
            this.bandedGridView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.bandedGridView_CellValueChanged);
            this.bandedGridView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.bandedGridView_KeyPress);
            this.bandedGridView.DoubleClick += new System.EventHandler(this.bandedGridView_DoubleClick);
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "Proyectos";
            this.gridBand1.MinWidth = 300;
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 400;
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
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
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem1});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 21;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ganttViewRibbonPage1});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.ribbonControl1.Size = new System.Drawing.Size(686, 146);
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
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Guardar";
            this.barButtonItem3.Id = 18;
            this.barButtonItem3.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem3.ImageOptions.SvgImage")));
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Deshacer todo";
            this.barButtonItem4.Id = 19;
            this.barButtonItem4.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem4.ImageOptions.SvgImage")));
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Exportar";
            this.barButtonItem1.Id = 20;
            this.barButtonItem1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem1.ImageOptions.SvgImage")));
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // ganttViewRibbonPage1
            // 
            this.ganttViewRibbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup3});
            this.ganttViewRibbonPage1.Name = "ganttViewRibbonPage1";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem4);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Datos";
            // 
            // TimeAllocationUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "TimeAllocationUserControl";
            this.Size = new System.Drawing.Size(686, 485);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
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
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraGantt.Ribbon.GanttViewRibbonPage ganttViewRibbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
    }
}
