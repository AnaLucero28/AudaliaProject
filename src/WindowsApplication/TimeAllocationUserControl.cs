using Audalia.DataHUBClient;
using DevExpress.Export;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Audalia.DataHUBCommon;

namespace WindowsApplication
{


    public enum AssignmentStatus { NotAssigned, Assigned, NewAssignment, DeletedAssignment };
    public enum AllocationStatus { NotAllocated, Allocated, NewAllocation, DeletedAllocation };

    public partial class TimeAllocationUserControl :  DevExpress.XtraEditors.XtraUserControl
    {
        
        DateTime allocationStartDate;
        DateTime allocationFinishDate;


        DataTable allocationDataTable;
        int firstDateColumn = 8;

        string projectId = "";
        string employeeId = "";


        public TimeAllocationUserControl()
        {
            InitializeComponent();

            //

            Init();
        }

        public TimeAllocationUserControl(string projectId, string employeeId)
        {
            InitializeComponent();

            //

            Init(projectId, employeeId);
        }

        void Init(string projectId = "", string employeeId = "")
        {
            if (projectId != "")
            {
                var project = Data.Audit.Projects[projectId];
                if (project.StartDate.Month < 9)
                {
                    allocationStartDate = new DateTime(project.StartDate.Year - 2, 9, 1);                    
                }
                else
                {
                    allocationStartDate = new DateTime(project.StartDate.Year - 1, 9, 1);                    
                }

                if (project.FinishDate.Month < 9)
                {                    
                    allocationFinishDate = new DateTime(project.FinishDate.Year + 1, 8, 31);
                }
                else
                {                    
                    allocationFinishDate = new DateTime(project.FinishDate.Year + 2, 8, 31);
                }

            }
            else
            {
                if (DateTime.Now.Month < 9)
                {
                    allocationStartDate = new DateTime(DateTime.Now.Year - 2, 9, 1);
                    allocationFinishDate = new DateTime(DateTime.Now.Year + 1, 8, 31);
                }
                else
                {
                    allocationStartDate = new DateTime(DateTime.Now.Year - 1, 9, 1);
                    allocationFinishDate = new DateTime(DateTime.Now.Year + 2, 8, 31);
                }
            }

            if (Data.Audit.UserPermissions <= UserPermissions.Read)
            {
                barButtonItem3.Enabled = false;
                barButtonItem4.Enabled = false;
                bandedGridView.OptionsBehavior.ReadOnly = true;
            }

            //


            this.projectId = projectId;
            this.employeeId = employeeId;

            allocationDataTable = new DataTable();
            var table = allocationDataTable;

            table.Columns.Add(new DataColumn($"EmployeeId", typeof(string)));
            table.Columns.Add(new DataColumn($"ProjectId", typeof(string)));
            table.Columns.Add(new DataColumn($"TaskId", typeof(string)));

            table.Columns.Add(new DataColumn($"Auditor", typeof(string)));
            //table.Columns.Add(new DataColumn($"Grupo", typeof(string)));
            table.Columns.Add(new DataColumn($"Proyecto", typeof(string)));

            table.Columns.Add(new DataColumn($"Allocated", typeof(int)));
            table.Columns.Add(new DataColumn($"Budgeted", typeof(int)));

            table.Columns.Add(new DataColumn($"Budget", typeof(string)));

            //for (DateTime date = new DateTime(2021, 1, 1); date < new DateTime(2021, 12, 31); date = date.AddDays(1.0))
            for (DateTime date = allocationStartDate; date <= allocationFinishDate; date = date.AddDays(1.0))
            {
                var fieldName = date.ToString("yyyyMMdd");
                table.Columns.Add(new DataColumn(fieldName, typeof(int)));
            }

            //



            //

            //AuditData.ReadDB();

            gridControl.BeginUpdate();
            bandedGridView.BeginUpdate();

            gridControl.DataSource = table;
            gridControl.DataMember = table.TableName;

            //

            bandedGridView.OptionsView.AllowCellMerge = true;
            repositoryItemSpinEdit1.Properties.IsFloatValue = false;
            repositoryItemSpinEdit1.Properties.MinValue = 0;
            repositoryItemSpinEdit1.Properties.MaxValue = 24;

            bandedGridView.Bands[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            bandedGridView.Bands[0].Width = 600;

            var totalMonths = ((allocationFinishDate.Year - allocationStartDate.Year) * 12) + allocationFinishDate.Month - allocationStartDate.Month;

            //for (int i = 1; i <= 12; i++)
            for (int i = 0; i <= totalMonths; i++)
            {
                var date = allocationStartDate.AddMonths(i);
                bandedGridView.Bands.Add(new DevExpress.XtraGrid.Views.BandedGrid.GridBand()
                {
                    Name = date.ToString("yyyyMM"),
                    Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month).Capitalize() + " " + (date.Year - 2000).ToString(),
                    Width = 800
                });
            }

            bandedGridView.Bands.Add(new DevExpress.XtraGrid.Views.BandedGrid.GridBand()
            {
                Caption = " "
            });

            bandedGridView.Columns["EmployeeId"].Caption = "EmployeeId";
            bandedGridView.Columns["EmployeeId"].Width = 100;
            bandedGridView.Columns["EmployeeId"].Visible = false;
            bandedGridView.Columns["EmployeeId"].OwnerBand = bandedGridView.Bands[0];

            bandedGridView.Columns["ProjectId"].Caption = "ProjectId";
            bandedGridView.Columns["ProjectId"].Width = 100;
            bandedGridView.Columns["ProjectId"].Visible = false;
            bandedGridView.Columns["ProjectId"].OwnerBand = bandedGridView.Bands[0];

            bandedGridView.Columns["TaskId"].Caption = "TaskId";
            bandedGridView.Columns["TaskId"].Width = 100;
            bandedGridView.Columns["TaskId"].Visible = false;
            bandedGridView.Columns["TaskId"].OwnerBand = bandedGridView.Bands[0];


            bandedGridView.Columns["Auditor"].Caption = "Auditor";
            bandedGridView.Columns["Auditor"].Width = 100;
            bandedGridView.Columns["Auditor"].GroupIndex = 1;
            bandedGridView.Columns["Auditor"].Visible = false;
            bandedGridView.Columns["Auditor"].OwnerBand = bandedGridView.Bands[0];

            /*
            bandedGridView.Columns[1].Caption = "Proyecto";
            bandedGridView.Columns[1].Width = 50;
            bandedGridView.Columns[1].OptionsColumn.AllowMerge = DefaultBoolean.True;
            bandedGridView.Columns[1].OwnerBand = bandedGridView.Bands[0];
            */

            bandedGridView.Columns["Proyecto"].Caption = "Proyecto";
            bandedGridView.Columns["Proyecto"].Width = 150;
            bandedGridView.Columns["Proyecto"].OwnerBand = bandedGridView.Bands[0];
            bandedGridView.Columns["Proyecto"].OptionsColumn.AllowMerge = DefaultBoolean.False;

            bandedGridView.Columns["Allocated"].Caption = "Allocated";
            bandedGridView.Columns["Allocated"].Width = 100;            
            bandedGridView.Columns["Allocated"].Visible = false;
            bandedGridView.Columns["Allocated"].OwnerBand = bandedGridView.Bands[0];

            bandedGridView.Columns["Budgeted"].Caption = "Budgeted";
            bandedGridView.Columns["Budgeted"].Width = 100;            
            bandedGridView.Columns["Budgeted"].Visible = false;
            bandedGridView.Columns["Budgeted"].OwnerBand = bandedGridView.Bands[0];

            bandedGridView.Columns["Budget"].Caption = "Asisgnado/Presupuestado";
            bandedGridView.Columns["Budget"].Width = 80;
            bandedGridView.Columns["Budget"].OwnerBand = bandedGridView.Bands[0];
            bandedGridView.Columns["Budget"].OptionsColumn.AllowMerge = DefaultBoolean.False;


            //for (DateTime date = new DateTime(2021,1,1); date < new DateTime(2021, 12, 31); date = date.AddDays(1.0))
            
            
            for (DateTime date = allocationStartDate; date <= allocationFinishDate; date = date.AddDays(1.0))
            {
                var fieldName = date.ToString("yyyyMMdd");

                bandedGridView.Columns[fieldName].OwnerBand = bandedGridView.Bands[date.ToString("yyyyMM")];
                bandedGridView.Columns[fieldName].Caption = date.ToString("dd");
                bandedGridView.Columns[fieldName].MinWidth = 28;
                bandedGridView.Columns[fieldName].OptionsColumn.AllowMerge = DefaultBoolean.False;

                if (IsWorkingDay(date) && date.Month == 1)
                {
                    //bandedGridView.Columns[fieldName].ColumnEdit = repositoryItemSpinEdit1;
                }
                else
                {
                    bandedGridView.Columns[fieldName].ColumnEdit = null;
                }             
            }


            //bandedGridView.OptionsView.ShowFooter = true;

            gridControl.DataMember = null;
            gridControl.DataSource = null;            

            List<DataColumn> dateColumns = new List<DataColumn>();
            for (DateTime date = allocationStartDate; date <= allocationFinishDate; date = date.AddDays(1.0))
            {
                var fieldName = date.ToString("yyyyMMdd");
                dateColumns.Add(new DataColumn("Assignment" + fieldName, typeof(string)));
                dateColumns.Add(new DataColumn("Allocation" + fieldName, typeof(string)));
            }

            table.Columns.AddRange(dateColumns.ToArray());

            /*
            for (DateTime date = allocationStartDate; date <= allocationFinishDate; date = date.AddDays(1.0))
            {
                var fieldName = date.ToString("yyyyMMdd");
                table.Columns.Add(new DataColumn("Assignment" + fieldName, typeof(string)));
                table.Columns.Add(new DataColumn("Allocation" + fieldName, typeof(string)));
            }
            */


            //

            //Fix();

            ReloadGrid(projectId, employeeId);
            
            gridControl.DataSource = table;
            gridControl.DataMember = table.TableName;

            bandedGridView.EndUpdate();
            gridControl.EndUpdate();

            //bandedGridView.ExpandAllGroups();

            //Data.Audit.OnObjectMessage += Audit_OnObjectMessage;

            //Scroll to ...
            if (!this.IsHandleCreated)
            {
                this.CreateHandle();
            }

                          
            this.BeginInvoke((Action)(() =>
            {
                bandedGridView.ExpandAllGroups();

                if (projectId != "")
                {
                    var project = Data.Audit.Projects[projectId];
                    string fieldName;

                    fieldName = project.FinishDate.ToString("yyyyMMdd");
                    bandedGridView.MakeColumnVisible(bandedGridView.Columns[fieldName]);

                    fieldName = project.StartDate.ToString("yyyyMMdd");
                    bandedGridView.MakeColumnVisible(bandedGridView.Columns[fieldName]);

                    /*
                    fieldName = project.FinishDate.AddDays(1).ToString("yyyyMMdd"); //new DateTime(2021, 11, 30).ToString("yyyyMMdd");
                    bandedGridView.MakeColumnVisible(bandedGridView.Columns[fieldName]);
                    fieldName = project.StartDate.AddDays(-1).ToString("yyyyMMdd"); //fieldName = new DateTime(2021, 10, 1).ToString("yyyyMMdd");
                    bandedGridView.MakeColumnVisible(bandedGridView.Columns[fieldName]);
                    */
                }
                else
                {
                    string fieldName;
                    fieldName = DateTime.Now.AddDays(31).ToString("yyyyMMdd"); //new DateTime(2021, 11, 30).ToString("yyyyMMdd");
                    bandedGridView.MakeColumnVisible(bandedGridView.Columns[fieldName]);
                    fieldName = DateTime.Now.AddDays(-15).ToString("yyyyMMdd"); //fieldName = new DateTime(2021, 10, 1).ToString("yyyyMMdd");
                    bandedGridView.MakeColumnVisible(bandedGridView.Columns[fieldName]);
                }
            }));
            
        }
        private void Fix()
        {
            foreach (var employee in Data.Audit.Employees.Values.OrderBy(p => p.Name))
            {
                foreach (var task in employee.Projects.OrderBy(p => p.Project.StartDate))
                {
                    if (task.Project.StartDate <= allocationFinishDate && task.Project.FinishDate >= allocationStartDate)
                    {
                        {
                            var firstDay = new DateTime(Math.Max(task.Project.StartDate.Ticks, allocationStartDate.Ticks));
                            var lastDay = new DateTime(Math.Min(task.Project.FinishDate.Ticks, allocationFinishDate.Ticks)).AddDays(1);

                            for (DateTime date = firstDay; date < lastDay; date = date.AddDays(1.0))
                            {
                                if (IsWorkingDay(date))
                                {                                    
                                    DataHUBClient.ServiceContract.PostProjectEmployeeAlloc(new ProjectEmployeeAllocBase() { ProjectId = task.Project.DBID, EmployeeId = employee.DBID, AllocDate = date});                                    
                                }
                            }
                        }                        
                    }
                }
            }

        }

