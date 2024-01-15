using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Audalia.DataHUBCommon;
using DevExpress.Spreadsheet;

namespace Audalia.DataHUBServer
{
    public class DataHUBReports
    {
        public static ReportBase GetReport(string reportId)
        {
            ReportBase result = null;


            return result;
        }

        public static ReportBase CreateReport(ReportBase report)
        {
            ReportBase result = null;

            try
            {
                Stream reportStream = null;

                switch (report.ReportTemplate)
                {
                    case ReportTemplate.Projects:                                                
                        reportStream = DataHUBReports.GetProyectosReport(report.Params["TaxYear"]);
                        break;

                    case ReportTemplate.Hours:                        
                        reportStream = DataHUBReports.GetHorasReport(report.Params["TaxYear"]);
                        break;

                    case ReportTemplate.BalanceAnalitico:
                        reportStream = DataHUBReports.GetBalanceAnaliticoReport(report.Params["From"], report.Params["To"]);
                        break;

                    case ReportTemplate.BalanceAnaliticoBeta:
                        reportStream = DataHUBReports.GetBalanceAnaliticoBetaReport(report.Params["From"], report.Params["To"]);
                        break;

                    case ReportTemplate.FacturacionPorSocio:
                        reportStream = DataHUBReports.GetFacturacionPorSocioReport(report.Params["From"], report.Params["To"]);
                        break;

                    case ReportTemplate.Imputaciones:
                        reportStream = DataHUBReports.GetImputacionReport(report.Params["From"], report.Params["To"]);
                        break;

                    case ReportTemplate.Realizacion:
                        reportStream = DataHUBReports.GetRealizacionReport(report.Params["From"], report.Params["To"]);
                        break;
                }

                var userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name.ToLower(); ;
                //System.Security.Principal.WindowsIdentity.GetCurrent().Name

                result = Audit.AuditData.PostReport(new ReportBase() { ReportTemplate = report.ReportTemplate, ReportName = report.ReportName, CreationDate = DateTime.Now, CreatorUserName = userName, Description = report.Description, Params = report.Params, FilePath = @"" });

                var filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\Reports\" + @"R" + int.Parse(result.DBID).ToString("D9");
                using (FileStream file = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write))
                    reportStream.CopyTo(file);
            }
            catch (Exception e)
            {
                DataHubLog.WriteLine("ReportBase CreateReport - Error: " + e.ToString());
            }

            return result;
        }

