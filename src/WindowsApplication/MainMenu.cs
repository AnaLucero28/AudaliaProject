using Audalia.DataHUBClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsApplication
{
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
           
            if (Data.Audit.UserPermissions <= UserPermissions.Read)
            {
                tileItem1.Enabled = Data.Audit.Me != null;
                tileItem2.Enabled = Data.Audit.Me != null;
                tileItem4.Enabled = Data.Audit.Me != null;
                tileItem6.Enabled = Data.Audit.Me != null;
            }

            tileItem3.Enabled = (Data.Audit.AppUserPermissions.ContainsKey(@"\REPORTS\2") && Data.Audit.AppUserPermissions[@"\REPORTS\2"] != "D");
        }

        private void tileItem1_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            //Gantt
            MainForm.ShowCurtain();
            MainForm.AddNewTab(new GanttUserControl(), "Gantt");
            MainForm.HideCurtain();
        }

        private void tileItem2_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            //Reports
            MainForm.ShowCurtain();
            MainForm.AddNewTab(new ReportsUserControl(), "Informes");
            MainForm.HideCurtain();
        }

        private void tileItem6_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            MainForm.ShowCurtain();
            MainForm.AddNewTab(new AllocationUserControl("251"), "Test");
            MainForm.HideCurtain();
        }

        private void tileItem4_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            MainForm.ShowCurtain();
            var control = new AvailabilityUserControl.AvailabilityUserControl();

            DateTime startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime finishDate = startDate.AddMonths(1).AddDays(-1);

            control.Init(startDate, finishDate, false);

            MainForm.AddNewTab(control, "Disponibilidad");            
            MainForm.HideCurtain();
        }

        private void tileItem3_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            //Reports
            MainForm.ShowCurtain();
            MainForm.AddNewTab(new ReportsUserControl(), "Informes");
            MainForm.HideCurtain();
        }
    }
}
