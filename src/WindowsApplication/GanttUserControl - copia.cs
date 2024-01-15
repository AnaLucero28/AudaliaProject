using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Xml.Serialization;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGantt.Base.Scheduling;
using DevExpress.XtraGantt.Exceptions;
using DevExpress.XtraGantt.Options;
using DevExpress.XtraGantt.Scheduling;
using DevExpress.XtraGantt;
using System.Data.SqlClient;
using System.Globalization;

namespace WindowsApplication
{
    public partial class GanttUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        List<AuditProject> projects;
        List<AuditEmployee> employees;
        List<AuditTask> tasks;

        bool groupByProject = false;
        bool arrangeEmployees = true;

        public GanttUserControl()
        {
            InitializeComponent();

            // <ganttControl1>
            ganttControl.OptionsBehavior.ScheduleMode = ScheduleMode.Manual;
            ganttControl.TreeListMappings.ParentFieldName = "ParentUID";
            ganttControl.TreeListMappings.KeyFieldName = "UID";
            ganttControl.ChartMappings.TextFieldName = "Resources";
            ganttControl.ChartMappings.InteractionTooltipTextFieldName = "Name";
            ganttControl.ChartMappings.DurationFieldName = "Duration";

            Init();
            


            projects = new List<AuditProject>();
            employees = new List<AuditEmployee>();
            tasks = new List<AuditTask>();
            ReadDB();
            LoadGantt();

            ganttControl.ExpandAll();
            ganttControl.Exceptions.AddRange(CreateExceptionRules());
            // </ganttControl1>        
        }

        public bool GroupByProject
        {
            get { return groupByProject; }
            set
            {
                if (groupByProject != value)
                {
                    groupByProject = value;
                    LoadGantt();
                }
            }
        }

        public bool ArrangeEmployees
        {
            get { return arrangeEmployees; }
            set
            {
                if (arrangeEmployees != value)
                {
                    arrangeEmployees = value;
                    LoadGantt();
                }
            }
        }

        public bool AllowShowCriticalPath
        {
            get { return true; }
        }

