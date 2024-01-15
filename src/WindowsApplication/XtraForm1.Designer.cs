namespace WindowsApplication
{
    partial class XtraForm1
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
            this.ganttControl1 = new DevExpress.XtraGantt.GanttControl();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)(this.ganttControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            this.SuspendLayout();
            // 
            // ganttControl1
            // 
            this.ganttControl1.Location = new System.Drawing.Point(34, 47);
            this.ganttControl1.Name = "ganttControl1";
            this.ganttControl1.Size = new System.Drawing.Size(164, 69);
            this.ganttControl1.TabIndex = 0;
            // 
            // treeList1
            // 
            this.treeList1.Location = new System.Drawing.Point(71, 156);
            this.treeList1.Name = "treeList1";
            this.treeList1.Size = new System.Drawing.Size(149, 55);
            this.treeList1.TabIndex = 1;
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(82, 12);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(8, 20);
            this.textEdit1.TabIndex = 2;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Location = new System.Drawing.Point(23, 139);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(103, 103);
            this.layoutControl1.TabIndex = 3;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(103, 103);
            this.Root.TextVisible = false;
            // 
            // XtraForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 268);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.treeList1);
            this.Controls.Add(this.ganttControl1);
            this.Name = "XtraForm1";
            this.Text = "XtraForm1";
            ((System.ComponentModel.ISupportInitialize)(this.ganttControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGantt.GanttControl ganttControl1;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
    }
}