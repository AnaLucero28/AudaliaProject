using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Audalia.DataHUBClient;
using Audalia.DataHUBCommon;

namespace Audalia.DataHUBClient
{
    public enum UserPermissions { None = 0, Read = 1, Write = 2, Grant = 3 };

    public class Data
    {
        static AuditData auditData;

        public static AuditData Audit
        {
            get
            {
                if (auditData == null)
                    auditData = new AuditData();

                return auditData; 
            }
        }
    }

    public class AuditData
    {
        public event DataHubObjectMessageEventHandler OnObjectMessage;

        UserPermissions userPermissions = UserPermissions.None;
        Dictionary<string, string> appUserPermissions;

        Dictionary<string, Project> projects;
        Dictionary<string, Employee> employees;
        Dictionary<string, ProjectEmployeeAssoc> projectEmployeeAssocs;
        Dictionary<string, ProjectEmployeeAlloc> projectEmployeeAllocs;
        List<HolidayBase> holidays;

        public Dictionary<string, Project> Projects { get => projects; set => projects = value; }


        public UserPermissions UserPermissions
        {
            get
            {
                //Test ->
                //userPermissions = UserPermissions.Write;

                if (userPermissions == UserPermissions.None)
                {                    
                    switch (DataHUBClient.ServiceContract.GetPermissions())
                    {
                        case "G":
                            userPermissions = UserPermissions.Grant;
                            break;
                        case "W":
                            userPermissions = UserPermissions.Write;
                            break;
                        default:
                            userPermissions = UserPermissions.Read;
                            break;
                    }                    
                }
             
                return userPermissions;
            }

        }

        public Dictionary<string, string> AppUserPermissions
        {
            get
            {                

                if (appUserPermissions == null)
                {
                    appUserPermissions = DataHUBClient.ServiceContract.GetAppUserPermissions();                    
                }

                return appUserPermissions;
            }
        }
        
        public Employee Me
        {
            get
            {
                string userName = @"audalia\" + Environment.UserName.ToLower();
                
                if (userName == @"audalia\jmvarona")
                    userName = @"audalia\a.alioe";
                    //userName = @"audalia\ncontreras";

                return Data.Audit.Employees.Values.Where(p => p.WindowsUser != null && p.WindowsUser.ToLower() == userName).FirstOrDefault();             
            }
        }

        public Dictionary<string, Employee> Employees { get => employees; set => employees = value; }

        public Dictionary<string, ProjectEmployeeAssoc> ProjectEmployeeAssocs { get => projectEmployeeAssocs; set => projectEmployeeAssocs = value; }

        public Dictionary<string, ProjectEmployeeAlloc> ProjectEmployeeAllocs { get => projectEmployeeAllocs; set => projectEmployeeAllocs = value; }

        public AuditData()
        {
            projects = new Dictionary<string, Project>();
            employees = new Dictionary<string, Employee>();
            projectEmployeeAssocs = new Dictionary<string, ProjectEmployeeAssoc>();

            DataHUBClient.OnObjectMessage += DataHUBClient_OnObjectMessage;
            DataHUBClient.OnObjectActionMessageList += DataHUBClient_OnObjectActionMessageList;
        }

        private void DataHUBClient_OnObjectActionMessageList(List<ObjectActionMessage> objectActionMessageList)
        {
            DataHUBUtilities.Synchronize<bool>(delegate
            {
                foreach (var message in objectActionMessageList)
                {
                    ProcessObjectMessage(message.ActionType, message.ObjectType, message.ObjectId, false, true);
                }

                if (objectActionMessageList.Exists(p => p.ObjectType == ObjectMessageObjectType.ProjectEmployeeAlloc))
                {
                    CheckOverlappedTasks();
                }
                
                return true;
            });

        }

        public string GetSetting(string param)
        {
            return DataHUBClient.ServiceContract.GetSetting(param);
        }

