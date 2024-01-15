
namespace WindowsApplication
{
    partial class AllocationUserControl
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
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bandedGridView = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.projectLabelControl = new DevExpress.XtraEditors.LabelControl();
            this.xxxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            this.gridControl.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.Location = new System.Drawing.Point(0, 0);
            this.gridControl.MainView = this.bandedGridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSpinEdit1});
            this.gridControl.Size = new System.Drawing.Size(730, 456);
            this.gridControl.TabIndex = 1;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridView});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xxxToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
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
            this.bandedGridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.bandedGridView.GridControl = this.gridControl;
            this.bandedGridView.Name = "bandedGridView";
            this.bandedGridView.OptionsSelection.MultiSelect = true;
            this.bandedGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.bandedGridView.OptionsView.ShowGroupPanel = false;
            this.bandedGridView.OptionsView.ShowIndicator = false;
            // 
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridBand1.AppearanceHeader.Options.UseFont = true;
            this.gridBand1.Caption = "Auditores";
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
            // projectLabelControl
            // 
            this.projectLabelControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.projectLabelControl.Appearance.BorderColor = System.Drawing.Color.Silver;
            this.projectLabelControl.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.projectLabelControl.Appearance.Options.UseBackColor = true;
            this.projectLabelControl.Appearance.Options.UseBorderColor = true;
            this.projectLabelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.projectLabelControl.Location = new System.Drawing.Point(610, 346);
            this.projectLabelControl.Name = "projectLabelControl";
            this.projectLabelControl.Padding = new System.Windows.Forms.Padding(4);
            this.projectLabelControl.Size = new System.Drawing.Size(73, 23);
            this.projectLabelControl.TabIndex = 3;
            this.projectLabelControl.Text = "labelControl1";
            this.projectLabelControl.Visible = false;
            // 
            // xxxToolStripMenuItem
            // 
            this.xxxToolStripMenuItem.Name = "xxxToolStripMenuItem";
            this.xxxToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.xxxToolStripMenuItem.Text = "xxx";
            this.xxxToolStripMenuItem.Click += new System.EventHandler(this.xxxToolStripMenuItem_Click);
            // 
            // AllocationUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.projectLabelControl);
            this.Controls.Add(this.gridControl);
            this.Name = "AllocationUserControl";
            this.Size = new System.Drawing.Size(730, 456);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraEditors.LabelControl projectLabelControl;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem xxxToolStripMenuItem;
    }
}
