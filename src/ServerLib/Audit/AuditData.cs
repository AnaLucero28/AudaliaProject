using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Audalia.DataHUBCommon;
using Newtonsoft.Json;

namespace Audalia.DataHUBServer.Audit
{
    public class AuditData
    {
        #region GET

        public static string GetSetting(string param)
        {
            var result = "";

            string commandText = "SELECT value FROM AUDIT.APP_USER_SETTINGS WHERE scope = @scope AND param = @param";
            SqlCommand command = new SqlCommand(commandText, Database.Connection);

            command.Parameters.Add("@scope", SqlDbType.VarChar);
            command.Parameters["@scope"].Value = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name.ToLower(); 

            command.Parameters.Add("@param", SqlDbType.VarChar);
            command.Parameters["@param"].Value = param; 

            SqlDataReader dataReader = command.ExecuteReader();
            
            while (dataReader.Read())
            {
                DataHubLog.WriteLine("AuditData.GetSetting(" + param + ") -> Result: " + dataReader["value"].ToString());                
                result = dataReader["value"].ToString();
                break;
            }

            return result;
        }

        public static string GetPermissions()
        {
            var result = "R";

            string commandText = "SELECT permissions FROM AUDIT.APP_USER WHERE windowsuser = @windowsuser";
            SqlCommand command = new SqlCommand(commandText, Database.Connection);
            command.Parameters.Add("@windowsuser", SqlDbType.VarChar);
            command.Parameters["@windowsuser"].Value = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name.ToLower();
            SqlDataReader dataReader = command.ExecuteReader();

            Console.WriteLine(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name.ToLower());
            while (dataReader.Read())
            {
                DataHubLog.WriteLine("AuditData.GetPermissions -> Result: " + dataReader["permissions"].ToString());
                //Console.WriteLine("Ok -> " + dataReader["permissions"].ToString());
                result = dataReader["permissions"].ToString();
                break;
            }

            return result;
        }

        public static Dictionary<string, string> GetAppUserPermissions()
        {
            var result = new Dictionary<string, string>();

            string commandText = "SELECT scope, permissions FROM AUDIT.APP_USER AS APP_USER " +
                " INNER JOIN AUDIT.APP_USER_PERMISSIONS AS APP_USER_PERMISSIONS ON (APP_USER_PERMISSIONS.userid = APP_USER.userid) " +
                " WHERE APP_USER.windowsuser = @windowsuser ";
            SqlCommand command = new SqlCommand(commandText, Database.Connection);
            command.Parameters.Add("@windowsuser", SqlDbType.VarChar);
            command.Parameters["@windowsuser"].Value = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name.ToLower();
            SqlDataReader dataReader = command.ExecuteReader();
            
            while (dataReader.Read())
            {             
                result.Add(dataReader["scope"].ToString(), dataReader["permissions"].ToString());         
            }

            return result;
        }

        public static List<HolidayBase> GetHolidays(int year)
        {
            var result = new List<HolidayBase>();

            string commandText = "SELECT * FROM AUDIT.HOLIDAY HOLIDAY WHERE year IS NULL OR year between @year - 1 and @year + 1";
            SqlCommand command = new SqlCommand(commandText, Database.Connection);
            command.Parameters.Add("@year", SqlDbType.VarChar);
            command.Parameters["@year"].Value = year;
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                result.Add(new HolidayBase()
                {
                    DBID = dataReader["holidayid"].ToString(),
                    Name = dataReader["name"].ToString(),
                    Year = dataReader["year"] == DBNull.Value ? 0 : (int)dataReader["year"], 
                    Month = dataReader.GetInt32(dataReader.GetOrdinal("Month")),
                    Day = dataReader.GetInt32(dataReader.GetOrdinal("Day"))
                });
            }

            return result;
        }

