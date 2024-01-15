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
using System.Globalization;
using Audalia.DataHUBClient;
using System.Threading;
using Audalia.DataHUBCommon;
using DevExpress.Utils.Menu;

namespace WindowsApplication
{
    public partial class GanttUserControl : DevExpress.XtraEditors.XtraUserControl
    {

        IList<GanttNode> GanttNodes;

        bool groupByProject = false;
        bool showEmptyEmployees = true;
        string selectedTaskDBID = "";
        string selectedProjectDBID = "";
        string selectedEmployeeDBID = "";
        string currentGanttFilter = "";

        DateTime ganttStartDate;
        DateTime ganttFinishDate;

        Employee me;

        public GanttUserControl()
        {
            InitializeComponent();

            //

            me = Data.Audit.Me;

            int year = DateTime.Now.Month <= 8 ? (DateTime.Now.Year - 1) : DateTime.Now.Year;
            ganttStartDate = new DateTime(year, 9, 1);
            ganttFinishDate = VisibleFinishDate(new DateTime(year + 1, 8, 31));

            /*
            if (DateTime.Now.Month < 9)
            {
                ganttStartDate = new DateTime(year - 1, 9, 1);                
                ganttFinishDate = new DateTime(year + 1, 8, 31);
            }
            else
            {
                ganttStartDate = new DateTime(year, 9, 1);
                ganttFinishDate = new DateTime(year + 2, 8, 31);
            }
            */

            ejercicioBarEditItem.EditValue = DateTime.Now.Month <= 8 ? (DateTime.Now.Year - 1).ToString() : DateTime.Now.Year.ToString();
            ejercicioRepositoryItemComboBox_SelectedIndexChanged(ejercicioRepositoryItemComboBox, null);

            Init();
            ganttControl.TreeListPopupMenuShowing += GanttControl_TreeListPopupMenuShowing;

            Application.OpenForms[0].FormClosing += GanttUserControl_FormClosing;                
        }

        void Init()
        {

            ganttControl.OptionsView.ShowBaselines = false; //true;
            ganttControl.OptionsBehavior.ScheduleMode = ScheduleMode.Manual;

            /*
            ganttControl.OptionsBehavior.ReadOnly = false;
            ganttControl.OptionsBehavior.Editable = true;
            ganttControl.OptionsCustomization.AllowModifyTasks = DefaultBoolean.True;
            ganttControl.OptionsCustomization.AllowModifyDependencies = DefaultBoolean.False;
            ganttControl.OptionsCustomization.AllowModifyProgress = DefaultBoolean.False;
            */
            ganttControl.OptionsBehavior.ReadOnly = true;
            ganttControl.OptionsBehavior.Editable = false;
            ganttControl.OptionsCustomization.AllowModifyTasks = DefaultBoolean.False;
            ganttControl.OptionsCustomization.AllowModifyDependencies = DefaultBoolean.False;
            ganttControl.OptionsCustomization.AllowModifyProgress = DefaultBoolean.False;

            ganttControl.TreeListMappings.ParentFieldName = "ParentUID";
            ganttControl.TreeListMappings.KeyFieldName = "UID";
            ganttControl.ChartMappings.TextFieldName = "Resources";
            ganttControl.ChartMappings.InteractionTooltipTextFieldName = "Name";
            ganttControl.ChartMappings.DurationFieldName = "Duration";

            ganttControl.ChartMappings.BaselineStartDateFieldName = "BaselineStartDate";
            ganttControl.ChartMappings.BaselineFinishDateFieldName = "BaselineFinishDate";
            ganttControl.ChartMappings.BaselineDurationFieldName = "BaselineDuration";

            //

            if (Data.Audit.UserPermissions <= UserPermissions.Read)
            {
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = false;
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = false;

                ganttControl.OptionsBehavior.ReadOnly = true;
                ganttControl.OptionsBehavior.Editable = false;
                ganttControl.OptionsCustomization.AllowModifyTasks = DefaultBoolean.False;
            }

            /*
            ganttControl.ChartStartDate = new DateTime(DateTime.Now.Year, 1, 1).AddMonths(-1);
            ganttControl.ChartFinishDate = new DateTime(DateTime.Now.Year, 12, 31).AddMonths(1);
            */

            DateTime deadLine = DateTime.Now;
            ganttControl.CustomDrawTimescaleColumn += (sender, e) =>
            {
                GanttTimescaleColumn column = e.Column;
                e.DrawBackground();


                if (column.StartDate.DayOfWeek == DayOfWeek.Saturday || column.StartDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    //e.DrawBackground();

                    float x1 = (float)e.GetPosition(column.StartDate);
                    float x2 = (float)Math.Min(e.GetPosition(column.StartDate.AddDays(1)), column.Bounds.Right);
                    float width = x2 - x1;// - 1;

                    RectangleF rect = new RectangleF(x1, column.Bounds.Y, width, column.Bounds.Height);
                    e.Cache.FillRectangle(Color.FromArgb(16, 0, 0, 0), rect);

                    //e.DrawHeader();
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

                        RectangleF rect = new RectangleF(x1, column.Bounds.Y, width, column.Bounds.Height);
                        //e.Cache.FillRectangle(Color.FromArgb(24, 255, 0, 0), rect);
                        e.Cache.FillRectangle(Color.FromArgb(255, 255, 224, 224), rect);
                        //e.DrawHeader();
                        e.Handled = true;
                    }
                }

                //Now Stripline
                if (column.StartDate <= deadLine && column.FinishDate >= deadLine)
                {
                    //e.DrawBackground();

                    float x = (float)e.GetPosition(deadLine);
                    float width = 1;
                    //float width = 2;

                    RectangleF rect = new RectangleF(x, column.Bounds.Y, width, column.Bounds.Height);
                    e.Cache.FillRectangle(Color.FromArgb(192, 255, 0, 0), rect);

                    rect = new RectangleF(x + 1, column.Bounds.Y, width, column.Bounds.Height);
                    e.Cache.FillRectangle(Color.FromArgb(192, 128, 0, 0), rect);

                    //e.DrawHeader();
                    e.Handled = true;
                }

                e.DrawHeader();
            };