        public string PostSetting(string param, string value)
        {
            return DataHUBClient.ServiceContract.PostSetting(param, value);
        }

        public List<BudgetBase> GetBudget(string projectId)
        {
            return DataHUBClient.ServiceContract.GetBudget(projectId);
        }

        public BudgetBase PostBudget(BudgetBase budget)
        {
            return DataHUBClient.ServiceContract.PostBudget(budget);
        }

        public BudgetBase PutBudget(BudgetBase budget)
        {
            return DataHUBClient.ServiceContract.PutBudget(budget);
        }

        public BudgetBase DeleteBudget(BudgetBase budget)
        {
            return DataHUBClient.ServiceContract.DeleteBudget(budget);
        }

        public List<ReportBase> GetReports()
        {
            return DataHUBClient.ServiceContract.GetReports();
        }

        private void ProcessObjectMessage(ObjectMessageActionType objectMessageActionType, ObjectMessageObjectType objectMessageObjectType, string objectId, bool checkOverlappedTasks, bool invokeObjectMessage)
        {
            //Update data

            switch (objectMessageObjectType)
            {
                case ObjectMessageObjectType.Project:
                    {
                        var projectbase = DataHUBClient.ServiceContract.GetProject(objectId);

                        if (projects.ContainsKey(objectId))
                        {
                            var project = projects[objectId];

                            project.Name = projectbase.Name;

                            project.StartDate1 = projectbase.StartDate1;
                            project.FinishDate1 = projectbase.FinishDate1;
                            project.StartDate2 = projectbase.StartDate2;
                            project.FinishDate2 = projectbase.FinishDate2;
                            project.StartDate3 = projectbase.StartDate3;
                            project.FinishDate3 = projectbase.FinishDate3;

                            project.BaselineStartDate = projectbase.BaselineStartDate;
                            project.BaselineFinishDate = projectbase.BaselineFinishDate;
                            project.Color = projectbase.Color;
                            project.Comments = projectbase.Comments;
                            project.IsModified = false;
                        }
                        else
                        {
                            var project = new Project()
                            {
                                DBID = projectbase.DBID,
                                Name = projectbase.Name,

                                StartDate1 = projectbase.StartDate1,
                                FinishDate1 = projectbase.FinishDate1,
                                StartDate2 = projectbase.StartDate2,
                                FinishDate2 = projectbase.FinishDate2,
                                StartDate3 = projectbase.StartDate3,
                                FinishDate3 = projectbase.FinishDate3,

                                BaselineStartDate = projectbase.BaselineStartDate,
                                BaselineFinishDate = projectbase.BaselineFinishDate,
                                Color = projectbase.Color,
                                Comments = projectbase.Comments,
                                IsModified = false
                            };

                            projects.Add(project.DBID, project);
                        }

                        break;
                    }

                case ObjectMessageObjectType.Employee:
                    {
                        var employeebase = DataHUBClient.ServiceContract.GetEmployee(objectId);

                        if (employees.ContainsKey(objectId))
                        {
                            var employee = employees[objectId];

                            employee.Name = employeebase.Name;
                            employee.Category = employeebase.Category;
                            employee.WindowsUser = employeebase.WindowsUser;
                            employee.Permissions = employeebase.Permissions;
                            employee.LeavingDate = employeebase.LeavingDate;
                            employee.IsModified = false;
                        }
                        else
                        {
                            var employee = new Employee()
                            {
                                DBID = employeebase.DBID,
                                Name = employeebase.Name,
                                Category = employeebase.Category,
                                WindowsUser = employeebase.WindowsUser,
                                //Status = employeebase.Status,
                                Permissions = employeebase.Permissions,
                                LeavingDate = employeebase.LeavingDate,
                                IsModified = false
                            };

                            employees.Add(employee.DBID, employee);
                        }

                        break;
                    }

                case ObjectMessageObjectType.ProjectEmployeeAssoc:
                    {

                        if (objectMessageActionType == ObjectMessageActionType.Delete)
                        {
                            //TODO: borrar o marcar como borrado ¿?
                            //projectEmployeeAssocs.Remove(objectId);                                                                                                
                        }
                        else
                        {
                            var projectEmployeeAssocBase = DataHUBClient.ServiceContract.GetProjectEmployeeAssoc(objectId);

                            if (projectEmployeeAssocs.ContainsKey(objectId))
                            {
                                var projectEmployeeAssoc = projectEmployeeAssocs[objectId];

                                projectEmployeeAssoc.ProjectId = projectEmployeeAssocBase.ProjectId;
                                projectEmployeeAssoc.EmployeeId = projectEmployeeAssocBase.EmployeeId;
                                projectEmployeeAssoc.Role = projectEmployeeAssocBase.Role;
                            }
                            else
                            {
                                var projectEmployeeAssoc = new ProjectEmployeeAssoc()
                                {
                                    DBID = objectId,
                                    ProjectId = projectEmployeeAssocBase.ProjectId,
                                    EmployeeId = projectEmployeeAssocBase.EmployeeId,
                                    Role = projectEmployeeAssocBase.Role
                                };

                                projectEmployeeAssocs.Add(projectEmployeeAssoc.DBID, projectEmployeeAssoc);
                            }
                        }

                        break;
                    }

                case ObjectMessageObjectType.ProjectEmployeeAlloc:
                    {

                        if (objectMessageActionType == ObjectMessageActionType.Delete)
                        {
                            //projectEmployeeAllocs.Remove(objectId);
                        }
                        else
                        {
                            var projectEmployeeAllocBase = DataHUBClient.ServiceContract.GetProjectEmployeeAlloc(objectId);

                            if (projectEmployeeAllocs.ContainsKey(objectId))
                            {
                                var projectEmployeeAlloc = projectEmployeeAllocs[objectId];

                                projectEmployeeAlloc.ProjectId = projectEmployeeAllocBase.ProjectId;
                                projectEmployeeAlloc.EmployeeId = projectEmployeeAllocBase.EmployeeId;
                                projectEmployeeAlloc.AllocDate = projectEmployeeAllocBase.AllocDate;
                                projectEmployeeAlloc.Quantity = projectEmployeeAllocBase.Quantity;
                            }
                            else
                            {
                                var projectEmployeeAlloc = new ProjectEmployeeAlloc()
                                {
                                    DBID = objectId,
                                    ProjectId = projectEmployeeAllocBase.ProjectId,
                                    EmployeeId = projectEmployeeAllocBase.EmployeeId,
                                    AllocDate = projectEmployeeAllocBase.AllocDate,
                                    Quantity = projectEmployeeAllocBase.Quantity
                                };

                                projectEmployeeAllocs.Add(projectEmployeeAlloc.DBID, projectEmployeeAlloc);
                            }
                        }

                        if (checkOverlappedTasks)
                            CheckOverlappedTasks();

                        break;

                    }
            }

            //Delete objects

            switch (objectMessageObjectType)
            {
                case ObjectMessageObjectType.ProjectEmployeeAssoc:
                    {
                        if (objectMessageActionType == ObjectMessageActionType.Delete)
                        {
                            var projectEmployeeAssoc = projectEmployeeAssocs[objectId];

                            var allocsToRemove = new List<string>();

                            foreach (var alloc in projectEmployeeAllocs.Where(p => p.Value.ProjectId == projectEmployeeAssoc.ProjectId && p.Value.EmployeeId == projectEmployeeAssoc.EmployeeId))
                            {
                                allocsToRemove.Add(alloc.Key);
                            }

                            foreach (var allocKey in allocsToRemove)
                            {
                                projectEmployeeAllocs.Remove(allocKey);
                            }

                            projectEmployeeAssocs.Remove(objectId);

                        }

                        break;
                    }

                case ObjectMessageObjectType.ProjectEmployeeAlloc:
                    {
                        if (objectMessageActionType == ObjectMessageActionType.Delete)
                        {
                            projectEmployeeAllocs.Remove(objectId);
                        }

                        break;
                    }
            }

            //
            
            if (invokeObjectMessage)
            {
                try
                {
                    OnObjectMessage?.Invoke(objectMessageActionType, objectMessageObjectType, objectId);
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }            
        }

        private void DataHUBClient_OnObjectMessage(ObjectMessageActionType objectMessageActionType, ObjectMessageObjectType objectMessageObjectType, string objectId)
        {
            DataHUBUtilities.Synchronize<bool>(delegate
            {
                ProcessObjectMessage(objectMessageActionType, objectMessageObjectType, objectId, true, true);
                return true;
            });

        }
        
        public void ReadDB()
        {
            projects = new Dictionary<string, Project>();
            employees = new Dictionary<string, Employee>();
            projectEmployeeAssocs = new Dictionary<string, ProjectEmployeeAssoc>();
            projectEmployeeAllocs = new Dictionary<string, ProjectEmployeeAlloc>();

            //Proyectos

            foreach (var project in DataHUBClient.ServiceContract.GetProjects())
            {
                projects.Add(project.DBID, new Project()
                {
                    DBID = project.DBID,
                    Name = project.Name,
                    BranchOffice = project.BranchOffice,
                    TaxYear = project.TaxYear,
                    IdGextor = project.IdGextor,

                    StartDate1 = project.StartDate1,
                    FinishDate1 = project.FinishDate1,
                    StartDate2 = project.StartDate2,
                    FinishDate2 = project.FinishDate2,
                    StartDate3 = project.StartDate3,
                    FinishDate3 = project.FinishDate3,

                    BaselineStartDate = project.BaselineStartDate,
                    BaselineFinishDate = project.BaselineFinishDate,
                    Color = project.Color,
                    Comments = project.Comments,
                    IsModified = false
                });
            }

            //Empleados

            foreach (var employee in DataHUBClient.ServiceContract.GetEmployees())
            {
                employees.Add(employee.DBID, new Employee()
                {
                    DBID = employee.DBID,
                    Name = employee.Name,
                    BranchOffice = employee.BranchOffice,
                    Category = employee.Category,
                    //Status = employee.Status,                    
                    Permissions = employee.Permissions,
                    LeavingDate = employee.LeavingDate,
                    WindowsUser = employee.WindowsUser,
                    IsModified = false
                });
            }

            //Proyecto/Empleado

            foreach (var assoc in DataHUBClient.ServiceContract.GetProjectEmployeeAssocs())
            {
                projectEmployeeAssocs.Add(assoc.DBID, new ProjectEmployeeAssoc()
                {
                    DBID = assoc.DBID,
                    ProjectId = assoc.ProjectId,
                    EmployeeId = assoc.EmployeeId,
                    Role = assoc.Role,
                    IsModified = false
                });
            }

            foreach (var alloc in DataHUBClient.ServiceContract.GetProjectEmployeeAllocs())
            {
                projectEmployeeAllocs.Add(alloc.DBID, new ProjectEmployeeAlloc()
                {
                    DBID = alloc.DBID,
                    ProjectId = alloc.ProjectId,
                    EmployeeId = alloc.EmployeeId,
                    AllocDate = alloc.AllocDate,
                    Quantity = alloc.Quantity,
                    IsModified = false
                });
            }

            CheckOverlappedTasks();

            /*
            //Test -->
            List<ObjectActionMessage> actions = new List<ObjectActionMessage>();
            actions.Add(new ObjectActionMessage() { ActionType = ObjectMessageActionType.Put, ObjectType = ObjectMessageObjectType.Project, ObjectId = "1" });
            actions.Add(new ObjectActionMessage() { ActionType = ObjectMessageActionType.Put, ObjectType = ObjectMessageObjectType.Project, ObjectId = "2" });            
            var test = DataHUBClient.ServiceContract.Test(actions);
            test.ToString();
            // <--
            */
        }
     
        void CheckOverlappedTasks()
        {
            var array = projectEmployeeAllocs.Values.ToArray();

            for (int i = 0; i < array.Length; i++)
            {
                var alloc1 = array[i];
                alloc1.IsOverlapped = false;
            }

            for (int i = 0; i < array.Length; i++)
            {
                var alloc1 = array[i];
                
                for (int j = i + 1; j < array.Length; j++)
                {
                    var alloc2 = array[j];
                    if (alloc2.IsOverlapped)
                        continue;
                    
                    if (alloc1.EmployeeId == alloc2.EmployeeId && alloc1.AllocDate == alloc2.AllocDate)
                    {
                        alloc1.IsOverlapped = true;
                        alloc2.IsOverlapped = true;
                        break;
                    }
                }                
            }

            /*
            foreach (var alloc in projectEmployeeAllocs)
            {
                alloc.Value.IsOverlapped = projectEmployeeAllocs.Values.Where(p => p.DBID != alloc.Value.DBID && p.EmployeeId == alloc.Value.EmployeeId && p.AllocDate == alloc.Value.AllocDate).Count() > 0;
            }
            */
        }

        public void SaveDB()
        {
            foreach (var project in projects.Values)
            {
                if (project.DBID == null)
                {
                    DataHUBClient.ServiceContract.PostProject(project);
                }
                else
                    if (project.IsModified)
                {
                    DataHUBClient.ServiceContract.PutProject(project);
                }
            }

            foreach (var assoc in projectEmployeeAssocs.Values)
            {
                if (assoc.DBID == null)
                {
                    DataHUBClient.ServiceContract.PostProjectEmployeeAssoc(assoc);
                }
                else
                    if (assoc.IsModified)
                {
                    DataHUBClient.ServiceContract.PutProjectEmployeeAssoc(assoc);
                }
            }

            foreach (var alloc in projectEmployeeAllocs.Values)
            {
                if (alloc.DBID == null)
                {
                    DataHUBClient.ServiceContract.PostProjectEmployeeAlloc(alloc);
                }
                else
                    if (alloc.IsModified)
                {
                    DataHUBClient.ServiceContract.PutProjectEmployeeAlloc(alloc);
                }
            }
        }

    public List<HolidayBase> Holidays
        {
            get
            {
                if (holidays == null)
                {
                    holidays = new List<HolidayBase>();

                    foreach (var holiday in DataHUBClient.ServiceContract.GetHolidays(DateTime.Now.Year))                    
                    {
                        if (holiday.Year == 0)
                        {
                            holidays.Add(new HolidayBase() { DBID = holiday.DBID, Name = holiday.Name, Year = DateTime.Now.Year - 1, Month = holiday.Month, Day = holiday.Day });
                            holidays.Add(new HolidayBase() { DBID = holiday.DBID, Name = holiday.Name, Year = DateTime.Now.Year, Month = holiday.Month, Day = holiday.Day });
                            holidays.Add(new HolidayBase() { DBID = holiday.DBID, Name = holiday.Name, Year = DateTime.Now.Year + 1, Month = holiday.Month, Day = holiday.Day });
                        }
                        else
                        {
                            holidays.Add(new HolidayBase() { DBID = holiday.DBID, Name = holiday.Name, Year = holiday.Year, Month = holiday.Month, Day = holiday.Day });
                        }
                    }

                    //holidays = DataHUBClient.ServiceContract.GetHolidays(DateTime.Now.Year);
                }
                return holidays;
            }
        }
    }   

