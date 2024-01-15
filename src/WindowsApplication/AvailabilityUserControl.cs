using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGantt.Base.Scheduling;
using DevExpress.XtraGantt.Exceptions;
using DevExpress.XtraGantt.Options;
using DevExpress.XtraGantt.Scheduling;
using DevExpress.XtraGantt;
using System.Globalization;
using DevExpress.XtraGantt.Chart.Item.Task;
using Audalia.DataHUBClient;
using Audalia.DataHUBCommon;
using System.Threading;
using DevExpress.XtraTreeList.ViewInfo;
using DevExpress.XtraGantt.Chart;

namespace WindowsApplication.AvailabilityUserControl
{
    public partial class AvailabilityUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        public IList<GanttNode> GanttNodes;


        bool groupByProject = false;
        bool arrangeEmployees = true;

        string selectedTaskDBID = "";
        string selectedProjectDBID = "";
        string selectedEmployeeDBID = "";

        DateTime striplineStart;
        DateTime striplineEnd;

        DateTime ganttStartDate;
        DateTime ganttFinishDate;


        public AvailabilityUserControl()
        {
            InitializeComponent();

            //

            if (DateTime.Now.Month < 9)
            {
                ganttStartDate = new DateTime(DateTime.Now.Year - 1, 9, 1);
                ganttFinishDate = new DateTime(DateTime.Now.Year + 1, 8, 31);
            }
            else
            {
                ganttStartDate = new DateTime(DateTime.Now.Year, 9, 1);
                ganttFinishDate = new DateTime(DateTime.Now.Year + 2, 8, 31);
            }

            InitRibbonFilters();
            InitGantt();
        }