        public static List<ProjectBase> GetProjects()
        {
            var result = new List<ProjectBase>();

            string projectsCommandText = "SELECT * FROM AUDIT.PROJECT PROJECT ";
            SqlCommand projectsCommand = new SqlCommand(projectsCommandText, Database.Connection);            
            SqlDataReader projectsReader = projectsCommand.ExecuteReader();
            while (projectsReader.Read())
            {
                var startDate = projectsReader["start_date"].ToDateTime(); //DateTime.ParseExact(projectsReader["start_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncStartDate();
                var finishDate = projectsReader["finish_date"].ToDateTime(); // DateTime.ParseExact(projectsReader["finish_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncFinishDate();

                var startDate2 = projectsReader["start_date2"].ToNullableDateTime(); //DateTime.ParseExact(projectsReader["start_date2"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncStartDate();
                var finishDate2 = projectsReader["finish_date2"].ToNullableDateTime(); //DateTime.ParseExact(projectsReader["finish_date2"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncFinishDate();

                var startDate3 = projectsReader["start_date3"].ToNullableDateTime(); //DateTime.ParseExact(projectsReader["start_date3"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncStartDate();
                var finishDate3 = projectsReader["finish_date3"].ToNullableDateTime(); // DateTime.ParseExact(projectsReader["finish_date3"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncFinishDate();

                var baselineStartDate = DateTime.ParseExact(projectsReader["baseline_start_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncStartDate();
                var baselineFinishDate = DateTime.ParseExact(projectsReader["baseline_finish_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncFinishDate();

                result.Add(new ProjectBase()
                {
                    DBID = projectsReader["projectid"].ToString(),
                    Name = projectsReader["name"].ToString(),
                    BranchOffice = projectsReader["branchoffice"].ToString(),
                    TaxYear = projectsReader["taxyear"].ToString(),
                    IdGextor = projectsReader["projectidgextor"].ToString(),

                    StartDate1 = startDate,
                    FinishDate1 = finishDate,

                    StartDate2 = startDate2,
                    FinishDate2 = finishDate2,

                    StartDate3 = startDate3,
                    FinishDate3 = finishDate3,

                    BaselineStartDate = baselineStartDate,
                    BaselineFinishDate = baselineFinishDate,
                    Color = projectsReader["color"].ToString(),
                    Comments = projectsReader["comments"].ToString()
                });
            }

            return result;
        }

        public static ProjectBase GetProject(string projectId)
        {
            ProjectBase result = null;

            string projectsCommandText = "SELECT * FROM AUDIT.PROJECT PROJECT WHERE projectid = @projectid ";
            SqlCommand projectsCommand = new SqlCommand(projectsCommandText, Database.Connection);            
            projectsCommand.Parameters.Add("@projectid", SqlDbType.VarChar);
            projectsCommand.Parameters["@projectid"].Value = projectId;            
            SqlDataReader projectsReader = projectsCommand.ExecuteReader();
            while (projectsReader.Read())
            {
                var startDate = projectsReader["start_date"].ToDateTime(); //DateTime.ParseExact(projectsReader["start_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncStartDate();                
                var finishDate = projectsReader["finish_date"].ToDateTime(); //DateTime.ParseExact(projectsReader["finish_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncFinishDate();

                var startDate2 = projectsReader["start_date2"].ToNullableDateTime(); //DateTime.ParseExact(projectsReader["start_date2"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncStartDate();
                var finishDate2 = projectsReader["finish_date2"].ToNullableDateTime(); //DateTime.ParseExact(projectsReader["finish_date2"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncFinishDate();

                var startDate3 = projectsReader["start_date3"].ToNullableDateTime(); //DateTime.ParseExact(projectsReader["start_date3"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncStartDate();
                var finishDate3 = projectsReader["finish_date3"].ToNullableDateTime(); //DateTime.ParseExact(projectsReader["finish_date3"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncFinishDate();
               
                var baselineStartDate = projectsReader["baseline_start_date"].ToDateTime(); //DateTime.ParseExact(projectsReader["baseline_start_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncStartDate();
                var baselineFinishDate = projectsReader["baseline_finish_date"].ToDateTime(); //DateTime.ParseExact(projectsReader["baseline_finish_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncFinishDate();

                result = new ProjectBase()
                {
                    DBID = projectsReader["projectid"].ToString(),
                    Name = projectsReader["name"].ToString(),
                    BranchOffice = projectsReader["branchoffice"].ToString(),
                    TaxYear = projectsReader["taxyear"].ToString(),
                    IdGextor = projectsReader["projectidgextor"].ToString(),


                    StartDate1 = startDate,
                    FinishDate1 = finishDate,

                    StartDate2 = startDate2,
                    FinishDate2 = finishDate2,

                    StartDate3 = startDate3,
                    FinishDate3 = finishDate3,

                    BaselineStartDate = baselineStartDate,
                    BaselineFinishDate = baselineFinishDate,
                    Color = projectsReader["color"].ToString(),
                    Comments = projectsReader["comments"].ToString()
                };
            }

            return result;
        }

        public static List<BudgetBase> GetBudget(string projectId)
        {
            var result = new List<BudgetBase>();

            string projectsCommandText = "SELECT * FROM AUDIT.BUDGET BUDGET WHERE projectid = @projectid ";
            SqlCommand projectsCommand = new SqlCommand(projectsCommandText, Database.Connection);
            projectsCommand.Parameters.Add("@projectid", SqlDbType.VarChar);
            projectsCommand.Parameters["@projectid"].Value = projectId;
            SqlDataReader projectsReader = projectsCommand.ExecuteReader();
            while (projectsReader.Read())
            {
                result.Add(new BudgetBase()
                {
                    DBID = projectsReader["budgetid"].ToString(),
                    ProjectId = projectsReader["projectid"].ToString(),
                    Category = projectsReader["category"].ToString(),
                    Quantity = Int32.Parse(projectsReader["quantity"].ToString()) //projectsReader.GetInt32(projectsReader.GetOrdinal("quantity"))  //(int)projectsReader["quantity"] 
                });
            }

            return result;
        }

        public static List<EmployeeBase> GetEmployees()
        {
            var result = new List<EmployeeBase>();

            string employeesCommandText = "SELECT * FROM AUDIT.EMPLOYEE EMPLOYEE";
            SqlCommand employeesCommand = new SqlCommand(employeesCommandText, Database.Connection);
            SqlDataReader employeesReader = employeesCommand.ExecuteReader();
            while (employeesReader.Read())
            {
                DateTime? leavingDate = null;
                if (employeesReader["leaving_date"] != DBNull.Value)
                    leavingDate = DateTime.ParseExact(employeesReader["leaving_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                result.Add(new EmployeeBase()
                {
                    DBID = employeesReader["employeeid"].ToString(),
                    Name = employeesReader["name"].ToString(),
                    BranchOffice = employeesReader["branchoffice"].ToString(),
                    Category = employeesReader["category"].ToString(),
                    WindowsUser = employeesReader["windowsuser"].ToString(),
                    //Status = employeesReader["status"].ToString(),                    
                    Permissions = employeesReader["permissions"].ToString(),
                    LeavingDate = leavingDate
                });
            }

            return result;
        }

        public static EmployeeBase GetEmployee(string employeeId)
        {
            var result = new EmployeeBase();

            string employeesCommandText = "SELECT * FROM AUDIT.EMPLOYEE EMPLOYEE WHERE employeeid = @employeeid";
            SqlCommand employeesCommand = new SqlCommand(employeesCommandText, Database.Connection);
            employeesCommand.Parameters.Add("@employeeid", SqlDbType.VarChar);
            employeesCommand.Parameters["@employeeid"].Value = employeeId;
            SqlDataReader employeesReader = employeesCommand.ExecuteReader();
            while (employeesReader.Read())
            {
                DateTime? leavingDate = null;
                if (employeesReader["leaving_date"] != DBNull.Value)
                    leavingDate = DateTime.ParseExact(employeesReader["leaving_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                result = new EmployeeBase()
                {
                    DBID = employeesReader["employeeid"].ToString(),
                    Name = employeesReader["name"].ToString(),
                    BranchOffice = employeesReader["branchoffice"].ToString(),
                    Category = employeesReader["category"].ToString(),
                    WindowsUser = employeesReader["windowsuser"].ToString(),
                    //Status = employeesReader["status"].ToString(),
                    Permissions = employeesReader["permissions"].ToString(),
                    LeavingDate = leavingDate
                };
            }

            return result;
        }
        
        public static List<ProjectEmployeeAssocBase> GetProjectEmployeeAssocs()
        {
            var result = new List<ProjectEmployeeAssocBase>();

            string taskEmployeesCommandText = "SELECT projectemployeeassocid, projectid, employeeid, role FROM AUDIT.PROJECT_EMPLOYEE_ASSOC PROJECT_EMPLOYEE_ASSOC";
            SqlCommand taskEmployeesCommand = new SqlCommand(taskEmployeesCommandText, Database.Connection);
            SqlDataReader taskEmployeesReader = taskEmployeesCommand.ExecuteReader();
            while (taskEmployeesReader.Read())
            {
                result.Add(new ProjectEmployeeAssocBase()
                {
                    DBID = taskEmployeesReader["projectemployeeassocid"].ToString(),
                    ProjectId = taskEmployeesReader["projectid"].ToString(),
                    EmployeeId = taskEmployeesReader["employeeid"].ToString(),
                    Role = taskEmployeesReader["role"].ToString()
                });
            }

            return result;
        }

        public static ProjectEmployeeAssocBase GetProjectEmployeeAssoc(string projectEmployeeAssocId)
        {
            ProjectEmployeeAssocBase result = null;

            string taskEmployeesCommandText = "SELECT projectemployeeassocid, projectid, employeeid, role FROM AUDIT.PROJECT_EMPLOYEE_ASSOC PROJECT_EMPLOYEE_ASSOC  WHERE projectemployeeassocid = @projectemployeeassocid ";
            SqlCommand taskEmployeesCommand = new SqlCommand(taskEmployeesCommandText, Database.Connection);
            taskEmployeesCommand.Parameters.Add("@projectemployeeassocid", SqlDbType.VarChar);
            taskEmployeesCommand.Parameters["@projectemployeeassocid"].Value = projectEmployeeAssocId;
            SqlDataReader taskEmployeesReader = taskEmployeesCommand.ExecuteReader();
            while (taskEmployeesReader.Read())
            {
                result = new ProjectEmployeeAssocBase()
                {
                    DBID = taskEmployeesReader["projectemployeeassocid"].ToString(),
                    ProjectId = taskEmployeesReader["projectid"].ToString(),
                    EmployeeId = taskEmployeesReader["employeeid"].ToString(),
                    Role = taskEmployeesReader["role"].ToString()
                };
            }

            return result;
        }

        public static List<ProjectEmployeeAllocBase> GetProjectEmployeeAllocs()
        {
            var result = new List<ProjectEmployeeAllocBase>();

            string taskEmployeesCommandText = "SELECT * FROM AUDIT.PROJECT_EMPLOYEE_ALLOC PROJECT_EMPLOYEE_ALLOC";
            SqlCommand taskEmployeesCommand = new SqlCommand(taskEmployeesCommandText, Database.Connection);
            SqlDataReader taskEmployeesReader = taskEmployeesCommand.ExecuteReader();
            while (taskEmployeesReader.Read())
            {
                var startDate = DateTime.ParseExact(taskEmployeesReader["alloc_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncStartDate();
                

                result.Add(new ProjectEmployeeAllocBase()
                {
                    DBID = taskEmployeesReader["projectemployeeallocid"].ToString(),
                    ProjectId = taskEmployeesReader["projectid"].ToString(),
                    EmployeeId = taskEmployeesReader["employeeid"].ToString(),
                    AllocDate = startDate,                
                    Quantity = int.Parse(taskEmployeesReader["quantity"].ToString())
                });
            }

            return result;
        }

        public static ProjectEmployeeAllocBase GetProjectEmployeeAlloc(string projectEmployeeAllocId)
        {
            ProjectEmployeeAllocBase result = null;

            string taskEmployeesCommandText = "SELECT * FROM AUDIT.PROJECT_EMPLOYEE_ALLOC PROJECT_EMPLOYEE_ALLOC  WHERE projectemployeeallocid = @projectemployeeallocid ";
            SqlCommand taskEmployeesCommand = new SqlCommand(taskEmployeesCommandText, Database.Connection);
            taskEmployeesCommand.Parameters.Add("@projectemployeeallocid", SqlDbType.VarChar);
            taskEmployeesCommand.Parameters["@projectemployeeallocid"].Value = projectEmployeeAllocId;
            SqlDataReader taskEmployeesReader = taskEmployeesCommand.ExecuteReader();
            while (taskEmployeesReader.Read())
            {
                var startDate = DateTime.ParseExact(taskEmployeesReader["alloc_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture).TruncStartDate();
                
                result = new ProjectEmployeeAllocBase()
                {
                    DBID = taskEmployeesReader["projectemployeeallocid"].ToString(),
                    ProjectId = taskEmployeesReader["projectid"].ToString(),
                    EmployeeId = taskEmployeesReader["employeeid"].ToString(),
                    AllocDate = startDate,
                    Quantity = int.Parse(taskEmployeesReader["quantity"].ToString())
                };
            }

            return result;
        }

        public static ReportBase GetReport(string reportId)
        {
            ReportBase result = null;

            string commandText = "SELECT * FROM AUDIT.REPORT REPORT WHERE reportid = @reportid ";
            SqlCommand projectsCommand = new SqlCommand(commandText, Database.Connection);
            projectsCommand.Parameters.Add("@reportid", SqlDbType.VarChar);
            projectsCommand.Parameters["@reportid"].Value = reportId;

            SqlDataReader dataReader = projectsCommand.ExecuteReader();
            while (dataReader.Read())
            {
                result = new ReportBase()
                {
                    DBID = dataReader["reportid"].ToString(),
                    ReportTemplate = (ReportTemplate)dataReader["reporttemplate"],
                    ReportName = dataReader["reportname"].ToString(),
                    CreatorUserName = dataReader["creatorusername"].ToString(),
                    CreationDate = dataReader["creationdate"].ToString().ToDateTime(),
                    Description = dataReader["description"].ToString(),
                    Params = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataReader["params"].ToString()),
                    FilePath = dataReader["filepath"].ToString(),
                };
            }

            return result;
        }

        public static List<ReportBase> GetReports(string creator = "")
        {
            var result = new List<ReportBase>();

            string commandText = "SELECT * FROM AUDIT.REPORT REPORT ";
            SqlCommand projectsCommand = new SqlCommand(commandText, Database.Connection);
            SqlDataReader dataReader = projectsCommand.ExecuteReader();
            while (dataReader.Read())
            {
                var creatorusername = creator;
                if (creatorusername == "" && OperationContext.Current != null)
                {
                    creatorusername = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name.ToLowerInvariant();
                }

                if (creatorusername == "" || dataReader["creatorusername"].ToString() == creatorusername)
                {
                    result.Add(new ReportBase()
                    {
                        DBID = dataReader["reportid"].ToString(),
                        ReportTemplate = (ReportTemplate)dataReader["reporttemplate"],
                        ReportName = dataReader["reportname"].ToString(),
                        CreatorUserName = dataReader["creatorusername"].ToString(),
                        CreationDate = dataReader["creationdate"].ToString().ToDateTime(),
                        Description = dataReader["description"].ToString(),
                        Params = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataReader["params"].ToString()),
                        FilePath = dataReader["filepath"].ToString(),
                    });
                }                
            }

            return result;
        }

        #endregion

        #region POST

        public static string PostSetting(string param, string value)
        {
            var result = "";

            string commandText =
                " MERGE AUDIT.APP_USER_SETTINGS SETTINGS " +
                " USING( VALUES(@scope, @param, @value)) AS DATA(scope, param, value) " +
                " ON SETTINGS.scope = DATA.scope AND SETTINGS.param = DATA.param " +
                " WHEN MATCHED THEN " +
                " UPDATE SET value = DATA.value " +
                " WHEN NOT MATCHED THEN " +
                " INSERT(scope, param, value) VALUES(DATA.scope, DATA.param, DATA.value); ";

            SqlCommand command = new SqlCommand(commandText, Database.Connection);

            command.Parameters.Add("@scope", SqlDbType.VarChar);
            command.Parameters["@scope"].Value = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name.ToLower();

            command.Parameters.Add("@param", SqlDbType.VarChar);
            command.Parameters["@param"].Value = param;

            command.Parameters.Add("@value", SqlDbType.VarChar);
            command.Parameters["@value"].Value = value;

            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                DataHubLog.WriteLine("AuditData.PostSetting(" + param + "," + value + ")");
                result = dataReader["value"].ToString();
                break;
            }

            return result;
        }

        public static ReportBase PostReport(ReportBase report)
        {
            var result = report;

            string sqlCommandText = "INSERT INTO AUDIT.REPORT (reporttemplate, reportname, creatorusername, creationdate, description, params, filepath) VALUES " +
                " (@reporttemplate, @reportname, @creatorusername, @creationdate, @description, @params, @filepath); " +
                " SELECT SCOPE_IDENTITY(); ";            

            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@reporttemplate", SqlDbType.VarChar);
            sqlCommand.Parameters["@reporttemplate"].Value = (int)report.ReportTemplate;

            sqlCommand.Parameters.Add("@reportname", SqlDbType.VarChar);
            sqlCommand.Parameters["@reportname"].Value = report.ReportName;

            sqlCommand.Parameters.Add("@creatorusername", SqlDbType.VarChar);
            sqlCommand.Parameters["@creatorusername"].Value = report.CreatorUserName;

            sqlCommand.Parameters.Add("@creationdate", SqlDbType.VarChar);
            sqlCommand.Parameters["@creationdate"].Value = report.CreationDate.ToString("yyyyMMddHHmmss");

            sqlCommand.Parameters.Add("@description", SqlDbType.VarChar);
            sqlCommand.Parameters["@description"].Value = report.Description;

            sqlCommand.Parameters.Add("@params", SqlDbType.VarChar);
            sqlCommand.Parameters["@params"].Value = JsonConvert.SerializeObject(report.Params);

            sqlCommand.Parameters.Add("@filepath", SqlDbType.VarChar);
            sqlCommand.Parameters["@filepath"].Value = report.FilePath;
            
            result.DBID = sqlCommand.ExecuteScalar().ToString();

            return result;
        }

        public static ProjectBase PostProject(ProjectBase project)
        {
            var result = project;

            {
                string sqlCommandText = "INSERT INTO AUDIT.PROJECT (name, taxyear, projectidgextor, branchoffice, start_date, finish_date, baseline_start_date, baseline_finish_date, color, comments, start_date2, finish_date2, start_date3, finish_date3) VALUES " +
                    " (@name, @taxyear, @projectidgextor, @branchoffice, @start_date, @finish_date, @baseline_start_date, @baseline_finish_date, @color, @comments, @start_date2, @finish_date2, @start_date3, @finish_date3); " +
                    " SELECT SCOPE_IDENTITY(); ";

                SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

                sqlCommand.Parameters.Add("@name", SqlDbType.VarChar);
                sqlCommand.Parameters["@name"].Value = project.Name;

                sqlCommand.Parameters.Add("@taxyear", SqlDbType.VarChar);
                sqlCommand.Parameters["@taxyear"].Value = project.TaxYear;

                sqlCommand.Parameters.Add("@projectidgextor", SqlDbType.VarChar);
                sqlCommand.Parameters["@projectidgextor"].Value = project.IdGextor;

                sqlCommand.Parameters.Add("@branchoffice", SqlDbType.VarChar);
                sqlCommand.Parameters["@branchoffice"].Value = project.BranchOffice;


                sqlCommand.Parameters.Add("@start_date", SqlDbType.VarChar);
                sqlCommand.Parameters["@start_date"].Value = project.StartDate1.ToString("yyyyMMddHHmmss");

                sqlCommand.Parameters.Add("@finish_date", SqlDbType.VarChar);
                sqlCommand.Parameters["@finish_date"].Value = project.FinishDate1.ToString("yyyyMMddHHmmss");


                sqlCommand.Parameters.Add("@start_date2", SqlDbType.VarChar);
                sqlCommand.Parameters["@start_date2"].Value = project.StartDate2.ToDatabaseValue();                

                sqlCommand.Parameters.Add("@finish_date2", SqlDbType.VarChar);
                sqlCommand.Parameters["@finish_date2"].Value = project.FinishDate2.ToDatabaseValue();

                sqlCommand.Parameters.Add("@start_date3", SqlDbType.VarChar);
                sqlCommand.Parameters["@start_date3"].Value = project.StartDate3.ToDatabaseValue();

                sqlCommand.Parameters.Add("@finish_date3", SqlDbType.VarChar);
                sqlCommand.Parameters["@finish_date3"].Value = project.FinishDate3.ToDatabaseValue();


                sqlCommand.Parameters.Add("@baseline_start_date", SqlDbType.VarChar);
                sqlCommand.Parameters["@baseline_start_date"].Value = project.BaselineStartDate.ToString("yyyyMMddHHmmss");

                sqlCommand.Parameters.Add("@baseline_finish_date", SqlDbType.VarChar);
                sqlCommand.Parameters["@baseline_finish_date"].Value = project.BaselineFinishDate.ToString("yyyyMMddHHmmss");

                sqlCommand.Parameters.Add("@color", SqlDbType.VarChar);
                sqlCommand.Parameters["@color"].Value = project.Color;

                sqlCommand.Parameters.Add("@comments", SqlDbType.VarChar);
                sqlCommand.Parameters["@comments"].Value = project.Comments;

                result.DBID = sqlCommand.ExecuteScalar().ToString();
            }

            {
                if (project.IdGextor != "")
                {
                    var codProys = project.IdGextor.Split(',');
                    foreach (var codProy in codProys)
                    {
                        var gextorCodProy = Int64.Parse(codProy).ToString("D10");

                        string sqlCommandText = "INSERT INTO AUDIT.PROJECT_GEXTOR (projectid, projectidgextor) VALUES (@projectid, @projectidgextor) ";

                        SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

                        sqlCommand.Parameters.Add("@projectid", SqlDbType.Int);
                        sqlCommand.Parameters["@projectid"].Value = project.DBID;

                        sqlCommand.Parameters.Add("@projectidgextor", SqlDbType.VarChar);
                        sqlCommand.Parameters["@projectidgextor"].Value = gextorCodProy;

                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }

            return result;
        }

        public static BudgetBase PostBudget(BudgetBase budget)
        {
            var result = budget;

            string sqlCommandText = "INSERT INTO AUDIT.BUDGET (projectid, category, quantity) VALUES " +
                " (@projectid, @category, @quantity); " +
                " SELECT SCOPE_IDENTITY(); ";

            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@projectid", SqlDbType.VarChar);
            sqlCommand.Parameters["@projectid"].Value = budget.ProjectId;

            sqlCommand.Parameters.Add("@category", SqlDbType.VarChar);
            sqlCommand.Parameters["@category"].Value = budget.Category;

            sqlCommand.Parameters.Add("@quantity", SqlDbType.Int);
            sqlCommand.Parameters["@quantity"].Value = budget.Quantity;

            result.DBID = sqlCommand.ExecuteScalar().ToString();

            return result;
        }

        public static EmployeeBase PostEmployee(EmployeeBase employee)
        {
            var result = new EmployeeBase();

            string sqlCommandText = "INSERT INTO AUDIT.EMPLOYEE (name, category, branchoffice, leaving_date) VALUES " +
                " (@name, @category, @branchoffice, @leaving_date); " +
                " SELECT SCOPE_IDENTITY(); ";

            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@name", SqlDbType.VarChar);
            sqlCommand.Parameters["@name"].Value = employee.Name;

            sqlCommand.Parameters.Add("@category", SqlDbType.VarChar);
            sqlCommand.Parameters["@category"].Value = employee.Category;

            sqlCommand.Parameters.Add("@branchoffice", SqlDbType.VarChar);
            sqlCommand.Parameters["@branchoffice"].Value = employee.BranchOffice;

            /*
            sqlCommand.Parameters.Add("@status", SqlDbType.VarChar);
            sqlCommand.Parameters["@status"].Value = employee.Status;
            */

            sqlCommand.Parameters.Add("@leaving_date", SqlDbType.VarChar);
            if (employee.LeavingDate != null)
            {
                sqlCommand.Parameters["@leaving_date"].Value = employee.LeavingDate.Value.ToString("yyyyMMddHHmmss");
            }
            else
            {
                sqlCommand.Parameters["@leaving_date"].Value = DBNull.Value;
            }

            result.DBID = sqlCommand.ExecuteScalar().ToString();

            return result;
        }

        public static ProjectEmployeeAssocBase PostProjectEmployeeAssoc(ProjectEmployeeAssocBase projectEmployeeAssoc)
        {
            var result = new ProjectEmployeeAssocBase();

            string sqlCommandText = "INSERT INTO AUDIT.PROJECT_EMPLOYEE_ASSOC " +
                " (projectid, employeeid, role) VALUES " +
                " (@projectid, @employeeid, @role); " +
                " SELECT SCOPE_IDENTITY(); ";

            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@projectid", SqlDbType.Int);
            sqlCommand.Parameters["@projectid"].Value = projectEmployeeAssoc.ProjectId;

            sqlCommand.Parameters.Add("@employeeid", SqlDbType.Int);
            sqlCommand.Parameters["@employeeid"].Value = projectEmployeeAssoc.EmployeeId;

            sqlCommand.Parameters.Add("@role", SqlDbType.VarChar);
            sqlCommand.Parameters["@role"].Value = projectEmployeeAssoc.Role;

            result.DBID = sqlCommand.ExecuteScalar().ToString();

            return result;
        }

        public static ProjectEmployeeAllocBase PostProjectEmployeeAlloc(ProjectEmployeeAllocBase projectEmployeeAlloc)
        {
            var result = new ProjectEmployeeAllocBase();

            string sqlCommandText = "INSERT INTO AUDIT.PROJECT_EMPLOYEE_ALLOC " +
                " (projectid, employeeid, alloc_date, quantity) VALUES " +
                " (@projectid, @employeeid, @alloc_date, @quantity); " +
                " SELECT SCOPE_IDENTITY(); ";

            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@projectid", SqlDbType.Int);
            sqlCommand.Parameters["@projectid"].Value = projectEmployeeAlloc.ProjectId;

            sqlCommand.Parameters.Add("@employeeid", SqlDbType.Int);
            sqlCommand.Parameters["@employeeid"].Value = projectEmployeeAlloc.EmployeeId;

            sqlCommand.Parameters.Add("@alloc_date", SqlDbType.VarChar);
            sqlCommand.Parameters["@alloc_date"].Value = projectEmployeeAlloc.AllocDate.ToString("yyyyMMddHHmmss");

            sqlCommand.Parameters.Add("@quantity", SqlDbType.Int);
            sqlCommand.Parameters["@quantity"].Value = projectEmployeeAlloc.Quantity;

            result.DBID = sqlCommand.ExecuteScalar().ToString();

            return result;
        }

        #endregion

        #region PUT

        public static ProjectBase PutProject(ProjectBase project)
        {
            var result = project;
            {
                string sqlCommandText = "UPDATE AUDIT.PROJECT " +
                    " SET name = @name, " +
                    " taxyear = @taxyear, " +
                    " projectidgextor = @projectidgextor, " +
                    //" taxyear = @taxyear, " +
                    " branchoffice = @branchoffice, " +
                    " start_date = @start_date, finish_date = @finish_date, baseline_start_date = @baseline_start_date, baseline_finish_date = @baseline_finish_date, " +

                    " start_date2 = @start_date2, finish_date2 = @finish_date2, " +
                    " start_date3 = @start_date3, finish_date3 = @finish_date3, " +

                    " color = @color, " +
                    " comments = @comments " +
                    " WHERE projectid = @projectid";

                SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

                sqlCommand.Parameters.Add("@projectid", SqlDbType.Int);
                sqlCommand.Parameters["@projectid"].Value = project.DBID;

                sqlCommand.Parameters.Add("@name", SqlDbType.VarChar);
                sqlCommand.Parameters["@name"].Value = project.Name;

                sqlCommand.Parameters.Add("@taxyear", SqlDbType.VarChar);
                sqlCommand.Parameters["@taxyear"].Value = project.TaxYear;

                sqlCommand.Parameters.Add("@projectidgextor", SqlDbType.VarChar);
                sqlCommand.Parameters["@projectidgextor"].Value = project.IdGextor;

                sqlCommand.Parameters.Add("@branchoffice", SqlDbType.VarChar);
                sqlCommand.Parameters["@branchoffice"].Value = project.BranchOffice;


                sqlCommand.Parameters.Add("@start_date", SqlDbType.VarChar);
                sqlCommand.Parameters["@start_date"].Value = project.StartDate1.ToString("yyyyMMddHHmmss");

                sqlCommand.Parameters.Add("@finish_date", SqlDbType.VarChar);
                sqlCommand.Parameters["@finish_date"].Value = project.FinishDate1.ToString("yyyyMMddHHmmss");

                sqlCommand.Parameters.Add("@start_date2", SqlDbType.VarChar);
                sqlCommand.Parameters["@start_date2"].Value = project.StartDate2.ToDatabaseValue();

                sqlCommand.Parameters.Add("@finish_date2", SqlDbType.VarChar);
                sqlCommand.Parameters["@finish_date2"].Value = project.FinishDate2.ToDatabaseValue();

                sqlCommand.Parameters.Add("@start_date3", SqlDbType.VarChar);
                sqlCommand.Parameters["@start_date3"].Value = project.StartDate3.ToDatabaseValue();

                sqlCommand.Parameters.Add("@finish_date3", SqlDbType.VarChar);
                sqlCommand.Parameters["@finish_date3"].Value = project.FinishDate3.ToDatabaseValue();


                sqlCommand.Parameters.Add("@baseline_start_date", SqlDbType.VarChar);
                sqlCommand.Parameters["@baseline_start_date"].Value = project.BaselineStartDate.ToString("yyyyMMddHHmmss");

                sqlCommand.Parameters.Add("@baseline_finish_date", SqlDbType.VarChar);
                sqlCommand.Parameters["@baseline_finish_date"].Value = project.BaselineFinishDate.ToString("yyyyMMddHHmmss");

                sqlCommand.Parameters.Add("@color", SqlDbType.VarChar);
                sqlCommand.Parameters["@color"].Value = project.Color;

                sqlCommand.Parameters.Add("@comments", SqlDbType.VarChar);
                sqlCommand.Parameters["@comments"].Value = project.Comments;

                sqlCommand.ExecuteNonQuery();
            }

            {
                string sqlCommandText = "DELETE AUDIT.PROJECT_GEXTOR " +                    
                    " WHERE projectid = @projectid";

                SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

                sqlCommand.Parameters.Add("@projectid", SqlDbType.Int);
                sqlCommand.Parameters["@projectid"].Value = project.DBID;
                
                sqlCommand.ExecuteNonQuery();
            }

            {
                if (project.IdGextor != "")
                {
                    var codProys = project.IdGextor.Split(',');
                    foreach (var codProy in codProys)
                    {
                        var gextorCodProy = Int64.Parse(codProy).ToString("D10");

                        string sqlCommandText = "INSERT INTO AUDIT.PROJECT_GEXTOR (projectid, projectidgextor) VALUES (@projectid, @projectidgextor) ";
                            
                        SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

                        sqlCommand.Parameters.Add("@projectid", SqlDbType.Int);
                        sqlCommand.Parameters["@projectid"].Value = project.DBID;

                        sqlCommand.Parameters.Add("@projectidgextor", SqlDbType.VarChar);
                        sqlCommand.Parameters["@projectidgextor"].Value = gextorCodProy;

                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }

            return result;

        }

        public static BudgetBase PutBudget(BudgetBase budget)
        {
            var result = budget;

            string sqlCommandText = "UPDATE AUDIT.BUDGET " +
                " SET projectid = @projectid, category = @category, quantity = @quantity " +
                " WHERE budgetid = @budgetid";

            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@budgetid", SqlDbType.Int);
            sqlCommand.Parameters["@budgetid"].Value = budget.DBID;

            sqlCommand.Parameters.Add("@projectid", SqlDbType.VarChar);
            sqlCommand.Parameters["@projectid"].Value = budget.ProjectId;

            sqlCommand.Parameters.Add("@category", SqlDbType.VarChar);
            sqlCommand.Parameters["@category"].Value = budget.Category;

            sqlCommand.Parameters.Add("@quantity", SqlDbType.Int);
            sqlCommand.Parameters["@quantity"].Value = budget.Quantity;
            
            sqlCommand.ExecuteNonQuery();

            return result;
        }

        public static EmployeeBase PutEmployee(EmployeeBase employee)
        {
            var result = employee;

            string sqlCommandText = "UPDATE AUDIT.EMPLOYEE " +
                " SET name = @name, category = @category, branchoffice = @branchoffice, leaving_date = @leaving_date " +
                " WHERE employeeid = @employeeid";

            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@employeeid", SqlDbType.Int);
            sqlCommand.Parameters["@employeeid"].Value = employee.DBID;

            sqlCommand.Parameters.Add("@name", SqlDbType.VarChar);
            sqlCommand.Parameters["@name"].Value = employee.Name;

            sqlCommand.Parameters.Add("@category", SqlDbType.VarChar);
            sqlCommand.Parameters["@category"].Value = employee.Category;

            sqlCommand.Parameters.Add("@branchoffice", SqlDbType.VarChar);
            sqlCommand.Parameters["@branchoffice"].Value = employee.BranchOffice;

            /*
            sqlCommand.Parameters.Add("@status", SqlDbType.VarChar);
            sqlCommand.Parameters["@status"].Value = employee.Status;
            */

            sqlCommand.Parameters.Add("@leaving_date", SqlDbType.VarChar);
            if (employee.LeavingDate != null)
            {
                sqlCommand.Parameters["@leaving_date"].Value = employee.LeavingDate.Value.ToString("yyyyMMddHHmmss");
            }
            else
            {
                sqlCommand.Parameters["@leaving_date"].Value = DBNull.Value;
            }

            sqlCommand.ExecuteNonQuery();

            return result;
        }

        public static ProjectEmployeeAssocBase PutProjectEmployeeAssoc(ProjectEmployeeAssocBase projectEmployeeAssoc)
        {
            var result = projectEmployeeAssoc;

            string sqlCommandText = "UPDATE AUDIT.PROJECT_EMPLOYEE_ASSOC " +
                " SET projectid = @projectid, employeeid = @employeeid, role = @role " +
                " WHERE projectemployeeassocid = @projectemployeeassocid";
            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@projectemployeeassocid", SqlDbType.Int);
            sqlCommand.Parameters["@projectemployeeassocid"].Value = projectEmployeeAssoc.DBID;

            sqlCommand.Parameters.Add("@projectid", SqlDbType.VarChar);
            sqlCommand.Parameters["@projectid"].Value = projectEmployeeAssoc.ProjectId;

            sqlCommand.Parameters.Add("@employeeid", SqlDbType.VarChar);
            sqlCommand.Parameters["@employeeid"].Value = projectEmployeeAssoc.EmployeeId;

            sqlCommand.Parameters.Add("@role", SqlDbType.VarChar);
            sqlCommand.Parameters["@role"].Value = projectEmployeeAssoc.Role;

            sqlCommand.ExecuteNonQuery();

            return result;
        }

        public static ProjectEmployeeAllocBase PutProjectEmployeeAlloc(ProjectEmployeeAllocBase projectEmployeeAllocBase)
        {
            var result = projectEmployeeAllocBase;

            string sqlCommandText = "UPDATE AUDIT.PROJECT_EMPLOYEE_ALLOC " +
                " SET projectid = @projectid, employeeid = @employeeid, alloc_date = @alloc_date, quantity = @quantity " +
                " WHERE projectemployeeallocid = @projectemployeeallocid";
            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@projectemployeeallocid", SqlDbType.Int);
            sqlCommand.Parameters["@projectemployeeallocid"].Value = projectEmployeeAllocBase.DBID;

            sqlCommand.Parameters.Add("@projectid", SqlDbType.VarChar);
            sqlCommand.Parameters["@projectid"].Value = projectEmployeeAllocBase.ProjectId;

            sqlCommand.Parameters.Add("@employeeid", SqlDbType.VarChar);
            sqlCommand.Parameters["@employeeid"].Value = projectEmployeeAllocBase.EmployeeId;

            sqlCommand.Parameters.Add("@alloc_date", SqlDbType.VarChar);
            sqlCommand.Parameters["@alloc_date"].Value = projectEmployeeAllocBase.AllocDate.ToString("yyyyMMddHHmmss");

            sqlCommand.Parameters.Add("@quantity", SqlDbType.Int);
            sqlCommand.Parameters["@quantity"].Value = projectEmployeeAllocBase.Quantity;

            sqlCommand.ExecuteNonQuery();

            return result;
        }

        #endregion

        #region DELETE

        public static BudgetBase DeleteBudget(BudgetBase budget)
        {
            var result = budget;

            string sqlCommandText = "DELETE AUDIT.BUDGET " +
                " WHERE budgetid = @budgetid";
            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@budgetid", SqlDbType.Int);
            sqlCommand.Parameters["@budgetid"].Value = budget.DBID;

            sqlCommand.ExecuteNonQuery();

            return result;
        }

        public static ProjectEmployeeAssocBase DeleteProjectEmployeeAssoc(ProjectEmployeeAssocBase projectEmployeeAssoc)
        {
            var result = projectEmployeeAssoc;

            {
                string sqlCommandText = "DELETE AUDIT.PROJECT_EMPLOYEE_ALLOC " +
                    " WHERE projectid = @projectid AND employeeid = @employeeid";
                SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

                sqlCommand.Parameters.Add("@projectid", SqlDbType.Int);
                sqlCommand.Parameters["@projectid"].Value = projectEmployeeAssoc.ProjectId;

                sqlCommand.Parameters.Add("@employeeid", SqlDbType.Int);
                sqlCommand.Parameters["@employeeid"].Value = projectEmployeeAssoc.EmployeeId;

                sqlCommand.ExecuteNonQuery();
            }

            {
                string sqlCommandText = "DELETE AUDIT.PROJECT_EMPLOYEE_ASSOC " +
                    " WHERE projectemployeeassocid = @projectemployeeassocid";
                SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

                sqlCommand.Parameters.Add("@projectemployeeassocid", SqlDbType.Int);
                sqlCommand.Parameters["@projectemployeeassocid"].Value = projectEmployeeAssoc.DBID;

                sqlCommand.ExecuteNonQuery();
            }

            return result;
        }

        public static ProjectEmployeeAllocBase DeleteProjectEmployeeAlloc(ProjectEmployeeAllocBase projectEmployeeAllocBase)
        {
            var result = projectEmployeeAllocBase;

            string sqlCommandText = "DELETE AUDIT.PROJECT_EMPLOYEE_ALLOC " +
                " WHERE projectemployeeallocid = @projectemployeeallocid";
            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Database.Connection);

            sqlCommand.Parameters.Add("@projectemployeeallocid", SqlDbType.Int);
            sqlCommand.Parameters["@projectemployeeallocid"].Value = projectEmployeeAllocBase.DBID;

            sqlCommand.ExecuteNonQuery();

            return result;
        }

        #endregion
    }
}