    [DataContract(Name = "ProjectBase", Namespace = "Audalia.DataHUBCommon")]
    public class Project: ProjectBase
    {
        public bool IsModified { get; set; }
        
        public IReadOnlyList<ProjectEmployeeAssoc> Employees
        {
            get
            {
                //return AuditData.ProjectEmployeeAssocs.Values.Where(p => p.ProjectId == this.DBID && !p.IsDeleted).ToList();
                return Data.Audit.ProjectEmployeeAssocs.Values.Where(p => p.ProjectId == this.DBID).ToList();
            }

        }        
        public override string Name 
        { 
            get 
            {
                return base.Name;
            }
            set
            {
                if (base.Name != value)
                {
                    base.Name = value;
                    this.IsModified = true;
                }
            }
        }
        public override string BranchOffice
        {
            get
            {
                return base.BranchOffice;
            }
            set
            {
                if (base.BranchOffice != value)
                {
                    base.BranchOffice = value;
                    this.IsModified = true;
                }
            }
        }

        public override string TaxYear
        {
            get
            {
                return base.TaxYear;
            }
            set
            {
                if (base.TaxYear != value)
                {
                    base.TaxYear = value;
                    this.IsModified = true;
                }
            }
        }
        public override string IdGextor
        {
            get
            {
                return base.IdGextor;
            }
            set
            {
                if (base.IdGextor != value)
                {
                    base.IdGextor = value;
                    this.IsModified = true;
                }
            }
        }