        public GanttControl GanttControl
        {
            get { return ganttControl; }
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


        private void InitRibbonFilters()
        {
            for (int i = 0; i <= 6; i++)
                ejercicioRepositoryItemComboBox.Items.Add((DateTime.Now.Year - i + 1).ToString());

            ejercicioBarEditItem.EditValue = DateTime.Now.Month <= 8 ? (DateTime.Now.Year - 1).ToString() : DateTime.Now.Year.ToString();
            ejercicioRepositoryItemComboBox_SelectedIndexChanged(ejercicioRepositoryItemComboBox, null);

            string taxYear = ejercicioBarEditItem.EditValue.ToString();

            employeeRepositoryItemComboBox.Items.Clear();
            foreach (var employee in Data.Audit.Employees.Values.OrderBy(p => p.Name))
            {
                employeeRepositoryItemComboBox.Items.Add(employee.Name);
            }

            employeeBarEditItem.EditValue = "";

        }

        private void LoadGantt()
        {
            LoadGantt2();
            return;
        }

        private void LoadGantt2()
        {
            ganttControl.BeginUpdate();
            ganttControl.ClearNodes();

            GanttNodes = new List<GanttNode>();

            //Empleados

            {
                //foreach (var employee in Data.Audit.Employees.Values.Where(p => p.Status != "Inactive").OrderBy(p => p.Name))
                foreach (var employee in Data.Audit.Employees.Values.OrderBy(p => p.Name)) //Where(p => p.LeavingDate == null || p.LeavingDate <= ganttFinishDate).
                {
                    if (employee.LeavingDate != null && employee.LeavingDate < ganttStartDate)
                        continue;

                    var employeeUID = @"\EMPLOYEE_" + employee.DBID;
                    //var categoryUID = @"\CATEGORY_" + employee.Category;
                    var parentUID = "";

                    GanttNodes.Add(new GanttNode(GanttNodeType.Employee)
                    {
                        UID = employeeUID,
                        ParentUID = parentUID,
                        DBID = employee.DBID,
                        Name = employee.Name,
                        BranchOffice = employee.BranchOffice,

                        StartDate = ganttStartDate, 
                        FinishDate = ganttFinishDate,
                        BaselineStartDate = ganttStartDate,
                        BaselineFinishDate = ganttFinishDate,

                        Progress = 100,
                        Predecessors = new List<string>(),

                        ProjectID = "",
                        EmployeeID = employee.DBID,
                        TaskID = "",

                        Project = "",
                        Employee = employee.Name,
                        Resources = "",
                        Category = employee.Category,

                        Hint = employee.Name

                    });
                }
            }            

            ////

            ganttControl.DataSource = GanttNodes;
            ganttControl.EndUpdate();
            ganttControl.ExpandAll();
        }

        
        public void InitGantt()
        {
            ganttControl.OptionsBehavior.ScheduleMode = ScheduleMode.Manual;
            ganttControl.OptionsBehavior.ReadOnly = false;
            ganttControl.OptionsBehavior.Editable = true;
            ganttControl.OptionsCustomization.AllowModifyTasks = DefaultBoolean.False;
            ganttControl.OptionsCustomization.AllowModifyDependencies = DefaultBoolean.False;
            ganttControl.OptionsCustomization.AllowModifyProgress = DefaultBoolean.False;

            ganttControl.TreeListMappings.ParentFieldName = "ParentUID";
            ganttControl.TreeListMappings.KeyFieldName = "UID";
            ganttControl.ChartMappings.TextFieldName = "Resources";
            ganttControl.ChartMappings.InteractionTooltipTextFieldName = "Name";
            ganttControl.ChartMappings.DurationFieldName = "Duration";
            ganttControl.ChartMappings.InteractionTooltipTextFieldName = "Hint";

            /*
            ganttControl.ChartStartDate = new DateTime(DateTime.Now.Year, 1, 1).AddMonths(-1);
            ganttControl.ChartFinishDate = new DateTime(DateTime.Now.Year, 12, 31).AddMonths(1);
            */

            //ganttControl.ChartStartDate = striplineStart.AddDays(-15); 
            //ganttControl.ChartFinishDate = striplineEnd.AddDays(15);

            /*
            DateTime striplineStart = new DateTime(2021, 3, 1);
            DateTime striplineEnd = new DateTime(2021, 3, 15);
            */

            Color striplineColor = Color.FromArgb(24, 255, 0, 0);

            ganttControl.CustomDrawTimescaleColumn += (sender, e) => {

                GanttTimescaleColumn column = e.Column;
                e.DrawBackground();

                {

                    float stripLineStartPoint = (float)Math.Max(e.GetPosition(striplineStart), column.Bounds.Left);
                    float stripLineEndPoint = (float)Math.Min(e.GetPosition(striplineEnd), column.Bounds.Right);
                    RectangleF boundsToDraw = new RectangleF(stripLineStartPoint, column.Bounds.Y, stripLineEndPoint - stripLineStartPoint, column.Bounds.Height);
                    if (boundsToDraw.Width > 0)
                        e.Cache.FillRectangle(striplineColor, boundsToDraw);
                    e.DrawHeader();
                }

                if (column.StartDate.DayOfWeek == DayOfWeek.Saturday || column.StartDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    //e.DrawBackground();

                    float x1 = (float)e.GetPosition(column.StartDate);
                    float x2 = (float)Math.Min(e.GetPosition(column.StartDate.AddDays(1)), column.Bounds.Right);
                    float width = x2 - x1;// - 1;

                    RectangleF deadLineRect = new RectangleF(x1, column.Bounds.Y, width, column.Bounds.Height);

                    e.Cache.FillRectangle(Color.FromArgb(16, 0, 0, 0), deadLineRect);

                    e.DrawHeader();
                    e.Handled = true;
                }


                foreach (var holidayBase in Data.Audit.Holidays)
                {
                    DateTime holiday = new DateTime(holidayBase.Year, holidayBase.Month, holidayBase.Day);

                    if (column.StartDate <= holiday && column.FinishDate >= holiday)
                    {
                        //e.DrawBackground();

                        float x1 = (float)e.GetPosition(holiday);
                        float x2 = (float)Math.Min(e.GetPosition(holiday.AddDays(1)), column.Bounds.Right);
                        float width = x2 - x1;

                        RectangleF deadLineRect = new RectangleF(x1, column.Bounds.Y, width, column.Bounds.Height);
                        //e.Cache.FillRectangle(Color.FromArgb(24, 255, 0, 0), deadLineRect);
                        e.Cache.FillRectangle(Color.FromArgb(255, 255, 224, 224), deadLineRect);
                        e.DrawHeader();
                        e.Handled = true;
                    }
                }

                e.Handled = true;
            };


            ganttControl.CustomDrawTask += (sender, e) =>
            {
                try
                {
                    switch ((GanttNodeType)e.Node.GetValue("GanttNodeType"))
                    {                       
                        case GanttNodeType.Employee:
                            {
                                var currentEmployee = Data.Audit.Employees[e.Node.GetValue("DBID").ToString()];

                                //Draw node

                                {
                                    var shape = e.Info.ShapeBounds;
                                    var ganttStartDate = ganttControl.GetChartVisibleStartDate().TruncStartDate();
                                    var ganttFinishDate = ganttControl.GetChartVisibleFinishDate().TruncFinishDate();

                                    float start = (float)e.GetPosition(ganttStartDate);
                                    float finish = (float)e.GetPosition(ganttFinishDate);
                                                                        
                                    e.Appearance.BaselineColor = Color.White;
                                    e.Appearance.BackColor = Color.FromArgb(64, 0, 0, 0);
                                    e.Appearance.ProgressColor = Color.FromArgb(64, 0, 0, 0);
                                    
                                    RectangleF rect2 = new RectangleF(start, shape.Y + shape.Height + 3, finish - start, 1);
                                    e.DrawShape(rect2, 0);

                                }
                                

                                //Draw holidays
                                {
                                    var shape = e.Info.ShapeBounds;

                                    //var project = task.Project;
                                    var employee = currentEmployee;
                                    var allocs = Data.Audit.ProjectEmployeeAllocs.Where(p => p.Value.ProjectId == "0" && p.Value.EmployeeId == employee.DBID).OrderBy(p => p.Value.AllocDate);

                                    var backColor = Color.FromArgb(255, 192, 32, 32);

                                    foreach (var alloc in allocs)
                                    {
                                        float start = (float)e.GetPosition(alloc.Value.AllocDate) - 1;
                                        float finish = (float)e.GetPosition(alloc.Value.AllocDate.AddDays(1));

                                        {
                                            RectangleF rect1 = new RectangleF(start + 1, shape.Y - 1, finish - start - 1, shape.Height + 2);
                                            e.Cache.FillRectangle(backColor, rect1);
                                        }
                                    }
                                }

                                //Draw leaves
                                {                                    
                                    var shape = e.Info.ShapeBounds;

                                    //var project = task.Project;
                                    var employee = currentEmployee;

                                    if (employee.LeavingDate != null && employee.LeavingDate < ganttFinishDate)
                                    {                                        
                                        var backColor = Color.FromArgb(96, 0, 0, 0);

                                        float start = (float)e.GetPosition(employee.LeavingDate.Value) - 1;
                                        float finish = (float)e.GetPosition(ganttFinishDate.AddMonths(1));

                                        RectangleF rect1 = new RectangleF(start + 1, shape.Y - 1, finish - start - 1, shape.Height + 2);
                                        e.Cache.FillRectangle(backColor, rect1);
                                    }                                                                      
                                }


                                int projectIx = 0;
                                string projectNames = "";
                                foreach (var task in currentEmployee.Projects.OrderBy(p => p.Project.StartDate))
                                {
                                    var shape = e.Info.ShapeBounds;

                                    
                                    //Draw project
                                    {
                                        e.Appearance.BaselineColor = Color.FromArgb(255, 0, 64, 128);

                                        float start = (float)e.GetPosition(task.Project.StartDate);
                                        float finish = (float)e.GetPosition(task.Project.FinishDate.AddDays(1));

                                        RectangleF rect1 = new RectangleF(start + 1, shape.Y - 2, finish - start - 1, shape.Height + 4);

                                        e.Appearance.BaselineColor = Color.FromArgb(255, 0, 0, 128);
                                        e.Appearance.BackColor = Color.FromArgb(128, 0, 0, 128); ;//Color.FromArgb(255, 0, 0, 128);
                                        e.Appearance.ProgressColor = Color.FromArgb(255, 0, 0, 128);
                                        //e.DrawShape(rect1, 0);                                                                            
                                        //e.Cache.DrawRectangle(new Pen(Color.FromArgb(128, 0, 0, 128), 1), rect1);
                                        //e.Cache.FillRectangle(Color.FromArgb(64, 0, 0, 128), rect1);
                                    }
                                    
                                    
                                    
                                    //Draw periods                                    
                                    {
                                        var project = task.Project;
                                        var employee = currentEmployee; //Data.Audit.Employees[e.Node.GetValue("EmployeeID").ToString()];
                                        var allocs = Data.Audit.ProjectEmployeeAllocs.Where(p => p.Value.ProjectId == project.DBID && p.Value.EmployeeId == employee.DBID).OrderBy(p => p.Value.AllocDate).OrderBy(p => p.Value.AllocDate);                                        
                                        var backColor = Color.FromArgb(255, 0, 64, 128);

                                        foreach (var alloc in allocs)
                                        {
                                            float start = (float)e.GetPosition(alloc.Value.AllocDate) - 1;
                                            float finish = (float)e.GetPosition(alloc.Value.AllocDate.AddDays(1));

                                            {
                                                RectangleF rect1 = new RectangleF(start + 1, shape.Y - 1, finish - start - 1, shape.Height + 2);
                                                e.Cache.FillRectangle(backColor, rect1);                                                
                                            }
                                        }

                                        /*
                                        //Draw labels v2
                                        var firstAlloc = allocs.FirstOrDefault();                                        
                                        if (firstAlloc.Value != null)
                                        {
                                            string text = "";
                                            text = firstAlloc.Value.Project.Name;
                                            var font = new Font(this.Font, FontStyle.Bold);                                            
                                            
                                            float start = (float)e.GetPosition(firstAlloc.Value.AllocDate) - 1;
                                            float finish = (float)e.GetPosition(firstAlloc.Value.AllocDate.AddDays(1));
                                            RectangleF rect1 = new RectangleF(start + 1, shape.Y - 1, finish - start - 1, shape.Height + 2);

                                            float textStartPosition = start;
                                            e.Cache.DrawString(text, font, new SolidBrush(Color.White), new PointF(textStartPosition + 4, rect1.Y + 2));
                                        }
                                        */

                                        /*
                                        //Draw labels v3
                                        foreach (var alloc in allocs)
                                        {
                                            string text = alloc.Value.Project.Name.Substring(0,1);
                                            var font = new Font(this.Font, FontStyle.Bold);

                                            float start = (float)e.GetPosition(alloc.Value.AllocDate) - 1;
                                            float finish = (float)e.GetPosition(alloc.Value.AllocDate.AddDays(1));
                                            RectangleF rect1 = new RectangleF(start + 1, shape.Y - 1, finish - start - 1, shape.Height + 2);

                                            float textStartPosition = start;
                                            e.Cache.DrawString(text, font, new SolidBrush(Color.FromArgb(192,255,255,255)), new PointF(textStartPosition + 7, rect1.Y + 1));
                                        }
                                        */

                                    }

                                   
                                    /*
                                    //Draw labels
                                    {
                                        e.Appearance.BaselineColor = Color.FromArgb(255, 0, 64, 128);

                                        float start = (float)e.GetPosition(task.Project.StartDate);
                                        float finish = (float)e.GetPosition(task.Project.FinishDate.AddDays(1));
                                        
                                        RectangleF rect1 = new RectangleF(start + 1, shape.Y - 2, finish - start - 1, shape.Height + 4);
                                       
                                        //if (selectedEmployeeDBID == task.EmployeeId)
                                        {
                                            if (task.Project.StartDate <= striplineEnd && task.Project.FinishDate >= striplineStart)
                                            {                                                
                                                projectNames += projectNames == "" ? "" : " / ";
                                                projectNames += task.Project.Name;                                                
                                            }
                                            
                                            var font = new Font(this.Font, FontStyle.Bold);                                            
                                            int textStartPosition = (int)Math.Max(e.GetPosition(striplineEnd), 0);
                                            int textWidth = (int)e.Cache.CalcTextSize(projectNames, font).Width;
                                            int textHeight = (int)e.Cache.CalcTextSize(projectNames, font).Height;
                                            e.Cache.FillRectangle(Color.FromArgb(192, 255, 255, 255), new Rectangle(textStartPosition + 2, (int)rect1.Y + 3, textWidth + 4, textHeight - 1));
                                            e.Cache.DrawString(projectNames, font, new SolidBrush(Color.Black), new PointF(textStartPosition + 4, rect1.Y + 2));
                                        }
                                    }
                                    */

                                }

                                e.Handled = true; 
                            }

                            break;
                        default:
                            e.Handled = false;
                            break;
                    }
                }
                catch
                {
                    //¿?
                }
            };

            ganttControl.CustomTaskDisplayText += (sender, e) =>
            {
                e.LeftText = e.Node.GetValue("Resources").ToString();
                e.RightText = string.Empty;
                e.InsideText = e.Node.GetValue("Name").ToString();
            };
                        
            ganttControl.Exceptions.AddRange(CreateExceptionRules());                       
        }

        public void Init(DateTime startDate, DateTime finishDate, bool showChecks = true)
        {
            if (showChecks)
            {
                ribbonControl1.Visible = false;
            }

            SelectedColumn.Visible = showChecks;

            striplineStart = startDate;
            striplineEnd = finishDate.AddDays(1);

            LoadGantt();
            ganttControl.ExpandAll();

            //

            //Application.DoEvents();
            //ganttControl.ForceInitialize();            
            //ganttControl.SetChartVisibleRange(striplineStart.AddMonths(-1), striplineEnd.AddDays(15));
            //ganttControl.ScrollChartToDate(striplineStart.AddMonths(-1));

            ganttControl.ZoomMode = GanttZoomMode.FixedIntervals;
            ganttControl.ZoomIn();
            ganttControl.ZoomIn();
            ganttControl.ZoomIn();
            ganttControl.ZoomMode = GanttZoomMode.Smooth;
            ganttControl.ScrollChartToDate(striplineStart.AddDays(-7));
            
            //ganttControl.ZoomIn();
            //ganttControl.ZoomOut();                        
        }

        public void SetDates(DateTime startDate, DateTime finishDate)
        {
            striplineStart = startDate;
            striplineEnd = finishDate.AddDays(1);

            //ganttControl.ForceInitialize();            
            //Application.DoEvents();
            //ganttControl.ForceInitialize();
            //ganttControl.SetChartVisibleRange(striplineStart.AddMonths(-1), striplineEnd.AddDays(15));
            //ganttControl.ScrollChartToDate(striplineStart.AddMonths(-1));

            ganttControl.ZoomMode = GanttZoomMode.FixedIntervals;
            ganttControl.ZoomIn();
            ganttControl.ZoomIn();
            ganttControl.ZoomIn();
            ganttControl.ZoomMode = GanttZoomMode.Smooth;
            ganttControl.ScrollChartToDate(striplineStart.AddDays(-7));

            //ganttControl.ZoomIn();
            //ganttControl.ZoomOut();
        }


        ExceptionRule[] CreateExceptionRules()
        {
            YearlyExceptionRule AnoNuevoDay = new YearlyExceptionRule()
            {
                DayOfMonth = 1,
                Month = Month.January
            };

            YearlyExceptionRule ReyesDay = new YearlyExceptionRule()
            {
                DayOfMonth = 6,
                Month = Month.January
            };

            YearlyExceptionRule SanJoseDay = new YearlyExceptionRule()
            {
                DayOfMonth = 19,
                Month = Month.March
            };

            DaysExceptionRule JuevesSantoDay = new DaysExceptionRule()
            {
                StartDate = new DateTime(2021, 4, 1),
                Occurrences = 1
            };

            DaysExceptionRule ViernesSantoDay = new DaysExceptionRule()
            {
                StartDate = new DateTime(2021, 4, 2),
                Occurrences = 1
            };

            YearlyExceptionRule TrabajoDay = new YearlyExceptionRule()
            {
                DayOfMonth = 1,
                Month = Month.May
            };

            YearlyExceptionRule ComunidadDeMadridDay = new YearlyExceptionRule()
            {
                DayOfMonth = 3,
                Month = Month.May
            };


            YearlyExceptionRule SanIsidroDay = new YearlyExceptionRule()
            {
                DayOfMonth = 15,
                Month = Month.May
            };

            YearlyExceptionRule FiestaNacionalDay = new YearlyExceptionRule()
            {
                DayOfMonth = 12,
                Month = Month.October
            };

            YearlyExceptionRule TodosLosSantosDay = new YearlyExceptionRule()
            {
                DayOfMonth = 1,
                Month = Month.November
            };

            YearlyExceptionRule ConstitucionDay = new YearlyExceptionRule()
            {
                DayOfMonth = 6,
                Month = Month.December
            };

            YearlyExceptionRule InmaculadaConcepcionDay = new YearlyExceptionRule()
            {
                DayOfMonth = 8,
                Month = Month.December
            };

            YearlyExceptionRule NavidadDay = new YearlyExceptionRule()
            {
                DayOfMonth = 25,
                Month = Month.December
            };

            return new ExceptionRule[] {
                AnoNuevoDay,
                ReyesDay,
                SanJoseDay,
                JuevesSantoDay,
                ViernesSantoDay,
                TrabajoDay,
                ComunidadDeMadridDay,
                SanIsidroDay,
                FiestaNacionalDay,
                TodosLosSantosDay,
                ConstitucionDay,
                InmaculadaConcepcionDay,
                NavidadDay
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
        

        private void ganttControl_CustomTaskScheduling(object sender, CustomTaskSchedulingEventArgs e)
        {
            e.Cancel = true;
        }


        private void ganttControl_DoubleClick(object sender, EventArgs e)
        {

            if (selectedProjectDBID != "")
            {
                ProjectForm.EditProject(selectedProjectDBID);
                LoadGantt();
            }
            else
            {
                if (selectedTaskDBID != "")
                {
                    EmployeeForm.EditEmployee(selectedTaskDBID);
                    LoadGantt();

                }
            }
        }

        public void RefreshGantt(GanttControlNode rootNode = null)
        {
            var nodeList = (rootNode != null) ? rootNode.Nodes : ganttControl.Nodes;
            foreach (GanttControlNode node in nodeList)
            {
                string dbid = node.GetValue("DBID").ToString();

                if (node.GetValue("GanttNodeType").ToString() == GanttNodeType.Task.ToString())
                {                    
                    var task = Data.Audit.Projects[dbid];
                    node.SetValue("StartDate", task.StartDate);
                    node.SetValue("Duration", task.FinishDate - task.StartDate);
                    ganttControl.RefreshNode(node);
                }
                else
                {
                    if (node.GetValue("GanttNodeType").ToString() == GanttNodeType.Project.ToString())
                    {                     
                        var project = Data.Audit.Projects[dbid];
                        node.SetValue("StartDate", project.StartDate);
                        node.SetValue("Duration", project.FinishDate - project.StartDate);
                        ganttControl.RefreshNode(node);
                    }
                }

                RefreshGantt(node);
            }
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (selectedTaskDBID != "")
            {
                var task = Data.Audit.Projects[selectedTaskDBID];
                ganttControl.AddFilter($"[Project] = '{task.Name}'");
            }
            else
            {
                if (selectedEmployeeDBID != "")
                {
                    var employee = Data.Audit.Employees[selectedEmployeeDBID];
                    
                    string filter = "";
                    foreach (var task in employee.Projects)
                    {
                        filter += (filter == "" ? "" : " OR ") + $"[Project] = '{task.Project.Name}'";
                    }
                    ganttControl.AddFilter(filter);
                }
            }

        }


        private void ganttControl_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            selectedTaskDBID = "";
            selectedProjectDBID = "";
            selectedEmployeeDBID = "";

            foreach (var node in ganttControl.Selection)
            {
                if (node.GetValue("GanttNodeType").ToString() == GanttNodeType.Task.ToString())
                {
                    selectedTaskDBID = node.GetValue("DBID").ToString();
                }
                else
                {
                    if (node.GetValue("GanttNodeType").ToString() == GanttNodeType.Project.ToString())
                    {
                        selectedProjectDBID = node.GetValue("DBID").ToString();

                    }
                    else
                    {
                        if (node.GetValue("GanttNodeType").ToString() == GanttNodeType.Employee.ToString())
                        {
                            selectedEmployeeDBID = node.GetValue("DBID").ToString();

                        }
                    }
                }
            }
        }

        private void ganttControl_NodeCellStyle(object sender, DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs e)
        {
            //if (e.Column.FieldName != "Budget") return;
            //if (Convert.ToInt32(e.Node.GetValue(e.Column.AbsoluteIndex)) > 500000)

            if ((bool)e.Node.GetValue(0))
            {
                e.Appearance.BackColor = Color.Navy; //FromArgb(80, 255, 0, 255);
                e.Appearance.ForeColor = Color.White;
                e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
        }

        private void ganttControl_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            ganttControl.SetFocusedRowCellValue("Selected", e.Value);
            ganttControl.PostEditor();
            ganttControl.FocusedNode = null;                                                    
        }

        string hintText = "";
        private void ganttControl_TaskToolTipShowing(object sender, GanttTaskToolTipShowingEventArgs e)
        {
            /*
            var id = e.Node.GetValue("DBID").ToString();
            var obj = Data.Audit.Employees[id];             
            e.Text = obj.Name;            
            */

            e.Text = hintText;
        }

        private void ganttControl_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var startDate = ganttControl.GetChartVisibleStartDate();
                var finishDate = ganttControl.GetChartVisibleFinishDate();
                var visibleTicks = finishDate.Ticks - startDate.Ticks;

                int relativeX = e.Location.X - ganttControl.SplitterPosition - 4;
                int ganttWidth = ganttControl.Width - ganttControl.SplitterPosition - 24;

                double relativePos = 0.5;
                if (ganttWidth > 0)
                    relativePos = (double)relativeX / (double)ganttWidth;

                Int64 estimatedTick = (Int64)(visibleTicks * relativePos);
                DateTime estimatedDate = startDate.AddTicks(estimatedTick);

                //hintText = "relativeX:" + relativeX.ToString() + " - ganttWidth:" + ganttWidth.ToString() + " - " + startDate.ToString("dd/MM/yyyy HH:mm:ss") + " - " +  estimatedDate.ToString("dd/MM/yyyy HH:mm:ss") + " - " + finishDate.ToString("dd/MM/yyyy HH:mm:ss");


                GanttControlHitInfo hitInfo = ganttControl.CalcHitInfo(e.Location);
                if (hitInfo != null)
                {
                    GanttChartHitTest hitTest = hitInfo.ChartHitTest;
                    if (hitTest != null)
                    {
                        RowInfo info = hitTest.RowInfo;
                        if (info != null)
                        {
                            var employeeId = info.Node.GetValue("DBID").ToString();
                            //var employee = Data.Audit.Employees[employeeId];
                            string list = "";
                            foreach (var task in Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.EmployeeId == employeeId && p.AllocDate == estimatedDate.Date))
                            {
                                list += (list == "" ? "" : Environment.NewLine) + task.Project.Name;
                            }

                            labelControl1.Text = list;
                        }
                    }
                }

                labelControl1.Visible = labelControl1.Text != "";
                labelControl1.Left = ganttControl.Left + e.Location.X + 6;
                labelControl1.Top = ganttControl.Top + e.Location.Y - labelControl1.Height - 4;
            }
            catch
            {
                // ¿?
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            //ganttControl.SetChartVisibleRange(striplineEnd.AddDays(-15), striplineEnd.AddDays(15));            
            //ganttControl.ScrollChartToDate(striplineEnd.AddDays(-15));
        }