            ganttControl.CustomDrawTask += (sender, e) =>
            {
                try
                {
                    switch ((GanttNodeType)e.Node.GetValue("GanttNodeType"))
                    {
                        case GanttNodeType.Project:
                            {
                                if (!Data.Audit.Projects.ContainsKey(e.Node.GetValue("ProjectID").ToString()))
                                {
                                    return;
                                }

                                var currentProject = Data.Audit.Projects[e.Node.GetValue("ProjectID").ToString()];
                                var shape = e.Info.ShapeBounds;

                                //Primera fase
                                {
                                    float start = (float)e.GetPosition(currentProject.StartDate1);
                                    float finish = (float)e.GetPosition(VisibleFinishDate(currentProject.FinishDate1.AddDays(1)));
                                    var shape2 = e.Info.ShapeBounds;
                                    shape2.X = start;
                                    shape2.Width = finish - start;


                                    Brush textBrush = e.Cache.GetSolidBrush(Color.Gray);

                                    var font = new Font(e.Appearance.Font, FontStyle.Bold);
                                    var textSize = e.Cache.CalcTextSize(currentProject.Name, font);

                                    //RectangleF rect = new RectangleF(shape.X + shape.Width + 6, shape.Y + 2, shape.X + shape.Width + 6 + textSize.Width, shape.Height + 2);
                                    RectangleF rect = new RectangleF(shape2.X + shape2.Width + 6, shape2.Y + 2, shape2.X + shape2.Width + 6 + textSize.Width, shape2.Height + 2);
                                    e.Cache.DrawString(currentProject.Name, font, textBrush, rect);

                                    e.Appearance.BackColor = Color.Navy; //Color.Black;
                                    e.Appearance.ProgressColor = Color.Navy; // Color.Black;
                                    e.Appearance.BaselineColor = Color.FromArgb(255, 255, 128, 0);

                                    e.DrawShape(shape2, 0);
                                    //e.DrawBaseline();
                                }


                                //Segunda fase
                                {
                                    if (currentProject.StartDate2 != null)
                                    {
                                        float start = (float)e.GetPosition(currentProject.StartDate2.Value);
                                        float finish = (float)e.GetPosition(VisibleFinishDate(currentProject.FinishDate2.Value.AddDays(1)));
                                        var shape2 = e.Info.ShapeBounds;
                                        shape2.X = start;
                                        shape2.Width = finish - start;

                                        Brush textBrush = e.Cache.GetSolidBrush(Color.Gray);
                                        var font = new Font(e.Appearance.Font, FontStyle.Bold);
                                        var rText = currentProject.Name + " - 2ª fase";
                                        var textSize = e.Cache.CalcTextSize(rText, font);
                                        RectangleF rect = new RectangleF(shape2.X + shape2.Width + 6, shape2.Y + 2, shape2.X + shape2.Width + 6 + textSize.Width, shape2.Height + 2);
                                        e.Cache.DrawString(rText, font, textBrush, rect);

                                        e.Appearance.BackColor = Color.Navy; //Color.Black;
                                        e.Appearance.ProgressColor = Color.Navy; // Color.Black;
                                        e.Appearance.BaselineColor = Color.FromArgb(255, 255, 128, 0);

                                        e.DrawShape(shape2, 0);
                                        //e.DrawBaseline();
                                    }
                                }

                                //Tercera fase
                                {

                                    if (currentProject.StartDate3 != null)
                                    {
                                        float start = (float)e.GetPosition(currentProject.StartDate3.Value);
                                        float finish = (float)e.GetPosition(VisibleFinishDate(currentProject.FinishDate3.Value.AddDays(1)));
                                        var shape2 = e.Info.ShapeBounds;
                                        shape2.X = start;
                                        shape2.Width = finish - start;

                                        Brush textBrush = e.Cache.GetSolidBrush(Color.Gray);
                                        var font = new Font(e.Appearance.Font, FontStyle.Bold);
                                        var rText = currentProject.Name + " - 3ª fase";
                                        var textSize = e.Cache.CalcTextSize(rText, font);
                                        RectangleF rect = new RectangleF(shape2.X + shape2.Width + 6, shape2.Y + 2, shape2.X + shape2.Width + 6 + textSize.Width, shape2.Height + 2);
                                        e.Cache.DrawString(rText, font, textBrush, rect);

                                        e.Appearance.BackColor = Color.Navy; //Color.Black;
                                        e.Appearance.ProgressColor = Color.Navy; // Color.Black;
                                        e.Appearance.BaselineColor = Color.FromArgb(255, 255, 128, 0);

                                        e.DrawShape(shape2, 0);
                                        //e.DrawBaseline();
                                    }
                                }



                                foreach (var alloc in Data.Audit.ProjectEmployeeAllocs.Where(p => p.Value.ProjectId == currentProject.DBID).OrderBy(p => p.Value.AllocDate))
                                {
                                    if (ShowAlloc(alloc.Value))
                                    {
                                        e.Appearance.BaselineColor = Color.FromArgb(255, 0, 64, 128);

                                        float start = (float)e.GetPosition(alloc.Value.AllocDate);
                                        float finish = (float)e.GetPosition(alloc.Value.AllocDate.AddDays(1));

                                        //RectangleF rect1 = new RectangleF(start + 1, shape.Y + 5, finish - start - 1, 4);
                                        RectangleF rect1 = new RectangleF(start + 1, shape.Y + 3, finish - start - 1, 6);
                                        //e.Cache.FillRectangle(Color.FromArgb(255, 0, 0, 128), rect1);
                                        e.Cache.FillRectangle(Color.FromArgb(255, 64, 64, 160), rect1);
                                    }
                                }

                                //DrawLeftText
                                {
                                    Brush textBrush = e.Cache.GetSolidBrush(Color.Gray);
                                    var leftText = e.Node.GetValue("Resources").ToString();
                                    var font = new Font(e.Appearance.Font, FontStyle.Regular);
                                    var textSize = e.Cache.CalcTextSize(leftText, font);

                                    RectangleF rect = new RectangleF(shape.X - textSize.Width - 6, shape.Y, shape.X, shape.Height + 2);
                                    e.Cache.DrawString(leftText, font, textBrush, rect);
                                    //e.DrawLeftText(); 
                                }

                                /*
                                //DrawRightText
                                {
                                    var project = Data.Audit.Projects[e.Node.GetValue("ProjectID").ToString()];

                                    Brush textBrush = e.Cache.GetSolidBrush(Color.Gray);
                                    var text = project.Name;
                                    var font = new Font(e.Appearance.Font, FontStyle.Regular);
                                    var textSize = e.Cache.CalcTextSize(text, font);
                                    RectangleF rect = new RectangleF(shape.X + shape.Width + 10, shape.Y, textSize.Width, shape.Height + 2);
                                    e.Cache.DrawString(text, font, textBrush, rect);
                                    //e.DrawRightText();
                                }
                                */

                                e.Handled = true;
                            }

                            break;
                        case GanttNodeType.Task:
                            {
                                //var currentTask = AuditData.Tasks.FirstOrDefault(p => p.DBID == e.Node.GetValue("DBID").ToString());
                                if (!Data.Audit.Projects.ContainsKey(e.Node.GetValue("ProjectID").ToString()))
                                {
                                    return;
                                }

                                var currentTask = Data.Audit.Projects[e.Node.GetValue("ProjectID").ToString()];

                                //Selected nodes

                                bool highlightedTask = false;

                                if (selectedTaskDBID != "")
                                {
                                    //highlightedTask = currentTask.DBID == selectedTaskDBID;

                                    if (groupByProject)
                                    {
                                        var employee = Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].Employee;
                                        string taskUID = e.Node.GetValue("UID").ToString();
                                        highlightedTask = taskUID.Contains(@"\EMPLOYEE_" + employee.DBID);
                                    }
                                    else
                                    {
                                        if (Data.Audit.ProjectEmployeeAssocs.ContainsKey(selectedTaskDBID))
                                        {
                                            var project = Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].Project;
                                            highlightedTask = currentTask.DBID == project.DBID;
                                        }
                                    }

                                }
                                else
                                {
                                    if (selectedProjectDBID != "")
                                    {
                                        //var project = AuditData.Projects.FirstOrDefault(p => p.DBID == selectedProjectDBID);
                                        if (Data.Audit.Projects.ContainsKey(selectedProjectDBID))
                                        {
                                            var project = Data.Audit.Projects[selectedProjectDBID];
                                            highlightedTask = currentTask.DBID == project.DBID;
                                        }
                                    }
                                    else
                                    {
                                        if (selectedEmployeeDBID != "")
                                        {
                                            //var employee = AuditData.Employees.FirstOrDefault(p => p.DBID == selectedEmployeeDBID);
                                            if (Data.Audit.Employees.ContainsKey(selectedEmployeeDBID))
                                            {
                                                var employee = Data.Audit.Employees[selectedEmployeeDBID];
                                                string taskUID = e.Node.GetValue("UID").ToString();
                                                highlightedTask = taskUID.Contains(@"\EMPLOYEE_" + employee.DBID);
                                            }
                                        }
                                    }
                                }

                                //Overlapped tasks

                                bool overlappedTask = false;
                                /*
                                var nodeEmployeeID = e.Node.GetValue("EmployeeID").ToString();
                                if (nodeEmployeeID != "")
                                {
                                    //var nodeEmployee = AuditData.Employees.FirstOrDefault(p => p.DBID == nodeEmployeeID);
                                    var nodeEmployee = Data.Audit.Employees[nodeEmployeeID];

                                    foreach (var taskEmployee in nodeEmployee.Projects)
                                    {
                                        if (currentTask.DBID != taskEmployee.Project.DBID && currentTask.FinishDate > taskEmployee.Project.StartDate && currentTask.StartDate < taskEmployee.Project.FinishDate)
                                        {
                                            overlappedTask = true;
                                            break;
                                        }
                                    }
                                }
                                */


                                //Precedence

                                bool precedenceBreach = false;
                                /*
                                var predecesors = (List<string>)e.Node.GetValue("Predecessors");
                                foreach (var predecesorUID in predecesors)
                                {
                                    var predecesorNode = GanttNodes.FirstOrDefault(p => p.UID == predecesorUID);
                                    //var predecesorTask = AuditData.Tasks.FirstOrDefault(p => p.DBID == predecesorNode.DBID);
                                    var predecesorTask = Data.Audit.Projects[predecesorNode.DBID];
                                    if (currentTask.StartDate < predecesorTask.FinishDate)
                                    {
                                        precedenceBreach = true;
                                    }
                                }
                                */


                                //

                                var borderColor = Color.FromArgb(255, 0, 64, 128);
                                var backColor = Color.FromArgb(255, 0, 64, 128);

                                if (highlightedTask)
                                {
                                    borderColor = Color.FromArgb(255, 128, 255, 128);
                                }

                                if (overlappedTask)
                                {
                                    backColor = Color.FromArgb(255, 255, 128, 0);
                                }

                                if (precedenceBreach)
                                {
                                    backColor = Color.FromArgb(255, 255, 0, 0);
                                }

                                //Draw node

                                var shape = e.Info.ShapeBounds;

                                if (highlightedTask)
                                {
                                    //RectangleF rect1 = new RectangleF(shape.X - 4, shape.Y - 4, shape.Width + 8, shape.Height + 8);
                                    //Rectangle rect1 = new Rectangle((int)(shape.X - 2), (int)(shape.Y - 2), (int)(shape.Width + 4), (int)(shape.Height + 4));
                                    //e.Appearance.BackColor = borderColor;
                                    //e.Appearance.ProgressColor = borderColor;
                                    //e.DrawShape(rect1, 100);
                                    //e.Cache.DrawRectangle(new Pen(borderColor), rect1);

                                    Rectangle rect;
                                    Color color;

                                    rect = new Rectangle((int)(shape.X - 2), (int)(shape.Y - 2), (int)(shape.Width + 4), (int)(shape.Height + 4));
                                    color = Color.FromArgb(255, borderColor);
                                    e.Cache.DrawRectangle(rect, color, 1);

                                    rect = new Rectangle((int)(shape.X - 3), (int)(shape.Y - 3), (int)(shape.Width + 6), (int)(shape.Height + 6));
                                    color = Color.FromArgb(128, borderColor);
                                    e.Cache.DrawRectangle(rect, color, 1);

                                    rect = new Rectangle((int)(shape.X - 4), (int)(shape.Y - 4), (int)(shape.Width + 8), (int)(shape.Height + 8));
                                    color = Color.FromArgb(64, borderColor);
                                    e.Cache.DrawRectangle(rect, color, 1);


                                    /*
                                    RectangleF rect2 = new RectangleF(shape.X - 1, shape.Y - 1, shape.Width + 2, shape.Height + 2);
                                    e.Appearance.BackColor = Color.Red; //Color.White;
                                    e.Appearance.ProgressColor = Color.White;
                                    e.DrawShape(rect2, 100);
                                    */
                                }

                                /*
                                {
                                    RectangleF rect3 = new RectangleF(shape.X - 1, shape.Y - 1, shape.Width + 2, shape.Height + 2);
                                    e.Appearance.BackColor = Color.FromArgb(192, 192, 192, 224);
                                    e.Appearance.ProgressColor = Color.FromArgb(192, 192, 192, 224);
                                    e.DrawShape(rect3, 100);
                                }
                                */

                                //Primera Fase
                                {
                                    if (ShowPeriod(currentTask.StartDate1, currentTask.FinishDate1))
                                    {
                                        float start = (float)e.GetPosition(currentTask.StartDate1);
                                        float finish = (float)e.GetPosition(VisibleFinishDate(currentTask.FinishDate1.AddDays(1)));
                                        var shape2 = e.Info.ShapeBounds;
                                        shape2.X = start;
                                        shape2.Width = finish - start;

                                        RectangleF rect3 = new RectangleF(shape2.X - 1, shape2.Y - 1, shape2.Width + 2, shape2.Height + 2);
                                        e.Appearance.BackColor = Color.FromArgb(255, 192, 192, 224);
                                        e.Appearance.ProgressColor = Color.FromArgb(255, 192, 192, 224);
                                        e.DrawShape(rect3, 100);
                                    }
                                }

                                //Segunda Fase
                                if (currentTask.StartDate2 != null)
                                {
                                    if (ShowPeriod(currentTask.StartDate2 ?? DateTime.Now, currentTask.FinishDate2 ??  DateTime.Now))
                                    {
                                        float start = (float)e.GetPosition(currentTask.StartDate2.Value);
                                        float finish = (float)e.GetPosition(VisibleFinishDate(currentTask.FinishDate2.Value.AddDays(1)));
                                        var shape2 = e.Info.ShapeBounds;
                                        shape2.X = start;
                                        shape2.Width = finish - start;

                                        RectangleF rect3 = new RectangleF(shape2.X - 1, shape2.Y - 1, shape2.Width + 2, shape2.Height + 2);
                                        e.Appearance.BackColor = Color.FromArgb(255, 192, 192, 224);
                                        e.Appearance.ProgressColor = Color.FromArgb(255, 192, 192, 224);
                                        e.DrawShape(rect3, 100);
                                    }
                                }

                                //Tercera Fase
                                if (currentTask.StartDate3 != null)
                                {
                                    if (ShowPeriod(currentTask.StartDate3 ?? DateTime.Now, currentTask.FinishDate3 ?? DateTime.Now))
                                    {
                                        float start = (float)e.GetPosition(currentTask.StartDate3.Value);
                                        float finish = (float)e.GetPosition(VisibleFinishDate(currentTask.FinishDate3.Value.AddDays(1)));
                                        var shape2 = e.Info.ShapeBounds;
                                        shape2.X = start;
                                        shape2.Width = finish - start;

                                        RectangleF rect3 = new RectangleF(shape2.X - 1, shape2.Y - 1, shape2.Width + 2, shape2.Height + 2);
                                        e.Appearance.BackColor = Color.FromArgb(255, 192, 192, 224);
                                        e.Appearance.ProgressColor = Color.FromArgb(255, 192, 192, 224);
                                        e.DrawShape(rect3, 100);
                                    }
                                }

                                //Draw periods

                                if (e.Node.GetValue("EmployeeID").ToString() != "")
                                {
                                    if (!Data.Audit.Projects.ContainsKey(e.Node.GetValue("ProjectID").ToString()))
                                    {
                                        return;
                                    }

                                    if (!Data.Audit.Employees.ContainsKey(e.Node.GetValue("EmployeeID").ToString()))
                                    {
                                        return;
                                    }

                                    var project = Data.Audit.Projects[e.Node.GetValue("ProjectID").ToString()];
                                    var employee = Data.Audit.Employees[e.Node.GetValue("EmployeeID").ToString()];
                                    var allocs = Data.Audit.ProjectEmployeeAllocs.Where(p => p.Value.ProjectId == project.DBID && p.Value.EmployeeId == employee.DBID).OrderBy(p => p.Value.AllocDate);

                                    foreach (var alloc in allocs)
                                    {
                                        if (ShowAlloc(alloc.Value))
                                        {
                                            backColor = Color.FromArgb(255, 0, 64, 128);

                                            if (Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.DBID != alloc.Value.DBID && p.EmployeeId == alloc.Value.EmployeeId && p.AllocDate == alloc.Value.AllocDate).Count() > 0)
                                            {
                                                //backColor = Color.FromArgb(255, 255, 128, 0);
                                                //backColor = Color.FromArgb(255, 255, 64, 255);
                                                backColor = Color.FromArgb(255, 255, 128, 0);
                                            }


                                            /*
                                            var backColor = Color.FromArgb(255, 0, 64, 128);                                        

                                            if (overlappedTask)
                                            {
                                                backColor = Color.FromArgb(255, 255, 128, 0);
                                            }
                                            */

                                            //Employee leaving date
                                            if (employee.LeavingDate <= alloc.Value.AllocDate)
                                            {
                                                //backColor = Color.FromArgb(255, 255, 128, 0);
                                                //backColor = Color.FromArgb(255, 255, 64, 255);
                                                backColor = Color.FromArgb(255, 255, 128, 0);
                                            }

                                            float start = (float)e.GetPosition(alloc.Value.AllocDate) - 1;
                                            float finish = (float)e.GetPosition(VisibleFinishDate(alloc.Value.AllocDate.AddDays(1)));
                                            RectangleF rect1 = new RectangleF(start + 1, shape.Y - 1, finish - start - 1, shape.Height + 2);
                                            e.Cache.FillRectangle(backColor, rect1);
                                        }
                                    }
                                }

                                //

                                e.DrawInsideText();

                                //DrawLeftText
                                {
                                    Brush textBrush = e.Cache.GetSolidBrush(Color.Gray);
                                    var leftText = e.Node.GetValue("Resources").ToString();
                                    var font = new Font(e.Appearance.Font, FontStyle.Regular);
                                    var textSize = e.Cache.CalcTextSize(leftText, font);

                                    RectangleF rect = new RectangleF(shape.X - textSize.Width - 6, shape.Y, shape.X, shape.Height + 2);
                                    e.Cache.DrawString(leftText, font, textBrush, rect);
                                    //e.DrawLeftText(); 
                                }

                                //DrawRightText
                                if (!groupByProject)
                                {
                                    var project = Data.Audit.Projects[e.Node.GetValue("ProjectID").ToString()];

                                    Brush textBrush = e.Cache.GetSolidBrush(Color.Gray);
                                    //Brush textBrush = e.Cache.GetSolidBrush(Color.Fuchsia);
                                    var text = project.Name;
                                    var font = new Font(e.Appearance.Font, FontStyle.Regular);
                                    var textSize = e.Cache.CalcTextSize(text, font);
                                    RectangleF rect = new RectangleF(shape.X + shape.Width + 10, shape.Y, textSize.Width, shape.Height + 2);
                                    e.Cache.DrawString(text, font, textBrush, rect);
                                    //e.DrawRightText();
                                }

                                /*
                                e.Appearance.BaselineColor = Color.FromArgb(255, 255, 128, 0);  //Color.FromArgb(255, 0, 64, 128);
                                var baselineRect = new RectangleF(e.Info.BaselineBounds.X, e.Info.BaselineBounds.Y + 2, e.Info.BaselineBounds.Width, e.Info.BaselineBounds.Height);
                                e.DrawBaseline(ºneRect);
                                */

                                //                              

                                e.Handled = true;
                            }