        private void ReadDB()
        {
            //Proyectos

            string projectsCommandText = "SELECT * FROM AUDIT.PROJECT PROJECT";
            SqlCommand projectsCommand = new SqlCommand(projectsCommandText, Database.Connection);
            SqlDataReader projectsReader = projectsCommand.ExecuteReader();
            while (projectsReader.Read())
            {
                var startDate = DateTime.ParseExact(projectsReader["start_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                var finishDate = DateTime.ParseExact(projectsReader["finish_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                projects.Add(new AuditProject()
                {
                    DBID = projectsReader["projectid"].ToString(),                    
                    Name = projectsReader["name"].ToString(),
                    StartDate = startDate,
                    FinishDate = finishDate                    
                });
            }

            //Empleados

            string employeesCommandText = "SELECT * FROM AUDIT.EMPLOYEE EMPLOYEE";
            SqlCommand employeesCommand = new SqlCommand(employeesCommandText, Database.Connection);
            SqlDataReader employeesReader = employeesCommand.ExecuteReader();
            while (employeesReader.Read())
            {
                employees.Add(new AuditEmployee()
                {
                    DBID = employeesReader["employeeid"].ToString(),
                    Name = employeesReader["name"].ToString()
                });
            }

            //Tareas

            string tasksCommandText = "SELECT * FROM AUDIT.TASK TASK";
            SqlCommand tasksCommand = new SqlCommand(tasksCommandText, Database.Connection);
            SqlDataReader tasksReader = tasksCommand.ExecuteReader();
            while (tasksReader.Read())
            {
                var startDate = DateTime.ParseExact(tasksReader["start_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                var finishDate = DateTime.ParseExact(tasksReader["finish_date"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                var project = projects.FirstOrDefault(p => p.DBID == tasksReader["projectid"].ToString());

                tasks.Add(new AuditTask()
                {
                    DBID = tasksReader["taskid"].ToString(),
                    Name = tasksReader["name"].ToString(),
                    Project = project,
                    StartDate = startDate,
                    FinishDate = finishDate
                });
            }

            //Tareas/Empleado

            string taskEmployeesCommandText = "SELECT * FROM AUDIT.EMPLOYEE_TASK_ASOC EMPLOYEE_TASK_ASOC";
            SqlCommand taskEmployeesCommand = new SqlCommand(taskEmployeesCommandText, Database.Connection);
            SqlDataReader taskEmployeesReader = taskEmployeesCommand.ExecuteReader();
            while (taskEmployeesReader.Read())
            {
                var task = tasks.FirstOrDefault(p => p.DBID == taskEmployeesReader["taskid"].ToString());
                var employee = employees.FirstOrDefault(p => p.DBID == taskEmployeesReader["employeeid"].ToString());
                task.Employees.Add(new EmployeeTaskAssoc() { role = taskEmployeesReader["role"].ToString(), Employee = employee });
            }


        }

        private void LoadGantt()
        {
            IList<GanttNode> GanttNodes = new List<GanttNode>();
            
            ////


            if (groupByProject) //Agrupa por proyectos
            {
                //Proyectos

                {
                    foreach (var project in projects.OrderBy(p => p.StartDate))
                    {
                        var startDate = project.StartDate;
                        var finishDate = project.FinishDate;
                        var projectUID = GanttNodes.Count.ToString();
                        GanttNodes.Add(new GanttNode(GanttNodeType.Project)
                        {
                            UID = projectUID,
                            ParentUID = "",
                            DBID = project.DBID,
                            Name = project.Name,
                            StartDate = startDate,
                            FinishDate = finishDate,
                            Progress = 100,
                            Predecessors = new List<string>(),
                            Resources = ""
                        });

                        GanttNodes.Add(new GanttNode(GanttNodeType.Folder)
                        {
                            UID = GanttNodes.Count.ToString(),
                            ParentUID = projectUID,
                            DBID = "TASKS_" + project.DBID,
                            Name = "Tareas",
                            StartDate = DateTime.Now,
                            FinishDate = DateTime.Now,
                            Progress = 100,
                            Predecessors = new List<string>(),
                            Resources = ""
                        });

                        GanttNodes.Add(new GanttNode(GanttNodeType.Folder)
                        {
                            UID = GanttNodes.Count.ToString(),
                            ParentUID = projectUID,
                            DBID = "EMPLOYEES_" + project.DBID,
                            Name = "Trabajadores",
                            StartDate = DateTime.Now,
                            FinishDate = DateTime.Now,
                            Progress = 100,
                            Predecessors = new List<string>(),
                            Resources = ""
                        });
                    }
                }

                //Tarea/Proyecto

                {
                    List<GanttNode> currentGanttNodes = new List<GanttNode>(GanttNodes);

                    foreach (var task in tasks.OrderBy(p => p.StartDate))
                    {
                        var startDate = task.StartDate;
                        var finishDate = task.FinishDate;
                        var predecessors = new List<string>();

                        foreach (var ganttNode in currentGanttNodes.Where(p => p.GanttNodeType == GanttNodeType.Folder && p.DBID == "TASKS_" + task.Project.DBID))
                        {
                            GanttNodes.Add(new GanttNode(GanttNodeType.Task)
                            {
                                UID = GanttNodes.Count.ToString(),
                                ParentUID = ganttNode.UID,
                                DBID = task.DBID,
                                Name = task.Name,
                                StartDate = startDate,
                                FinishDate = finishDate,
                                Progress = 100,
                                Predecessors = predecessors,
                                Resources = ""
                            });
                        }
                    }
                }

                //Empleados/Proyecto

                {
                    List<GanttNode> currentGanttNodes = new List<GanttNode>(GanttNodes);

                    foreach (var task in tasks.OrderBy(p => p.StartDate))
                    {
                        var startDate = task.StartDate;
                        var finishDate = task.FinishDate;
                        var predecessors = new List<string>();
                        var projectNode = currentGanttNodes.FirstOrDefault(p => p.GanttNodeType == GanttNodeType.Folder && p.DBID == "EMPLOYEES_" + task.Project.DBID);

                        foreach (var employeeTaskAssoc in task.Employees)
                        {
                            if (GanttNodes.FirstOrDefault(p => p.GanttNodeType == GanttNodeType.Employee && p.ParentUID == projectNode.UID && p.DBID == employeeTaskAssoc.Employee.DBID) == null)
                            {
                                GanttNodes.Insert(0, new GanttNode(GanttNodeType.Employee)
                                {
                                    UID = GanttNodes.Count.ToString(),
                                    ParentUID = projectNode.UID,
                                    DBID = employeeTaskAssoc.Employee.DBID,
                                    Name = employeeTaskAssoc.Employee.Name,
                                    StartDate = startDate,
                                    FinishDate = finishDate,
                                    Progress = 100,
                                    Predecessors = predecessors,
                                    Resources = employeeTaskAssoc.role == "AUDIT" ? "Auditor" : "Responsable"
                                });
                            }
                        }
                    }
                }
            }
            else //Agrupa por empleados
            {
                //Empleados

                {
                    foreach (var employee in employees.OrderBy(p => p.StartDate))
                    {
                        var startDate = employee.StartDate;
                        var finishDate = employee.FinishDate;
                        var employeeUID = @"\EMPLOYEE_" + employee.DBID;

                        GanttNodes.Add(new GanttNode(GanttNodeType.Employee)
                        {
                            UID = employeeUID,
                            ParentUID = "",
                            DBID = employee.DBID,
                            Name = employee.Name,
                            StartDate = DateTime.Now,
                            FinishDate = DateTime.Now,
                            Progress = 100,
                            Predecessors = new List<string>(),
                            Resources = ""
                        });

                        GanttNodes.Add(new GanttNode(GanttNodeType.Folder)
                        {
                            UID = employeeUID + @"\PROJECTS",
                            ParentUID = employeeUID,
                            DBID = "PROJECTS_" + employee.DBID,
                            Name = "Proyectos",
                            StartDate = DateTime.Now,
                            FinishDate = DateTime.Now,
                            Progress = 100,
                            Predecessors = new List<string>(),
                            Resources = ""
                        });
                    }
                }

                //Empleado/Proyecto

                {
                    List<GanttNode> currentGanttNodes = new List<GanttNode>(GanttNodes);

                    foreach (var task in tasks.OrderBy(p => p.StartDate))
                    {
                        foreach (var employeeTaskAssoc in task.Employees)
                        {
                            var parentUID = @"\EMPLOYEE_" + employeeTaskAssoc.Employee.DBID + @"\PROJECTS";
                            var projectUID = parentUID + @"\PROJECT_" + task.Project.DBID;
                            if (GanttNodes.FirstOrDefault(p => p.UID == projectUID) == null)
                            {
                                GanttNodes.Add(new GanttNode(GanttNodeType.Project)
                                {
                                    UID = projectUID,
                                    ParentUID = parentUID,
                                    DBID = task.Project.DBID,
                                    Name = task.Project.Name,
                                    StartDate = task.Project.StartDate,
                                    FinishDate = task.Project.FinishDate,
                                    Duration = task.Project.FinishDate - task.Project.StartDate,
                                    Progress = 0,
                                    Predecessors = new List<string>(),
                                    Resources = ""
                                });
                            }
                        }
                    }

                }

                //Empleado/Proyecto/Tarea

                {
                    List<GanttNode> currentGanttNodes = new List<GanttNode>(GanttNodes);

                    foreach (var task in tasks.OrderBy(p => p.StartDate))
                    {
                        foreach (var employeeTaskAssoc in task.Employees)
                        {
                            var parentUID = @"\EMPLOYEE_" + employeeTaskAssoc.Employee.DBID + @"\PROJECTS" + @"\PROJECT_" + task.Project.DBID;
                            var taskUID = parentUID + @"\TASK_" + task.DBID;

                            GanttNodes.Add(new GanttNode(GanttNodeType.Project)
                            {
                                UID = taskUID,
                                ParentUID = parentUID,
                                DBID = task.DBID,
                                Name = task.Name,
                                StartDate = task.StartDate,
                                FinishDate = task.FinishDate,
                                Duration = task.Project.FinishDate - task.Project.StartDate,
                                Progress = 100,
                                Predecessors = new List<string>(),
                                Resources = RoleDescription(employeeTaskAssoc.role)
                            });
                        }
                    }
                }
            }

            ////

            ganttControl.DataSource = GanttNodes;
            //ganttControl.ScheduleFromStartDate(GanttNodes[0].StartDate);
            ganttControl.ExpandAll();
        }

        private string RoleDescription(string roleCode)
        {
            string result = "";
            switch (roleCode)
            {
                case "AUDIT":
                    result = "Auditor";
                    break;
                case "JEFE":
                    result = "Auditor Jefe";
                    break;
                case "SUPRV":
                    result = "Auditor Supervisor";
                    break;
                default:
                    result = roleCode;
                    break;
            }

            return result;
        }

        private void LoadTasks()
        {
            //MessageBox.Show("mmm?");

            bool groupByEmployees = barCheckItem1.Checked;
            bool arrangeByEmployees = barCheckItem3.Checked;
            
            IList<GanttNode> tasks = new List<GanttNode>();

            //Proyectos

            string proyectosCommandText = "SELECT PROYECTO.idproyecto, MAX(PROYECTO.nombre) nombre, MIN(TAREA.fechainicio) fechainicio, MAX(TAREA.fechafin) fechafin  FROM AUDITORIA.PROYECTO PROYECTO" +
                " LEFT OUTER JOIN AUDITORIA.TAREA TAREA ON (TAREA.idproyecto = PROYECTO.idproyecto) " +
                " GROUP BY PROYECTO.idproyecto ";

            SqlCommand proyectosCommand = new SqlCommand(proyectosCommandText, Database.Connection);
            SqlDataReader proyectosReader = proyectosCommand.ExecuteReader();
            while (proyectosReader.Read())
            {
                var startDate = DateTime.ParseExact(proyectosReader["fechainicio"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                var finishDate = DateTime.ParseExact(proyectosReader["fechafin"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                tasks.Add(new GanttNode(GanttNodeType.Project)
                {
                    UID = "PROYECTO_" + proyectosReader["idproyecto"].ToString(),
                    ParentUID = "",
                    DBID = "PROYECTO_" + proyectosReader["idproyecto"].ToString(),
                    StartDate = startDate,
                    FinishDate = finishDate,
                    Name = proyectosReader["nombre"].ToString()
                });
            }

            //Trabajadores

            string trabajadoresCommandText = "SELECT PROYECTO.idproyecto, ASIGNACION.idtrabajador, MAX(COALESCE(TRABAJADOR_ASSOC.idtrabajador_subject, -1)) idtrabajador_subject, MAX(PROYECTO.nombre) nombre_proyecto, MAX(TRABAJADOR.nombre) nombre_trabajador,  " +
                " MIN(TAREA.fechainicio) fechainicio, MAX(TAREA.fechafin) fechafin " +
                " FROM AUDITORIA.PROYECTO PROYECTO " +
                " LEFT OUTER JOIN AUDITORIA.TAREA TAREA ON (TAREA.idproyecto = PROYECTO.idproyecto) " +
                " LEFT OUTER JOIN AUDITORIA.ASIGNACION ASIGNACION ON (ASIGNACION.idtarea = TAREA.idtarea) " +
                " LEFT OUTER JOIN AUDITORIA.TRABAJADOR TRABAJADOR ON (TRABAJADOR.idtrabajador = ASIGNACION.idtrabajador) " +
                " LEFT OUTER JOIN AUDITORIA.TRABAJADOR_ASSOC TRABAJADOR_ASSOC ON (TRABAJADOR_ASSOC.idtrabajador_object = TRABAJADOR.idtrabajador) " +
                " GROUP BY PROYECTO.idproyecto, ASIGNACION.idtrabajador";

            SqlCommand trabajadoresCommand = new SqlCommand(trabajadoresCommandText, Database.Connection);
            SqlDataReader trabajadoresReader = trabajadoresCommand.ExecuteReader();
            while (trabajadoresReader.Read())
            {
                var startDate = DateTime.ParseExact(trabajadoresReader["fechainicio"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                var finishDate = DateTime.ParseExact(trabajadoresReader["fechafin"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                string parentUID = "";
                if (trabajadoresReader["idtrabajador_subject"].ToString() == "-1")
                {
                    parentUID = "PROYECTO_" + trabajadoresReader["idproyecto"].ToString();
                }
                else
                {
                    parentUID = $"PROYECTO_{trabajadoresReader["idproyecto"]}_TRABAJADOR_{trabajadoresReader["idtrabajador_subject"]}";
                }


                tasks.Add(new GanttNode(GanttNodeType.Employee)
                {
                    UID = $"PROYECTO_{trabajadoresReader["idproyecto"]}_TRABAJADOR_{trabajadoresReader["idtrabajador"]}",
                    ParentUID = parentUID, //"PROYECTO_" + trabajadoresReader["idproyecto"].ToString(),
                    DBID = $"TRABAJADOR_{trabajadoresReader["idtrabajador"]}",
                    StartDate = startDate,
                    FinishDate = finishDate,
                    Name = trabajadoresReader["nombre_trabajador"].ToString()
                }); 
            }

            //Tareas

            string tareasCommandText = "SELECT TAREA.idtarea, TAREA.nombre, PROYECTO.idproyecto, ASIGNACION.idtrabajador, PROYECTO.nombre nombre_proyecto, TRABAJADOR.nombre nombre_trabajador,  " +
                " TAREA.fechainicio, TAREA.fechafin " +
                " FROM AUDITORIA.PROYECTO PROYECTO " +
                " LEFT OUTER JOIN AUDITORIA.TAREA TAREA ON(TAREA.idproyecto = PROYECTO.idproyecto) " +
                " LEFT OUTER JOIN AUDITORIA.ASIGNACION ASIGNACION ON(ASIGNACION.idtarea = TAREA.idtarea) " +
                " LEFT OUTER JOIN AUDITORIA.TRABAJADOR TRABAJADOR ON(TRABAJADOR.idtrabajador = ASIGNACION.idtrabajador) " +
                " ORDER BY TAREA.fechainicio DESC, TAREA.fechafin DESC";

            SqlCommand tareasCommand = new SqlCommand(tareasCommandText, Database.Connection);
            SqlDataReader tareasReader = tareasCommand.ExecuteReader();
            while (tareasReader.Read())
            {
                var startDate = DateTime.ParseExact(tareasReader["fechainicio"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                var finishDate = DateTime.ParseExact(tareasReader["fechafin"].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                var predecessors = new List<string>();

                tasks.Insert(0, new GanttNode(GanttNodeType.Task)
                {
                    UID = $"PROYECTO_{tareasReader["idproyecto"]}_TRABAJADOR_{tareasReader["idtrabajador"]}_TAREA_" + tareasReader["idtarea"].ToString(),
                    ParentUID = $"PROYECTO_{tareasReader["idproyecto"]}_TRABAJADOR_{tareasReader["idtrabajador"]}",
                    DBID = $"TAREA_{tareasReader["idtarea"]}",
                    Name = tareasReader["nombre"].ToString(),
                    StartDate = startDate,
                    FinishDate = finishDate,
                    Progress = 100,
                    Predecessors = predecessors,
                    Resources = tareasReader["IdProyecto"].ToString()
                });
            }

            //Tareas Predecesoras
            
            string predecessorCommandText = "SELECT idtarea_subject, idtarea_object FROM AUDITORIA.TAREA_ASSOC";             
            SqlCommand predecessorCommand = new SqlCommand(predecessorCommandText, Database.Connection);
            SqlDataReader predecessorReader = predecessorCommand.ExecuteReader();
            while (predecessorReader.Read())
            {
                foreach (var parentTask in tasks.Where(x => x.DBID == $"TAREA_{predecessorReader["idtarea_subject"]}"))
                {
                    foreach (var childTask in tasks.Where(x => x.DBID == $"TAREA_{predecessorReader["idtarea_object"]}"))
                    {
                        childTask.Predecessors.Add(parentTask.UID);
                    }
                }
            }
            
            //

            ganttControl.DataSource = tasks;
            ganttControl.ScheduleFromStartDate(tasks[0].StartDate);
        }

        #region Codigo copiado 

        void Init()
        {
            icbSchedulingMode.Properties.Items.Add(new ImageComboBoxItem("Default", ScheduleMode.Default, -1));
            icbSchedulingMode.Properties.Items.Add(new ImageComboBoxItem("AutoScheduled", ScheduleMode.Auto, -1));
            icbSchedulingMode.Properties.Items.Add(new ImageComboBoxItem("ManuallyScheduled", ScheduleMode.Manual, -1));
            icbSchedulingMode.EditValue = ganttControl.OptionsBehavior.ScheduleMode;
            icbTooltipLocation.Properties.Items.Add(new ImageComboBoxItem("Default", InteractionTooltipLocation.Default, -1));
            icbTooltipLocation.Properties.Items.Add(new ImageComboBoxItem("TopLeft", InteractionTooltipLocation.TopLeft, -1));
            icbTooltipLocation.Properties.Items.Add(new ImageComboBoxItem("TopRight", InteractionTooltipLocation.TopRight, -1));
            icbTooltipLocation.Properties.Items.Add(new ImageComboBoxItem("BottomLeft", InteractionTooltipLocation.BottomLeft, -1));
            icbTooltipLocation.Properties.Items.Add(new ImageComboBoxItem("BottomRight", InteractionTooltipLocation.BottomRight, -1));
            icbTooltipLocation.Properties.Items.Add(new ImageComboBoxItem("None", InteractionTooltipLocation.None, -1));
            icbTooltipLocation.EditValue = ganttControl.OptionsView.InteractionTooltipLocation;
            ceAllowModifyTasks.Checked = ganttControl.OptionsCustomization.AllowModifyTasks != DefaultBoolean.False;
            ceAllowModifyDependencies.Checked = ganttControl.OptionsCustomization.AllowModifyDependencies != DefaultBoolean.False;
            ceAllowModifyProgress.Checked = ganttControl.OptionsCustomization.AllowModifyProgress != DefaultBoolean.False;
            ganttControl.OptionsView.CriticalPathHighlightMode = CriticalPathHighlightMode.Single;
            cpeCriticalPathTasks.Color = DXSkinColors.FillColors.Danger;
            cpeCriticalPathDependencies.Color = DXSkinColors.FillColors.Danger;
        }
        void cpeCriticalPathTasks_EditValueChanged(object sender, EventArgs e)
        {
            ganttControl.Appearance.CriticalPathTask.BackColor = cpeCriticalPathTasks.Color;
        }
        void cpeCriticalPathDependencies_EditValueChanged(object sender, EventArgs e)
        {
            ganttControl.Appearance.CriticalPathDependency.BackColor = cpeCriticalPathDependencies.Color;
        }
        ExceptionRule[] CreateExceptionRules()
        {
            YearlyExceptionRule NewYearDay = new YearlyExceptionRule()
            {
                DayOfMonth = 1,
                Month = Month.January
            };
            YearlyDayOfWeekExceptionRule MartinLutherDay = new YearlyDayOfWeekExceptionRule()
            {
                DayOfWeek = DayOfWeek.Monday,
                Month = Month.January,
                WeekOfMonth = WeekOfMonth.Third
            };
            YearlyDayOfWeekExceptionRule PresidentDay = new YearlyDayOfWeekExceptionRule()
            {
                DayOfWeek = DayOfWeek.Monday,
                Month = Month.February,
                WeekOfMonth = WeekOfMonth.Third
            };
            YearlyDayOfWeekExceptionRule MemorialDay = new YearlyDayOfWeekExceptionRule()
            {
                DayOfWeek = DayOfWeek.Monday,
                Month = Month.May,
                WeekOfMonth = WeekOfMonth.Last
            };
            YearlyExceptionRule IndependenceDay = new YearlyExceptionRule()
            {
                DayOfMonth = 4,
                Month = Month.July
            };
            YearlyDayOfWeekExceptionRule LaborDay = new YearlyDayOfWeekExceptionRule()
            {
                DayOfWeek = DayOfWeek.Monday,
                Month = Month.September,
                WeekOfMonth = WeekOfMonth.First
            };
            YearlyDayOfWeekExceptionRule ColumbusDay = new YearlyDayOfWeekExceptionRule()
            {
                DayOfWeek = DayOfWeek.Monday,
                Month = Month.October,
                WeekOfMonth = WeekOfMonth.Second
            };
            YearlyExceptionRule VeteransDay = new YearlyExceptionRule()
            {
                DayOfMonth = 11,
                Month = Month.November
            };
            YearlyDayOfWeekExceptionRule ThanksgivingDay = new YearlyDayOfWeekExceptionRule()
            {
                DayOfWeek = DayOfWeek.Thursday,
                Month = Month.November,
                WeekOfMonth = WeekOfMonth.Forth
            };
            YearlyExceptionRule ChristmasDay = new YearlyExceptionRule()
            {
                DayOfMonth = 25,
                Month = Month.December
            };
            return new ExceptionRule[] {
                NewYearDay,
                MartinLutherDay,
                PresidentDay,
                MemorialDay,
                IndependenceDay,
                LaborDay,
                ColumbusDay,
                VeteransDay,
                ThanksgivingDay,
                ChristmasDay
            };
        }

        protected string[] WhatsThisCodeFileNames
        {
            get { return new string[] { "SoftwareDevelopment" }; }
        }
        protected string WhatsThisXMLFileName
        {
            get { return "softwareDevelopment"; }
        }

        public GanttControl MainControl
        {
            get { return ganttControl; }
        }
        public bool AllowPrintOptions
        {
            get { return true; }
        }
        public bool AllowGenerateReport
        {
            get { return false; }
        }

        void icbSchedulingMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ganttControl.OptionsBehavior.ScheduleMode = (ScheduleMode)icbSchedulingMode.EditValue;
        }

        void icbTooltipLocation_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ganttControl.OptionsView.InteractionTooltipLocation = (InteractionTooltipLocation)icbTooltipLocation.EditValue;
        }

        void ceAllowModifyTasks_CheckedChanged(object sender, System.EventArgs e)
        {
            ganttControl.OptionsCustomization.AllowModifyTasks = ceAllowModifyTasks.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        }

        void ceAllowModifyDependencies_CheckedChanged(object sender, EventArgs e)
        {
            ganttControl.OptionsCustomization.AllowModifyDependencies = ceAllowModifyDependencies.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        }
        void ceAllowModifyProgress_CheckedChanged(object sender, EventArgs e)
        {
            ganttControl.OptionsCustomization.AllowModifyProgress = ceAllowModifyProgress.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        }

        #endregion

        private void ganttControl_CustomTaskScheduling(object sender, CustomTaskSchedulingEventArgs e)
        {
            e.Cancel = true;
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Trabajador
            this.GroupByProject = !barCheckItem1.Checked;           
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Proyecto
            this.GroupByProject = barCheckItem2.Checked;
        }
    }

    public class AuditProject
    {
        public string DBID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        List<AuditTask> tasks;
        public List<AuditTask> Tasks
        {
            get 
            {
                if (tasks == null)
                    tasks = new List<AuditTask>();

                return tasks;
            }

        }
    }

    public class AuditEmployee
    {
        public string DBID { get; set; }
        public string Name { get; set; }
        
        public DateTime StartDate
        {
            get
            {
                return DateTime.Now.AddDays(-1);
            }
        }
        public DateTime FinishDate
        {
            get
            {
                return DateTime.Now.AddDays(1);
            }
        }
    }

    public class AuditTask
    {
        public string DBID { get; set; }
        public string Name { get; set; }
        public AuditProject Project { get; set; }
        public DateTime StartDate { get; set; }        
        public DateTime FinishDate { get; set; }

        List<EmployeeTaskAssoc> employees;
        public List<EmployeeTaskAssoc> Employees
        {
            get
            {
                if (employees == null)
                    employees = new List<EmployeeTaskAssoc>();

                return employees;
            }

        }
    }

    public class EmployeeTaskAssoc
    {
        public AuditEmployee Employee { get; set; }
        public string role { get; set; }        
    }

    public enum GanttNodeType {Folder, Project, Employee, Task};

    public class GanttNode
    {
        public GanttNode(GanttNodeType ganttNodeType)
        {
            GanttNodeType = ganttNodeType;
            Predecessors = new List<string>();            
        }
        public GanttNodeType GanttNodeType { get; set; }

        public string ParentUID { get; set; }        
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public TimeSpan Duration { get; set; }        
        public DateTime? BaselineStartDate { get; set; }
        public DateTime? BaselineFinishDate { get; set; }
        public string Name { get; set; }
        public string UID { get; set; }
        public string DBID { get; set; }
        public List<string> Predecessors { get; set; }
        public string Resources { get; set; }
        public double Progress { get; set; }    
    }
}
