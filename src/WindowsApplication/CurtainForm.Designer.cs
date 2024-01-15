
namespace WindowsApplication
{
    partial class CurtainForm
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
            this.pictureEdit = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureEdit
            // 
            this.pictureEdit.EditValue = global::WindowsApplication.Properties.Resources.ajax_loader;
            this.pictureEdit.Location = new System.Drawing.Point(138, 73);
            this.pictureEdit.Margin = new System.Windows.Forms.Padding(0);
            this.pictureEdit.Name = "pictureEdit";
            this.pictureEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit.Size = new System.Drawing.Size(150, 150);
            this.pictureEdit.TabIndex = 1;
            this.pictureEdit.UseDirectXPaint = DevExpress.Utils.DefaultBoolean.True;
            // 
            // CurtainForm
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 320);
            this.Controls.Add(this.pictureEdit);
            this.Name = "CurtainForm";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CurtainForm";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pictureEdit;
    }
}
