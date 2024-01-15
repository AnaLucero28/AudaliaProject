using Audalia.DataHUBClient;
using DevExpress.Skins;
using DevExpress.Skins.XtraForm;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsApplication.Properties;

namespace WindowsApplication
{
    public partial class MainForm : DevExpress.XtraBars.TabForm
    {
        static int OpenFormCount = 0;
        static bool AddingTab = false;
        static MainForm instance;
        static CurtainForm curtainForm;
        public MainForm()
        {
            InitializeComponent();
            instance = this;

            tabFormControl.PageCreated += tabFormControl_PageCreated;
            tabFormControl.OuterFormCreating += OnOuterFormCreating;

            //

            DataHUBClient.Initiate();
            Data.Audit.ReadDB();
        }

        protected override DevExpress.Skins.XtraForm.FormPainter CreateFormBorderPainter()
        {
            return new MyFormPainter(this, LookAndFeel);
        }

        public static void ShowCurtain()
        {
            if (curtainForm == null)
            {
                curtainForm = new CurtainForm();
                curtainForm.FormBorderStyle = FormBorderStyle.None;
                curtainForm.AllowTransparency = true;
                curtainForm.BackColor = Color.White;
                curtainForm.Opacity = 0.8;
            }

            curtainForm.Show();
        }

        public static void HideCurtain()
        {
            if (curtainForm != null)
            {
                curtainForm.Close();
                curtainForm = null;
            }
        }

        public void OpenMainMenu(bool createPage = true)
        {
            AddingTab = true;
            if (createPage)
                instance.tabFormControl.AddNewPage();
            var page = instance.tabFormControl.Pages[instance.tabFormControl.Pages.Count - 1];
            page.Text = " Menu Principal";
            var control = new MainMenu();
            control.Dock = DockStyle.Fill;
            page.ContentContainer.Controls.Add(control);
            AddingTab = false;
        }

        void OnOuterFormCreating(object sender, OuterFormCreatingEventArgs e)
        {     
            /*
            MainForm form = new MainForm();
            form.TabFormControl.Pages.Clear();
            e.Form = form;        
            */
        }

        private void tabFormControl_PageCreated(object sender, PageCreatedEventArgs e)
        {
            OpenFormCount++;
            if (!AddingTab)
            {
                this.OpenMainMenu(false);
                /*
                e.Page.Text = " Menu Principal";
                var form = new MainMenu();
                form.Dock = DockStyle.Fill;
                e.Page.ContentContainer.Controls.Add(form);
                form.Show();
                */
            }
        }

        public static void AddNewTab(UserControl control, string title)
        {            
            AddingTab = true;
            instance.tabFormControl.AddNewPage();
            var page = instance.tabFormControl.Pages[instance.tabFormControl.Pages.Count - 1];
            page.Text = " " + title;
            control.Dock = DockStyle.Fill;
            page.ContentContainer.Controls.Add(control);            
            AddingTab = false;            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //tabFormControl.AddNewPage();            
        }

    }

    public class MyFormPainter : DevExpress.XtraBars.TabFormPainter
    {
        public MyFormPainter(Control owner, ISkinProvider provider) : base(owner, provider) { }

        /*
        protected override void DrawCaptionBackground(GraphicsCache cache, Rectangle formBounds)
        {
            cache.DrawRectangle(new Pen(Color.Fuchsia), formBounds);
        }
        */
        
        protected override void DrawText(DevExpress.Utils.Drawing.GraphicsCache cache)
        {
            /*
            Rectangle r;
            if (Application.OpenForms[0].WindowState == FormWindowState.Maximized)
                r = new Rectangle(14, 13, 85, 29);
            else
                r = new Rectangle(8, 6, 85, 29);
            */

            Rectangle r;
            if (Application.OpenForms[0].WindowState == FormWindowState.Maximized)
                r = new Rectangle(16, 14, 85, 29);
            else
                r = new Rectangle(8, 6, 85, 29);


            //cache.FillRectangle(Color.FromArgb(240, 240, 240), r);
            cache.FillRectangle(Color.FromArgb(248, 248, 248), r);

            Image img = (Image)Resources.Audalia85x29;
            cache.DrawImage(img, r);

            //cache.FillRectangle(Color.FromArgb(240, 240, 240), r);
            //Image img = (Image)Resources.AudaliaIconTranspR64;

            /*
            string text = Text;
            if (text == null || text.Length == 0 || this.TextBounds.IsEmpty) return;
            AppearanceObject appearance = new AppearanceObject(GetDefaultAppearance());
            appearance.Font = new Font("Arial", 12, FontStyle.Italic);
            appearance.TextOptions.Trimming = Trimming.EllipsisCharacter;
            Rectangle r = RectangleHelper.GetCenterBounds(TextBounds, new Size(TextBounds.Width, appearance.CalcDefaultTextSize(cache.Graphics).Height));
            DrawTextShadow(cache, appearance, r);
            cache.DrawString(text, appearance.Font, appearance.GetForeBrush(cache), r, appearance.GetStringFormat());                        
            */
            //base.DrawText(cache);
        }
        
    }
}