        private void ReloadGrid(string projectId = "", string employeeId = "")
        {
            var table = allocationDataTable;
            
            bandedGridView.BeginUpdate();
            table.Clear();

            foreach (var employee in Data.Audit.Employees.Values.OrderBy(p => p.Name))
            {
                if (employeeId != "" && employeeId != employee.DBID)
                    continue;

                if (projectId != "" && employee.Projects.FirstOrDefault(p => p.ProjectId == projectId) == null)
                    continue;

                {
                    var row = table.NewRow();
                    row["EmployeeId"] = employee.DBID;
                    row["ProjectId"] = "0";
                    row["TaskId"] = "0";
                    row["Auditor"] = employee.Name;
                    row["Proyecto"] = " Vacaciones, baja...";

                    //Holidays
                    int totalAllocated = 0;
                    foreach (var alloc in Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.EmployeeId == employee.DBID && p.ProjectId == "0").OrderBy(p => p.AllocDate)) 
                    {
                        var fieldName = alloc.AllocDate.ToString("yyyyMMdd");
                        row[fieldName] = alloc.Quantity;
                        row["Allocation" + fieldName] = (int)AllocationStatus.Allocated;
                        totalAllocated += alloc.Quantity;
                    }

                    row["Allocated"] = totalAllocated;
                    var budgetText = totalAllocated.ToString();                                        
                    budgetText += " Horas";

                    row["Budget"] = budgetText;


                    table.Rows.Add(row);
                }

                

                //Projects
                foreach (var task in employee.Projects.OrderBy(p => p.Project.StartDate))
                {
                    if (projectId != "" && projectId != task.ProjectId)
                        continue;

                    if (task.Project.StartDate <= allocationFinishDate && task.Project.FinishDate >= allocationStartDate)
                    {
                        var row = table.NewRow();

                        row["EmployeeId"] = employee.DBID;
                        row["ProjectId"] = task.Project.DBID;
                        row["TaskId"] = task.Project.DBID;
                        row["Auditor"] = employee.Name;                        
                        row["Proyecto"] = task.Project.Name + " (" + task.Project.TaxYear + ")";

                        {
                            var firstDay = new DateTime(Math.Max(task.Project.StartDate.Ticks, allocationStartDate.Ticks));
                            var lastDay = new DateTime(Math.Min(task.Project.FinishDate.Ticks, allocationFinishDate.Ticks)).AddDays(1);

                            for (DateTime date = firstDay; date < lastDay; date = date.AddDays(1.0))
                            {
                                if (IsWorkingDay(date))
                                {
                                    var fieldName = date.ToString("yyyyMMdd");
                                    row["Assignment" + fieldName] = (int)AssignmentStatus.Assigned;
                                }
                            }
                        }

                        //Allocation                        
                        foreach (var alloc in Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.EmployeeId == employee.DBID && p.ProjectId == task.ProjectId).OrderBy(p => p.AllocDate)) //employee.Projects.OrderBy(p => p.Project.StartDate))
                        {
                            var fieldName = alloc.AllocDate.ToString("yyyyMMdd");                            
                            row[fieldName] = alloc.Quantity;
                            row["Allocation" + fieldName] = (int)AllocationStatus.Allocated;                            
                        }

                        //Total allocation
                        int totalAllocated = 0;
                        foreach (var alloc in Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.ProjectId == task.ProjectId)) 
                        {                            
                            totalAllocated += alloc.Quantity;
                        }

                        //Budget
                        List<BudgetBase> budget = Data.Audit.GetBudget(task.ProjectId);
                        int totalBudgeted = 0;
                        foreach (var budgetCateg in budget)
                        {                            
                            totalBudgeted += budgetCateg.Quantity;
                        }

                        row["Allocated"] = totalAllocated;
                        row["Budgeted"] = totalBudgeted;

                        var budgetText = totalAllocated.ToString();
                        if (projectId != "0")
                            budgetText += @" de " + row["Budgeted"].ToString();
                        budgetText += " Horas";

                        row["Budget"] = budgetText; //totalAllocated.ToString() + @" de " + totalBudgeted.ToString() + " Horas";


                        table.Rows.Add(row);
                    }
                }

                
            }

            bandedGridView.EndUpdate();
            bandedGridView.ExpandAllGroups();            
        }

        /*
        //Deshabilitado --> se refresca al guardar
        private void Audit_OnObjectMessage(ObjectMessageActionType objectMessageActionType, ObjectMessageObjectType objectMessageObjectType, string objectId)
        {
            //ReloadGrid();
            switch (objectMessageActionType )
            {
                case ObjectMessageActionType.Put:
                    switch (objectMessageObjectType)
                    {
                        case ObjectMessageObjectType.Project:
                            {
                                var project = Data.Audit.Projects[objectId];
                                foreach (var row in allocationDataTable.Select($"ProjectId = '{project.DBID}' "))
                                {
                                    for (DateTime date = allocationStartDate; date < allocationFinishDate; date = date.AddDays(1.0))
                                    {                                        
                                        var fieldName = date.ToString("yyyyMMdd");
                                        row["Assignment" + fieldName] = DBNull.Value;
                                    }

                                    var firstDay = new DateTime(Math.Max(project.StartDate.Ticks, allocationStartDate.Ticks));
                                    var lastDay = new DateTime(Math.Min(project.FinishDate.Ticks, allocationFinishDate.Ticks)).AddDays(1);

                                    for (DateTime date = firstDay; date < lastDay; date = date.AddDays(1.0))
                                    {
                                        if (IsWorkingDay(date))
                                        {
                                            var fieldName = date.ToString("yyyyMMdd");
                                            row["Assignment" + fieldName] = (int)AssignmentStatus.Assigned;
                                        }
                                    }

                                }
                            }
                            break;

                        case ObjectMessageObjectType.ProjectEmployeeAssoc:
                            {
                           
                            }
                            break;

                        case ObjectMessageObjectType.ProjectEmployeeAlloc:
                            {
                                var alloc = Data.Audit.ProjectEmployeeAllocs[objectId];
                                var row = allocationDataTable.Select($"ProjectId = '{alloc.ProjectId}' and EmployeeId = '{alloc.EmployeeId}' ").FirstOrDefault();
                                var fieldName = alloc.AllocDate.ToString("yyyyMMdd");
                                row[fieldName] = alloc.Quantity;
                                row["Allocation" + fieldName] = (int)AllocationStatus.Allocated;
                            }
                            break;
                    }
                    break;

                case ObjectMessageActionType.Post:
                    switch (objectMessageObjectType)
                    {
                        case ObjectMessageObjectType.ProjectEmployeeAssoc:
                            {
                                var assoc = Data.Audit.ProjectEmployeeAssocs[objectId];
                                var row = allocationDataTable.NewRow();

                                row["EmployeeId"] = assoc.EmployeeId;
                                row["ProjectId"] = assoc.ProjectId;
                                row["TaskId"] = assoc.ProjectId;
                                row["Auditor"] = assoc.Employee.Name;
                                row["Proyecto"] = assoc.Project.Name;

                                var firstDay = new DateTime(Math.Max(assoc.Project.StartDate.Ticks, allocationStartDate.Ticks));
                                var lastDay = new DateTime(Math.Min(assoc.Project.FinishDate.Ticks, allocationFinishDate.Ticks)).AddDays(1);

                                for (DateTime date = firstDay; date < lastDay; date = date.AddDays(1.0))
                                {
                                    if (IsWorkingDay(date))
                                    {
                                        var fieldName = date.ToString("yyyyMMdd");
                                        row["Assignment" + fieldName] = (int)AssignmentStatus.Assigned;
                                    }
                                }

                                foreach (var alloc in Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.EmployeeId == assoc.EmployeeId && p.ProjectId == assoc.ProjectId).OrderBy(p => p.AllocDate)) 
                                {
                                    var fieldName = alloc.AllocDate.ToString("yyyyMMdd");
                                    row[fieldName] = alloc.Quantity;
                                    row["Allocation" + fieldName] = (int)AllocationStatus.Allocated;
                                }

                                allocationDataTable.Rows.Add(row);
                            }
                            break;

                        case ObjectMessageObjectType.ProjectEmployeeAlloc:
                            {
                                var alloc = Data.Audit.ProjectEmployeeAllocs[objectId];
                                var row = allocationDataTable.Select($"ProjectId = '{alloc.ProjectId}' and EmployeeId = '{alloc.EmployeeId}' ").FirstOrDefault();
                                if (row != null)
                                {
                                    var fieldName = alloc.AllocDate.ToString("yyyyMMdd");
                                    row[fieldName] = alloc.Quantity;
                                    row["Allocation" + fieldName] = (int)AllocationStatus.Allocated;
                                }
                            }
                            break;
                    }
                    break;

                case ObjectMessageActionType.Delete:
                    switch (objectMessageObjectType)
                    {
                        case ObjectMessageObjectType.ProjectEmployeeAssoc:
                            {
                                var assoc = Data.Audit.ProjectEmployeeAssocs[objectId];
                                var row = allocationDataTable.Select($"ProjectId = '{assoc.ProjectId}' and EmployeeId = '{assoc.EmployeeId}' ").FirstOrDefault();
                                if (row != null)
                                {
                                    row.Delete();
                                }
                            }
                            break;

                        case ObjectMessageObjectType.ProjectEmployeeAlloc:
                            {
                                if (Data.Audit.ProjectEmployeeAllocs.ContainsKey(objectId))
                                {
                                    var alloc = Data.Audit.ProjectEmployeeAllocs[objectId];
                                    var row = allocationDataTable.Select($"ProjectId = '{alloc.ProjectId}' and EmployeeId = '{alloc.EmployeeId}' ").FirstOrDefault();
                                    if (row != null)
                                    {
                                        var fieldName = alloc.AllocDate.ToString("yyyyMMdd");
                                        row[fieldName] = DBNull.Value;
                                        row["Allocation" + fieldName] = DBNull.Value;  //(int)AllocationStatus.NotAllocated;
                                    }
                                }
                            }
                            break;
                    }
                    break;
            }

            bandedGridView.RefreshData();


        }
        */
        private void bandedGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            e.Handled = false;
            return;


            if (e.Column.AbsoluteIndex >= firstDateColumn)
            {
                DateTime date = DateTime.ParseExact(e.Column.FieldName, "yyyyMMdd", CultureInfo.InvariantCulture);

                var alpha = 255; // (date.Month == 1) ? 255 : 64;

                //
                
                int horas = -1;
                if (e.CellValue.ToString() != "")
                    int.TryParse(e.CellValue.ToString(), out horas);


                switch (horas)
                {
                    case -1:
                        {
                            e.Appearance.ForeColor = Color.White;
                            e.Appearance.BackColor = Color.White;
                            var rect = e.Bounds;
                            rect.Inflate(1, 1);
                            e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                        }
                        break;
                    case 0:
                        {
                            e.Appearance.ForeColor = Color.FromArgb(192, 64, 128, 255);
                            e.Appearance.BackColor = Color.White;

                            var rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 1, e.Bounds.Height + 1);
                            e.Cache.DrawRectangle(rect, e.Appearance.ForeColor, 2);                           
                        }
                        break;
                    case 1:
                        {
                            e.Appearance.ForeColor = Color.FromArgb(192, 64, 128, 255);
                            e.Appearance.BackColor = Color.FromArgb(192, 64, 128, 255);
                            var rect = e.Bounds;
                            rect.Inflate(1, 1);
                            e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                        }
                        break;
                    case 2:
                        {
                            e.Appearance.ForeColor = Color.FromArgb(192, 64, 255, 128);
                            e.Appearance.BackColor = Color.FromArgb(192, 64, 255, 128);
                            var rect = e.Bounds;
                            rect.Inflate(1, 1);
                            e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                        }
                        break;
                    default:
                        {
                            e.Appearance.ForeColor = Color.White;
                            e.Appearance.BackColor = Color.White;
                            var rect = e.Bounds;
                            rect.Inflate(1, 1);
                            e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                        }
                        break;
                }

                if (e.Appearance.ForeColor == Color.White)
                {
                    if (!IsWorkingDay(date))
                        e.Appearance.BackColor = Color.FromArgb(255, 240, 240, 240);

                    if (IsHolydayDay(date))
                        e.Appearance.BackColor = Color.FromArgb(alpha, 255, 224, 224);

                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }
               
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void bandedGridView_CustomDrawRowFooter(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            e.Handled = true;
        }

        bool IsHolydayDay(DateTime date)
        {
            return Data.Audit.Holidays.FirstOrDefault(p => p.Year == date.Year && p.Month == date.Month && p.Day == date.Day) != null;
        }

        bool IsWorkingDay(DateTime date)
        {
            return !(IsHolydayDay(date) || date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
        }

        private void bandedGridView_CustomDrawRowFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            if (e.Column.AbsoluteIndex >= firstDateColumn)
            {
                DateTime date = DateTime.ParseExact(e.Column.FieldName, "yyyyMMdd", CultureInfo.InvariantCulture);
                if (IsWorkingDay(date))
                {
                    var alpha = 255; //(date.Month == 1) ? 255 : 64;

                    var color = Color.FromArgb(alpha, 128, 255, 128);

                    int horas = 0;
                    int.TryParse(e.Info.DisplayText, out horas);
                    
                    if (horas < 1)
                        color = Color.FromArgb(alpha, 255, 128, 128);
                    if (horas > 1)
                        color = Color.FromArgb(alpha, 255, 255, 128);

                    int dx = e.Bounds.Height;
                    Brush brush = e.Cache.GetGradientBrush(e.Bounds,  Color.White, color, LinearGradientMode.Vertical);
                    Rectangle r = e.Bounds;

                    
                    e.Appearance.ForeColor = Color.FromArgb(alpha, 0, 0, 0);

                    /*
                    //Draw a 3D border
                    BorderPainter painter = BorderHelper.GetPainter(DevExpress.XtraEditors.Controls.BorderStyles.Style3D);
                    AppearanceObject borderAppearance = new AppearanceObject(e.Appearance);
                    borderAppearance.BorderColor = Color.DarkGray;
                    painter.DrawObject(new BorderObjectInfoArgs(e.Cache, borderAppearance, r));
                    */

                    //Fill the inner region of the cell
                    r.Inflate(-1, -1);
                    e.Cache.FillRectangle(brush, r);
                    //Draw a summary value
                    r.Inflate(-2, 0);
                    e.Appearance.DrawString(e.Cache, e.Info.DisplayText + " ", r);
                    //Prevent default drawing of the cell
                }
            }

            
            e.Handled = true;
        }

        private void bandedGridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (bandedGridView.FocusedColumn.AbsoluteIndex < firstDateColumn)
            {
                e.Cancel = true;
            }            
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            bandedGridView.PostEditor();
            bandedGridView.UpdateCurrentRow();

            foreach (DataRow row in allocationDataTable.Rows)
            {
                var employee = Data.Audit.Employees[row["EmployeeId"].ToString()];
                Project project = null;
                if (row["ProjectId"].ToString() != "")
                    project = Data.Audit.Projects[row["ProjectId"].ToString()];

                for (DateTime date = allocationStartDate; date <= allocationFinishDate; date = date.AddDays(1.0))
                {
                    var fieldName = date.ToString("yyyyMMdd");
                    
                    AllocationStatus allocationStatus = AllocationStatus.NotAllocated;
                    if (row["Allocation" + fieldName].ToString() != "")
                        allocationStatus = (AllocationStatus)int.Parse(row["Allocation" + fieldName].ToString());

                    int qty = 0;
                    int.TryParse(row[fieldName].ToString(), out qty);

                    switch (allocationStatus)
                    {
                        case AllocationStatus.NewAllocation:
                            DataHUBClient.ServiceContract.PostProjectEmployeeAlloc(new ProjectEmployeeAllocBase() { ProjectId = project.DBID, EmployeeId = employee.DBID, AllocDate = date, Quantity = qty});

                            break;

                        case AllocationStatus.DeletedAllocation:
                            var alloc = Data.Audit.ProjectEmployeeAllocs.FirstOrDefault(p => p.Value.ProjectId == project.DBID && p.Value.EmployeeId == employee.DBID && p.Value.AllocDate == date);
                            if (alloc.Value != null)
                                DataHUBClient.ServiceContract.DeleteProjectEmployeeAlloc(new ProjectEmployeeAllocBase() { DBID = alloc.Value.DBID });

                            break;
                    }                    
                }
            }

            Application.DoEvents();

            Data.Audit.ReadDB();
            ReloadGrid(projectId, employeeId);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gridControl.ExportToXlsx(@"c:\temp\horas.xlsx", new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
                Process.Start(@"c:\temp\horas.xlsx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void bandedGridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Column.AbsoluteIndex >= firstDateColumn)
                {
                    ToggleFocusedCell();
                }
            }
        }

        private void bandedGridView_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            var view = bandedGridView;

            if (e.Column.AbsoluteIndex == firstDateColumn - 1)
            {
                var rowIndex = bandedGridView.GetDataSourceRowIndex(e.RowHandle);
                DataRow row = ((DataTable)gridControl.DataSource).Rows[rowIndex];

                int projectId = 0;
                Int32.TryParse(row["ProjectId"].ToString(), out projectId);

                if (projectId > 0)
                {
                    int budgeted = 0;
                    Int32.TryParse(row["Budgeted"].ToString(), out budgeted);
                    int allocated = 0;
                    Int32.TryParse(row["Allocated"].ToString(), out allocated);

                    if (allocated > budgeted)
                    {
                        e.Appearance.BackColor = Color.FromArgb(255, 192, 192);
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.FromArgb(192, 255, 192);
                    }
                }
                else
                {
                    e.Appearance.BackColor = Color.White;
                }
            }
            else
            if (e.Column.AbsoluteIndex >= firstDateColumn)
            {
                if (e.Column.FieldName.Substring(0, 1) == "A")
                    return;

                DateTime date = DateTime.ParseExact(e.Column.FieldName, "yyyyMMdd", CultureInfo.InvariantCulture);
                var alpha = 255; //(date.Month == 1) ? 255 : 64;

                int horas = -1;
                if (e.CellValue.ToString() != "")
                    int.TryParse(e.CellValue.ToString(), out horas);

                var rowIndex = bandedGridView.GetDataSourceRowIndex(e.RowHandle);               
                DataRow row = ((DataTable)gridControl.DataSource).Rows[rowIndex];

                AssignmentStatus assignmentStatus = AssignmentStatus.NotAssigned;
                if (row["Assignment" + e.Column.FieldName].ToString() != "")
                    assignmentStatus = (AssignmentStatus)int.Parse(row["Assignment" + e.Column.FieldName].ToString());                

                AllocationStatus allocationStatus = AllocationStatus.NotAllocated;
                if (row["Allocation" + e.Column.FieldName].ToString() != "")
                    allocationStatus = (AllocationStatus)int.Parse(row["Allocation" + e.Column.FieldName].ToString());                

                e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;


                switch (assignmentStatus)
                {
                    case AssignmentStatus.NotAssigned:
                    case AssignmentStatus.DeletedAssignment:
                        switch (allocationStatus)
                        {
                            case AllocationStatus.NotAllocated:
                            case AllocationStatus.DeletedAllocation:
                                e.Appearance.ForeColor = Color.Black;
                                e.Appearance.BackColor = Color.White;
                                break;

                            case AllocationStatus.Allocated:
                            case AllocationStatus.NewAllocation:
                                e.Appearance.FontStyleDelta = FontStyle.Bold;
                                e.Appearance.ForeColor = Color.FromArgb(255, 255, 160, 160); //Color.FromArgb(192, 255, 192, 64);
                                e.Appearance.BackColor = Color.Navy; //Color.FromArgb(192, 255, 192, 64);
                                break;
                        }
                        break;

                    case AssignmentStatus.Assigned:
                    case AssignmentStatus.NewAssignment:
                        switch (allocationStatus)
                        {
                            case AllocationStatus.NotAllocated:
                            case AllocationStatus.DeletedAllocation:
                                e.Appearance.ForeColor = Color.FromArgb(192, 192, 240, 255);
                                e.Appearance.BackColor = Color.FromArgb(192, 192, 240, 255);
                                break;

                            case AllocationStatus.Allocated:
                            case AllocationStatus.NewAllocation:
                                e.Appearance.FontStyleDelta = FontStyle.Bold;
                                e.Appearance.ForeColor = Color.White; //Color.FromArgb(192, 64, 255, 128);
                                e.Appearance.BackColor = Color.Navy; //Color.FromArgb(192, 64, 255, 128);                                
                                break;
                        }
                        break;
                }
                
                if (e.Appearance.ForeColor == Color.Black)
                {
                    if (!IsWorkingDay(date))
                        e.Appearance.BackColor = Color.FromArgb(255, 240, 240, 240);

                    if (IsHolydayDay(date))
                        e.Appearance.BackColor = Color.FromArgb(alpha, 255, 224, 224);
                }

            }
            else
            {
                e.Appearance.ForeColor = Color.Black;
                e.Appearance.BackColor = Color.White;
            }
            

        }

        private void bandedGridView_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            if (ea.Button == MouseButtons.Left)
            {
                GridView view = sender as GridView;
                GridHitInfo info = view.CalcHitInfo(ea.Location);
                if (info.InRow || info.InRowCell)
                {
                    if (info.Column != null && info.Column.AbsoluteIndex >= firstDateColumn)
                    {
                        ToggleFocusedCell();
                    }

                    //string colCaption = info.Column == null ? "N/A" : info.Column.GetCaption();
                    //MessageBox.Show(string.Format("DoubleClick on row: {0}, column: {1}.", info.RowHandle, colCaption));
                }
            }
        }

        private void UpdateBudget(string projectId)
        {
            int totalAllocated = 0;
            foreach (DataRow row in allocationDataTable.Rows)
            {
                if (row["ProjectId"].ToString() == projectId)
                {
                    for (DateTime date = allocationStartDate; date <= allocationFinishDate; date = date.AddDays(1.0))
                    {
                        var fieldName = date.ToString("yyyyMMdd");
                        if (row[fieldName] != DBNull.Value)
                            totalAllocated += int.Parse(row[fieldName].ToString());
                    }
                }                
            }

            //Suma las horas de los auditores que no se ven
            if (this.employeeId != "")
            {
                foreach (var alloc in Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.ProjectId == projectId && p.EmployeeId != employeeId))
                {
                    totalAllocated += alloc.Quantity;
                }
            }            

            foreach (DataRow row in allocationDataTable.Rows)
            {
                if (row["ProjectId"].ToString() == projectId)
                {
                    row["Allocated"] = totalAllocated;

                    var budgetText = totalAllocated.ToString();
                    if (projectId != "0")
                        budgetText += @" de " + row["Budgeted"].ToString();
                    budgetText += " Horas";

                    row["Budget"] = budgetText; //totalAllocated.ToString() + @" de " + row["Budgeted"].ToString() + " Horas";
                }
            }
        }

        private void ToggleFocusedCell()
        {
            if (Data.Audit.UserPermissions <= UserPermissions.Read)
                return;

            DataRow row = bandedGridView.GetFocusedDataRow();
            var fieldName = bandedGridView.FocusedColumn.FieldName;

            AllocationStatus allocationStatus = AllocationStatus.NotAllocated;
            if (row["Allocation" + fieldName].ToString() != "")
                allocationStatus = (AllocationStatus)int.Parse(row["Allocation" + fieldName].ToString());

            switch (allocationStatus)
            {
                case AllocationStatus.Allocated:
                    row["Allocation" + fieldName] = (int)AllocationStatus.DeletedAllocation;
                    row[fieldName] = DBNull.Value;
                    break;
                case AllocationStatus.NotAllocated:
                    row["Allocation" + fieldName] = (int)AllocationStatus.NewAllocation;
                    if (row[fieldName] == DBNull.Value)
                        row[fieldName] = 8;
                    break;
                case AllocationStatus.NewAllocation:
                    row["Allocation" + fieldName] = (int)AllocationStatus.NotAllocated;
                    row[fieldName] = DBNull.Value;
                    break;
                case AllocationStatus.DeletedAllocation:
                    row["Allocation" + fieldName] = (int)AllocationStatus.Allocated;
                    if (row[fieldName] == DBNull.Value)
                        row[fieldName] = 8;
                    break;
            }

            bandedGridView.PostEditor();
            bandedGridView.UpdateCurrentRow();
            UpdateBudget(row["ProjectId"].ToString());
        }

        private void bandedGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32)
            {
                if (bandedGridView.FocusedColumn.AbsoluteIndex >= firstDateColumn)
                {
                    ToggleFocusedCell();
                }
            }
        }

        private void bandedGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DataRow row = bandedGridView.GetFocusedDataRow();
            var fieldName = bandedGridView.FocusedColumn.FieldName;

            AllocationStatus allocationStatus = AllocationStatus.NotAllocated;
            if (row["Allocation" + fieldName].ToString() != "")
                allocationStatus = (AllocationStatus)int.Parse(row["Allocation" + fieldName].ToString());

            int value = 0;
            int.TryParse(e.Value.ToString(), out value);

            switch (allocationStatus)
            {
                case AllocationStatus.Allocated:
                    if (value == 0)  //if (row[fieldName] == DBNull.Value || (int)row[fieldName] == 0)
                        row["Allocation" + fieldName] = (int)AllocationStatus.DeletedAllocation;
                    row[fieldName] = e.Value;
                    break;
                case AllocationStatus.NotAllocated:
                    if (value > 0) //if (row[fieldName] != DBNull.Value && (int)row[fieldName] != 0)
                        row["Allocation" + fieldName] = (int)AllocationStatus.NewAllocation;
                    row[fieldName] = e.Value;
                    break;
                case AllocationStatus.NewAllocation:
                    if (value == 0) //if (row[fieldName] == DBNull.Value || (int)row[fieldName] == 0)
                        row["Allocation" + fieldName] = (int)AllocationStatus.NotAllocated;
                    row[fieldName] = e.Value;
                    break;
                case AllocationStatus.DeletedAllocation:
                    if (value > 0) //if (row[fieldName] != DBNull.Value && (int)row[fieldName] != 0)
                        row["Allocation" + fieldName] = (int)AllocationStatus.Allocated;
                    row[fieldName] = e.Value;
                    break;
            }

            bandedGridView.PostEditor();
            bandedGridView.UpdateCurrentRow();
            UpdateBudget(row["ProjectId"].ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