        public DateTime StartDate
        {
            get
            {
                return base.StartDate1;
            }
            /*
            set
            {
                if (base.StartDate1 != value)
                {
                    base.StartDate1 = value;
                    this.IsModified = true;
                }
            }
            */
        }
        public DateTime FinishDate
        {
            get
            {
                return base.FinishDate3 ?? base.FinishDate2 ?? base.FinishDate1;
                /*
                DateTime result = base.FinishDate1;
                if (base.FinishDate3 != null)
                    result = (DateTime)base.FinishDate3;
                else
                    if (base.FinishDate2 != null)
                    result = (DateTime)base.FinishDate2;

                return result;
                */
            }

            /*
            set
            {
                if (base.FinishDate1 != value)
                {
                    base.FinishDate1 = value;
                    this.IsModified = true;
                }
            }
            */
        }




        public override DateTime StartDate1
        {
            get
            {
                return base.StartDate1;
            }
            set
            {
                if (base.StartDate1 != value)
                {
                    base.StartDate1 = value;
                    this.IsModified = true;
                }
            }
        }
        public override DateTime FinishDate1
        {
            get
            {
                return base.FinishDate1;
            }
            set
            {
                if (base.FinishDate1 != value)
                {
                    base.FinishDate1 = value;
                    this.IsModified = true;
                }
            }
        }