        private void ganttControl_Load(object sender, EventArgs e)
        {

            /*
            ganttControl.ZoomMode = GanttZoomMode.FixedIntervals;
            ganttControl.ZoomIn();
            ganttControl.ZoomIn();
            ganttControl.ZoomIn();
            ganttControl.ZoomMode = GanttZoomMode.Smooth;
            */
        }


        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Yo


            if (Data.Audit.Me == null)
                return;

            string filterString = "[Employee] = '" + Data.Audit.Me.Name + "'";
            

            if (!ganttControl.ActiveFilterString.Contains(filterString))
            {                
                string newFilter = "";
                if (ganttControl.ActiveFilterString != "")
                    newFilter = "(" + ganttControl.ActiveFilterString + ") And ";
                newFilter += filterString;

                ganttControl.ActiveFilterString = newFilter;
            }

            employeeBarEditItem.EditValue = Data.Audit.Me.Name;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Mi Oficina

            if (Data.Audit.Me == null)
                return;

            string filterString = "[BranchOffice] = '" + Data.Audit.Me.BranchOffice + "'";
            if (!ganttControl.ActiveFilterString.Contains(filterString))
            {
                string newFilter = "";
                if (ganttControl.ActiveFilterString != "")
                    newFilter = "(" + ganttControl.ActiveFilterString + ") And ";
                newFilter += filterString;

                ganttControl.ActiveFilterString = newFilter;
            }
        }

        private void ejercicioRepositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ejercicioBarEditItem.EditValue.ToString() == "")
                return;


            int year = DateTime.Now.Month <= 8 ? (int.Parse(ejercicioBarEditItem.EditValue.ToString()) - 1) : int.Parse(ejercicioBarEditItem.EditValue.ToString());
            ganttStartDate = new DateTime(year, 9, 1);
            ganttFinishDate = new DateTime(year + 1, 8, 31);

            LoadGantt();
           
            string taxYear = ejercicioBarEditItem.EditValue.ToString();
        }

        private void employeeRepositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (employeeBarEditItem.EditValue.ToString() == "")
                return;

            string filterString = "[Employee] = '" + employeeBarEditItem.EditValue + "'";

            if (ganttControl.ActiveFilterString.Contains("[Employee]"))
            {
                var ixEmployee = ganttControl.ActiveFilterString.IndexOf("[Employee]");
                if (ganttControl.ActiveFilterString.IndexOf("[Employee]", ixEmployee + 1) > 0)
                {
                    ganttControl.ActiveFilterString = filterString;
                }
                else
                {
                    var ixMark1 = ganttControl.ActiveFilterString.IndexOf("'", ixEmployee);
                    var ixMark2 = ganttControl.ActiveFilterString.IndexOf("'", ixMark1 + 1);
                    string newFilter = ganttControl.ActiveFilterString.Remove(ixEmployee, ixMark2 - ixEmployee + 1);
                    newFilter = newFilter.Insert(ixEmployee, filterString);
                    ganttControl.ActiveFilterString = newFilter;
                }
            }
            else
            {
                ganttControl.ActiveFilterString += (ganttControl.ActiveFilterString != "" ? " And " : "") + filterString;
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ganttControl.ActiveFilterString = "";
            InitRibbonFilters();
            LoadGantt();
        }
    }



    public enum GanttNodeType { Folder, Project, Employee, Task };

    public class GanttNode
    {
        public GanttNode(GanttNodeType ganttNodeType)
        {
            GanttNodeType = ganttNodeType;
            Predecessors = new List<string>();
        }
        public GanttNodeType GanttNodeType { get; set; }
        public string ParentUID { get; set; }
        public string BranchOffice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime? BaselineStartDate { get; set; }
        public DateTime? BaselineFinishDate { get; set; }
        public string Name { get; set; }
        public string UID { get; set; }
        public string DBID { get; set; }
        public List<string> Predecessors { get; set; }

        public string Project { get; set; }
        public string Employee { get; set; }
        public string Resources { get; set; }

        public double Progress { get; set; }

        public string ProjectID { get; set; }
        public string EmployeeID { get; set; }
        public string TaskID { get; set; }

        public bool Selected { get; set; }
        public string Category { get; set; }

        public string Hint { get; set; }
    }
}



