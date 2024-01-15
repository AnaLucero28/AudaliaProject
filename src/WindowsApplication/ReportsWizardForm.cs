using Audalia.DataHUBClient;
using Audalia.DataHUBCommon;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WindowsApplication
{
    public partial class ReportsWizardForm : DevExpress.XtraEditors.XtraForm
    {
        BaseReportParams reportParams;

        public ReportsWizardForm()
        {
            InitializeComponent();


            //

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Template", typeof(int));
            dataTable.Columns.Add("Área", typeof(string));
            dataTable.Columns.Add("Plantilla", typeof(string));

            
            gridControl1.DataSource = dataTable;            
            gridControl1.DataMember = dataTable.TableName;
            gridView1.Columns[0].Visible = false;
            gridView1.Columns[1].Width = 25;            

            //Plantillas segun el área
            dataTable.Rows.Add((int)ReportTemplate.Projects, "Auditoría", "Proyectos"); 
            dataTable.Rows.Add((int)ReportTemplate.Hours, "Auditoría", "Horas presupuestadas/planificadas/imputadas");

            //

            //string userName = @"audalia\" + Environment.UserName.ToLower();
            //if (userName == @"audalia\jmvarona")
            if (Data.Audit.AppUserPermissions.ContainsKey(@"\REPORTS\2") && Data.Audit.AppUserPermissions[@"\REPORTS\2"] != "D")
            {
                dataTable.Rows.Add((int)ReportTemplate.FacturacionPorSocio, "Administración", "Facturación por socio");
                dataTable.Rows.Add((int)ReportTemplate.Imputaciones, "Administración", "Imputación");
                dataTable.Rows.Add((int)ReportTemplate.Realizacion, "Administración", "Realización");
                dataTable.Rows.Add((int)ReportTemplate.BalanceAnalitico, "Administración", "Balance analítico");
                dataTable.Rows.Add((int)ReportTemplate.BalanceAnaliticoBeta, "Administración", "Balance analítico (Beta)");
            }
            
            //

            /*
            reportParams = new HorasReportParams();
            reportParams.Ejercicio = DateTime.Now.Year;
            propertyGridControl1.SelectedObject = reportParams;
            */
        }

        private void wizardControl1_FinishClick(object sender, CancelEventArgs e)
        {
            try
            {
                var filePath = Path.ChangeExtension(Path.GetTempFileName(), ".xlsx");
                using (var fileStream = File.Create(filePath))
                {
                    var report = new ReportBase();
                    report.ReportTemplate = (ReportTemplate)gridView1.GetFocusedDataRow()["Template"];
                    report.ReportName = gridView1.GetFocusedDataRow()["Área"].ToString() + " - " + gridView1.GetFocusedDataRow()["Plantilla"].ToString();

                    switch (report.ReportTemplate)
                    {
                        case ReportTemplate.Projects:
                            report.Description = gridView1.GetFocusedDataRow()["Área"].ToString() + " - " + gridView1.GetFocusedDataRow()["Plantilla"].ToString() + " - Ejercicio: " + ((BasicReportParams)reportParams).TaxYear.ToString();
                            report.Params = new Dictionary<string, string>() { ["TaxYear"] = ((BasicReportParams)reportParams).TaxYear.ToString() };
                            break;

                        case ReportTemplate.Hours:
                            report.Description = gridView1.GetFocusedDataRow()["Área"].ToString() + " - " + gridView1.GetFocusedDataRow()["Plantilla"].ToString() + " - Ejercicio: " + ((BasicReportParams)reportParams).TaxYear.ToString();
                            report.Params = new Dictionary<string, string>() { ["TaxYear"] = ((BasicReportParams)reportParams).TaxYear.ToString() };
                            break;

                        case ReportTemplate.FacturacionPorSocio:
                        case ReportTemplate.Imputaciones:
                        case ReportTemplate.Realizacion:
                        case ReportTemplate.BalanceAnalitico:
                        case ReportTemplate.BalanceAnaliticoBeta:
                            report.Description = gridView1.GetFocusedDataRow()["Área"].ToString() + " - " + gridView1.GetFocusedDataRow()["Plantilla"].ToString() + " - Desde: " + ((BalanceAnaliticoReportParams)reportParams).From.ToString() + " - Hasta: " + ((BalanceAnaliticoReportParams)reportParams).To.ToString();
                            report.Params = new Dictionary<string, string>() { ["From"] = ((BalanceAnaliticoReportParams)reportParams).From.ToDatabaseString(), ["To"] = ((BalanceAnaliticoReportParams)reportParams).To.ToDatabaseString() };
                            break;
                    }
                    

                    report = DataHUBClient.ServiceContract.CreateReport(report); //((ReportTemplate)gridView1.GetFocusedDataRow()["Id"], reportParams.Ejercicio.ToString()); 
                    var reportStream = DataHUBClient.ServiceContract.DownloadReport(report);
                    reportStream.CopyTo(fileStream);
                }

                Excel.Application excel = new Excel.Application();
                excel.Visible = true;
                excel.Workbooks.Open(Filename: filePath, ReadOnly: true);
                File.Delete(filePath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var reportTemplate = (ReportTemplate)gridView1.GetFocusedDataRow()["Template"];

            switch (reportTemplate)
            {
                case ReportTemplate.Projects:
                    var projectsReportParams = new BasicReportParams();
                    projectsReportParams.TaxYear = DateTime.Now.Year;
                    reportParams = projectsReportParams;
                    break;

                case ReportTemplate.Hours:
                    break;

                case ReportTemplate.FacturacionPorSocio:
                case ReportTemplate.Imputaciones:
                case ReportTemplate.Realizacion:
                case ReportTemplate.BalanceAnalitico:
                case ReportTemplate.BalanceAnaliticoBeta:
                    var balanceAnaliticoReportParams = new BalanceAnaliticoReportParams();

                    if (DateTime.Now.Month > 8)
                    {
                        balanceAnaliticoReportParams.From = new DateTime(DateTime.Now.Year, 9, 1);
                        balanceAnaliticoReportParams.To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                    }
                    else
                    {
                        balanceAnaliticoReportParams.From = new DateTime(DateTime.Now.Year - 1, 9, 1);
                        balanceAnaliticoReportParams.To = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                    }

                    reportParams = balanceAnaliticoReportParams;
                    break;
            }

            propertyGridControl1.SelectedObject = reportParams;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            wizardControl1.SelectedPageIndex = wizardControl1.SelectedPageIndex + 1;
        }
    }

    public class BaseReportParams
    {
    }

    public class BasicReportParams: BaseReportParams
    {
        [Description("Año..."), Category("Parámetros"), DisplayName("Ejercicio")]
        public int TaxYear { get; set; }
    }

    public class BalanceAnaliticoReportParams: BaseReportParams
    {
        [Description("Dia..."), Category("Parámetros"), DisplayName("Desde")]
        public DateTime From { get; set; }

        [Description("Dia..."), Category("Parámetros"), DisplayName("Hasta")]
        public DateTime To { get; set; }
    }

}