        public override DateTime? StartDate2
        {
            get
            {
                return base.StartDate2;
            }
            set
            {
                if (base.StartDate2 != value)
                {
                    base.StartDate2 = value;
                    this.IsModified = true;
                }
            }
        }
        public override DateTime? FinishDate2
        {
            get
            {
                return base.FinishDate2;
            }
            set
            {
                if (base.FinishDate2 != value)
                {
                    base.FinishDate2 = value;
                    this.IsModified = true;
                }
            }
        }

        public override DateTime? StartDate3
        {
            get
            {
                return base.StartDate3;
            }
            set
            {
                if (base.StartDate3 != value)
                {
                    base.StartDate3 = value;
                    this.IsModified = true;
                }
            }
        }
        public override DateTime? FinishDate3
        {
            get
            {
                return base.FinishDate3;
            }
            set
            {
                if (base.FinishDate3 != value)
                {
                    base.FinishDate3 = value;
                    this.IsModified = true;
                }
            }
        }



        public override DateTime BaselineStartDate
        {
            get
            {
                return base.BaselineStartDate;
            }
            set
            {
                if (base.BaselineStartDate != value)
                {
                    base.BaselineStartDate = value;
                    this.IsModified = true;
                }
            }
        }
        public override DateTime BaselineFinishDate
        {
            get
            {
                return base.BaselineFinishDate;
            }
            set
            {
                if (base.BaselineFinishDate != value)
                {
                    base.BaselineFinishDate = value;
                    this.IsModified = true;
                }
            }
        }