        public static Stream GetProyectos3Report(string taxYear)
        {            
            //throw new Exception("Prueba");

            string start = taxYear.ToString() + "0101000000";
            string finish = taxYear.ToString() + "1231235959";

            string commandText =

                @" EXEC sp_addlinkedsrvlogin @rmtsrvname = 'SVRBBDDGEX', @useself = 'false', @rmtuser='datahub', @rmtpassword = '2021GEX' " + Environment.NewLine +

                " SELECT " +
                
                "   AUDIT_PROJECT.projectid, " +
                "   AUDIT_PROJECT.name project_name, " +
                "   AUDIT_PROJECT.branchoffice, " +

                "   AUDIT_PROJECT.start_date, " +
                "   AUDIT_PROJECT.finish_date, " +
                "   AUDIT_PROJECT.baseline_start_date, " +
                "   AUDIT_PROJECT.baseline_finish_date " +

                " 	, " +
                " 	(SELECT STRING_AGG(CodProy154, ', ') WITHIN GROUP (ORDER BY CodProy154) " +
                " 	FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 " +
                " 	INNER JOIN AUDALIA.AUDIT.PROJECT_GEXTOR PROJECT_GEXTOR ON (GES_GFClienteProyectos154.CodProy154 COLLATE Modern_Spanish_CS_AS = PROJECT_GEXTOR.projectidgextor COLLATE Modern_Spanish_CS_AS) " +
                //" 	INNER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_SOCIO ON (AUDIT_SOCIO.employeeidgextor = GES_GFClienteProyectos154.UsuarioSocio154) " +
                " 	WHERE PROJECT_GEXTOR.projectid = AUDIT_PROJECT.projectid " +
                " 	) AS GextorIds " +
                " 	, " +
                " 	(SELECT STRING_AGG(NomProy154, ', ')  WITHIN GROUP (ORDER BY CodProy154) " +
                " 	FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 " +
                " 	INNER JOIN AUDALIA.AUDIT.PROJECT_GEXTOR PROJECT_GEXTOR ON (GES_GFClienteProyectos154.CodProy154 COLLATE Modern_Spanish_CS_AS = PROJECT_GEXTOR.projectidgextor COLLATE Modern_Spanish_CS_AS) " +
                //" 	INNER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_SOCIO ON (AUDIT_SOCIO.employeeidgextor = GES_GFClienteProyectos154.UsuarioSocio154) " +
                " 	WHERE PROJECT_GEXTOR.projectid = AUDIT_PROJECT.projectid " +
                " 	) AS GextorNames " +
                " 	, " +
                " 	(SELECT STRING_AGG(name, ', ') WITHIN GROUP (ORDER BY name) FROM " +
                " 		(SELECT DISTINCT AUDIT_SOCIO.name " +
                " 		FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 " +
                " 		INNER JOIN AUDALIA.AUDIT.PROJECT_GEXTOR PROJECT_GEXTOR ON (GES_GFClienteProyectos154.CodProy154 COLLATE Modern_Spanish_CS_AS = PROJECT_GEXTOR.projectidgextor COLLATE Modern_Spanish_CS_AS) " +
                " 		INNER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_SOCIO ON (AUDIT_SOCIO.employeeidgextor = GES_GFClienteProyectos154.UsuarioSocio154) " +
                " 		WHERE PROJECT_GEXTOR.projectid = AUDIT_PROJECT.projectid " +
                " 		) AS DistinctSocios " +
                " 	) AS Socio " +

                "   FROM AUDALIA.AUDIT.PROJECT AS AUDIT_PROJECT " +
                
                "   WHERE AUDIT_PROJECT.projectid > 0 " +
                "   AND (" +
                "       AUDIT_PROJECT.start_date BETWEEN " + start + " AND " + finish + " OR " +
                "       AUDIT_PROJECT.finish_date BETWEEN " + start + " AND " + finish + " OR " +
                "       AUDIT_PROJECT.baseline_start_date BETWEEN " + start + " AND " + finish + " OR " +
                "       AUDIT_PROJECT.baseline_finish_date BETWEEN " + start + " AND " + finish +
                "   ) " +

                " ORDER BY AUDIT_PROJECT.name ";

            SqlCommand sqlCommand = new SqlCommand(commandText, Database.Connection);
            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Proyecto", typeof(string));
            dataTable.Columns.Add("Delegación", typeof(string));

            dataTable.Columns.Add("Inicio", typeof(DateTime));
            dataTable.Columns.Add("Fin", typeof(DateTime));
            dataTable.Columns.Add("Inicio_Ref", typeof(DateTime));
            dataTable.Columns.Add("Fin_Ref", typeof(DateTime));

            dataTable.Columns.Add("Socios", typeof(string));
            dataTable.Columns.Add("Ids Gextor", typeof(string));
            dataTable.Columns.Add("Proyectos Gextor", typeof(string));
            

            int rowcount = 1;
            while (dataReader.Read())
            {
                rowcount++;

                dataTable.Rows.Add(
                    dataReader["project_name"].ToString(),
                    dataReader["branchoffice"].ToString(),

                    dataReader["start_date"].ToString().ToDateTime(),
                    dataReader["finish_date"].ToString().ToDateTime(),
                    dataReader["baseline_start_date"].ToString().ToDateTime(),
                    dataReader["baseline_finish_date"].ToString().ToDateTime(),

                    dataReader["Socio"].ToString().Replace(", ", Environment.NewLine),
                    dataReader["GextorIds"].ToString().Replace(", ", Environment.NewLine),
                    dataReader["GextorNames"].ToString().Replace(", ", Environment.NewLine)
                    );
            }

            Workbook workbook = new Workbook();
            workbook.Worksheets[0].Import(dataTable, true, 0, 0);

            Worksheet sourceWorksheet = workbook.Worksheets[0];
            sourceWorksheet.Name = "Proyectos";

            CellRange range = sourceWorksheet["A1:I" + rowcount.ToString()];            
            Table table = sourceWorksheet.Tables.Add(range, true);
            table.Style = workbook.TableStyles["TableStyleMedium6"];

            Formatting rangeFormatting = range.BeginUpdateFormatting();
            rangeFormatting.Alignment.WrapText = true;
            rangeFormatting.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
            range.AutoFitColumns();
            range.AutoFitRows();
            range.EndUpdateFormatting(rangeFormatting);

            Stream result = new MemoryStream();
            workbook.SaveDocument(result, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            result.Seek(0, SeekOrigin.Begin);
            return result;

        }

        public static Stream GetProyectos2Report(string taxYear)
        {

            string start = taxYear.ToString() + "0101000000";
            string finish = taxYear.ToString() + "1231235959";

            string commandText =                
                " SELECT " +            
                "   AUDIT_PROJECT.projectidgextor, " +
                "   AUDIT_PROJECT.projectid, " +
                "   AUDIT_PROJECT.name project_name, " +
                "   AUDIT_PROJECT.start_date, " +
                "   AUDIT_PROJECT.finish_date, " +
                "   AUDIT_PROJECT.baseline_start_date, " +
                "   AUDIT_PROJECT.baseline_finish_date " +
                
                "   FROM AUDALIA.AUDIT.PROJECT AS AUDIT_PROJECT " +                
                "   WHERE AUDIT_PROJECT.projectid > 0 " +
                "   AND (" +
                "       AUDIT_PROJECT.start_date BETWEEN " + start + " AND " + finish + " OR " +
                "       AUDIT_PROJECT.finish_date BETWEEN " + start + " AND " + finish + " OR " +
                "       AUDIT_PROJECT.baseline_start_date BETWEEN " + start + " AND " + finish + " OR " +
                "       AUDIT_PROJECT.baseline_finish_date BETWEEN " + start + " AND " + finish +
                "   ) " +

                " ORDER BY AUDIT_PROJECT.name ";

            SqlCommand sqlCommand = new SqlCommand(commandText, Database.Connection);
            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();
            
            dataTable.Columns.Add("Proyecto", typeof(string));
            dataTable.Columns.Add("Socio", typeof(string));

            dataTable.Columns.Add("Inicio", typeof(DateTime));
            dataTable.Columns.Add("Fin", typeof(DateTime));
            dataTable.Columns.Add("Inicio_Ref", typeof(DateTime));
            dataTable.Columns.Add("Fin_Ref", typeof(DateTime));

            dataTable.Columns.Add("Gextor", typeof(string));

            int rowcount = 1;
            while (dataReader.Read())
            {
                string gextorCodProys = "";
                string gextorNomProys = "";
                string socioName = "";

                if (dataReader["projectidgextor"].ToString() != "")
                {
                    var codProys = dataReader["projectidgextor"].ToString().Split(',');
                    foreach (var codProy in codProys)
                    {
                        gextorCodProys += (gextorCodProys == "" ? "" : ",") + Int64.Parse(codProy).ToString("D10");
                    }

                    string gextorCommandText = @" EXEC sp_addlinkedsrvlogin @rmtsrvname = 'SVRBBDDGEX', @useself = 'false', @rmtuser='datahub', @rmtpassword = '2021GEX' " + Environment.NewLine +
                        " SELECT " +
                        "   GES_GFClienteProyectos154.NomProy154, " +
                        "   AUDIT_SOCIO.name socio_name  " +
                        "   FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 GES_GFClienteProyectos154 " +
                        "   LEFT OUTER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_SOCIO ON (AUDIT_SOCIO.employeeidgextor = GES_GFClienteProyectos154.UsuarioSocio154) " +
                        "   WHERE GES_GFClienteProyectos154.CodProy154 IN (" + gextorCodProys + ") ";

                    SqlCommand gextorSqlCommand = new SqlCommand(gextorCommandText, Database.Connection);
                    SqlDataReader gextorDataReader = gextorSqlCommand.ExecuteReader();

                    while (gextorDataReader.Read())
                    {
                        gextorNomProys += (gextorNomProys == "" ? "" : ", ") + gextorDataReader["NomProy154"].ToString();
                        socioName += (socioName == "" ? "" : ", ") + gextorDataReader["socio_name"].ToString();
                    }
                }

                rowcount++;

                dataTable.Rows.Add(                    
                    dataReader["project_name"].ToString(),
                    socioName,
                    dataReader["start_date"].ToString().ToDateTime(),
                    dataReader["finish_date"].ToString().ToDateTime(),
                    dataReader["baseline_start_date"].ToString().ToDateTime(),
                    dataReader["baseline_finish_date"].ToString().ToDateTime(),
                    gextorNomProys);
            }

            Workbook workbook = new Workbook();
            workbook.Worksheets[0].Import(dataTable, true, 0, 0);

            Worksheet sourceWorksheet = workbook.Worksheets[0];
            sourceWorksheet.Name = "Proyectos";

            CellRange range = sourceWorksheet["A1:G" + rowcount.ToString()];
            Table table = sourceWorksheet.Tables.Add(range, true);
            table.Style = workbook.TableStyles["TableStyleMedium6"];

            Stream result = new MemoryStream();
            workbook.SaveDocument(result, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            result.Seek(0, SeekOrigin.Begin);
            return result;

        }

        public static Stream GetProyectosReport(string taxYear)
        {
            return GetProyectos3Report(taxYear);

            string start = taxYear.ToString() + "0101000000";
            string finish = taxYear.ToString() + "1231235959";

            string commandText =

                @" EXEC sp_addlinkedsrvlogin @rmtsrvname = 'SVRBBDDGEX', @useself = 'false', @rmtuser='datahub', @rmtpassword = '2021GEX' " + Environment.NewLine +

                " SELECT " +
                "   GES_GFClienteProyectos154.CodProy154 CodProy, " +

                "   AUDIT_PROJECT.projectid, " +
                "   AUDIT_PROJECT.name project_name, " +

                "   AUDIT_PROJECT.start_date, " +
                "   AUDIT_PROJECT.finish_date, " +
                "   AUDIT_PROJECT.baseline_start_date, " +
                "   AUDIT_PROJECT.baseline_finish_date, " +

                "   AUDIT_SOCIO.name socio_name  " +

                "   FROM AUDALIA.AUDIT.PROJECT AS AUDIT_PROJECT " +
                "   LEFT OUTER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 GES_GFClienteProyectos154 ON (GES_GFClienteProyectos154.CodProy154 COLLATE Modern_Spanish_CS_AS = AUDIT_PROJECT.projectidgextor COLLATE Modern_Spanish_CS_AS) " +
                "   LEFT OUTER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_SOCIO ON (AUDIT_SOCIO.employeeidgextor = GES_GFClienteProyectos154.UsuarioSocio154) " +
                "   WHERE AUDIT_PROJECT.projectid > 0 " +
                "   AND (" +
                "       AUDIT_PROJECT.start_date BETWEEN " + start + " AND " + finish + " OR " +
                "       AUDIT_PROJECT.finish_date BETWEEN " + start + " AND " + finish + " OR " +
                "       AUDIT_PROJECT.baseline_start_date BETWEEN " + start + " AND " + finish + " OR " +
                "       AUDIT_PROJECT.baseline_finish_date BETWEEN " + start + " AND " + finish +
                "   ) " +

                " ORDER BY AUDIT_PROJECT.name ";

            SqlCommand sqlCommand = new SqlCommand(commandText, Database.Connection);
            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id_GEXTOR", typeof(string));
            dataTable.Columns.Add("Proyecto", typeof(string));
            dataTable.Columns.Add("Socio", typeof(string));

            dataTable.Columns.Add("Inicio", typeof(DateTime));
            dataTable.Columns.Add("Fin", typeof(DateTime));
            dataTable.Columns.Add("Inicio_Ref", typeof(DateTime));
            dataTable.Columns.Add("Fin_Ref", typeof(DateTime));

            int rowcount = 1;
            while (dataReader.Read())
            {
                rowcount++;

                dataTable.Rows.Add(
                    dataReader["CodProy"].ToString(),
                    dataReader["project_name"].ToString(),
                    dataReader["socio_name"].ToString(),
                    dataReader["start_date"].ToString().ToDateTime(),
                    dataReader["finish_date"].ToString().ToDateTime(),
                    dataReader["baseline_start_date"].ToString().ToDateTime(),
                    dataReader["baseline_finish_date"].ToString().ToDateTime());
            }

            Workbook workbook = new Workbook();
            workbook.Worksheets[0].Import(dataTable, true, 0, 0);

            Worksheet sourceWorksheet = workbook.Worksheets[0];
            sourceWorksheet.Name = "Proyectos";

            CellRange range = sourceWorksheet["A1:G" + rowcount.ToString()];
            Table table = sourceWorksheet.Tables.Add(range, true);
            table.Style = workbook.TableStyles["TableStyleMedium6"];

            Stream result = new MemoryStream();
            workbook.SaveDocument(result, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            result.Seek(0, SeekOrigin.Begin);
            return result;

        }

        public static Stream GetHoras2Report(string taxYear)
        {
            string start = taxYear.ToString() + "0101000000";
            string finish = taxYear.ToString() + "1231235959";

            string projectsCommandText =
                " SELECT * FROM " +
                " ( " +
                " 	SELECT " +
                " 	AUDIT_PROJECT.projectid, "  +
                "   AUDIT_PROJECT.name ProjectName, " +
                "   AUDIT_PROJECT.branchoffice, " +


                " 	(SELECT STRING_AGG(CodProy154, ', ') WITHIN GROUP (ORDER BY CodProy154) " +
                " 	FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 " +
                " 	INNER JOIN AUDALIA.AUDIT.PROJECT_GEXTOR PROJECT_GEXTOR ON (GES_GFClienteProyectos154.CodProy154 COLLATE Modern_Spanish_CS_AS = PROJECT_GEXTOR.projectidgextor COLLATE Modern_Spanish_CS_AS) " +                
                " 	WHERE PROJECT_GEXTOR.projectid = AUDIT_PROJECT.projectid " +
                " 	) AS GextorIds, " +

                " 	(SELECT STRING_AGG(NomProy154, ', ')  WITHIN GROUP (ORDER BY CodProy154) " +
                " 	FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 " +
                " 	INNER JOIN AUDALIA.AUDIT.PROJECT_GEXTOR PROJECT_GEXTOR ON (GES_GFClienteProyectos154.CodProy154 COLLATE Modern_Spanish_CS_AS = PROJECT_GEXTOR.projectidgextor COLLATE Modern_Spanish_CS_AS) " +                
                " 	WHERE PROJECT_GEXTOR.projectid = AUDIT_PROJECT.projectid " +
                " 	) AS GextorNames, " +

                " 	(SELECT STRING_AGG(name, ', ') WITHIN GROUP (ORDER BY name) FROM " +
                " 		(SELECT DISTINCT AUDIT_SOCIO.name " +
                " 		FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 " +
                " 		INNER JOIN AUDALIA.AUDIT.PROJECT_GEXTOR PROJECT_GEXTOR ON (GES_GFClienteProyectos154.CodProy154 COLLATE Modern_Spanish_CS_AS = PROJECT_GEXTOR.projectidgextor COLLATE Modern_Spanish_CS_AS) " +
                " 		INNER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_SOCIO ON (AUDIT_SOCIO.employeeidgextor = GES_GFClienteProyectos154.UsuarioSocio154) " +
                " 		WHERE PROJECT_GEXTOR.projectid = AUDIT_PROJECT.projectid " +
                " 		) AS DistinctSocios " +
                " 	) AS Socio " +

                " 	FROM AUDALIA.AUDIT.PROJECT AS AUDIT_PROJECT " +
                " 	WHERE AUDIT_PROJECT.projectid > 0 " +
                " ) AS PROJECTS ";


            string commandText =
                //" EXEC sp_addlinkedsrvlogin @rmtsrvname = 'SVRBBDDGEX', @useself = 'false', @rmtuser='datahub', @rmtpassword = '2021GEX' ";
                " SELECT * FROM ( ";

            //Presupuestado 
            commandText += projectsCommandText + " INNER JOIN " +
                " ( " +
                " 	SELECT " +
                " 	'Presupuestado' Tabla, " +
                " 	AUDIT_PROJECT.projectid BUDGET_projectid, " +
                " 	AUDIT_BUDGET.category EmployeeCategory, " +
                " 	'' Auditor, " +
                " 	Null Fecha, " +
                " 	AUDIT_BUDGET.quantity Horas " +

                " 	FROM AUDALIA.AUDIT.PROJECT AS AUDIT_PROJECT " +
                " 	INNER JOIN AUDALIA.AUDIT.BUDGET AS AUDIT_BUDGET ON (AUDIT_BUDGET.projectid = AUDIT_PROJECT.projectid) " +
                " ) AS BUDGET ON (BUDGET.BUDGET_projectid = PROJECTS.projectid) ";

            //Planificado 
            commandText += " UNION ALL " + projectsCommandText + " INNER JOIN " + 
                "( " +
                "	SELECT " +
                "	'Planificado' Tabla, " +
                "	AUDIT_PROJECT.projectid PLANNING_projectid, " +
                "	AUDIT_EMPLOYEE.category EmployeeCategory, " +
                "	AUDIT_EMPLOYEE.name Auditor, " +
                "	CONVERT(DATETIME, SUBSTRING(AUDIT_ALLOC.alloc_date, 1, 8), 120) Fecha, " +
                "	AUDIT_ALLOC.quantity Horas " +

                "	FROM AUDALIA.AUDIT.PROJECT AS AUDIT_PROJECT " +
                "	INNER JOIN AUDALIA.AUDIT.PROJECT_EMPLOYEE_ALLOC AS AUDIT_ALLOC ON (AUDIT_ALLOC.projectid = AUDIT_PROJECT.projectid) " +
                "	INNER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_EMPLOYEE ON (AUDIT_EMPLOYEE.employeeid = AUDIT_ALLOC.employeeid) " +
                " ) AS PLANNING ON (PLANNING.PLANNING_projectid = PROJECTS.projectid) ";

            //Imputado 
            commandText += " UNION ALL " +

                " 	SELECT " +
                " 	AUDIT_PROJECT.projectid, " +
                "   AUDIT_PROJECT.name ProjectName, " +
                "   AUDIT_PROJECT.branchoffice, " +

                "   GES_GFClienteProyectos154.CodProy154 AS GextorIds, " +
                "   GES_GFClienteProyectos154.NomProy154 AS GextorNames, " +
                "   AUDIT_SOCIO.name AS Socio, " +

                " 	'Imputado' Tabla, " +
                " 	AUDIT_PROJECT.projectid ALLOCATION_projectid,	 " +
                " 	AUDIT_EMPLOYEE.category EmployeeCategory, " +
                "   COALESCE(AUDIT_EMPLOYEE.name, '_Otros') Auditor, " +
                "   GES_GFProyectosIncurridos451.Fecha451 Fecha, " +
                "   GES_GFProyectosIncurridos451.Horas451 Horas " +

                " 	FROM AUDALIA.AUDIT.PROJECT AS AUDIT_PROJECT " +
                " 	INNER JOIN AUDALIA.AUDIT.PROJECT_GEXTOR PROJECT_GEXTOR ON (PROJECT_GEXTOR.projectid = AUDIT_PROJECT.projectid) " +
                " 	INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 GES_GFClienteProyectos154 ON (GES_GFClienteProyectos154.CodProy154 COLLATE Modern_Spanish_CS_AS = PROJECT_GEXTOR.projectidgextor COLLATE Modern_Spanish_CS_AS) " +
                " 	INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFProyectosIncurridos451 GES_GFProyectosIncurridos451 ON (GES_GFClienteProyectos154.CodProy154 = GES_GFProyectosIncurridos451.CodProy451) " +
                " 	INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFPersonal443 GES_GFPersonal443 ON (GES_GFPersonal443.Usuario443 = GES_GFProyectosIncurridos451.Usuario451) " +
                " 	LEFT OUTER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_SOCIO ON (AUDIT_SOCIO.employeeidgextor = GES_GFClienteProyectos154.UsuarioSocio154) " +
                " 	LEFT OUTER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_EMPLOYEE ON (AUDIT_EMPLOYEE.employeeidgextor = GES_GFPersonal443.Usuario443) ";                

            commandText += ") AS DATOS WHERE fecha IS NULL OR fecha BETWEEN @startDate AND @finishDate ";
            commandText += " ORDER BY ProjectName, fecha, auditor ";

            SqlCommand sqlCommand = new SqlCommand(commandText, Database.Connection);

            sqlCommand.Parameters.Add("@startDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@startDate"].Value = new DateTime(int.Parse(taxYear), 1, 1);

            sqlCommand.Parameters.Add("@finishDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@finishDate"].Value = new DateTime(int.Parse(taxYear), 12, 31);

            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Tabla", typeof(string));
            dataTable.Columns.Add("Proyecto", typeof(string));
            dataTable.Columns.Add("Delegación", typeof(string));
            dataTable.Columns.Add("Socio", typeof(string));
            dataTable.Columns.Add("Categoria", typeof(string));
            dataTable.Columns.Add("Auditor", typeof(string));
            dataTable.Columns.Add("Fecha", typeof(DateTime));
            dataTable.Columns.Add("Horas", typeof(double));

            dataTable.Columns.Add("Ids Gextor", typeof(string));
            dataTable.Columns.Add("Proyectos Gextor", typeof(string));

            int rowcount = 1;
            while (dataReader.Read())
            {
                rowcount++;

                DateTime? fecha = null;
                //if (dataReader["Fecha"] != DBNull.Value)
                if (dataReader["Fecha"].ToString() != "")
                    fecha = (DateTime)dataReader["Fecha"];
                else
                    fecha = new DateTime(int.Parse(taxYear), 9, 1);


                double? horas = null;
                //if (dataReader["Horas"] != DBNull.Value)
                if (dataReader["Horas"].ToString() != "")
                    horas = Double.Parse(dataReader["Horas"].ToString());

                dataTable.Rows.Add(
                    dataReader["Tabla"].ToString(),
                    dataReader["ProjectName"].ToString(),
                    dataReader["branchoffice"].ToString(),
                    dataReader["Socio"].ToString(),
                    dataReader["EmployeeCategory"].ToString(),
                    dataReader["Auditor"].ToString(),
                    fecha,
                    horas,
                    dataReader["GextorIds"].ToString(),
                    dataReader["GextorNames"].ToString());

            }
            Workbook workbook = new Workbook();
            workbook.Worksheets[0].Import(dataTable, true, 0, 0);


            Worksheet sourceWorksheet = workbook.Worksheets[0];
            sourceWorksheet.Name = "Datos";
            Worksheet worksheet = workbook.Worksheets.Add("Informe");
            workbook.Worksheets.ActiveWorksheet = worksheet;

            PivotTable pivotTable = worksheet.PivotTables.Add(sourceWorksheet["A1:J" + rowcount.ToString()], worksheet["A1"]);

            pivotTable.RowFields.Add(pivotTable.Fields["Proyecto"]);

            pivotTable.Fields["Categoria"].SortType = PivotFieldSortType.Manual;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Socio").Position = 0;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Director").Position = 1;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Gerente").Position = 2;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Senior").Position = 3;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Asistente").Position = 4;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Becario").Position = 5;
            pivotTable.RowFields.Add(pivotTable.Fields["Categoria"]);

            pivotTable.Fields["Tabla"].SortType = PivotFieldSortType.Manual;
            pivotTable.Fields["Tabla"].Items.FirstOrDefault(p => p.Value == "Presupuestado").Position = 0;
            pivotTable.Fields["Tabla"].Items.FirstOrDefault(p => p.Value == "Planificado").Position = 1;

            if (pivotTable.Fields["Tabla"].Items.FirstOrDefault(p => p.Value == "Imputado") != null)
                pivotTable.Fields["Tabla"].Items.FirstOrDefault(p => p.Value == "Imputado").Position = 2;

            pivotTable.Fields["Tabla"].SubtotalCaption = "Total";
            pivotTable.ColumnFields.Add(pivotTable.Fields["Tabla"]);

            pivotTable.Fields["Fecha"].GroupItems(PivotFieldGroupByType.Months);
            pivotTable.ColumnFields.Add(pivotTable.Fields["Fecha"]);

            var horasField = pivotTable.DataFields.Add(pivotTable.Fields["Horas"]);
            horasField.NumberFormat = @"#,##0.00";
            

            pivotTable.View.GrandTotalCaption = "Total";
            pivotTable.Layout.ShowRowGrandTotals = false;
            pivotTable.Style = workbook.TableStyles["PivotStyleMedium13"]; //.DefaultPivotStyle;

            Stream result = new MemoryStream();
            workbook.SaveDocument(result, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            result.Seek(0, SeekOrigin.Begin);
            return result;            
        }

        public static Stream GetHorasReport(string taxYear)
        {
            return GetHoras2Report(taxYear);

            string commandText =

                @" EXEC sp_addlinkedsrvlogin @rmtsrvname = 'SVRBBDDGEX', @useself = 'false', @rmtuser='datahub', @rmtpassword = '2021GEX' " + Environment.NewLine +

                " SELECT Tabla, ProjectId, ProjectName, Socio, EmployeeCategory, Auditor, Fecha, Horas " +
                " FROM (" +

                " SELECT 'Presupuestado' Tabla, " +
                "   Projects.ProjectId, " +
                "   Projects.ProjectName, " +
                "   Projects.Socio, " +
                "   Projects.EmployeeCategory, " +
                "   '' Auditor, " +
                "   CONVERT(DATETIME, null) Fecha, " +
                "   Budget.Horas " +
                " FROM " +
                " ( " +
                " SELECT " +
                "   AUDIT_PROJECT.projectid ProjectId, AUDIT_PROJECT.name ProjectName, " +
                "   AUDIT_SOCIO.name Socio,  " +
                "   AUDIT_CATEGORY.Name EmployeeCategory " +

                "   FROM AUDALIA.AUDIT.PROJECT AS AUDIT_PROJECT " +
                "   INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 GES_GFClienteProyectos154 ON (GES_GFClienteProyectos154.CodProy154 COLLATE Modern_Spanish_CS_AS = AUDIT_PROJECT.projectidgextor COLLATE Modern_Spanish_CS_AS) " +
                "   INNER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_SOCIO ON (AUDIT_SOCIO.employeeidgextor = GES_GFClienteProyectos154.UsuarioSocio154) " +
                "   CROSS JOIN AUDALIA.AUDIT.CATEGORY AS AUDIT_CATEGORY " +
                "   WHERE AUDIT_PROJECT.projectid > 0 " +
                " 	) AS Projects " +

                " LEFT JOIN " +

                " ( " +
                " SELECT  " +
                "   AUDIT_PROJECT.projectid ProjectId, " +
                "   AUDIT_BUDGET.category EmployeeCategory, " +
                "   AUDIT_BUDGET.quantity Horas " +

                " 	FROM AUDALIA.AUDIT.PROJECT AS AUDIT_PROJECT  " +
                " 	INNER JOIN AUDALIA.AUDIT.BUDGET AS AUDIT_BUDGET ON (AUDIT_BUDGET.projectid = AUDIT_PROJECT.projectid) " +
                " ) AS Budget ON (Budget.ProjectId = Projects.ProjectId AND Budget.EmployeeCategory = Projects.EmployeeCategory) " +


                " UNION ALL " +

                " ( " +
                " SELECT " +
                "   'Planificado' Tabla, " +
                "   AUDIT_PROJECT.projectid ProjectId, " +
                "   MAX(AUDIT_PROJECT.name) ProjectName, " +
                "   AUDIT_SOCIO.name Socio,  " +
                "   AUDIT_EMPLOYEE.category EmployeeCategory, " +
                "   AUDIT_EMPLOYEE.name Auditor, " +
                "   CONVERT(DATETIME, SUBSTRING(AUDIT_ALLOC.alloc_date, 1, 8), 120) Fecha, " +
                "   SUM(AUDIT_ALLOC.quantity) Horas " +

                " 	FROM AUDALIA.AUDIT.PROJECT AS AUDIT_PROJECT  " +
                " 	INNER JOIN AUDALIA.AUDIT.PROJECT_EMPLOYEE_ALLOC AS AUDIT_ALLOC ON (AUDIT_ALLOC.projectid = AUDIT_PROJECT.projectid) " +
                " 	INNER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_EMPLOYEE ON (AUDIT_EMPLOYEE.employeeid = AUDIT_ALLOC.employeeid) " +
                " 	INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 GES_GFClienteProyectos154 ON (GES_GFClienteProyectos154.CodProy154 COLLATE Modern_Spanish_CS_AS = AUDIT_PROJECT.projectidgextor COLLATE Modern_Spanish_CS_AS) " +
                "   INNER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_SOCIO ON (AUDIT_SOCIO.employeeidgextor = GES_GFClienteProyectos154.UsuarioSocio154) " +
                "   WHERE AUDIT_PROJECT.projectid > 0 " +
                " 	GROUP BY AUDIT_PROJECT.projectid, AUDIT_SOCIO.name, AUDIT_EMPLOYEE.category, AUDIT_EMPLOYEE.name, AUDIT_ALLOC.alloc_date " +
                " )  " +

                " UNION ALL " +

                " ( " +
                " SELECT " +
                "   'Imputado' Tabla, " +
                "   AUDIT_PROJECT.projectid ProjectId, " +
                "   MAX(AUDIT_PROJECT.name) ProjectName, " +
                "   AUDIT_SOCIO.name Socio,  " +
                "   AUDIT_EMPLOYEE.category EmployeeCategory, " +
                "   AUDIT_EMPLOYEE.name Auditor, " +
                "   GES_GFProyectosIncurridos451.Fecha451 Fecha, " +
                "   SUM(GES_GFProyectosIncurridos451.Horas451) Horas " +

                " 	FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GFProyectosIncurridos451 GES_GFProyectosIncurridos451 " +
                " 	INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFPersonal443 GES_GFPersonal443 ON (GES_GFPersonal443.Usuario443 = GES_GFProyectosIncurridos451.Usuario451) " +
                " 	INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFCliente18 GES_GFCliente18 ON (GES_GFCliente18.Cliente18 = GES_GFProyectosIncurridos451.Cliente451) " +
                " 	INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 GES_GFClienteProyectos154 ON (GES_GFClienteProyectos154.CodProy154 = GES_GFProyectosIncurridos451.CodProy451) " +

                " 	INNER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_EMPLOYEE ON (AUDIT_EMPLOYEE.employeeidgextor = GES_GFPersonal443.Usuario443) " +
                " 	INNER JOIN AUDALIA.AUDIT.PROJECT AS AUDIT_PROJECT ON (AUDIT_PROJECT.projectidgextor COLLATE Modern_Spanish_CS_AS = GES_GFClienteProyectos154.CodProy154 COLLATE Modern_Spanish_CS_AS) " +
                "   INNER JOIN AUDALIA.AUDIT.EMPLOYEE AS AUDIT_SOCIO ON (AUDIT_SOCIO.employeeidgextor = GES_GFClienteProyectos154.UsuarioSocio154) " +

                " 	WHERE Fecha451 > '2021-01-01 00:00:00.000' " +
                " 	GROUP BY AUDIT_PROJECT.projectid, AUDIT_SOCIO.name, AUDIT_EMPLOYEE.category, AUDIT_EMPLOYEE.name, GES_GFProyectosIncurridos451.Fecha451  " +
                " ) " +

                " ) AS DATA ORDER BY 1,2,3 ";

            SqlCommand sqlCommand = new SqlCommand(commandText, Database.Connection);
            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Tabla", typeof(string));
            dataTable.Columns.Add("Proyecto", typeof(string));
            dataTable.Columns.Add("Socio", typeof(string));
            dataTable.Columns.Add("Categoria", typeof(string));
            dataTable.Columns.Add("Auditor", typeof(string));
            dataTable.Columns.Add("Fecha", typeof(DateTime));
            dataTable.Columns.Add("Horas", typeof(double));
            int rowcount = 1;
            while (dataReader.Read())
            {
                rowcount++;

                DateTime? fecha = null;
                if (dataReader["Fecha"] != DBNull.Value)
                    fecha = (DateTime)dataReader["Fecha"];

                double? horas = null;
                if (dataReader["Horas"] != DBNull.Value)
                    horas = (double)dataReader["Horas"];

                dataTable.Rows.Add(
                    dataReader["Tabla"].ToString(),
                    dataReader["ProjectName"].ToString(),
                    dataReader["Socio"].ToString(),
                    dataReader["EmployeeCategory"].ToString(),
                    dataReader["Auditor"].ToString(),
                    fecha,
                    horas);

            }
            Workbook workbook = new Workbook();
            workbook.Worksheets[0].Import(dataTable, true, 0, 0);


            Worksheet sourceWorksheet = workbook.Worksheets[0];
            sourceWorksheet.Name = "Datos";
            Worksheet worksheet = workbook.Worksheets.Add("Informe");
            workbook.Worksheets.ActiveWorksheet = worksheet;

            PivotTable pivotTable = worksheet.PivotTables.Add(sourceWorksheet["A1:G" + rowcount.ToString()], worksheet["A1"]);

            pivotTable.RowFields.Add(pivotTable.Fields["Proyecto"]);

            pivotTable.Fields["Categoria"].SortType = PivotFieldSortType.Manual;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Socio").Position = 0;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Director").Position = 1;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Gerente").Position = 2;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Senior").Position = 3;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Asistente").Position = 4;
            pivotTable.Fields["Categoria"].Items.FirstOrDefault(p => p.Value == "Becario").Position = 5;
            pivotTable.RowFields.Add(pivotTable.Fields["Categoria"]);