                            break;
                        case GanttNodeType.Employee:
                            {
                                //var currentEmployee = AuditData.Employees.FirstOrDefault(p => p.DBID == e.Node.GetValue("DBID").ToString());
                                if (!Data.Audit.Employees.ContainsKey(e.Node.GetValue("EmployeeID").ToString()))
                                {
                                    return;
                                }

                                var currentEmployee = Data.Audit.Employees[e.Node.GetValue("EmployeeID").ToString()];

                                //Draw node

                                {
                                    var ganttStartDate = ganttControl.GetChartVisibleStartDate().TruncStartDate();
                                    var ganttFinishDate = ganttControl.GetChartVisibleFinishDate().TruncFinishDate();

                                    //e.Appearance.BaselineColor = Color.FromArgb(255, 0, 255, 0);
                                    e.Appearance.BaselineColor = Color.FromArgb(192, 0, 255, 0);

                                    float start = (float)e.GetPosition(ganttStartDate);
                                    float finish = (float)e.GetPosition(ganttFinishDate);

                                    var shape = e.Info.ShapeBounds;
                                    //RectangleF rect1 = new RectangleF(start, shape.Y + 5, finish - start, 2); 
                                    RectangleF rect1 = new RectangleF(start, shape.Y - 15, finish - start, 24);
                                    //e.DrawBaseline(rect1);
                                }

                                /*
                                //Draw leaves
                                {
                                    var ganttStartDate = ganttControl.GetChartVisibleStartDate().TruncStartDate();
                                    var ganttFinishDate = ganttControl.GetChartVisibleFinishDate().TruncFinishDate();

                                    if (currentEmployee.LeavingDate != null && currentEmployee.LeavingDate < ganttFinishDate)
                                    {
                                        float leavingStart = (float)e.GetPosition(currentEmployee.LeavingDate.Value);
                                        var shape = e.Info.ShapeBounds;
                                        RectangleF rect1 = new RectangleF(leavingStart, shape.Y + 3, shape.Width - leavingStart, 6);
                                        e.Cache.FillRectangle(Color.FromArgb(255, 0, 0, 0), rect1);
                                    }
                                }
                                */


                                //Draw projects                                
                                foreach (var alloc in Data.Audit.ProjectEmployeeAllocs.Where(p => p.Value.EmployeeId == currentEmployee.DBID && p.Value.ProjectId != "0").OrderBy(p => p.Value.AllocDate))
                                {
                                    e.Appearance.BaselineColor = Color.FromArgb(255, 0, 64, 128);
                                    float start = (float)e.GetPosition(alloc.Value.AllocDate);
                                    float finish = (float)e.GetPosition(VisibleFinishDate(alloc.Value.AllocDate.AddDays(1)));
                                    var shape = e.Info.ShapeBounds;

                                    Color backColor = Color.FromArgb(255, 64, 64, 160);

                                    //if (Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.DBID != alloc.Value.DBID && p.EmployeeId == alloc.Value.EmployeeId && p.AllocDate == alloc.Value.AllocDate).Count() > 0)                                    
                                    if (alloc.Value.IsOverlapped)
                                    {
                                        backColor = Color.FromArgb(255, 255, 128, 0);
                                    }

                                    RectangleF rect1 = new RectangleF(start + 1, shape.Y + 3, finish - start - 1, 6);
                                    e.Cache.FillRectangle(backColor, rect1);
                                }

                                //Draw holidays
                                foreach (var alloc in Data.Audit.ProjectEmployeeAllocs.Where(p => p.Value.ProjectId == "0" && p.Value.EmployeeId == currentEmployee.DBID).OrderBy(p => p.Value.AllocDate))
                                {
                                    if (alloc.Value.AllocDate.AddDays(1) <= VisibleFinishDate(alloc.Value.AllocDate.AddDays(1)))
                                    {
                                        float start = (float)e.GetPosition(alloc.Value.AllocDate);
                                        float finish = (float)e.GetPosition(VisibleFinishDate(alloc.Value.AllocDate.AddDays(1)));

                                        var shape = e.Info.ShapeBounds;
                                        Color backColor = Color.FromArgb(255, 192, 0, 96); //Color.FromArgb(255, 255, 0, 128);//Color.FromArgb(255, 192, 32, 32);
                                        RectangleF rect1 = new RectangleF(start + 1, shape.Y + 3, finish - start - 1, 6);
                                        e.Cache.FillRectangle(backColor, rect1);
                                    }
                                }

                                //Draw leaves
                                if (currentEmployee.LeavingDate != null)
                                {
                                    var ganttStartDate = ganttControl.GetChartVisibleStartDate().TruncStartDate();
                                    var ganttFinishDate = ganttControl.GetChartVisibleFinishDate().TruncFinishDate();
                                    for (var day = ganttStartDate; day.Date <= ganttFinishDate; day = day.AddDays(1))
                                    {
                                        if (day >= currentEmployee.LeavingDate && IsWorkingDay(day))
                                        {
                                            if (day <= VisibleFinishDate(day))
                                            {
                                                Color backColor = Color.FromArgb(255, 64, 64, 64); //Color.Black;
                                                if (Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.EmployeeId == currentEmployee.DBID && p.AllocDate == day).Count() > 0)
                                                {
                                                    //backColor = Color.FromArgb(255, 255, 64, 255);
                                                    backColor = Color.FromArgb(255, 255, 128, 0);
                                                }

                                                var shape = e.Info.ShapeBounds;
                                                float start = (float)e.GetPosition(day);
                                                float finish = (float)e.GetPosition(day.AddDays(1));
                                                RectangleF rect1 = new RectangleF(start + 1, shape.Y + 3, finish - start - 1, 6);
                                                e.Cache.FillRectangle(backColor, rect1);
                                            }
                                        }
                                    }
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
                e.LeftText = string.Empty;  //e.Node.GetValue("Resources").ToString();
                e.RightText = string.Empty; //e.Node.GetValue("Name").ToString();  //string.Empty;
                e.InsideText = string.Empty;  //e.Node.GetValue("Name").ToString();
            };

            //

            InitRibbonFilters();

            bool groupByProjectAux;
            if (bool.TryParse(Data.Audit.GetSetting("GroupByProject"), out groupByProjectAux))
                this.GroupByProject = groupByProjectAux;

            ganttControl.ActiveFilterString = Data.Audit.GetSetting("GanttFilter");
            ganttControl.CustomRowFilter += GanttControl_CustomRowFilter;

            //ganttControl.AfterExpand -= ganttControl_AfterExpand;
            //ganttControl.AfterCollapse -= ganttControl_AfterCollapse;

            LoadGantt();

            ganttControl.ExpandAll();
            ganttControl.CollapseAll();

            //RestoreExpandedNodes();
            RestoreLayout2();