        public override string Color
        {
            get
            {
                return base.Color;
            }
            set
            {
                if (base.Color != value)
                {
                    base.Color = value;
                    this.IsModified = true;
                }
            }
        }
        public override string Comments
        {
            get
            {
                return base.Comments;
            }
            set
            {
                if (base.Comments != value)
                {
                    base.Comments = value;
                    this.IsModified = true;
                }
            }
        }
    }

    [DataContract(Name = "EmployeeBase", Namespace = "Audalia.DataHUBCommon")]
    public class Employee: EmployeeBase
    {
        public override string BranchOffice
        {
            get
            {
                return base.BranchOffice;
            }
            set
            {
                if (base.BranchOffice != value)
                {
                    base.BranchOffice = value;
                    this.IsModified = true;
                }
            }
        }

        public bool IsModified { get; set; }

        public IReadOnlyList<ProjectEmployeeAssoc> Projects
        {
            get
            {
                //return AuditData.ProjectEmployeeAssocs.Values.Where(p => p.EmployeeId == this.DBID && !p.IsDeleted).ToList();
                return Data.Audit.ProjectEmployeeAssocs.Values.Where(p => p.EmployeeId == this.DBID).ToList();
            }
        }
    }

    [DataContract(Name = "ProjectEmployeeAssocBase", Namespace = "Audalia.DataHUBCommon")]
    public class ProjectEmployeeAssoc : ProjectEmployeeAssocBase
    {
        public bool IsModified { get; set; }
        //public bool IsDeleted { get; set; }