            pivotTable.Fields["Tabla"].SortType = PivotFieldSortType.Manual;
            pivotTable.Fields["Tabla"].Items.FirstOrDefault(p => p.Value == "Presupuestado").Position = 0;
            pivotTable.Fields["Tabla"].Items.FirstOrDefault(p => p.Value == "Planificado").Position = 1;
            pivotTable.Fields["Tabla"].Items.FirstOrDefault(p => p.Value == "Imputado").Position = 2;
            pivotTable.Fields["Tabla"].SubtotalCaption = "Total";
            pivotTable.ColumnFields.Add(pivotTable.Fields["Tabla"]);

            pivotTable.Fields["Fecha"].GroupItems(PivotFieldGroupByType.Months);
            pivotTable.ColumnFields.Add(pivotTable.Fields["Fecha"]);

            var horasField = pivotTable.DataFields.Add(pivotTable.Fields["Horas"]);
            horasField.NumberFormat = @"#,##0.00";

            pivotTable.View.GrandTotalCaption = "Total";
            pivotTable.Layout.ShowRowGrandTotals = false;
            pivotTable.Style = workbook.TableStyles["PivotStyleMedium13"]; //.DefaultPivotStyle;

            Stream result = new MemoryStream();
            workbook.SaveDocument(result, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            result.Seek(0, SeekOrigin.Begin);
            return result;

            /*
            var filePath = Path.GetTempFileName();
            workbook.SaveDocument(filePath, DevExpress.Spreadsheet.DocumentFormat.Xlsx);            
            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            Stream result = new MemoryStream();
            fileStream.CopyTo(result);
            fileStream.Dispose();
            File.Delete(filePath);
            result.Seek(0, SeekOrigin.Begin);
            return result;
            */



            /*
             sourceWorksheet["D1:D" + rowcount.ToString()].NumberFormat  = "mmm-yy";
             //Apply custom number format.
             worksheet["B3:E3"].NumberFormat = "[Green]#.00;[Red]#.00;[Blue]0.00;[Magenta]\"product: \"@";
            */

            //Process.Start(@"c:\temp\query.xlsx");

        }

