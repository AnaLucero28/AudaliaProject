using Audalia.DataHUBClient;
using Audalia.DataHUBCommon;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WindowsApplication
{
    public partial class ReportsUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        public ReportsUserControl()
        {
            InitializeComponent();

            if (Data.Audit.UserPermissions <= UserPermissions.Read)
            {
                bbiDelete.Enabled = false;
            }

            BindingList<ReportRow> dataSource = new BindingList<ReportRow>();

            /*
            foreach (var reportBase in Data.Audit.GetReports())
            {
                dataSource.Add(new ReportRow()
                    {
                        DBID = reportBase.DBID,
                        ReportName = reportBase.ReportName,
                        CreatorUserName = reportBase.CreatorUserName,
                        CreationDate = reportBase.CreationDate,
                        Description = reportBase.Description,
                        Params = string.Join(", ", reportBase.Params.Select(p => p.Key + " = " + p.Value).ToArray())
                }
                );
            }
            */
           
            gridControl.DataSource = dataSource;
            gridView.Columns[0].Visible = false;
            gridView.Columns[1].Width = 200;
            gridView.Columns[2].Width = 100;
            gridView.Columns[3].Width = 100;
            gridView.Columns[3].SortIndex = 0;
            gridView.Columns[3].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            gridView.Columns[3].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            gridView.Columns[3].DisplayFormat.FormatString = CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern + " - " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;

            gridView.Columns[4].Width = 300;
            gridView.Columns[5].Width = 100;

            //bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
            Refresh();
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        public void Refresh()
        {
            BindingList<ReportRow> dataSource = new BindingList<ReportRow>();
            string userName = @"audalia\" + Environment.UserName.ToLower();
            foreach (var reportBase in Data.Audit.GetReports())
            {
                //if (reportBase.CreatorUserName.ToLowerInvariant() == Data.Audit.Me.WindowsUser.ToLowerInvariant())
                //if (reportBase.CreatorUserName.ToLowerInvariant() == userName.ToLowerInvariant())
                {
                    dataSource.Add(new ReportRow()
                    {
                        DBID = reportBase.DBID,
                        ReportName = reportBase.ReportName,
                        CreatorUserName = reportBase.CreatorUserName,
                        CreationDate = reportBase.CreationDate,
                        Description = reportBase.Description,
                        Params = string.Join(", ", reportBase.Params.Select(p => p.Key + " = " + p.Value).ToArray())
                    }
                    );
                }
            }

            gridControl.DataSource = dataSource;
            bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
        }

        public class ReportRow
        {
            public string DBID { get; set; }

            [Display(Name = "Nombre")]
            public string ReportName { get; set; }

            [Display(Name = "Usuario")]
            public string CreatorUserName { get; set; }

            [Display(Name = "Fecha de creación")]
            public DateTime CreationDate { get; set; }

            [Display(Name = "Descripción")]
            public string Description { get; set; }

            [Display(Name = "Parámetros")]
            public string Params { get; set; }            
        }
        /*
        public BindingList<Customer> GetDataSource()
        {
            BindingList<Customer> result = new BindingList<Customer>();

          
            result.Add(new Customer()
            {
                ID = 1,
                Name = "ACME",
                Address = "2525 E El Segundo Blvd",
                City = "El Segundo",
                State = "CA",
                ZipCode = "90245",
                Phone = "(310) 536-0611"
            });
            result.Add(new Customer()
            {
                ID = 2,
                Name = "Electronics Depot",
                Address = "2455 Paces Ferry Road NW",
                City = "Atlanta",
                State = "GA",
                ZipCode = "30339",
                Phone = "(800) 595-3232"
            });
          

            return result;
        }
        */
        /*
        public class Customer
        {
            [Key, Display(AutoGenerateField = false)]
            public int ID { get; set; }
            [Required]
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            [Display(Name = "Zip Code")]
            public string ZipCode { get; set; }
            public string Phone { get; set; }
        }
        */
        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {            
            var wizad = new ReportsWizardForm();
            //wizad.Parent = this;
            wizad.ShowDialog();
            Refresh();

            /*
            var filePath = Path.ChangeExtension(Path.GetTempFileName(), ".xlsx");            
            using (var fileStream = File.Create(filePath))
            {
                var reportStream = DataHUBClient.ServiceContract.Report();                
                reportStream.CopyTo(fileStream);
            }
                        
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            excel.Workbooks.Open(Filename: filePath, ReadOnly: true);
            File.Delete(filePath);
            */
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            BindingList<ReportRow> list = (BindingList<ReportRow>)gridView.DataSource;

            foreach (int rowHandle in gridView.GetSelectedRows())
            {
                var reportRow = list[gridView.GetDataSourceRowIndex(rowHandle)];
                var report = new ReportBase() { DBID = reportRow.DBID };
                var filePath = Path.ChangeExtension(Path.GetTempFileName(), ".xlsx");

                using (var fileStream = File.Create(filePath))
                {                                        
                    var reportStream = DataHUBClient.ServiceContract.DownloadReport(report);                    
                    reportStream.CopyTo(fileStream);
                }
                Excel.Application excel = new Excel.Application();
                excel.Visible = true;
                excel.Workbooks.Open(Filename: filePath, ReadOnly: true);
                File.Delete(filePath);
                

                break;
            }

            //var reportId = gridView.GetFocusedDataRow()["DBID"];
        }

        private void bbiDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MessageBox.Show("¿Desea eliminar el informe seleccionado?", "Eliminar informe", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                BindingList<ReportRow> list = (BindingList<ReportRow>)gridView.DataSource;

                int handle = -1;
                foreach (int rowHandle in gridView.GetSelectedRows())
                {
                    handle = rowHandle;

                    var reportRow = list[gridView.GetDataSourceRowIndex(rowHandle)];
                    var report = new ReportBase() { DBID = reportRow.DBID };

                    DataHUBClient.ServiceContract.DeleteReport(report);
             
                    break;
                }

                if (handle >= 0)
                {
                    gridView.DeleteRow(handle);
                }
            }
        }
    }
}