        public Project Project
        {
            get
            {
                return Data.Audit.Projects[this.ProjectId];
            }

            set
            {
                this.ProjectId = value.DBID;
            }
        }

        public Employee Employee
        {
            get
            {
                return Data.Audit.Employees[this.EmployeeId];
            }

            set
            {
                this.EmployeeId = value.DBID;
            }
        }

        public override string ProjectId
        {
            get
            {
                return base.ProjectId;
            }
            set
            {
                if (base.ProjectId != value)
                {
                    base.ProjectId = value;
                    this.IsModified = true;
                }
            }
        }

        public override string EmployeeId
        {
            get
            {
                return base.EmployeeId;
            }
            set
            {
                if (base.EmployeeId != value)
                {
                    base.EmployeeId = value;
                    this.IsModified = true;
                }
            }
        }

        public override string Role
        {
            get
            {
                return base.Role;
            }
            set
            {
                if (base.Role != value)
                {
                    base.Role = value;
                    this.IsModified = true;
                }
            }
        }

    }

    [DataContract(Name = "ProjectEmployeeAllocBase", Namespace = "Audalia.DataHUBCommon")]
    public class ProjectEmployeeAlloc : ProjectEmployeeAllocBase
    {
        public bool IsModified { get; set; }
        
        public Project Project
        {
            get
            {
                return Data.Audit.Projects[this.ProjectId];
            }

            set
            {
                this.ProjectId = value.DBID;
            }
        }

        public Employee Employee
        {
            get
            {
                return Data.Audit.Employees[this.EmployeeId];
            }

            set
            {
                this.EmployeeId = value.DBID;
            }
        }

        public override string ProjectId
        {
            get
            {
                return base.ProjectId;
            }
            set
            {
                if (base.ProjectId != value)
                {
                    base.ProjectId = value;
                    this.IsModified = true;
                }
            }
        }

        public override string EmployeeId
        {
            get
            {
                return base.EmployeeId;
            }
            set
            {
                if (base.EmployeeId != value)
                {
                    base.EmployeeId = value;
                    this.IsModified = true;
                }
            }
        }

        public override DateTime AllocDate
        {
            get
            {
                return base.AllocDate;
            }
            set
            {
                if (base.AllocDate != value)
                {
                    base.AllocDate = value;
                    this.IsModified = true;
                }
            }
        }

        public bool IsOverlapped { get; set; }
    }

}