            //ganttControl.TopVisibleNodeIndexChanged += GanttControl_TopVisibleNodeIndexChanged;
            //ganttControl.AfterExpand += ganttControl_AfterExpand;
            //ganttControl.AfterCollapse += ganttControl_AfterCollapse;

            Data.Audit.OnObjectMessage += Audit_OnObjectMessage;

            ganttControl.ScrollChartToDate(DateTime.Now);
            ganttControl.ZoomMode = GanttZoomMode.FixedIntervals;
            ganttControl.ZoomIn();
            ganttControl.ZoomIn();
            ganttControl.ZoomIn();
            ganttControl.ZoomMode = GanttZoomMode.Smooth;

            ganttControl.CreateControl();
            ganttControl.BeginInvoke((MethodInvoker)delegate
            {
                CenterAt(DateTime.Now);
            });

        }


        private void GanttUserControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveLayout2();
        }

        private void GanttControl_TreeListPopupMenuShowing(object sender, DevExpress.XtraTreeList.PopupMenuShowingEventArgs e)
        {
            if (selectedProjectDBID != "")
            {
                var project = Data.Audit.Projects[selectedProjectDBID];
                {
                    DXMenuItem customItem = new DXMenuItem("Filtrar participantes del proyecto " + project.Name);
                    customItem.BeginGroup = true;
                    customItem.Click += toolStripMenuItem1_Click;
                    e.Menu.Items.Add(customItem);
                }

                {
                    DXMenuItem customItem = new DXMenuItem("Asignar horas del proyecto " + project.Name);
                    customItem.Click += toolStripMenuItem2_Click;
                    e.Menu.Items.Add(customItem);
                }
            }
            else
            if (selectedTaskDBID != "")
            {
                if (groupByProject)
                {
                    var employee = Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].Employee;

                    {
                        DXMenuItem customItem = new DXMenuItem("Filtrar proyectos del auditor " + employee.Name);
                        customItem.BeginGroup = true;
                        customItem.Click += toolStripMenuItem1_Click;
                        e.Menu.Items.Add(customItem);
                    }

                    {
                        DXMenuItem customItem = new DXMenuItem("Asignar horas del auditor " + employee.Name);
                        customItem.Click += toolStripMenuItem2_Click;
                        e.Menu.Items.Add(customItem);
                    }
                }
                else
                {
                    var project = Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].Project;
                    {
                        DXMenuItem customItem = new DXMenuItem("Filtrar participantes del proyecto " + project.Name);
                        customItem.BeginGroup = true;
                        customItem.Click += toolStripMenuItem1_Click;
                        e.Menu.Items.Add(customItem);
                    }

                    {
                        DXMenuItem customItem = new DXMenuItem("Asignar horas del proyecto " + project.Name);
                        customItem.Click += toolStripMenuItem2_Click;
                        e.Menu.Items.Add(customItem);
                    }
                }
            }
            else
            if (selectedEmployeeDBID != "")
            {
                var employee = Data.Audit.Employees[selectedEmployeeDBID];

                {
                    DXMenuItem customItem = new DXMenuItem("Filtrar proyectos del auditor " + employee.Name);
                    customItem.BeginGroup = true;
                    customItem.Click += toolStripMenuItem1_Click;
                    e.Menu.Items.Add(customItem);
                }

                {
                    DXMenuItem customItem = new DXMenuItem("Asignar horas del auditor " + employee.Name);
                    customItem.Click += toolStripMenuItem2_Click;
                    e.Menu.Items.Add(customItem);
                }
            }

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (selectedProjectDBID != "")
            {
                var project = Data.Audit.Projects[selectedProjectDBID];
                contextMenuStrip1.Items[0].Text = "Filtrar participantes del proyecto " + project.Name;
                contextMenuStrip1.Items[1].Text = "Asignar horas del proyecto " + project.Name;
            }
            else
            if (selectedTaskDBID != "")
            {
                if (groupByProject)
                {
                    var employee = Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].Employee;
                    contextMenuStrip1.Items[0].Text = "Filtrar proyectos del auditor " + employee.Name;
                    contextMenuStrip1.Items[1].Text = "Asignar horas del auditor " + employee.Name;
                }
                else
                {
                    var project = Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].Project;
                    contextMenuStrip1.Items[0].Text = "Filtrar participantes del proyecto " + project.Name;
                    contextMenuStrip1.Items[1].Text = "Asignar horas del proyecto " + project.Name;
                }
            }
            else
            if (selectedEmployeeDBID != "")
            {
                var employee = Data.Audit.Employees[selectedEmployeeDBID];
                contextMenuStrip1.Items[0].Text = "Filtrar proyectos del auditor " + employee.Name;
                contextMenuStrip1.Items[1].Text = "Asignar horas del auditor " + employee.Name;
            }
        }

        public bool GroupByProject
        {
            get { return groupByProject; }
            set
            {
                if (groupByProject != value)
                {
                    SaveLayout2();
                    groupByProject = value;
                    barCheckItem2.Checked = this.GroupByProject;
                    Data.Audit.PostSetting("GroupByProject", groupByProject.ToString());
                    LoadGantt();
                    RestoreLayout2();
                }
            }
        }

        private void AddGanttProject(Project project)
        {
            var startDate = project.StartDate;
            var finishDate = VisibleFinishDate(project.FinishDate.AddDays(1));

            DateTime? startDate1 = project.StartDate1;
            DateTime? finishDate1 = VisibleFinishDate(project.FinishDate1.AddDays(1));

            DateTime? startDate2 = project.StartDate2;
            DateTime? finishDate2 = null;
            if (project.FinishDate2 != null)
                finishDate2 = VisibleFinishDate(project.FinishDate2.Value.AddDays(1));

            DateTime? startDate3 = project.StartDate3;
            DateTime? finishDate3 = null;
            if (project.FinishDate3 != null)
                finishDate3 = VisibleFinishDate(project.FinishDate3.Value.AddDays(1));

            var allResources = "";

            foreach (var projectEmployeeAssoc in project.Employees.OrderBy(p => p.Employee.Name))
            {
                allResources += ((allResources == "") ? "" : " - ") + projectEmployeeAssoc.Employee.Name;
            }

            GanttNodes.Add(new GanttNode(GanttNodeType.Project)
            {
                UID = @"\PROJECT_" + project.DBID,
                ParentUID = "",
                //DBID = project.DBID,
                Name = project.Name + " (" + project.TaxYear.ToString() +")",
                BranchOffice = project.BranchOffice,

                StartDate = startDate,
                FinishDate = finishDate,

                StartDate1 = startDate1,
                FinishDate1 = finishDate1,
                StartDate2 = startDate2,
                FinishDate2 = finishDate2,
                StartDate3 = startDate3,
                FinishDate3 = finishDate3,

                Duration = finishDate - startDate,
                Progress = 100,
                Predecessors = new List<string>(),

                BaselineStartDate = project.BaselineStartDate,
                BaselineFinishDate = project.BaselineFinishDate.AddDays(1),
                BaselineDuration = project.BaselineFinishDate.AddDays(1) - project.BaselineStartDate,

                ProjectID = project.DBID,
                EmployeeID = "",
                TaskID = "",

                Project = project.Name,
                TaxYear = project.TaxYear,
                Employee = "",
                Resources = allResources
            });
        }

        private void AddGanttEmployee(Employee employee)
        {
            int index = GanttNodes.Count;
            for (int i = 0; i < GanttNodes.Count; i++)
            {
                if (GanttNodes[i].GanttNodeType == GanttNodeType.Employee && String.Compare(GanttNodes[i].Name, employee.Name, StringComparison.CurrentCulture) > 0)
                {
                    index = i;
                    break;
                }
            }

            GanttNodes.Insert(index, new GanttNode(GanttNodeType.Employee)
            //GanttNodes.Add(new GanttNode(GanttNodeType.Employee)
            {
                UID = @"\EMPLOYEE_" + employee.DBID,
                ParentUID = "",
                //DBID = employee.DBID,
                Name = employee.Name,

                BranchOffice = employee.BranchOffice,

                StartDate = ganttStartDate,
                FinishDate = ganttFinishDate,

                StartDate1 = ganttStartDate,
                FinishDate1 = ganttFinishDate,
                StartDate2 = ganttStartDate,
                FinishDate2 = ganttFinishDate,
                StartDate3 = ganttStartDate,
                FinishDate3 = ganttFinishDate,

                BaselineStartDate = ganttStartDate,
                BaselineFinishDate = ganttFinishDate,

                Progress = 100,
                Predecessors = new List<string>(),

                ProjectID = "",
                EmployeeID = employee.DBID,
                TaskID = "",

                Project = "",
                Employee = employee.Name,
                Category = employee.Category,
                Resources = ""                
            });
        }

        private void AddGanttTask(ProjectEmployeeAssoc projectEmployeeAssoc)
        {
            if (this.GroupByProject)
            {
                var projectNode = GanttNodes.FirstOrDefault(p => p.UID == @"\PROJECT_" + projectEmployeeAssoc.ProjectId);
                if (projectNode != null)
                {
                    var startDate = projectEmployeeAssoc.Project.StartDate;
                    var finishDate = VisibleFinishDate(projectEmployeeAssoc.Project.FinishDate.AddDays(1));

                    DateTime? startDate1 = projectEmployeeAssoc.Project.StartDate1;
                    DateTime? finishDate1 = VisibleFinishDate(projectEmployeeAssoc.Project.FinishDate1.AddDays(1));

                    DateTime? startDate2 = projectEmployeeAssoc.Project.StartDate2;
                    DateTime? finishDate2 = null;
                    if (projectEmployeeAssoc.Project.FinishDate2 != null)
                        finishDate2 = VisibleFinishDate(projectEmployeeAssoc.Project.FinishDate2.Value.AddDays(1));

                    DateTime? startDate3 = projectEmployeeAssoc.Project.StartDate3;
                    DateTime? finishDate3 = null;
                    if (projectEmployeeAssoc.Project.FinishDate3 != null)
                        finishDate3 = VisibleFinishDate(projectEmployeeAssoc.Project.FinishDate3.Value.AddDays(1));

                    var allResources = "";

                    allResources = projectEmployeeAssoc.Employee.Name;

                    /*
                    foreach (var resource in projectEmployeeAssoc.Project.Employees.OrderBy(p => p.Employee.Name))
                    {
                        allResources += ((allResources == "") ? "" : " - ") + resource.Employee.Name;
                    }
                    */

                    GanttNodes.Add(new GanttNode(GanttNodeType.Task)
                    {
                        UID = @"\PROJECT_EMPLOYEE_ASSOC" + projectEmployeeAssoc.DBID,
                        ParentUID = projectNode.UID,
                        //DBID = projectEmployeeAssoc.Project.DBID,
                        Name = projectEmployeeAssoc.Employee.Name,

                        BranchOffice = projectEmployeeAssoc.Project.BranchOffice,

                        StartDate = startDate,
                        FinishDate = finishDate,

                        StartDate1 = startDate1,
                        FinishDate1 = finishDate1,
                        StartDate2 = startDate2,
                        FinishDate2 = finishDate2,
                        StartDate3 = startDate3,
                        FinishDate3 = finishDate3,

                        Duration = finishDate - startDate,
                        Progress = 100,
                        Predecessors = new List<string>(),

                        BaselineStartDate = projectEmployeeAssoc.Project.BaselineStartDate,
                        BaselineFinishDate = projectEmployeeAssoc.Project.BaselineFinishDate.AddDays(1),

                        ProjectID = projectEmployeeAssoc.Project.DBID,
                        EmployeeID = projectEmployeeAssoc.Employee.DBID,
                        TaskID = projectEmployeeAssoc.DBID,

                        Project = projectEmployeeAssoc.Project.Name,
                        TaxYear = projectEmployeeAssoc.Project.TaxYear,
                        Employee = projectEmployeeAssoc.Employee.Name,
                        Category = projectEmployeeAssoc.Employee.Category,
                        Resources = allResources
                    });
                }
            }
            else
            {
                var employeeNode = GanttNodes.FirstOrDefault(p => p.UID == @"\EMPLOYEE_" + projectEmployeeAssoc.Employee.DBID);
                if (employeeNode != null)
                {
                    var startDate = projectEmployeeAssoc.Project.StartDate;
                    var finishDate = VisibleFinishDate(projectEmployeeAssoc.Project.FinishDate.AddDays(1));

                    DateTime? startDate1 = projectEmployeeAssoc.Project.StartDate1;
                    DateTime? finishDate1 = VisibleFinishDate(projectEmployeeAssoc.Project.FinishDate1.AddDays(1));

                    DateTime? startDate2 = projectEmployeeAssoc.Project.StartDate2;
                    DateTime? finishDate2 = null;
                    if (projectEmployeeAssoc.Project.FinishDate2 != null)
                        finishDate2 = VisibleFinishDate(projectEmployeeAssoc.Project.FinishDate2.Value.AddDays(1));

                    DateTime? startDate3 = projectEmployeeAssoc.Project.StartDate3;
                    DateTime? finishDate3 = null;
                    if (projectEmployeeAssoc.Project.FinishDate3 != null)
                        finishDate3 = VisibleFinishDate(projectEmployeeAssoc.Project.FinishDate3.Value.AddDays(1));

                    var allResources = "";

                    foreach (var resource in projectEmployeeAssoc.Project.Employees.OrderBy(p => p.Employee.Name))
                    {
                        allResources += ((allResources == "") ? "" : " - ") + resource.Employee.Name;
                    }

                    GanttNodes.Add(new GanttNode(GanttNodeType.Task)
                    {
                        UID = @"\PROJECT_EMPLOYEE_ASSOC" + projectEmployeeAssoc.DBID,
                        ParentUID = employeeNode.UID,
                        //DBID = projectEmployeeAssoc.Project.DBID,
                        Name = projectEmployeeAssoc.Project.Name + " (" + projectEmployeeAssoc.Project.TaxYear.ToString() + ")",

                        BranchOffice = projectEmployeeAssoc.Project.BranchOffice,

                        StartDate = startDate,
                        FinishDate = finishDate,

                        StartDate1 = startDate1,
                        FinishDate1 = finishDate1,
                        StartDate2 = startDate2,
                        FinishDate2 = finishDate2,
                        StartDate3 = startDate3,
                        FinishDate3 = finishDate3,

                        Duration = finishDate - startDate,
                        Progress = 100,
                        Predecessors = new List<string>(),

                        BaselineStartDate = projectEmployeeAssoc.Project.BaselineStartDate,
                        BaselineFinishDate = projectEmployeeAssoc.Project.BaselineFinishDate.AddDays(1),

                        ProjectID = projectEmployeeAssoc.Project.DBID,
                        EmployeeID = projectEmployeeAssoc.Employee.DBID,
                        TaskID = projectEmployeeAssoc.DBID,

                        Project = projectEmployeeAssoc.Project.Name,
                        TaxYear = projectEmployeeAssoc.Project.TaxYear,
                        Employee = projectEmployeeAssoc.Employee.Name,
                        Resources = allResources
                    });
                }
            }
        }

        private void DeleteGanttEmployeeFromProject(string objectId)
        {

            var employeeID = "";
            var ganttNode = GanttNodes.FirstOrDefault(p => p.UID == @"\PROJECT_EMPLOYEE_ASSOC" + objectId);
            if (ganttNode != null)
            {
                employeeID = ganttNode.EmployeeID;
                GanttNodes.Remove(ganttNode);
            }


        }

        
        private DateTime VisibleFinishDate(DateTime finishDate)
        {
            return finishDate;

            //

            if (me == null)
                return finishDate;

            DateTime result = finishDate;

            switch (me.Category)
            {
                case "":
                case "Gerente":
                case "Director":
                case "Socio":
                    result = finishDate;
                    break;

                default:

                    DateTime finish = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(61);
                    //DateTime finish = DateTime.Now.AddDays(60); 
                    if (finish <= finishDate) 
                    {
                        result = finish;             
                    }

                    break;
            }

            return result;
        }
            

        private bool ShowProject(Project project)
        {
            return true;

            //

            if (me == null)
                return true;

            bool result = false;

            switch (me.Category)
            {
                case "":
                case "Gerente":
                case "Director":
                case "Socio":
                    result = true;
                    break;

                default:
                    //DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                    DateTime finish = DateTime.Now.AddDays(60); //DateTime finish = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2);
                    if (project.StartDate <= finish) /*((project.StartDate1 < finish && project.FinishDate1 > start) ||
                            (project.StartDate2 < finish && project.FinishDate2 > start) ||
                            (project.StartDate3 < finish && project.FinishDate3 > start))*/
                    {
                        result = true;
                    }

                    break;
            }            

            return result;
        }

        private bool ShowAlloc(ProjectEmployeeAlloc projectEmployeeAlloc)
        {
            return true;

            //

            if (me == null)
                return true;

            bool result = false;

            switch (me.Category)
            {
                case "":
                case "Gerente":
                case "Director":
                case "Socio":
                    result = true;
                    break;

                default:
                    //DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                    DateTime finish = DateTime.Now.AddDays(60); //DateTime finish = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2);
                    if (projectEmployeeAlloc.AllocDate <= finish) //&& projectEmployeeAlloc.AllocDate > start))
                    {
                        result = true;
                    }

                    break;
            }

            return result;
        }

        private bool ShowPeriod(DateTime startDate, DateTime finisDate)
        {
            return true;

            ///////

            if (me == null)
                return true;

            bool result = false;

            switch (me.Category)
            {
                case "":
                case "Gerente":
                case "Director":
                case "Socio":
                    result = true;
                    break;

                default:
                    //DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                    DateTime finish = DateTime.Now.AddDays(60); //new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2);
                    if (startDate <= finish) //if ((startDate < finish && finisDate > start))
                    {
                        result = true;
                    }

                    break;
            }

            return result;
        }

        




        private void LoadGantt()
        {
            //ganttUpdating = true;
            var startDate = ganttControl.GetChartVisibleStartDate();
            var finishDate = ganttControl.GetChartVisibleFinishDate();
            string taxYear = ejercicioBarEditItem.EditValue.ToString();

            ganttControl.BeginUpdate();
            
            GanttNodes = new List<GanttNode>();

            ////

            if (groupByProject)
            {
                //Proyecto/Tarea

                foreach (var project in Data.Audit.Projects.Values.Where(p => p.DBID != "0").OrderBy(p => p.StartDate))
                {
                    //if (project.StartDate <= ganttFinishDate && project.FinishDate >= ganttStartDate)
                    if (project.TaxYear == taxYear || (project.StartDate <= ganttFinishDate && project.FinishDate >= ganttStartDate))
                    {
                        if (ShowProject(project))
                        {
                            AddGanttProject(project);
                        }
                    }
                }

                //Proyecto/Empleado

                // -->

                foreach (var project in Data.Audit.Projects.Values.OrderBy(p => p.StartDate))
                {
                    foreach (var projectEmployeeAssoc in project.Employees.OrderBy(p => p.Employee.Name))
                    {
                        if (project.TaxYear == taxYear || (project.StartDate <= ganttFinishDate && project.FinishDate >= ganttStartDate))
                        {
                            if (ShowProject(project))
                            {
                                AddGanttTask(projectEmployeeAssoc);
                            }
                        }
                    }
                }

                // <-- 
            }
            else //Agrupa por empleados
            {
                //Empleados

                //foreach (var employee in Data.Audit.Employees.Values.Where(p => p.Status != "Inactive").OrderBy(p => p.Name))
                //foreach (var employee in Data.Audit.Employees.Values.Where(p => p.LeavingDate == null || p.LeavingDate <= ganttFinishDate).OrderBy(p => p.Name))
                foreach (var employee in Data.Audit.Employees.Values.OrderBy(p => p.Name))
                {
                    if (employee.LeavingDate != null && employee.LeavingDate < ganttStartDate)
                        continue;

                    //if (showEmptyEmployees || employee.Projects.Count > 0)
                    if (showEmptyEmployees || employee.Projects.Where(p => p.Project.TaxYear == taxYear || (p.Project.StartDate <= ganttFinishDate && p.Project.FinishDate >= ganttStartDate)).ToList().Count > 0)
                        AddGanttEmployee(employee);
                }

                //Empleado/Proyecto

                foreach (var project in Data.Audit.Projects.Values.OrderBy(p => p.StartDate))
                {
                    foreach (var projectEmployeeAssoc in project.Employees.OrderBy(p => p.Employee.Name))
                    {
                        if (project.TaxYear == taxYear || (project.StartDate <= ganttFinishDate && project.FinishDate >= ganttStartDate))
                            if (ShowProject(project))
                            {
                                AddGanttTask(projectEmployeeAssoc);
                            }
                    }
                }
            }

            ////

            ganttControl.DataSource = GanttNodes;
            //ganttControl.ExpandAll();            
            ganttControl.EndUpdate();
            ganttControl.SetChartVisibleRange(startDate, finishDate);
            //ganttUpdating = false;
        }

        
        bool IsHolydayDay(DateTime date)
        {
            return Data.Audit.Holidays.FirstOrDefault(p => p.Year == date.Year && p.Month == date.Month && p.Day == date.Day) != null;
        }

        bool IsWorkingDay(DateTime date)
        {
            return !(IsHolydayDay(date) || date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
        }

        private void GanttControl_CustomRowFilter(object sender, DevExpress.XtraTreeList.CustomRowFilterEventArgs e)
        {
            if (currentGanttFilter != ganttControl.ActiveFilterString)
            {
                currentGanttFilter = ganttControl.ActiveFilterString;
                Data.Audit.PostSetting("GanttFilter", currentGanttFilter);
            }
        }

        private void InitRibbonFilters()
        {
            
            

            string taxYear = ejercicioBarEditItem.EditValue.ToString();

            projectRepositoryItemComboBox.Items.Clear();
            foreach (var project in Data.Audit.Projects.Values.OrderBy(p => p.Name))
            {
                if (project.TaxYear == taxYear)
                    projectRepositoryItemComboBox.Items.Add(project.Name);
            }            

            projectBarEditItem.EditValue = "";

            for (int i = 0; i <= 6; i++)
                ejercicioRepositoryItemComboBox.Items.Add((DateTime.Now.Year - i + 1).ToString()); //+ " - " + (DateTime.Now.Year - i + 2).ToString());

            employeeRepositoryItemComboBox.Items.Clear();
            foreach (var employee in Data.Audit.Employees.Values.OrderBy(p => p.Name))
            {
                employeeRepositoryItemComboBox.Items.Add(employee.Name);
            }

            employeeBarEditItem.EditValue = ""; 

            
            monthRepositoryItemComboBox.Items.Clear();
            for (int i = 0; i <= 11; i++)
            {
                //string monthString = ganttStartDate.AddMonths(i).ToString("MMMM - yyyy"); //(new DateTime(DateTime.Now.Year, i, 1)).ToString("MMMM - yyyy");
                //string monthString = ganttStartDate.AddMonths(i).ToString("MMMM");

                string monthString = new DateTime(int.Parse(ejercicioBarEditItem.EditValue.ToString()), 9, 1).AddMonths(i).ToString("MMMM - yyyy");
                monthRepositoryItemComboBox.Items.Add(monthString);
            }
            monthBarEditItem.EditValue = "";            
            
        }

        private void ResetRibbonFilters()
        {
            InitRibbonFilters();
        }

        private void Audit_OnObjectMessage(ObjectMessageActionType objectMessageActionType, ObjectMessageObjectType objectMessageObjectType, string objectId)
        {
            //SaveLayout2();            
            ganttControl.BeginUpdate();

            switch (objectMessageActionType)
            {
                case (ObjectMessageActionType.Put):
                    {
                        switch (objectMessageObjectType)
                        {
                            case (ObjectMessageObjectType.Project):
                                {

                                    if (groupByProject)
                                    {
                                        var uID = @"\PROJECT_" + objectId;
                                        var node = GanttNodes.FirstOrDefault(p => p.UID == uID);
                                        if (node != null)
                                        {
                                            var project = Data.Audit.Projects[objectId];
                                            node.Name = project.Name;
                                            node.BranchOffice = project.BranchOffice;
                                        }
                                    }
                                    else
                                    {
                                        foreach (var node in GanttNodes.Where(p => p.ProjectID == objectId))
                                        {
                                            var project = Data.Audit.Projects[objectId];
                                            node.Name = project.Name;
                                            node.BranchOffice = project.BranchOffice;
                                        }
                                    }

                                    break;
                                }

                            case (ObjectMessageObjectType.Employee):
                                {
                                    if (!groupByProject)
                                    {
                                        var uID = @"\EMPLOYEE_" + objectId;
                                        var node = GanttNodes.FirstOrDefault(p => p.UID == uID);
                                        if (node != null)
                                        {
                                            var employee = Data.Audit.Employees[objectId];
                                            node.Name = employee.Name;
                                            node.BranchOffice = employee.BranchOffice;
                                        }
                                    }
                                    break;
                                }
                        }

                        break;
                    }

                case (ObjectMessageActionType.Post):
                    {
                        switch (objectMessageObjectType)
                        {
                            case (ObjectMessageObjectType.Project):
                                {
                                    if (groupByProject)
                                        AddGanttProject(Data.Audit.Projects[objectId]);
                                    break;
                                }

                            case (ObjectMessageObjectType.Employee):
                                {
                                    if (!groupByProject)
                                        AddGanttEmployee(Data.Audit.Employees[objectId]);
                                    break;
                                }

                            case (ObjectMessageObjectType.ProjectEmployeeAssoc):
                                {
                                    AddGanttTask(Data.Audit.ProjectEmployeeAssocs[objectId]);
                                    break;
                                }
                        }

                        break;
                    }

                case (ObjectMessageActionType.Delete):
                    {
                        switch (objectMessageObjectType)
                        {
                            case (ObjectMessageObjectType.ProjectEmployeeAssoc):
                                {
                                    DeleteGanttEmployeeFromProject(objectId);
                                    break;
                                }
                        }

                        break;
                    }
            }

            ganttControl.RefreshDataSource();
            RefreshGantt();            
            //RestoreLayout2();
            ganttControl.EndUpdate();            
        }
       
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

        private void ganttControl_DoubleClick(object sender, EventArgs e)
        {
            if (selectedProjectDBID != "")
            {
                ProjectForm.EditProject(selectedProjectDBID);
            }
            else
                if (selectedTaskDBID != "")
            {
                if (groupByProject)
                {
                    var employeeId = Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].EmployeeId;
                    EmployeeForm.EditEmployee(employeeId);
                }
                else
                {
                    var projectId = Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].ProjectId;
                    ProjectForm.EditProject(projectId);
                }
            }
            else
                if (selectedEmployeeDBID != "")
            {
                EmployeeForm.EditEmployee(selectedEmployeeDBID);
            }

            UpdateGantt();

            DXMouseEventArgs ee = e as DXMouseEventArgs;
            ee.Handled = true;
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ribbonControl1.Enabled = false;
                ProjectForm.CreateProject();
            }
            finally
            {
                ribbonControl1.Enabled = true;
            }

            UpdateGantt();
            //LoadGantt();
        }

        private void ganttControl_TaskFinishDateModified(object sender, TaskFinishModifiedEventArgs e)
        {
            if (e.OriginalTaskFinish != ((DateTime)e.ProcessedTask.Node.GetValue("FinishDate")).RoundDate())
            {
                //string dbid = e.ProcessedTask.Node.GetValue("DBID").ToString();
                if (e.ProcessedTask.Node.GetValue("GanttNodeType").ToString() == GanttNodeType.Task.ToString())
                {
                    //var task = AuditData.Tasks.FirstOrDefault(p => p.DBID == dbid);
                    var task = Data.Audit.Projects[e.ProcessedTask.Node.GetValue("ProjectID").ToString()];
                    var startDate = ((DateTime)e.ProcessedTask.Node.GetValue("StartDate")).RoundDate();
                    var finishDate = ((DateTime)e.ProcessedTask.Node.GetValue("FinishDate")).RoundDate();

                    /*
                    foreach (var period in Data.Audit.ProjectEmployeeAllocs.Where(p => p.Value.ProjectId == task.DBID))
                    {
                        if (period.Value.FinishDate == task.FinishDate)
                        {
                            period.Value.FinishDate = finishDate.AddDays(-1);
                            //TODO: Pulir los periodos solapadaos, con fecha de inicio posterior a fin...
                        }
                    }
                    */

                    task.StartDate1 = startDate;
                    task.FinishDate1 = finishDate.AddDays(-1);
                    //RecalculateProject(task.Project.DBID);
                    RefreshGantt();
                }
            }
        }

        private void ganttControl_TaskMoved(object sender, TaskMovedEventArgs e)
        {
            if ((e.OriginalTaskStart != ((DateTime)e.ProcessedTask.Node.GetValue("StartDate")).RoundDate()) || (e.OriginalTaskFinish != ((DateTime)e.ProcessedTask.Node.GetValue("FinishDate")).RoundDate()))
            {
                if (e.ProcessedTask.Node.GetValue("GanttNodeType").ToString() == GanttNodeType.Task.ToString())
                {
                    var project = Data.Audit.ProjectEmployeeAssocs[e.ProcessedTask.Node.GetValue("TaskID").ToString()].Project;

                    var startDate = ((DateTime)e.ProcessedTask.Node.GetValue("StartDate")).RoundDate();
                    var finishDate = ((DateTime)e.ProcessedTask.Node.GetValue("FinishDate")).RoundDate();

                    e.ProcessedTask.Node.SetValue("StartDate", startDate);
                    e.ProcessedTask.Node.SetValue("Duration", finishDate - startDate);
                    //e.ProcessedTask.Node.SetValue("FinishDate", finishDate);

                    var diff = project.StartDate - startDate;
                    project.StartDate1 = startDate;
                    project.FinishDate1 = finishDate.AddDays(-1);

                    foreach (var period in Data.Audit.ProjectEmployeeAllocs.Where(p => p.Value.ProjectId == project.DBID))
                    {
                        period.Value.AllocDate = period.Value.AllocDate - diff;
                    }

                    RefreshGantt();
                }
                else
                {
                    if (e.ProcessedTask.Node.GetValue("GanttNodeType").ToString() == GanttNodeType.Project.ToString())
                    {
                        //var project = AuditData.Projects.FirstOrDefault(p => p.DBID == dbid);
                        var project = Data.Audit.Projects[e.ProcessedTask.Node.GetValue("ProjectID").ToString()];

                        var startDate = ((DateTime)e.ProcessedTask.Node.GetValue("StartDate")).RoundDate();
                        var finishDate = ((DateTime)e.ProcessedTask.Node.GetValue("FinishDate")).RoundDate();

                        //Move tasks
                        /*
                        var diff = project.StartDate - startDate;
                        foreach (var task in project.Tasks)
                        {
                            task.StartDate = task.StartDate - diff;
                            task.FinishDate = task.FinishDate - diff;
                        }
                        */

                        //Move assignment periods

                        var diff = project.StartDate - startDate;
                        foreach (var period in Data.Audit.ProjectEmployeeAllocs.Where(p => p.Value.ProjectId == project.DBID))
                        {
                            period.Value.AllocDate = period.Value.AllocDate - diff;
                        }

                        //Move project

                        e.ProcessedTask.Node.SetValue("StartDate", startDate);
                        e.ProcessedTask.Node.SetValue("Duration", finishDate - startDate);
                        //e.ProcessedTask.Node.SetValue("FinishDate", finishDate);
                        project.StartDate1 = startDate;
                        project.FinishDate1 = finishDate;
                        //RecalculateProject(project.DBID);
                        RefreshGantt();
                    }
                }
            }
        }

        void RefreshGantt(GanttControlNode rootNode = null)
        {
            ganttControl.BeginUpdate();

            var nodeList = (rootNode != null) ? rootNode.Nodes : ganttControl.Nodes;
            foreach (GanttControlNode node in nodeList)
            {

                switch (node.GetValue("GanttNodeType"))
                {
                    case GanttNodeType.Project:
                        {
                            var project = Data.Audit.Projects[node.GetValue("ProjectID").ToString()];

                            var allResources = "";
                            foreach (var projectEmployeeAssoc in project.Employees.OrderBy(p => p.Employee.Name))
                            {
                                allResources += ((allResources == "") ? "" : " - ") + projectEmployeeAssoc.Employee.Name;
                            }

                            node.SetValue("Name", project.Name);
                            node.SetValue("StartDate", project.StartDate);
                            node.SetValue("Duration", project.FinishDate.AddDays(1) - project.StartDate);

                            node.SetValue("BaselineStartDate", project.BaselineStartDate);
                            node.SetValue("BaselineFinishDate", project.BaselineFinishDate.AddDays(1));

                            node.SetValue("Resources", allResources);

                            ganttControl.RefreshNode(node);

                            break;
                        }

                    case GanttNodeType.Task:
                        {
                            var task = Data.Audit.ProjectEmployeeAssocs[node.GetValue("TaskID").ToString()];

                            if (groupByProject)
                            {
                                node.SetValue("Name", task.Employee.Name);
                                node.SetValue("Category", task.Employee.Category);

                                node.SetValue("StartDate", task.Project.StartDate);
                                node.SetValue("Duration", task.Project.FinishDate.AddDays(1) - task.Project.StartDate);

                                ganttControl.RefreshNode(node);
                            }
                            else
                            {
                                var allResources = "";
                                foreach (var projectEmployeeAssoc in task.Project.Employees.OrderBy(p => p.Employee.Name))
                                {
                                    allResources += ((allResources == "") ? "" : " - ") + projectEmployeeAssoc.Employee.Name;
                                }

                                node.SetValue("Name", task.Project.Name);
                                node.SetValue("StartDate", task.Project.StartDate);
                                node.SetValue("Duration", task.Project.FinishDate.AddDays(1) - task.Project.StartDate);

                                node.SetValue("BaselineStartDate", task.Project.BaselineStartDate);
                                node.SetValue("BaselineFinishDate", task.Project.BaselineFinishDate.AddDays(1));

                                node.SetValue("Resources", allResources);
                            }

                            ganttControl.RefreshNode(node);

                            break;
                        }

                    case GanttNodeType.Employee:
                        {
                            var employee = Data.Audit.Employees[node.GetValue("EmployeeID").ToString()];

                            node.SetValue("Name", employee.Name);
                            node.SetValue("Category", employee.Category);

                            ganttControl.RefreshNode(node);

                            break;
                        }
                }

                RefreshGantt(node);
            }

            ganttControl.EndUpdate();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Data.Audit.SaveDB();
            //AuditData.ReadDB();
            //LoadGantt();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("¿Desea deshacer todos los cambios no guardados?", "Deshacer", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Data.Audit.ReadDB();
                LoadGantt();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ResetRibbonFilters();

            if (selectedProjectDBID != "")
            {
                var project = Data.Audit.Projects[selectedProjectDBID];
                ganttControl.ActiveFilterString = $"[Project] = '{project.Name.Replace("'", "''")}'";
            }
            else
            if (selectedTaskDBID != "")
            {
                if (groupByProject)
                {
                    var employee = Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].Employee;

                    string filter = "";
                    foreach (var task in employee.Projects)
                    {
                        filter += (filter == "" ? "" : " OR ") + $"[Project] = '{task.Project.Name.Replace("'", "''")}'";
                    }
                    ganttControl.ActiveFilterString = filter;
                }
                else
                {
                    var project = Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].Project;
                    ganttControl.ActiveFilterString = $"[Project] = '{project.Name.Replace("'", "''")}'";
                }
            }
            else
            if (selectedEmployeeDBID != "")
            {
                var employee = Data.Audit.Employees[selectedEmployeeDBID];

                string filter = "";
                foreach (var task in employee.Projects)
                {
                    filter += (filter == "" ? "" : " OR ") + $"[Project] = '{task.Project.Name.Replace("'", "''")}'";
                }
                ganttControl.ActiveFilterString = filter;
            }
        }

        private void ganttControl_CustomColumnDisplayText(object sender, DevExpress.XtraTreeList.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column == startDateColumn || e.Column == finishDateColumn)
            {
                if (e.Node != null && e.Node.GetValue("GanttNodeType").ToString() == GanttNodeType.Employee.ToString())
                    e.DisplayText = "";

                if (groupByProject && e.Node != null && e.Node.GetValue("GanttNodeType").ToString() == GanttNodeType.Task.ToString())
                    e.DisplayText = "";
            }

            if (e.Column == finishDateColumn)
            {
                if (groupByProject && e.Node != null && e.Node.GetValue("GanttNodeType").ToString() == GanttNodeType.Project.ToString())
                {
                    DateTime value = (DateTime)e.Node.GetValue("FinishDate");
                    e.DisplayText = value.AddDays(-1).ToShortDateString();
                }

                if (!groupByProject && e.Node != null && e.Node.GetValue("GanttNodeType").ToString() == GanttNodeType.Task.ToString())
                {
                    DateTime value = (DateTime)e.Node.GetValue("FinishDate");
                    e.DisplayText = value.AddDays(-1).ToShortDateString();
                }
            }

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ribbonControl1.Enabled = false;
                EmployeeForm.CreateEmployee();
            }
            finally
            {
                ribbonControl1.Enabled = true;
            }

            UpdateGantt();
        }
               
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (selectedProjectDBID != "")
            {
                MainForm.ShowCurtain();
                MainForm.AddNewTab(new TimeAllocationUserControl(selectedProjectDBID, ""), "Asignación de horas");
                MainForm.HideCurtain();
            }
            else
            if (selectedTaskDBID != "")
            {
                MainForm.ShowCurtain();

                if (groupByProject)
                {
                    MainForm.AddNewTab(new TimeAllocationUserControl("", Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].EmployeeId), "Asignación de horas");
                }
                else
                {
                    MainForm.AddNewTab(new TimeAllocationUserControl(Data.Audit.ProjectEmployeeAssocs[selectedTaskDBID].ProjectId, ""), "Asignación de horas");
                }

                MainForm.HideCurtain();
            }
            else
                if (selectedEmployeeDBID != "")
            {
                MainForm.ShowCurtain();
                MainForm.AddNewTab(new TimeAllocationUserControl("", selectedEmployeeDBID), "Asignación de horas");
                MainForm.HideCurtain();
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UpdateGantt();
        }

        private void UpdateGantt()
        {
            SaveLayout2();
            Data.Audit.ReadDB();
            LoadGantt();
            RestoreLayout2();
        }

        private void barCheckItem3_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveLayout2();
            showEmptyEmployees = barCheckItem3.Checked;
            LoadGantt();
            RestoreLayout2();
        }

        private void ganttControl_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            //if (!ganttUpdating) SaveLayout2();

            selectedTaskDBID = "";
            selectedProjectDBID = "";
            selectedEmployeeDBID = "";

            foreach (var node in ganttControl.Selection)
            {
                if (node.GetValue("GanttNodeType").ToString() == GanttNodeType.Task.ToString())
                {
                    selectedTaskDBID = node.GetValue("TaskID").ToString();
                }
                else
                {
                    if (node.GetValue("GanttNodeType").ToString() == GanttNodeType.Project.ToString())
                    {
                        selectedProjectDBID = node.GetValue("ProjectID").ToString();

                    }
                    else
                    {
                        if (node.GetValue("GanttNodeType").ToString() == GanttNodeType.Employee.ToString())
                        {
                            selectedEmployeeDBID = node.GetValue("EmployeeID").ToString();

                        }
                    }
                }
            }
        }

        void SaveLayout2()
        {
            try
            {

                //Expanded nodes

                var expandedNodes = new List<string>();

                foreach (GanttControlNode node in ganttControl.Nodes)
                {
                    if (node.Expanded && node.GetValue("UID") != null)
                    {
                        expandedNodes.Add(node.GetValue("UID").ToString());
                    }
                }

                if (groupByProject)
                {
                    Data.Audit.PostSetting("ProjectView_ExpandedNodes", String.Join(",", expandedNodes));
                }
                else
                {
                    Data.Audit.PostSetting("EmployeeView_ExpandedNodes", String.Join(",", expandedNodes));
                }

                //Focused node

                string focusedNodeUID = "";
                if (ganttControl.FocusedNode != null && ganttControl.FocusedNode.GetValue("UID") != null)
                {
                    focusedNodeUID = ganttControl.FocusedNode.GetValue("UID").ToString();
                }

                if (groupByProject)
                {
                    Data.Audit.PostSetting("ProjectView_FocusedNodeUID", focusedNodeUID);
                }
                else
                {
                    Data.Audit.PostSetting("EmployeeView_FocusedNodeUID", focusedNodeUID);
                }

                //Top visible node

                if (groupByProject)
                {
                    Data.Audit.PostSetting("ProjectView_TopVisibleNodeIndex", ganttControl.TopVisibleNodeIndex.ToString());
                }
                else
                {
                    Data.Audit.PostSetting("EmployeeView_TopVisibleNodeIndex", ganttControl.TopVisibleNodeIndex.ToString());
                }
            }
            catch
            {
                //?¿
            }
        }

        private bool RestoreFocusedNode(GanttControlNodes nodes, string focusedNodeUID)
        {
            bool result = false;

            if (focusedNodeUID != "")
            {

                foreach (GanttControlNode node in nodes)
                {
                    if (node.GetValue("UID").ToString() == focusedNodeUID)
                    {
                        ganttControl.FocusedNode = node;
                        result = true;
                        break;
                    }
                    else
                    {
                        RestoreFocusedNode(node.Nodes, focusedNodeUID);
                    }
                }
            }

            return result;
        }

        void RestoreLayout2()
        {
            try
            {
                //ganttUpdating = true;

                //Expanded nodes

                List<string> expandedNodes;
                if (groupByProject)
                {
                    expandedNodes = Data.Audit.GetSetting("ProjectView_ExpandedNodes").Split(',').ToList();
                }
                else
                {
                    expandedNodes = Data.Audit.GetSetting("EmployeeView_ExpandedNodes").Split(',').ToList();
                }

                //ganttControl.AfterExpand -= ganttControl_AfterExpand;
                //ganttControl.AfterCollapse -= ganttControl_AfterCollapse;

                ganttControl.CollapseAll();

                foreach (GanttControlNode node in ganttControl.Nodes)
                {
                    if (expandedNodes.Contains(node.GetValue("UID").ToString()))
                    {
                        node.Expanded = true;
                    }
                }

                //ganttControl.AfterExpand += ganttControl_AfterExpand;
                //ganttControl.AfterCollapse += ganttControl_AfterCollapse;

                //Focused node

                string focusedNodeUID = "";

                if (groupByProject)
                {
                    focusedNodeUID = Data.Audit.GetSetting("ProjectView_FocusedNodeUID");
                }
                else
                {
                    focusedNodeUID = Data.Audit.GetSetting("EmployeeView_FocusedNodeUID");
                }

                if (focusedNodeUID != "")
                {
                    RestoreFocusedNode(ganttControl.Nodes, focusedNodeUID);
                }

                //Top visible node

                int topVisibleNodeIndex = 0;

                if (groupByProject)
                {
                    topVisibleNodeIndex = Data.Audit.GetSetting("ProjectView_TopVisibleNodeIndex").ToInt();
                }
                else
                {
                    topVisibleNodeIndex = Data.Audit.GetSetting("EmployeeView_TopVisibleNodeIndex").ToInt();
                }

                ganttControl.TopVisibleNodeIndex = topVisibleNodeIndex;

                //ganttUpdating = false;
            }
            catch
            {
                //?¿
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ganttControl.ActiveFilterString = "";
            InitRibbonFilters();

            Data.Audit.ReadDB();
            LoadGantt();

            ganttControl.CollapseAll();
            CenterAt(DateTime.Now);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Auditor 

            if (Data.Audit.Me == null)
                return;

            string filterString = "[Employee] = '" + Data.Audit.Me.Name + "'";                    

            /*
            foreach (CheckedListBoxItem item in repositoryItemCheckedComboBoxEdit2.Items)            
                item.CheckState = CheckState.Unchecked;
            */

            employeeBarEditItem.EditValue = Data.Audit.Me.Name;

            /*
            foreach (CheckedListBoxItem item in repositoryItemCheckedComboBoxEdit2.Items)
            {
                if (item.Description == Data.Audit.Me.Name)
                {
                    item.CheckState = CheckState.Checked;                                        
                }                
            }
            */

            if (!ganttControl.ActiveFilterString.Contains(filterString))
            {
                //ganttControl.ActiveFilterString += (ganttControl.ActiveFilterString != "" ? " And " : "") + filterString;                

                string newFilter = "";
                if (ganttControl.ActiveFilterString != "")
                    newFilter = "(" + ganttControl.ActiveFilterString + ") And ";
                newFilter += filterString;

                ganttControl.ActiveFilterString = newFilter;


            }

            if (groupByProject)
            {
                ganttControl.ExpandAll();
            }
            else
            {
                RestoreFocusedNode(ganttControl.Nodes, @"\EMPLOYEE_" + Data.Audit.Me.DBID);
                if (ganttControl.FocusedNode != null)
                    ganttControl.FocusedNode.Expand();
            }

        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            //Este mes
            
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime finishDate = startDate.AddMonths(1).AddDays(-1);
            
            string monthString = startDate.ToString("MMMM - yyyy");
            //monthBarEditItem.EditValue = monthString;
            
                        
            string filterString = "(" +
                "    ([StartDate1] <= #" + finishDate.ToString("yyyy-MM-dd") + "# And [FinishDate1] >= #" + startDate.ToString("yyyy-MM-dd") + "#)" +
                " Or ([StartDate2] <= #" + finishDate.ToString("yyyy-MM-dd") + "# And [FinishDate2] >= #" + startDate.ToString("yyyy-MM-dd") + "#)" +
                " Or ([StartDate3] <= #" + finishDate.ToString("yyyy-MM-dd") + "# And [FinishDate3] >= #" + startDate.ToString("yyyy-MM-dd") + "#)" +
                ")";


            if (ganttControl.ActiveFilterString.Contains("[StartDate1]"))
            {
                var ixStartDate = ganttControl.ActiveFilterString.IndexOf("[StartDate1]");
                if (ganttControl.ActiveFilterString.IndexOf("[StartDate1]", ixStartDate + 1) > 0)
                {
                    ganttControl.ActiveFilterString = filterString;

                }
                else
                {
                    var ixFinishDate = ganttControl.ActiveFilterString.IndexOf("[FinishDate3]");
                    var ixMark3 = ganttControl.ActiveFilterString.IndexOf("#", ixFinishDate);
                    var ixMark4 = ganttControl.ActiveFilterString.IndexOf("#", ixMark3 + 1);

                    string newFilter = ganttControl.ActiveFilterString.Remove(ixStartDate, ixMark4 - ixStartDate + 1);
                    newFilter = newFilter.Insert(ixStartDate, filterString);
                    ganttControl.ActiveFilterString = newFilter;
                }
            }
            else
            {
                ganttControl.ActiveFilterString += (ganttControl.ActiveFilterString != "" ? " And " : "") + filterString;
            }


        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Mi delegación
            
            if (Data.Audit.Me == null)
                return;

            string filterString = "[BranchOffice] = '" + Data.Audit.Me.BranchOffice + "'";            
            if (!ganttControl.ActiveFilterString.Contains(filterString))
            {
                //ganttControl.ActiveFilterString += (ganttControl.ActiveFilterString != "" ? " And " : "") + filterString;
                string newFilter = "";
                if (ganttControl.ActiveFilterString != "")
                    newFilter = "(" + ganttControl.ActiveFilterString + ") And ";
                newFilter += filterString;

                ganttControl.ActiveFilterString = newFilter;
            }
        }      

        private void ejercicioRepositoryItemComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ejercicioBarEditItem.EditValue.ToString() == "")
                return;


            int year = int.Parse(ejercicioBarEditItem.EditValue.ToString()); //DateTime.Now.Month <= 8 ? (int.Parse(ejercicioBarEditItem.EditValue.ToString()) - 1) : int.Parse(ejercicioBarEditItem.EditValue.ToString());
            ganttStartDate = new DateTime(year, 9, 1);
            ganttFinishDate = VisibleFinishDate(new DateTime(year + 1, 8, 31));

            //

            UpdateGantt();
                
            //
            /*
            string filterString = "[TaxYear] = '" + ejercicioBarEditItem.EditValue + "'";

            if (ganttControl.ActiveFilterString.Contains("[TaxYear]"))
            {
                var ixEjercicio = ganttControl.ActiveFilterString.IndexOf("[TaxYear]");
                if (ganttControl.ActiveFilterString.IndexOf("[TaxYear]", ixEjercicio + 1) > 0)
                {
                    ganttControl.ActiveFilterString = filterString;
                }
                else
                {
                    var ixMark1 = ganttControl.ActiveFilterString.IndexOf("'", ixEjercicio);
                    var ixMark2 = ganttControl.ActiveFilterString.IndexOf("'", ixMark1 + 1);
                    string newFilter = ganttControl.ActiveFilterString.Remove(ixEjercicio, ixMark2 - ixEjercicio + 1);
                    newFilter = newFilter.Insert(ixEjercicio, filterString);
                    ganttControl.ActiveFilterString = newFilter;
                }
            }
            else
            {
                ganttControl.ActiveFilterString += (ganttControl.ActiveFilterString != "" ? " And " : "") + filterString;
            }
            */

            //


            monthRepositoryItemComboBox.Items.Clear();
            for (int i = 0; i <= 11; i++)
            {
                string monthString = new DateTime(int.Parse(ejercicioBarEditItem.EditValue.ToString()), 9, 1).AddMonths(i).ToString("MMMM - yyyy");
                monthRepositoryItemComboBox.Items.Add(monthString);
            }
            monthBarEditItem.EditValue = "";

            string taxYear = ejercicioBarEditItem.EditValue.ToString();

            projectRepositoryItemComboBox.Items.Clear();
            foreach (var project in Data.Audit.Projects.Values.OrderBy(p => p.Name))
            {
                if (project.TaxYear == taxYear)
                    projectRepositoryItemComboBox.Items.Add(project.Name);
            }

        }

        private void projectRepositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (projectBarEditItem.EditValue.ToString() == "")
                return;

            string filterString = "[Project] = '" + projectBarEditItem.EditValue + "'";

            if (ganttControl.ActiveFilterString.Contains("[Project]"))
            {                
                var ixProject = ganttControl.ActiveFilterString.IndexOf("[Project]");
                if (ganttControl.ActiveFilterString.IndexOf("[Project]", ixProject + 1) > 0)
                {
                    ganttControl.ActiveFilterString = filterString;
                }
                else
                {
                    var ixMark1 = ganttControl.ActiveFilterString.IndexOf("'", ixProject);
                    var ixMark2 = ganttControl.ActiveFilterString.IndexOf("'", ixMark1 + 1);
                    string newFilter = ganttControl.ActiveFilterString.Remove(ixProject, ixMark2 - ixProject + 1);
                    newFilter = newFilter.Insert(ixProject, filterString);
                    ganttControl.ActiveFilterString = newFilter;
                }
            }
            else
            {                
                ganttControl.ActiveFilterString += (ganttControl.ActiveFilterString != "" ? " And " : "") + filterString;
            }
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

        private void monthRepositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
         
            if (monthBarEditItem.EditValue.ToString() == "")
                return;

            int year = DateTime.Now.Year;

            if (ejercicioBarEditItem.EditValue.ToString() != "")
            {
                int.TryParse(ejercicioBarEditItem.EditValue.ToString(), out year);            
            }
            
            ComboBoxEdit comboBoxEdit = (ComboBoxEdit)sender;
            //DateTime startDate = ganttStartDate.AddMonths(comboBoxEdit.SelectedIndex);            
            DateTime startDate = new DateTime(year, 9, 1).AddMonths(comboBoxEdit.SelectedIndex);
            DateTime finishDate = startDate.AddMonths(1).AddDays(-1);


            string filterString = "(" +
                "([StartDate1] <= #" + finishDate.ToString("yyyy-MM-dd") + "# And [FinishDate1] >= #" + startDate.ToString("yyyy-MM-dd") + "#)" +
                " Or ([StartDate2] <= #" + finishDate.ToString("yyyy-MM-dd") + "# And [FinishDate2] >= #" + startDate.ToString("yyyy-MM-dd") + "#)" +
                " Or ([StartDate3] <= #" + finishDate.ToString("yyyy-MM-dd") + "# And [FinishDate3] >= #" + startDate.ToString("yyyy-MM-dd") + "#)" +
                ")";

            if (ganttControl.ActiveFilterString.Contains("[StartDate1]"))
            {
                var ixStartDate = ganttControl.ActiveFilterString.IndexOf("[StartDate1]");
                if (ganttControl.ActiveFilterString.IndexOf("[StartDate1]", ixStartDate + 1) > 0)
                {
                    ganttControl.ActiveFilterString = filterString;

                }
                else
                {
                    var ixFinishDate = ganttControl.ActiveFilterString.IndexOf("[FinishDate3]");
                    var ixMark3 = ganttControl.ActiveFilterString.IndexOf("#", ixFinishDate);
                    var ixMark4 = ganttControl.ActiveFilterString.IndexOf("#", ixMark3 + 1);

                    string newFilter = ganttControl.ActiveFilterString.Remove(ixStartDate, ixMark4 - ixStartDate + 1);
                    newFilter = newFilter.Insert(ixStartDate, filterString);
                    ganttControl.ActiveFilterString = newFilter;
                }
            }
            else
            {
                ganttControl.ActiveFilterString += (ganttControl.ActiveFilterString != "" ? " And " : "") + filterString;
            }
            
        }

        private void CenterAt(DateTime dateTime)
        {
            var start = ganttControl.GetChartVisibleStartDate();
            var finish = ganttControl.GetChartVisibleFinishDate();
            long middleTics = (finish - start).Ticks / 2;
            var newStart = dateTime.AddTicks(-middleTics);
            var newFinish = dateTime.AddTicks(middleTics);
            ganttControl.SetChartVisibleRange(newStart, newFinish);
        }        
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


        public DateTime? StartDate1 { get; set; }
        public DateTime? FinishDate1 { get; set; }
        public DateTime? StartDate2 { get; set; }
        public DateTime? FinishDate2 { get; set; }
        public DateTime? StartDate3 { get; set; }
        public DateTime? FinishDate3 { get; set; }

        public TimeSpan Duration { get; set; }        
        public DateTime? BaselineStartDate { get; set; }
        public DateTime? BaselineFinishDate { get; set; }
        public TimeSpan BaselineDuration { get; set; }
        public string BranchOffice { get; set; }
        public string Name { get; set; }
        public string UID { get; set; }        
        public List<string> Predecessors { get; set; }
        public string Project { get; set; }
        public string TaxYear { get; set; }
        public string Category { get; set; }
        public string Employee { get; set; }
        public string Resources { get; set; }
        public double Progress { get; set; }
        public string ProjectID { get; set; }
        public string EmployeeID { get; set; }
        public string TaskID { get; set; }        
    }
}