        public static Stream DownloadReport(ReportBase report)
        {            
            Stream result = new MemoryStream();
            //var filePath = DataHubConfiguration.BinPath + @"\..\Reports\" + @"R" + int.Parse(report.DBID).ToString("D9");
            //var filePath = DataHubConfiguration.PathToCodeBase + @"\..\Reports\" + @"R" + int.Parse(report.DBID).ToString("D9");
            //var filePath = @"C:\Proyectos\DataHUB\bin\Server" + @"\..\Reports\" + @"R" + int.Parse(report.DBID).ToString("D9");
            var filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\Reports\" + @"R" + int.Parse(report.DBID).ToString("D9");

            if (File.Exists(filePath))
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);                
                fileStream.CopyTo(result);
                fileStream.Dispose();
                result.Seek(0, SeekOrigin.Begin);
            }
            return result;
        }

        public static void DeleteReport(ReportBase report)
        {
            string sqlCommandText = "DELETE AUDIT.REPORT " +
                                    " WHERE reportid = @reportid";
            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@reportid", SqlDbType.Int);
            sqlCommand.Parameters["@reportid"].Value = report.DBID;

            sqlCommand.ExecuteNonQuery();


            var filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\Reports\" + @"R" + int.Parse(report.DBID).ToString("D9");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }        
        }

        public static Stream GetBalanceAnaliticoReport(string from, string to)
        {                        
            string commandText = "SELECT  " +  //TOP 500
                " CASE " +
                " 	WHEN LEFT(GEX_ANApuntesAnaliticos01.CodNivel101,1) = 'B' THEN 'Barcelona' " +
                " 	WHEN LEFT(GEX_ANApuntesAnaliticos01.CodNivel101,1) = 'M' THEN 'Madrid' " +
                " END AS Barcelona_Madrid " +

                " ,GEX_ANApuntesAnaliticos01.CodNivel101 Division_Codigo " +
                " ,GEX_ANN1Definicion07.DesCodNivel107 Division_Titulo " +

                " ,CASE " +
                " 	WHEN GEX_ANApuntesAnaliticos01.NatAna01 = 'G' THEN 'Gasto' " +
                " 	WHEN GEX_ANApuntesAnaliticos01.NatAna01 = 'I' THEN 'Ingreso' " +
                " END AS Gasto_Ingreso " +

                " ,GEX_ANConceptosCuentas02.CodConcepto02 Concepto_Codigo " +
                " ,GEX_ANConceptosDefin03.DesConcepto03 Concepto_Titulo " +
                " ,GEX_CGDiarioApuntes45.ApCuenta45 Cuenta_Codigo " +
                " ,GEX_CGCuentasDefinicion40.TituloCC40 Cuenta_Titulo " +
                " ,GEX_ANApuntesAnaliticos01.ImpAnaE01 AS Importe " +

                " ,CASE " +
                " 	WHEN GEX_ANApuntesAnaliticos01.NatAna01 = 'G' THEN -GEX_ANApuntesAnaliticos01.ImpAnaE01 " +
                " 	WHEN GEX_ANApuntesAnaliticos01.NatAna01 = 'I' THEN GEX_ANApuntesAnaliticos01.ImpAnaE01 " +
                " END AS Importe_Signo " +

                " ,GEX_ANApuntesAnaliticos01.Partic01 AS Porcentaje " +

                " FROM SVRBBDDGEX.EXTRA01000.dbo.GEX_ANApuntesAnaliticos01 AS GEX_ANApuntesAnaliticos01 " +

                " INNER JOIN SVRBBDDGEX.EXTRA01000.dbo.GEX_CGDiarioApuntes45 AS GEX_CGDiarioApuntes45 " +
                " 	ON (GEX_CGDiarioApuntes45.Ejercicio45 = GEX_ANApuntesAnaliticos01.Ejercicio01 AND " +
                " 		GEX_CGDiarioApuntes45.IDApunte45 = GEX_ANApuntesAnaliticos01.CodMov01) " +

                /*
                " INNER JOIN SVRBBDDGEX.EXTRA01000.dbo.GEX_ANConceptosCuentas02 AS GEX_ANConceptosCuentas02 " +
                " 	ON (GEX_ANConceptosCuentas02.Ejercicio02 = GEX_CGDiarioApuntes45.Ejercicio45 AND 		 " +
                " 		GEX_ANConceptosCuentas02.Cuenta02 = GEX_CGDiarioApuntes45.ApCuenta45) " +
                */

                " INNER JOIN  " +
                " 	( " +
                " 	SELECT Ejercicio02, Cuenta02, MAX(CodConcepto02) AS CodConcepto02 " +
                " 	FROM SVRBBDDGEX.EXTRA01000.dbo.GEX_ANConceptosCuentas02 	 " +
                " 	GROUP BY Ejercicio02, Cuenta02 " +
                " 	) AS GEX_ANConceptosCuentas02  " +
                " 	ON (GEX_ANConceptosCuentas02.Ejercicio02 = GEX_CGDiarioApuntes45.Ejercicio45 AND " +
                " 			GEX_ANConceptosCuentas02.Cuenta02 = GEX_CGDiarioApuntes45.ApCuenta45) " +

                " INNER JOIN SVRBBDDGEX.EXTRA01000.dbo.GEX_ANConceptosDefin03 AS GEX_ANConceptosDefin03 " +
                " 	ON (GEX_ANConceptosDefin03.Ejercicio03 = GEX_CGDiarioApuntes45.Ejercicio45 AND " +
                " 		GEX_ANConceptosDefin03.CodConcepto03 = GEX_ANConceptosCuentas02.CodConcepto02) " +

                " INNER JOIN SVRBBDDGEX.EXTRA01000.dbo.GEX_CGCuentasDefinicion40 AS GEX_CGCuentasDefinicion40 " +
                " 	ON (GEX_CGCuentasDefinicion40.Ejercicio40 = GEX_CGDiarioApuntes45.Ejercicio45 AND " +
                " 		GEX_CGCuentasDefinicion40.Cuenta40 = GEX_CGDiarioApuntes45.ApCuenta45) " +

                " INNER JOIN SVRBBDDGEX.EXTRA01000.dbo.GEX_ANN1Definicion07 AS GEX_ANN1Definicion07 " +
                " 	ON (GEX_ANN1Definicion07.Ejercicio07 = GEX_CGDiarioApuntes45.Ejercicio45 AND " +
                " 		GEX_ANN1Definicion07.CodLinea07 = CodLinea01 AND " +
                " 		GEX_ANN1Definicion07.CodNivel107 = CodNivel101) " +

                " WHERE GEX_CGDiarioApuntes45.Ejercicio45 = @ejercicio " +
                " AND GEX_ANApuntesAnaliticos01.FechaAna01 BETWEEN @startDate AND @finishDate ";

            SqlCommand sqlCommand = new SqlCommand(commandText, Database.Connection);

            sqlCommand.Parameters.Add("@startDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@startDate"].Value = from.ToDateTime();

            sqlCommand.Parameters.Add("@finishDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@finishDate"].Value = to.ToDateTime();

            int ejercicio = from.ToDateTime().Year;
            if (from.ToDateTime().Month < 9)
                ejercicio = from.ToDateTime().Year - 1;

            sqlCommand.Parameters.Add("@ejercicio", SqlDbType.Int);
            sqlCommand.Parameters["@ejercicio"].Value = ejercicio;

            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Barcelona_Madrid", typeof(string));
            dataTable.Columns.Add("Division_Codigo", typeof(string));
            dataTable.Columns.Add("Division_Titulo", typeof(string));
            dataTable.Columns.Add("Gasto_Ingreso", typeof(string));
            dataTable.Columns.Add("Concepto_Codigo", typeof(string));
            dataTable.Columns.Add("Concepto_Titulo", typeof(string));
            dataTable.Columns.Add("Cuenta_Codigo", typeof(string));
            dataTable.Columns.Add("Cuenta_Titulo", typeof(string));
            dataTable.Columns.Add("Importe", typeof(double));
            dataTable.Columns.Add("Importe_Signo", typeof(double));
            dataTable.Columns.Add("Porcentaje", typeof(double));
           
            int rowcount = 0;
            while (dataReader.Read())
            {
                rowcount++;

                dataTable.Rows.Add(
                    dataReader["Barcelona_Madrid"].ToString(),
                    dataReader["Division_Codigo"].ToString(),
                    dataReader["Division_Titulo"].ToString(),
                    dataReader["Gasto_Ingreso"].ToString(),
                    dataReader["Concepto_Codigo"].ToString(),
                    dataReader["Concepto_Titulo"].ToString(),
                    dataReader["Cuenta_Codigo"].ToString(),
                    dataReader["Cuenta_Titulo"].ToString(),
                    dataReader["Importe"].ToString(),
                    dataReader["Importe_Signo"].ToString(),
                    dataReader["Porcentaje"].ToString()
                    );                
            }

            Workbook workbook = new Workbook();
            string templatePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\Templates\Balance analítico.xltx";
            workbook.LoadDocument(templatePath);
            //workbook.LoadDocument(@"C:\Proyectos\DataHUB\bin\Templates\Balance analítico.xltx");
            Worksheet datosWorksheet = workbook.Worksheets["Datos"];            
            workbook.Worksheets.ActiveWorksheet = datosWorksheet;

            workbook.BeginUpdate();

            datosWorksheet.Import(dataTable, true, 0, 0);

            for (int i = 0; i < rowcount; i++)
            {
                int row = i + 2;
                datosWorksheet.Cells[row - 1, 11].Formula = @"=BUSCARV(C" + row + @";Divisiones!A:B;2;0)";                
                datosWorksheet.Cells[row - 1, 12].Formula = @"=CONCATENAR(TEXTO(E" + row + @";""00""); "" - "";F" + row + @")";
                datosWorksheet.Cells[row - 1, 13].Formula = @"=CONCATENAR(TEXTO(G" + row + @";""00""); "" - "";H" + row + @")";                
                datosWorksheet.Cells[row - 1, 14].Formula = @"=BUSCARV(VALOR(G" + row + @");'Cuentas'!A:C;2;0)";
                datosWorksheet.Cells[row - 1, 15].Formula = @"=BUSCARV(VALOR(G" + row + @");'Cuentas'!A:C;3;0)";
            }

            workbook.EndUpdate();            
            workbook.CalculateFullRebuild();
            Worksheet pygWorksheet = workbook.Worksheets["PyG Análitico PD+Socios"];
            workbook.Worksheets.ActiveWorksheet = pygWorksheet;
            workbook.PivotCaches.RefreshAll();

            Stream result = new MemoryStream();
            workbook.SaveDocument(result, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        public static Stream GetBalanceAnaliticoBetaReport(string from, string to)
        {
            string commandText = "SELECT  " +  //TOP 500
                " GEX_CGDiarioApuntes45.IDApunte45 AS Apunte_Codigo " +
                " ,GEX_CGDiarioApuntes45.NomCpto45 AS Apunte_Titulo " +
                " ,GEX_ANApuntesAnaliticos01.FechaAna01 AS Fecha " +

                " ,CASE " +
                " 	WHEN LEFT(GEX_ANApuntesAnaliticos01.CodNivel101,1) = 'B' THEN 'Barcelona' " +
                " 	WHEN LEFT(GEX_ANApuntesAnaliticos01.CodNivel101,1) = 'M' THEN 'Madrid' " +
                " END AS Barcelona_Madrid " +

                " ,GEX_ANApuntesAnaliticos01.CodNivel101 Division_Codigo " +
                " ,GEX_ANN1Definicion07.DesCodNivel107 Division_Titulo " +

                " ,CASE " +
                " 	WHEN GEX_ANApuntesAnaliticos01.NatAna01 = 'G' THEN 'Gasto' " +
                " 	WHEN GEX_ANApuntesAnaliticos01.NatAna01 = 'I' THEN 'Ingreso' " +
                " END AS Gasto_Ingreso " +

                " ,GEX_ANConceptosCuentas02.CodConcepto02 Concepto_Codigo " +
                " ,GEX_ANConceptosDefin03.DesConcepto03 Concepto_Titulo " +
                " ,GEX_CGDiarioApuntes45.ApCuenta45 Cuenta_Codigo " +
                " ,GEX_CGCuentasDefinicion40.TituloCC40 Cuenta_Titulo " +
                " ,GEX_ANApuntesAnaliticos01.ImpAnaE01 AS Importe " +

                " ,CASE " +
                " 	WHEN GEX_ANApuntesAnaliticos01.NatAna01 = 'G' THEN -GEX_ANApuntesAnaliticos01.ImpAnaE01 " +
                " 	WHEN GEX_ANApuntesAnaliticos01.NatAna01 = 'I' THEN GEX_ANApuntesAnaliticos01.ImpAnaE01 " +
                " END AS Importe_Signo " +

                " ,GEX_ANApuntesAnaliticos01.Partic01 AS Porcentaje " +

                " FROM SVRBBDDGEX.EXTRA01000.dbo.GEX_ANApuntesAnaliticos01 AS GEX_ANApuntesAnaliticos01 " +

                " INNER JOIN SVRBBDDGEX.EXTRA01000.dbo.GEX_CGDiarioApuntes45 AS GEX_CGDiarioApuntes45 " +
                " 	ON (GEX_CGDiarioApuntes45.Ejercicio45 = GEX_ANApuntesAnaliticos01.Ejercicio01 AND " +
                " 		GEX_CGDiarioApuntes45.IDApunte45 = GEX_ANApuntesAnaliticos01.CodMov01) " +

                " INNER JOIN  " +
                " 	( " +
                " 	SELECT Ejercicio02, Cuenta02, MAX(CodConcepto02) AS CodConcepto02 " +
                " 	FROM SVRBBDDGEX.EXTRA01000.dbo.GEX_ANConceptosCuentas02 	 " +
                " 	GROUP BY Ejercicio02, Cuenta02 " +
                " 	) AS GEX_ANConceptosCuentas02  " +
                " 	ON (GEX_ANConceptosCuentas02.Ejercicio02 = GEX_CGDiarioApuntes45.Ejercicio45 AND " +
                " 			GEX_ANConceptosCuentas02.Cuenta02 = GEX_CGDiarioApuntes45.ApCuenta45) " +

                " INNER JOIN SVRBBDDGEX.EXTRA01000.dbo.GEX_ANConceptosDefin03 AS GEX_ANConceptosDefin03 " +
                " 	ON (GEX_ANConceptosDefin03.Ejercicio03 = GEX_CGDiarioApuntes45.Ejercicio45 AND " +
                " 		GEX_ANConceptosDefin03.CodConcepto03 = GEX_ANConceptosCuentas02.CodConcepto02) " +

                " INNER JOIN SVRBBDDGEX.EXTRA01000.dbo.GEX_CGCuentasDefinicion40 AS GEX_CGCuentasDefinicion40 " +
                " 	ON (GEX_CGCuentasDefinicion40.Ejercicio40 = GEX_CGDiarioApuntes45.Ejercicio45 AND " +
                " 		GEX_CGCuentasDefinicion40.Cuenta40 = GEX_CGDiarioApuntes45.ApCuenta45) " +

                " INNER JOIN SVRBBDDGEX.EXTRA01000.dbo.GEX_ANN1Definicion07 AS GEX_ANN1Definicion07 " +
                " 	ON (GEX_ANN1Definicion07.Ejercicio07 = GEX_CGDiarioApuntes45.Ejercicio45 AND " +
                " 		GEX_ANN1Definicion07.CodLinea07 = CodLinea01 AND " +
                " 		GEX_ANN1Definicion07.CodNivel107 = CodNivel101) " +

                " WHERE GEX_CGDiarioApuntes45.Ejercicio45 = 2021 " +
                " AND GEX_ANApuntesAnaliticos01.FechaAna01 BETWEEN @startDate AND @finishDate ";

            SqlCommand sqlCommand = new SqlCommand(commandText, Database.Connection);

            sqlCommand.Parameters.Add("@startDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@startDate"].Value = from.ToDateTime();

            sqlCommand.Parameters.Add("@finishDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@finishDate"].Value = to.ToDateTime();

            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();            
            dataTable.Columns.Add("Barcelona_Madrid", typeof(string));
            dataTable.Columns.Add("Division_Codigo", typeof(string));
            dataTable.Columns.Add("Division_Titulo", typeof(string));
            dataTable.Columns.Add("Gasto_Ingreso", typeof(string));
            dataTable.Columns.Add("Concepto_Codigo", typeof(string));
            dataTable.Columns.Add("Concepto_Titulo", typeof(string));
            dataTable.Columns.Add("Cuenta_Codigo", typeof(string));
            dataTable.Columns.Add("Cuenta_Titulo", typeof(string));
            dataTable.Columns.Add("Importe", typeof(double));
            dataTable.Columns.Add("Importe_Signo", typeof(double));
            dataTable.Columns.Add("Porcentaje", typeof(double));
            dataTable.Columns.Add("Apunte_Codigo", typeof(string));
            dataTable.Columns.Add("Apunte_Titulo", typeof(string));
            dataTable.Columns.Add("Fecha", typeof(DateTime));

            int rowcount = 0;
            while (dataReader.Read())
            {
                rowcount++;

                dataTable.Rows.Add(                    
                    dataReader["Barcelona_Madrid"].ToString(),
                    dataReader["Division_Codigo"].ToString(),
                    dataReader["Division_Titulo"].ToString(),
                    dataReader["Gasto_Ingreso"].ToString(),
                    dataReader["Concepto_Codigo"].ToString(),
                    dataReader["Concepto_Titulo"].ToString(),
                    dataReader["Cuenta_Codigo"].ToString(),
                    dataReader["Cuenta_Titulo"].ToString(),
                    dataReader["Importe"].ToString(),
                    dataReader["Importe_Signo"].ToString(),
                    dataReader["Porcentaje"].ToString(),
                    dataReader["Apunte_Codigo"].ToString(),
                    dataReader["Apunte_Titulo"].ToString(),
                    dataReader["Fecha"].ToString()
                    );
            }

            Workbook workbook = new Workbook();
            string templatePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\Templates\Balance analítico (Beta).xltx";
            workbook.LoadDocument(templatePath);
            //workbook.LoadDocument(@"C:\Proyectos\DataHUB\bin\Templates\Balance analítico.xltx");
            Worksheet datosWorksheet = workbook.Worksheets["Datos"];
            workbook.Worksheets.ActiveWorksheet = datosWorksheet;

            workbook.BeginUpdate();

            datosWorksheet.Import(dataTable, true, 0, 0);

            for (int i = 0; i < rowcount; i++)
            {
                int row = i + 2;
                datosWorksheet.Cells[row - 1, 14].Formula = @"=BUSCARV(C" + row + @";Divisiones!A:B;2;0)";
                datosWorksheet.Cells[row - 1, 15].Formula = @"=CONCATENAR(TEXTO(E" + row + @";""00""); "" - "";F" + row + @")";
                datosWorksheet.Cells[row - 1, 16].Formula = @"=CONCATENAR(TEXTO(G" + row + @";""00""); "" - "";H" + row + @")";
                datosWorksheet.Cells[row - 1, 17].Formula = @"=BUSCARV(VALOR(G" + row + @");'Cuentas'!A:C;2;0)";
                datosWorksheet.Cells[row - 1, 18].Formula = @"=BUSCARV(VALOR(G" + row + @");'Cuentas'!A:C;3;0)";
                datosWorksheet.Cells[row - 1, 19].Formula = @"=CONCATENAR(L" + row + @"; "" - "";M" + row + @")";                
            }

            workbook.EndUpdate();
            workbook.CalculateFullRebuild();
            Worksheet pygWorksheet = workbook.Worksheets["PyG Análitico PD+Socios"];
            workbook.Worksheets.ActiveWorksheet = pygWorksheet;
            workbook.PivotCaches.RefreshAll();

            Stream result = new MemoryStream();
            workbook.SaveDocument(result, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        public static Stream GetFacturacionPorSocioReport(string from, string to)
        {
            string commandText = "SELECT  " +

                //" PersonalSocio.Usuario443 IdSocio, " +
                " COALESCE(PersonalSocio.Nombre443, 'N/C') Socio, " +

                " Cliente.Cliente18 IdCliente, " +
                " COALESCE(Cliente.Razon18, 'N/C') Cliente, " +

                " ClienteProyectos.CodProy154 IdProyecto, " +
                " COALESCE(ClienteProyectos.NomProy154, 'N/C') Proyecto, " +

                //" COALESCE(ClienteProyectos.IDDepartamento154, 0) Departamento, " +
                " Departamentos.Descripcion441 Departamento, " + 

                //" CONVERT(VARCHAR,Facturas.FecFactura51, 103) FechaFactura, " +
                " Facturas.FecFactura51 Fecha, " +
                " 'Factura - ' + CONVERT(VARCHAR,Facturas.Ejercicio51) + '/' + CONVERT(VARCHAR,Facturas.NumFactura51)  + ' - ' + DocumentosLin.Descripcion45 Factura, " +
                " CASE WHEN Facturas.TipoDocum51 = 'F' THEN DocumentosLin.ImpLinE45 ELSE -1 * DocumentosLin.ImpLinE45 END AS Importe, " +

                " DocumentosLin.Referencia45 IdArticulo, " +
                " Articulo.Nombre26 NombreArticulo, " +
                " Articulo.CtaVentas26 CuentaVentas " +
    
                " FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GVFacturas51 Facturas" +
                " INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GVFacturasAlbaran59 FacturasAlbaran ON (FacturasAlbaran.IdFactura59 = Facturas.IdFactura51) " +
                " INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GVDocumentosCab44 DocumentosCab ON (DocumentosCab.Identificador44 = FacturasAlbaran.IdAlbaran59) " +
                " INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GVDocumentosLin45 DocumentosLin ON (DocumentosLin.IdCab45 = DocumentosCab.Identificador44 AND DocumentosLin.ImpLinE45 > 0) " +
                " INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 ClienteProyectos ON (ClienteProyectos.Cliente154 = Facturas.Cliente51 AND ClienteProyectos.CodProy154 = DocumentosLin.CodProyecto45) " +
                " INNER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFDepartamentosPersonal441 Departamentos ON (Departamentos.IDDepartamento441 = ClienteProyectos.IDDepartamento154) " +
                " LEFT OUTER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFCliente18 Cliente ON (Cliente.Cliente18 = Facturas.Cliente51) " +
                " LEFT OUTER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFPersonal443 PersonalSocio ON (PersonalSocio.Usuario443 = ClienteProyectos.UsuarioSocio154) " +
                " LEFT OUTER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFArticulo26 Articulo ON (Articulo.Referencia26 = DocumentosLin.Referencia45) " +

                //" WHERE Facturas.FecFactura51 >= '2021-09-01 00:00:00.000' and Facturas.FecFactura51 < '2023-04-01 00:00:00.000' ";
                " WHERE Facturas.FecFactura51 BETWEEN @startDate AND @finishDate ";

            SqlCommand sqlCommand = new SqlCommand(commandText, Database.Connection);

            sqlCommand.Parameters.Add("@startDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@startDate"].Value = from.ToDateTime();

            sqlCommand.Parameters.Add("@finishDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@finishDate"].Value = to.ToDateTime();

            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();            
            dataTable.Columns.Add("Socio", typeof(string));
            dataTable.Columns.Add("IdCliente", typeof(string));
            dataTable.Columns.Add("Cliente", typeof(string));
            dataTable.Columns.Add("IdProyecto", typeof(string));
            dataTable.Columns.Add("Proyecto", typeof(string));
            dataTable.Columns.Add("Departamento", typeof(string));            
            dataTable.Columns.Add("Fecha", typeof(DateTime));
            dataTable.Columns.Add("Factura", typeof(string));
            dataTable.Columns.Add("Importe", typeof(double));

            dataTable.Columns.Add("IdArticulo", typeof(string));
            dataTable.Columns.Add("NombreArticulo", typeof(string));
            dataTable.Columns.Add("CuentaVentas", typeof(string));

            int rowcount = 0;
            while (dataReader.Read())
            {
                rowcount++;

                dataTable.Rows.Add(                    
                    dataReader["Socio"].ToString(),
                    dataReader["IdCliente"].ToString(),
                    dataReader["Cliente"].ToString(),
                    dataReader["IdProyecto"].ToString(),
                    dataReader["Proyecto"].ToString(),
                    dataReader["Departamento"].ToString(),                    
                    dataReader["Fecha"].ToString(),
                    dataReader["Factura"].ToString(),
                    dataReader["Importe"].ToString(),
                    dataReader["IdArticulo"].ToString(),
                    dataReader["NombreArticulo"].ToString(),
                    dataReader["CuentaVentas"].ToString()
                    );
            }

            Workbook workbook = new Workbook();
            string templatePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\Templates\FacturacionPorSocio.xltx";
            workbook.LoadDocument(templatePath);            
            Worksheet datosWorksheet = workbook.Worksheets["Datos"];
            workbook.Worksheets.ActiveWorksheet = datosWorksheet;

            workbook.BeginUpdate();

            datosWorksheet.Import(dataTable, true, 0, 0);

            workbook.EndUpdate();
            workbook.CalculateFullRebuild();
            Worksheet pygWorksheet = workbook.Worksheets["Facturación x Socio"];
            workbook.Worksheets.ActiveWorksheet = pygWorksheet;            

            workbook.PivotCaches.RefreshAll();

            Stream result = new MemoryStream();
            workbook.SaveDocument(result, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        public static Stream GetImputacionReport(string from, string to)
        {
            string commandText = "SELECT  " +

                " PersonalEmpleado.Usuario443 IdEmpleado, " +
                " PersonalEmpleado.Nombre443 NombreEmpleado, " +
                
                " PersonalSocio.Usuario443 IdSocio, " +                
                " COALESCE(PersonalSocio.Nombre443, 'N/C') NombreSocio, " +

                " Cliente.Cliente18 IdCliente, " +               
                " COALESCE(Cliente.Razon18, 'N/C') NombreCliente, " +

                " ClienteProyectos.CodProy154 IdProyecto, " +                
                " COALESCE(ClienteProyectos.NomProy154, 'N/C') NombreProyecto, " +

                " TiposProyecto.NomTipo440 TipoProyecto, " +
                " CASE WHEN (TiposProyecto.NomTipo440 IS NULL OR TiposProyecto.NomTipo440 = 'No Facturables') THEN 'No' ELSE 'Si' END AS Facturable, " +

                " CONVERT(VARCHAR,ProyectosIncurridos.Fecha451, 103) Fecha,  " +                
                " ProyectosIncurridos.Horas451 Horas " +


                /*
                " COALESCE(PersonalSocio.Nombre443, 'N/C') Socio, " +

                " Cliente.Cliente18 IdCliente, " +
                " COALESCE(Cliente.Razon18, 'N/C') Cliente, " +

                " ClienteProyectos.CodProy154 IdProyecto, " +
                " COALESCE(ClienteProyectos.NomProy154, 'N/C') Proyecto, " +

                " Departamentos.Descripcion441 Departamento, " +

                " Facturas.FecFactura51 Fecha, " +
                " 'Factura - ' + CONVERT(VARCHAR,Facturas.Ejercicio51) + '/' + CONVERT(VARCHAR,Facturas.NumFactura51)  + ' - ' + DocumentosLin.Descripcion45 Factura, " +
                " CASE WHEN Facturas.TipoDocum51 = 'F' THEN DocumentosLin.ImpLinE45 ELSE -1 * DocumentosLin.ImpLinE45 END AS Importe " +
                */

                " FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GFProyectosIncurridos451 ProyectosIncurridos " +
                " LEFT OUTER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 ClienteProyectos ON (ProyectosIncurridos.CodProy451 = ClienteProyectos.CodProy154) " +
                " LEFT OUTER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFCliente18 Cliente ON (Cliente.Cliente18 = ClienteProyectos.Cliente154) " +
                " LEFT OUTER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFPersonal443 PersonalSocio ON (PersonalSocio.Usuario443 = ClienteProyectos.UsuarioSocio154)  " +
                " LEFT OUTER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFPersonal443 PersonalEmpleado ON (PersonalEmpleado.Usuario443 = ProyectosIncurridos.Usuario451)  " +
                " LEFT OUTER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFTiposProyecto440 TiposProyecto ON (TiposProyecto.CodTipo440 = ClienteProyectos.TipoProyecto154) " +

                " WHERE ProyectosIncurridos.Fecha451 BETWEEN @startDate AND @finishDate ";

            SqlCommand sqlCommand = new SqlCommand(commandText, Database.Connection);

            sqlCommand.Parameters.Add("@startDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@startDate"].Value = from.ToDateTime();

            sqlCommand.Parameters.Add("@finishDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@finishDate"].Value = to.ToDateTime();

            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("IdEmpleado", typeof(string));
            dataTable.Columns.Add("NombreEmpleado", typeof(string));
            dataTable.Columns.Add("IdSocio", typeof(string));
            dataTable.Columns.Add("NombreSocio", typeof(string));
            dataTable.Columns.Add("IdCliente", typeof(string));
            dataTable.Columns.Add("NombreCliente", typeof(string));
            dataTable.Columns.Add("IdProyecto", typeof(string));
            dataTable.Columns.Add("NombreProyecto", typeof(string));
            dataTable.Columns.Add("TipoProyecto", typeof(string));
            dataTable.Columns.Add("Facturable", typeof(string));
            dataTable.Columns.Add("Fecha", typeof(DateTime));
            dataTable.Columns.Add("Horas", typeof(Double));           

            int rowcount = 0;
            while (dataReader.Read())
            {
                rowcount++;

                dataTable.Rows.Add(
                    dataReader["IdEmpleado"].ToString(),
                    dataReader["NombreEmpleado"].ToString(),
                    dataReader["IdSocio"].ToString(),
                    dataReader["NombreSocio"].ToString(),
                    dataReader["IdCliente"].ToString(),
                    dataReader["NombreCliente"].ToString(),
                    dataReader["IdProyecto"].ToString(),
                    dataReader["NombreProyecto"].ToString(),
                    dataReader["TipoProyecto"].ToString(),
                    dataReader["Facturable"].ToString(),
                    dataReader["Fecha"].ToString(),
                    dataReader["Horas"].ToString()
                    );
            }

            Workbook workbook = new Workbook();
            string templatePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\Templates\Imputaciones.xltx";
            workbook.LoadDocument(templatePath);
            Worksheet datosWorksheet = workbook.Worksheets["Datos"];
            workbook.Worksheets.ActiveWorksheet = datosWorksheet;

            workbook.BeginUpdate();

            datosWorksheet.Import(dataTable, true, 0, 0);

            workbook.EndUpdate();
            workbook.CalculateFullRebuild();
            Worksheet pygWorksheet = workbook.Worksheets["Imputaciones"];
            workbook.Worksheets.ActiveWorksheet = pygWorksheet;

            workbook.PivotCaches.RefreshAll();

            Stream result = new MemoryStream();
            workbook.SaveDocument(result, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }


        public static Stream GetRealizacionReport(string from, string to)
        {
            string commandText = "SELECT  " +
                " Proyectos.*, COALESCE(FacturacionPrevista.Horas, 0) Horas, COALESCE(FacturacionPrevista.FacturacionPrevista, 0) [Facturación Prevista], COALESCE(FacturacionReal.FacturacionReal, 0) [Facturación Real] FROM " +
                " ( " +
                " 	SELECT Departamento.Descripcion441 AS Departamento, " +
                " 	Cliente.Cliente18 AS IdCliente, Cliente.Empresa18 AS [Nombre Cliente],  " +
                " 	Proyecto.CodProy154 AS IdProyecto, RTRIM(Proyecto.NomProy154) AS [Nombre Proyecto], " +
                " 	Socio.Nombre443 [Socio], " +
                " 	Responsable.Nombre443 [Responsable] " +

                " 	FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 Proyecto  " +
                " 			 " +
                " 	LEFT JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFCliente18 Cliente " +
                " 		ON (Cliente.Cliente18 = Proyecto.Cliente154) " +

                " 	LEFT JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFDepartamentosPersonal441 Departamento " +
                " 		ON (Departamento.IDDepartamento441 = Proyecto.IDDepartamento154) " +

                " 	LEFT OUTER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFPersonal443 AS Socio ON (Socio.Usuario443 = Proyecto.UsuarioSocio154)  " +
                " 	LEFT OUTER JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFPersonal443 AS Responsable ON (Responsable.Usuario443 = Proyecto.UsuarioResponsable154)  " +
                "  " +
                " 	AND Proyecto.IDDepartamento154 IN (4,3) " +
                " ) AS Proyectos " +
                "  " +
                " LEFT OUTER JOIN " +

                " ( " +
                " 	SELECT " +
                " 	IdCliente, 	 " +
                " 	IdProyecto, 	 " +
                " 	SUM(Horas) AS Horas, SUM(Importe) AS FacturacionPrevista " +
                " 	FROM  " +
                " 	( " +
                " 		SELECT Cliente.Cliente18 AS IdCliente,  " +
                " 		Proyecto.CodProy154 AS IdProyecto,  " +
                " 		Incurrido.Usuario451, Incurrido.Fecha451, Categoria.Categoria958,  " +
                " 		Incurrido.Horas451 Horas, Tarifa.PrecioHora957 PrecioHora, " +
                " 		ISNULL((Incurrido.Horas451 * Tarifa.PrecioHora957),0) AS Importe " +

                " 		FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GFProyectosIncurridos451 Incurrido " +

                " 		LEFT JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 Proyecto  " +
                " 			ON (Proyecto.CodProy154 = Incurrido.CodProy451 AND Proyecto.Cliente154 = Incurrido.Cliente451) " +

                " 		LEFT JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFPersonalCategorias958 Categoria " +
                " 			ON (Incurrido.Usuario451 = Categoria.Usuario958 AND Incurrido.Fecha451 >= Categoria.FechaDesde958 AND (Incurrido.Fecha451 <= Categoria.FechaHasta958 OR Categoria.FechaHasta958 IS NULL)) " +

                " 		LEFT JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFTarifasVenta957 Tarifa " +
                " 			ON (Categoria.Categoria958 = Tarifa.Categoria957 AND Categoria.FechaDesde958 >= Tarifa.FechaDesde957 AND (Categoria.FechaHasta958 <= Tarifa.FechaHasta957 OR Categoria.FechaHasta958 IS NULL)) " +

                " 		LEFT JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFCliente18 Cliente " +
                " 			ON (Cliente.Cliente18 = Proyecto.Cliente154) " +

                " 		WHERE Incurrido.Fecha451 >= @startDate " +
                " 		AND Incurrido.Fecha451 <= @finishDate " +
                " 		AND Proyecto.IDDepartamento154 IN (4,3) " +
                " 	) AS Incurridos " +

                " 	GROUP BY IdCliente, IdProyecto " +
                " ) AS FacturacionPrevista  " +
                " 	ON (FacturacionPrevista.IdCliente = Proyectos.IdCliente AND FacturacionPrevista.IdProyecto = Proyectos.IdProyecto) " +

                " LEFT OUTER JOIN " +

                " ( " +
                " 	SELECT IdCliente, IdProyecto,  " +
                " 	SUM(Importe) AS FacturacionReal " +
                " 	FROM  " +
                " 	( " +
                " 		SELECT Cliente.Cliente18 IdCliente, Proyecto.CodProy154 IdProyecto,  " +
                " 		CASE WHEN Factura.TipoDocum51 = 'F' THEN Linea.ImpLinE45 ELSE -1*Linea.ImpLinE45 END AS Importe " +

                " 		FROM SVRBBDDGEX.EXTRA02000.dbo.GES_GVFacturas51 Factura " +

                " 		LEFT JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GVFacturasAlbaran59 Albaran " +
                " 			ON (Factura.IdFactura51 = Albaran.IdFactura59) " +

                " 		LEFT JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GVDocumentosCab44 Documento  " +
                " 			ON (Albaran.IdAlbaran59 = Documento.Identificador44) " +

                " 		LEFT JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GVDocumentosLin45 Linea " +
                " 			ON (Documento.Identificador44 = Linea.IdCab45) " +

                " 		LEFT JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFCliente18 Cliente  " +
                " 			ON (Factura.Cliente51 = Cliente.Cliente18) " +

                " 		LEFT JOIN SVRBBDDGEX.EXTRA02000.dbo.GES_GFClienteProyectos154 Proyecto  " +
                " 			ON (Factura.Cliente51 = Proyecto.Cliente154 AND CodProyecto45 = Proyecto.CodProy154) " +

                " 		WHERE Factura.FecFactura51 >= @startDate " +
                " 		AND Factura.FecFactura51 <= @finishDate " +
                " 		AND Proyecto.IDDepartamento154 IN (4,3) " +
                " 	) AS Importes " +
                " 	GROUP BY IdCliente, IdProyecto " +
                " ) AS FacturacionReal " +
                " 	ON (FacturacionReal.IdCliente = Proyectos.IdCliente AND FacturacionReal.IdProyecto = Proyectos.IdProyecto) " +

                " WHERE (COALESCE(FacturacionPrevista, 0) <> 0 OR COALESCE(FacturacionReal, 0) <> 0) " +

                " ORDER BY Departamento, [Nombre Cliente], [Nombre Proyecto] ";

            SqlCommand sqlCommand = new SqlCommand(commandText, Database.Connection);

            sqlCommand.Parameters.Add("@startDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@startDate"].Value = from.ToDateTime();

            sqlCommand.Parameters.Add("@finishDate", SqlDbType.DateTime);
            sqlCommand.Parameters["@finishDate"].Value = to.ToDateTime();

            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Departamento", typeof(string));
            dataTable.Columns.Add("IdCliente", typeof(string));
            dataTable.Columns.Add("Nombre Cliente", typeof(string));
            dataTable.Columns.Add("IdProyecto", typeof(string));
            dataTable.Columns.Add("Nombre Proyecto", typeof(string));
            dataTable.Columns.Add("Socio", typeof(string));
            dataTable.Columns.Add("Responsable", typeof(string));
            dataTable.Columns.Add("Horas", typeof(Double));
            dataTable.Columns.Add("Facturación Prevista", typeof(Double));
            dataTable.Columns.Add("Facturación Real", typeof(Double));
            dataTable.Columns.Add("Realización", typeof(Double));


            int rowcount = 0;
            while (dataReader.Read())
            {
                rowcount++;

                dataTable.Rows.Add(
                    dataReader["Departamento"].ToString(),
                    dataReader["IdCliente"].ToString(),
                    dataReader["Nombre Cliente"].ToString(),
                    dataReader["IdProyecto"].ToString(),
                    dataReader["Nombre Proyecto"].ToString(),
                    dataReader["Socio"].ToString(),
                    dataReader["Responsable"].ToString(),
                    dataReader["Horas"].ToString(),
                    dataReader["Facturación Prevista"].ToString(),
                    dataReader["Facturación Real"].ToString()                    
                    );
            }

            Workbook workbook = new Workbook();
            workbook.Worksheets[0].Import(dataTable, true, 0, 0);

            Worksheet sourceWorksheet = workbook.Worksheets[0];
            sourceWorksheet.Name = "Proyectos";

            for (int i = 0; i < rowcount; i++)
            {
                int row = i + 2;
                sourceWorksheet.Cells[row - 1, 7].NumberFormat = @"#,##0.00";
                sourceWorksheet.Cells[row - 1, 8].NumberFormat = @"#,##0.00 €;[Red]-#,##0.00 €";
                sourceWorksheet.Cells[row - 1, 9].NumberFormat = @"#,##0.00 €;[Red]-#,##0.00 €";

                sourceWorksheet.Cells[row - 1, 10].Formula = @"=J" + row + @"/I" + row;
                sourceWorksheet.Cells[row - 1, 10].NumberFormat = @"#,#0.0%";
            }

            CellRange range = sourceWorksheet["A1:K" + (rowcount + 1).ToString()];
            Table table = sourceWorksheet.Tables.Add(range, true);
            table.Style = workbook.TableStyles["TableStyleMedium6"];

            Formatting rangeFormatting = range.BeginUpdateFormatting();
            rangeFormatting.Alignment.WrapText = false;
            rangeFormatting.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
            range.AutoFitColumns();
            range.AutoFitRows();
            range.EndUpdateFormatting(rangeFormatting);

            Stream result = new MemoryStream();
            workbook.SaveDocument(result, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

    }

}
