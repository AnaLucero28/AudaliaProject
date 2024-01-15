using Audalia.DataHUBClient;
using Audalia.DataHUBCommon;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace WindowsApplication
{
    //public enum RowCellStatus { NotAllocated, AllocatedToOtherProject, AllocatedToCurrentProject, NewAllocationToCurrentProject, DeletedAllocationFromOtherProject, DeletedAllocationFromCurrentProject };
    //public delegate void EventHandler(string message);

    public partial class AllocationUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        DateTime allocationStartDate;
        DateTime allocationFinishDate;

        DataTable allocationDataTable;
        int firstDateColumn = 0;

        string projectId = "";
        string employeeId = "";

        DateTime projectStartDate1;
        DateTime projectFinishDate1;

        DateTime? projectStartDate2 = null;
        DateTime? projectFinishDate2 = null;

        DateTime? projectStartDate3 = null;
        DateTime? projectFinishDate3 = null;

        //bool modified = false;
        //public bool Modified { get => modified; set => modified = value; }

        public event EventHandler OnModify;

        void InvokeOnModify()
        {
            OnModify?.Invoke(this, null);
        }
        
        public DevExpress.XtraGrid.GridControl GridControl
        {
            get
            {
                return this.gridControl;
            }
        }

        public DevExpress.XtraGrid.Views.BandedGrid.BandedGridView GridView
        {
            get
            {
                return this.bandedGridView;
            }
        }

        public DataTable AllocationDataTable
        {
            get
            {
                return allocationDataTable;
            }
        }

        public DateTime AllocationStartDate
        {
            get { return allocationStartDate; }
        }
        public DateTime AllocationFinishDate
        {
            get { return allocationFinishDate; }
        }
        
        public AllocationUserControl(string projectId)
        {
            InitializeComponent();

            this.projectId = projectId;            
            Init(projectId, "");

            //Init("251", "");
        }

        void Init(string projectId = "", string employeeId = "")
        {
            gridControl.BeginUpdate();

            bandedGridView.CustomDrawCell += bandedGridView_CustomDrawCell;
            bandedGridView.CustomDrawColumnHeader += BandedGridView_CustomDrawColumnHeader;
            bandedGridView.ShowingEditor += BandedGridView_ShowingEditor;
            bandedGridView.ShownEditor += BandedGridView_ShownEditor;            
            bandedGridView.RowCellClick += BandedGridView_RowCellClick;
            gridControl.MouseMove += GridControl_MouseMove;

            //

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

            if (Data.Audit.UserPermissions <= UserPermissions.Read)
            {                
                bandedGridView.OptionsBehavior.ReadOnly = true;
            }

            //

            this.projectId = projectId;
            this.employeeId = employeeId;

            if (projectId != "")
            {
                var project = Data.Audit.Projects[projectId];

                projectStartDate1 = project.StartDate1;
                projectFinishDate1 = project.FinishDate1;

                projectStartDate2 = project.StartDate2;
                projectFinishDate2 = project.FinishDate2;

                projectStartDate3 = project.StartDate3;
                projectFinishDate3 = project.FinishDate3;
            }

            allocationDataTable = new DataTable();
            var table = allocationDataTable;
            table.Columns.Add(new DataColumn($"Checked", typeof(bool)));
            table.Columns.Add(new DataColumn($"EmployeeId", typeof(string)));
            table.Columns.Add(new DataColumn($"Auditor", typeof(string)));
            table.Columns.Add(new DataColumn($"Category", typeof(string)));
            table.Columns.Add(new DataColumn($"BranchOffice", typeof(string)));
            table.Columns.Add(new DataColumn($"LeavingDate", typeof(DateTime)));
            
            firstDateColumn = table.Columns.Count;
            for (DateTime date = allocationStartDate; date <= allocationFinishDate; date = date.AddDays(1.0))
            {
                //Allocated hours 

                var fieldName = date.ToString("yyyyMMdd");
                table.Columns.Add(new DataColumn(fieldName, typeof(int)));
            }

            //            
            
            gridControl.DataSource = table;
            gridControl.DataMember = table.TableName;

            //Grid columns

            gridControl.DataSource = null;
           
            bandedGridView.OptionsView.AllowCellMerge = true;
            bandedGridView.Bands[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            bandedGridView.Bands[0].Width = 600;

            var totalMonths = ((allocationFinishDate.Year - allocationStartDate.Year) * 12) + allocationFinishDate.Month - allocationStartDate.Month;            
            for (int i = 0; i <= totalMonths; i++)
            {
                var date = allocationStartDate.AddMonths(i);
                bandedGridView.Bands.Add(new DevExpress.XtraGrid.Views.BandedGrid.GridBand()
                {
                    Name = date.ToString("yyyyMM"),
                    Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month).Capitalize() + " " + date.ToString("yy"),
                    Width = 800
                });
            }

            bandedGridView.Bands.Add(new DevExpress.XtraGrid.Views.BandedGrid.GridBand()
            {
                Caption = " "
            });
            
            bandedGridView.Columns["Checked"].Caption = " ";
            bandedGridView.Columns["Checked"].Width = 24;
            bandedGridView.Columns["Checked"].OwnerBand = bandedGridView.Bands[0];
            bandedGridView.Columns["Checked"].OptionsColumn.AllowMerge = DefaultBoolean.False;
            
            bandedGridView.Columns["EmployeeId"].Caption = "EmployeeId";
            bandedGridView.Columns["EmployeeId"].Width = 100;
            bandedGridView.Columns["EmployeeId"].Visible = false;
            bandedGridView.Columns["EmployeeId"].OwnerBand = bandedGridView.Bands[0];            
            
            bandedGridView.Columns["Auditor"].Caption = "Auditor";
            bandedGridView.Columns["Auditor"].Width = 150;
            bandedGridView.Columns["Auditor"].OwnerBand = bandedGridView.Bands[0];
            bandedGridView.Columns["Auditor"].OptionsColumn.AllowMerge = DefaultBoolean.False;
            bandedGridView.Columns["Auditor"].SortIndex = 0;
            
            bandedGridView.Columns["Category"].Caption = "Categoría";
            bandedGridView.Columns["Category"].Width = 70;
            bandedGridView.Columns["Category"].OwnerBand = bandedGridView.Bands[0];
            bandedGridView.Columns["Category"].OptionsColumn.AllowMerge = DefaultBoolean.False;
            
            bandedGridView.Columns["BranchOffice"].Caption = "Delegación";
            bandedGridView.Columns["BranchOffice"].Width = 70;
            bandedGridView.Columns["BranchOffice"].OwnerBand = bandedGridView.Bands[0];
            bandedGridView.Columns["BranchOffice"].OptionsColumn.AllowMerge = DefaultBoolean.False;

            bandedGridView.Columns["LeavingDate"].Caption = "Fecha de baja";
            bandedGridView.Columns["LeavingDate"].Width = 100;
            bandedGridView.Columns["LeavingDate"].Visible = false;
            bandedGridView.Columns["LeavingDate"].OwnerBand = bandedGridView.Bands[0];

            for (DateTime date = allocationStartDate; date <= allocationFinishDate; date = date.AddDays(1.0))
            {
                var fieldName = date.ToString("yyyyMMdd");
                
                bandedGridView.Columns[fieldName].OwnerBand = bandedGridView.Bands[date.ToString("yyyyMM")];
                bandedGridView.Columns[fieldName].Caption = date.ToString("dd");
                bandedGridView.Columns[fieldName].MinWidth = 28;
                bandedGridView.Columns[fieldName].OptionsColumn.AllowMerge = DefaultBoolean.False;                
            }

            //Flags
            
            for (DateTime date = allocationStartDate; date <= allocationFinishDate; date = date.AddDays(1.0))
            {                
                var fieldName = date.ToString("yyyyMMdd");                

                table.Columns.Add(new DataColumn("CurrentProjectStatus" + fieldName, typeof(int)));
                table.Columns.Add(new DataColumn("OtherProjectStatus" + fieldName, typeof(int)));                
                table.Columns.Add(new DataColumn("OtherProjectValue" + fieldName, typeof(int)));                

                table.Columns.Add(new DataColumn("Hint" + fieldName, typeof(string)));
                table.Columns.Add(new DataColumn("Holidays" + fieldName, typeof(string)));
                table.Columns.Add(new DataColumn("Leave" + fieldName, typeof(string)));                            
            }

            bandedGridView.EndInit();

            //

            ReloadGrid(projectId, employeeId);            
            gridControl.DataSource = table;


            gridControl.EndUpdate();

            //Scroll to ...
            if (!this.IsHandleCreated)
            {
                this.CreateHandle();
            }

            this.BeginInvoke((Action)(() =>
            {
                if (projectId != "")
                {
                    var project = Data.Audit.Projects[projectId];                   
                    projectStartDate1 = project.StartDate1;
                    projectFinishDate1 = project.FinishDate1;

                    projectStartDate2 = project.StartDate2;
                    projectFinishDate2 = project.FinishDate2;

                    projectStartDate3 = project.StartDate3;
                    projectFinishDate3 = project.FinishDate3;
                }
                
                {
                    string fieldName;                 
                    fieldName = projectFinishDate1.AddDays(30).ToString("yyyyMMdd");
                    if (bandedGridView.Columns.ColumnByFieldName(fieldName) != null)
                    {
                        bandedGridView.MakeColumnVisible(bandedGridView.Columns[fieldName]);
                    }
                    else
                    {
                        bandedGridView.MakeColumnVisible(bandedGridView.Columns[bandedGridView.Columns.Count - 1]);
                    }                
                    
                    fieldName = projectStartDate1.ToString("yyyyMMdd");
                    bandedGridView.MakeColumnVisible(bandedGridView.Columns[fieldName]);
                }                
            }));
        }


        private bool IsProjectDay(DateTime date)
        {
            bool result = false;

            if (date >= projectStartDate1 && date <= projectFinishDate1)
                result = true;
            else
                if (date >= projectStartDate2 && date <= projectFinishDate2)
                result = true;
            else
                if (date >= projectStartDate3 && date <= projectFinishDate3)
                result = true;

            return result;
        }

        private void BandedGridView_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.AbsoluteIndex >= firstDateColumn)
            {
                DateTime date = DateTime.ParseExact(e.Column.FieldName, "yyyyMMdd", CultureInfo.InvariantCulture);

                //if (IsProjectDay(date) && IsWorkingDay(date))
                if (IsProjectDay(date)) 
                {
                    e.Cache.FillRectangle(Color.Navy, e.Bounds);
                    //e.Cache.FillRectangle(Color.Gray, e.Bounds);
                    var rect = e.Bounds;
                    rect.Inflate(1, 0);
                    //e.Cache.DrawRectangle(new Pen(Color.White), rect);
                    e.Cache.DrawRectangle(new Pen(Color.LightGray), rect);
                    Brush textBrush = e.Cache.GetSolidBrush(Color.White);                    
                    //Brush textBrush = e.Cache.GetSolidBrush(Color.Navy);
                    e.Appearance.DrawString(e.Cache, e.Info.Caption, e.Info.CaptionRect, textBrush);                    
                }
                else
                {
                    e.DefaultDraw();
                }
            }
            else
                e.DefaultDraw();

            e.Handled = true;
        }

        public void ReloadGrid(string projectId = "", string employeeId = "")
        {           
            var table = allocationDataTable;

            gridControl.BeginUpdate();
            bandedGridView.BeginUpdate();

            table.Clear();

            foreach (var employee in Data.Audit.Employees.Values.OrderBy(p => p.Name))
            {                
                var row = table.NewRow();

                row["EmployeeId"] = employee.DBID;
                row["Auditor"] = employee.Name;
                row["Category"] = employee.Category;
                row["BranchOffice"] = employee.BranchOffice;
                if (employee.LeavingDate == null)
                    row["LeavingDate"] = DBNull.Value;
                else
                    row["LeavingDate"] = employee.LeavingDate;

                //Projects

                row["Checked"] = false;

                foreach (var task in employee.Projects.OrderBy(p => p.Project.StartDate))
                {
                    if (task.ProjectId == projectId)
                        row["Checked"] = true;

                    //Allocation                        
                    foreach (var alloc in Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.EmployeeId == employee.DBID && p.ProjectId == task.ProjectId).OrderBy(p => p.AllocDate))
                    {
                        var fieldName = alloc.AllocDate.ToString("yyyyMMdd");
                        row[fieldName] = (row[fieldName] is DBNull ? 0 : (int)row[fieldName]) + alloc.Quantity;

                        if (task.ProjectId == projectId)
                        {
                            row["CurrentProjectStatus" + fieldName] = (int)AllocationStatus.Allocated;
                            row["Hint" + fieldName] += (row["Hint" + fieldName] is DBNull ? "" : Environment.NewLine) + task.Project.Name;
                        }
                        else
                        {
                            row["OtherProjectStatus" + fieldName] = (int)AllocationStatus.Allocated;
                            row["OtherProjectValue" + fieldName] = (row["OtherProjectValue" + fieldName] is DBNull ? 0 : (int)row["OtherProjectValue" + fieldName]) + alloc.Quantity;
                            row["Hint" + fieldName] += (row["Hint" + fieldName] is DBNull ? "" : Environment.NewLine) + task.Project.Name;
                        }
                    }
                }

                //Leave

                if (employee.LeavingDate != null)
                {
                    for (DateTime date = (DateTime)employee.LeavingDate; date <= allocationFinishDate; date = date.AddDays(1.0))
                    {
                        var fieldName = date.ToString("yyyyMMdd");
                        row["Leave" + fieldName] = 1;
                    }
                }

                //Holidays

                foreach (var alloc in Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.EmployeeId == employee.DBID && p.ProjectId == "0").OrderBy(p => p.AllocDate))
                {
                    var fieldName = alloc.AllocDate.ToString("yyyyMMdd");
                    row["Holidays" + fieldName] = 1;
                    row["Hint" + fieldName] += (row["Hint" + fieldName] is DBNull ? "" : Environment.NewLine) + Data.Audit.Projects["0"].Name;
                }


                table.Rows.Add(row);
            }

            bandedGridView.RefreshData();
            bandedGridView.EndUpdate();
            bandedGridView.ExpandAllGroups();

            gridControl.EndUpdate();
        }
                       
        private void BandedGridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (bandedGridView.FocusedColumn.AbsoluteIndex == 0)
            {
                //Check de asignación al proyecto
            }
            else
            if (bandedGridView.FocusedColumn.AbsoluteIndex > 0 && bandedGridView.FocusedColumn.AbsoluteIndex < firstDateColumn)
            {
                //Campos de la izqda
                e.Cancel = true;
            }            
            else
            {                
                //Campos de fecha
                e.Cancel = true;

                //TODO: Habilitar ¿?
                /*
                var rowIndex = bandedGridView.GetDataSourceRowIndex(bandedGridView.FocusedRowHandle);
                DataRow row = ((DataTable)gridControl.DataSource).Rows[rowIndex];

                if (row["OtherProjectStatus" + bandedGridView.FocusedColumn.FieldName].ToAllocationStatus() == AllocationStatus.Allocated)
                {
                    e.Cancel = true;
                }
                */
            }
        }

        private void BandedGridView_ShownEditor(object sender, EventArgs e)
        {
            if (bandedGridView.FocusedColumn.AbsoluteIndex == 0)
            {
                bandedGridView.ActiveEditor.EditValueChanging += ActiveEditor_EditValueChanging;
            }
        }

        private void ActiveEditor_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            var rowIndex = bandedGridView.GetDataSourceRowIndex(bandedGridView.FocusedRowHandle);
            DataRow row = ((DataTable)gridControl.DataSource).Rows[rowIndex];
            var projectName = projectId != "" ? " \"" + Data.Audit.Projects[projectId].Name + "\"" : "";

            if (!(bool)e.NewValue)
            {
                if (MessageBox.Show("¿Desasignar al auditor \"" + row["Auditor"] + "\" del proyecto" + projectName + " y eliminar todas las jornadas asociadas?", "Desasignar Auditor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    row.BeginEdit();
                    row["Checked"] = e.NewValue;
                    row.EndEdit();
                                        
                    for (DateTime date = allocationStartDate; date <= allocationFinishDate; date = date.AddDays(1.0))
                    {
                        var fieldName = date.ToString("yyyyMMdd");

                        if (row["CurrentProjectStatus" + fieldName].ToAllocationStatus() == AllocationStatus.Allocated)
                        {
                            row["CurrentProjectStatus" + fieldName] = (int)AllocationStatus.DeletedAllocation;
                            //row[fieldName] = DBNull.Value;
                        }
                        else
                            if (row["CurrentProjectStatus" + fieldName].ToAllocationStatus() == AllocationStatus.NewAllocation)
                        {
                            row["CurrentProjectStatus" + fieldName] = (int)AllocationStatus.NotAllocated;
                            row[fieldName] = DBNull.Value;
                        }
                    }

                    bandedGridView.PostEditor();
                    bandedGridView.UpdateCurrentRow();

                    InvokeOnModify();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {                                
                row.BeginEdit();
                row["Checked"] = e.NewValue;

                if (MessageBox.Show("¿Asignar todas las jornadas disponibles del auditor \"" + row["Auditor"] + "\" al proyecto" + projectName + "?", "Asignar Auditor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    AutoAllocateSelectedEmployee(row);
                }

                row.EndEdit();
                bandedGridView.PostEditor();
                bandedGridView.UpdateCurrentRow();

                InvokeOnModify();
            }
        }

        private void AutoAllocateSelectedEmployee(DataRow row)
        {
            var startDate = projectStartDate1;
            var finisDate = projectFinishDate3 ?? projectFinishDate2 ?? projectFinishDate1;
            var employee = Data.Audit.Employees[row["EmployeeId"].ToString()];

            for (DateTime date = startDate; date <= finisDate; date = date.AddDays(1.0))
            {
                var fieldName = date.ToString("yyyyMMdd");
                if (IsProjectDay(date)
                    && (employee.LeavingDate == null || date < employee.LeavingDate)
                    && date >= DateTime.Today
                    && row["OtherProjectStatus" + fieldName].ToAllocationStatus() != AllocationStatus.Allocated
                    && IsWorkingDay(date)
                    && row["Holidays" + fieldName].ToString() != "1")
                {
                    if (row["CurrentProjectStatus" + fieldName].ToAllocationStatus() == AllocationStatus.NotAllocated)
                    {
                        row["CurrentProjectStatus" + fieldName] = (int)AllocationStatus.NewAllocation;
                        row[fieldName] = 8;
                    }
                    else
                        if (row["CurrentProjectStatus" + fieldName].ToAllocationStatus() == AllocationStatus.DeletedAllocation)
                    {
                        row["CurrentProjectStatus" + fieldName] = (int)AllocationStatus.Allocated;
                        row[fieldName] = 8;
                    }
                }
            }
        }

        bool IsHolydayDay(DateTime date)
        {
            return Data.Audit.Holidays.FirstOrDefault(p => p.Year == date.Year && p.Month == date.Month && p.Day == date.Day) != null;
        }

        bool IsWorkingDay(DateTime date)
        {
            return !(IsHolydayDay(date) || date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
        }
        
        private void bandedGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            e.Handled = false;
                        
            if (e.Column.AbsoluteIndex < firstDateColumn)
            {                
                var rowIndex = bandedGridView.GetDataSourceRowIndex(e.RowHandle);
                DataRow row = ((DataTable)gridControl.DataSource).Rows[rowIndex];

                if (bandedGridView.IsRowSelected(e.RowHandle))
                {
                    e.Appearance.BackColor = Color.FromArgb(8, 0, 0, 128);
                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }

                if ((bool)row["Checked"])
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 0, 0, 128);
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                    e.Appearance.ForeColor = Color.White;
                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }
            }
            else
            {
                DateTime date = DateTime.ParseExact(e.Column.FieldName, "yyyyMMdd", CultureInfo.InvariantCulture);

                //

                var rowIndex = bandedGridView.GetDataSourceRowIndex(e.RowHandle);
                DataRow row = ((DataTable)gridControl.DataSource).Rows[rowIndex];

                //Fondo para la fila seleccionada
                if (bandedGridView.IsRowSelected(e.RowHandle))
                {
                    e.Appearance.BackColor = Color.FromArgb(8, 0, 0, 128);
                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }

                //Fondo para fines de semana
                if (!IsWorkingDay(date))
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 240, 240, 240);
                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }

                //Fondo para dias festivos
                if (IsHolydayDay(date))
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 255, 224, 224);
                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }

                //Fondo para periodos de vacaciones del auditor
                if (row["Holidays" + e.Column.FieldName].ToString() == "1")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 255, 160, 160);
                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }

                //Fondo para periodo desde la baja del auditor
                if (row["Leave" + e.Column.FieldName].ToString() == "1")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 128, 128, 128);
                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }               

                //Asignado al proyecto actual
                if (row["CurrentProjectStatus" + e.Column.FieldName].ToAllocationStatus() == AllocationStatus.Allocated || 
                    row["CurrentProjectStatus" + e.Column.FieldName].ToAllocationStatus() == AllocationStatus.NewAllocation)
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 0, 0, 128);
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;

                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }

                //Asignado a otro proyecto
                if (row["OtherProjectStatus" + e.Column.FieldName].ToAllocationStatus() == AllocationStatus.Allocated)
                {
                    e.Appearance.BackColor = Color.FromArgb(16, 255, 255, 0);
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;

                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }

                //Conflicto de proyectos
                if (row["OtherProjectValue" + e.Column.FieldName].ToInt() > 8)
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 255, 64, 255); 
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;

                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }

                //Conflicto con el proyecto actual
                if ((row["CurrentProjectStatus" + e.Column.FieldName].ToAllocationStatus() == AllocationStatus.Allocated ||
                    row["CurrentProjectStatus" + e.Column.FieldName].ToAllocationStatus() == AllocationStatus.NewAllocation) &&
                    row["OtherProjectStatus" + e.Column.FieldName].ToAllocationStatus() == AllocationStatus.Allocated)
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 255, 64, 255);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;

                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }

                //Asignado en vacaciones o baja
                if ((row["CurrentProjectStatus" + e.Column.FieldName].ToAllocationStatus() == AllocationStatus.Allocated ||
                    row["CurrentProjectStatus" + e.Column.FieldName].ToAllocationStatus() == AllocationStatus.NewAllocation || 
                    row["OtherProjectStatus" + e.Column.FieldName].ToAllocationStatus() == AllocationStatus.Allocated)
                    && (row["Holidays" + e.Column.FieldName].ToString() == "1" || row["Leave" + e.Column.FieldName].ToString() == "1"))
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 255, 64, 255); 
                    e.Appearance.ForeColor = Color.White; 
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;

                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.FillRectangle(e.Appearance.BackColor, rect);
                }

                //Desasignado del proyecto actual
                if (row["CurrentProjectStatus" + e.Column.FieldName].ToAllocationStatus() == AllocationStatus.DeletedAllocation)
                {
                    e.Appearance.BackColor = Color.LightGray;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;

                    var rect = e.Bounds;
                    rect.Inflate(1, 1);                                     
                    e.DefaultDraw();
                    e.Cache.DrawLine(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height, Color.FromArgb(128, 0, 0, 0), 2);
                    e.Cache.DrawLine(rect.X + rect.Width, rect.Y, rect.X, rect.Y + rect.Height, Color.FromArgb(128, 0, 0, 0), 2);                                        
                    e.Handled = true;
                }

                //Marco para el periodo del proyecto actual
                //if (date >= projectStartDate && date <= projectFinishDate && IsWorkingDay(date)) 
                if (IsProjectDay(date) && IsWorkingDay(date))
                {
                    var rect = e.Bounds;
                    rect.Inflate(1, 1);
                    e.Cache.DrawRectangle(new Pen(Color.Navy, 1), rect);
                }                
            }           
        }

        private void ToggleFocusedCell()
        {
            if (Data.Audit.UserPermissions <= UserPermissions.Read)
                return;

            if (bandedGridView.OptionsBehavior.ReadOnly)
                return;

            DataRow row = bandedGridView.GetFocusedDataRow();
            var fieldName = bandedGridView.FocusedColumn.FieldName;
            
            if (row["OtherProjectStatus" + fieldName].ToAllocationStatus() == AllocationStatus.Allocated)
            {                
                switch (row["CurrentProjectStatus" + fieldName].ToAllocationStatus())
                {
                    case AllocationStatus.Allocated:
                        row["CurrentProjectStatus" + fieldName] = (int)AllocationStatus.DeletedAllocation;                        
                        break;

                    case AllocationStatus.DeletedAllocation:
                        row["CurrentProjectStatus" + fieldName] = (int)AllocationStatus.Allocated;
                        row["Checked"] = true;
                        if (row[fieldName] == DBNull.Value)
                            row[fieldName] = 8;
                        break;

                   //TODO: Otros casos -> No se puede asignar, ¿mostrar mensaje?
                }

                bandedGridView.PostEditor();
                bandedGridView.UpdateCurrentRow();
                InvokeOnModify();
            }
            else
            {
                switch (row["CurrentProjectStatus" + fieldName].ToAllocationStatus())
                {
                    case AllocationStatus.Allocated:
                        row["CurrentProjectStatus" + fieldName] = (int)AllocationStatus.DeletedAllocation;
                        //row[fieldName] = DBNull.Value;
                        break;
                    case AllocationStatus.NotAllocated:
                        row["CurrentProjectStatus" + fieldName] = (int)AllocationStatus.NewAllocation;
                        row["Checked"] = true;
                        row[fieldName] = 8;
                        break;
                    case AllocationStatus.NewAllocation:
                        row["CurrentProjectStatus" + fieldName] = (int)AllocationStatus.NotAllocated;
                        row[fieldName] = DBNull.Value;
                        break;
                    case AllocationStatus.DeletedAllocation:
                        row["CurrentProjectStatus" + fieldName] = (int)AllocationStatus.Allocated;
                        row["Checked"] = true;
                        if (row[fieldName] == DBNull.Value)
                            row[fieldName] = 8;
                        break;
                }

                bandedGridView.PostEditor();
                bandedGridView.UpdateCurrentRow();

                InvokeOnModify();
            }
        }        

        private void BandedGridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left)
            {
                if (e.Column.AbsoluteIndex >= firstDateColumn)
                {
                    ToggleFocusedCell();
                }
            }
        }

        private void GridControl_MouseMove(object sender, MouseEventArgs e)
        {
            string list = "";
            DXMouseEventArgs ea = e as DXMouseEventArgs;            
            GridHitInfo info = bandedGridView.CalcHitInfo(ea.Location);

            if (info.InRow || info.InRowCell)
            {                
                if (info.Column != null && info.Column.AbsoluteIndex >= firstDateColumn)
                {
                    DateTime date = DateTime.ParseExact(info.Column.FieldName, "yyyyMMdd", CultureInfo.InvariantCulture);
                    var rowIndex = bandedGridView.GetDataSourceRowIndex(info.RowHandle);
                    DataRow row = ((DataTable)gridControl.DataSource).Rows[rowIndex];

                    var employeeId = row["EmployeeId"].ToString();
                    /*
                    foreach (var task in Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.EmployeeId == employeeId && p.AllocDate == date))
                    {
                        list += (list == "" ? "" : Environment.NewLine) + task.Project.Name;
                    } 
                    */

                    var fieldName = date.ToString("yyyyMMdd");
                    list = row["Hint" + fieldName].ToString();
                }
            }

            projectLabelControl.Visible = list != "";
            projectLabelControl.Text = list;
            projectLabelControl.Left = gridControl.Left + e.Location.X + 6;
            projectLabelControl.Top = gridControl.Top + e.Location.Y - projectLabelControl.Height - 4;            
        }

        public void SetDates(DateTime startDate1, DateTime finishDate1, DateTime? startDate2, DateTime? finishDate2, DateTime? startDate3, DateTime? finishDate3)
        {
            projectStartDate1 = startDate1;
            projectFinishDate1 = finishDate1;

            projectStartDate2 = startDate2;
            projectFinishDate2 = finishDate2;

            projectStartDate3 = startDate3;
            projectFinishDate3 = finishDate3;

            gridControl.Refresh();

            this.BeginInvoke((Action)(() =>
            {
                string fieldName;

                fieldName = projectStartDate1.ToString("yyyyMMdd");                
                if (bandedGridView.Columns.ColumnByFieldName(fieldName) != null)
                    bandedGridView.MakeColumnVisible(bandedGridView.Columns[fieldName]);

                fieldName = (projectFinishDate3 ?? projectFinishDate2 ?? projectFinishDate1).ToString("yyyyMMdd");
                if (bandedGridView.Columns.ColumnByFieldName(fieldName) != null)
                    bandedGridView.MakeColumnVisible(bandedGridView.Columns[fieldName]);
            }));
            
            string filterString =  $"[LeavingDate] is null || [LeavingDate] >= #{projectStartDate1.ToString("yyyy-MM-dd")}#";

            if (bandedGridView.ActiveFilterString.Contains("[LeavingDate]"))
            {
                var ixLeavingDate = bandedGridView.ActiveFilterString.IndexOf("[LeavingDate]");
                var ixMark3 = bandedGridView.ActiveFilterString.IndexOf("#", ixLeavingDate);
                var ixMark4 = bandedGridView.ActiveFilterString.IndexOf("#", ixMark3 + 1);

                string newFilter = bandedGridView.ActiveFilterString.Remove(ixLeavingDate, ixMark4 - ixLeavingDate + 1);
                newFilter = newFilter.Insert(ixLeavingDate, filterString);
                bandedGridView.ActiveFilterString = newFilter;
            }
            else
            {
                bandedGridView.ActiveFilterString += (bandedGridView.ActiveFilterString != "" ? " And " : "") + filterString;
            }

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            DataRow row = null;
            foreach (var selectedRowHandle in bandedGridView.GetSelectedRows())
            {
                if (selectedRowHandle >= 0)
                    row = bandedGridView.GetDataRow(selectedRowHandle);
            }
                        
            if (row != null)
            {
                contextMenuStrip1.Items[0].Text = "Asignar todas las jornadas disponibles del auditor \"" + row["Auditor"] + "\"";
            }            
        }

        private void xxxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow row = null;
            foreach (var selectedRowHandle in bandedGridView.GetSelectedRows())
            {
                if (selectedRowHandle >= 0)
                    row = bandedGridView.GetDataRow(selectedRowHandle);
            }

            if (row != null)
            {
                AutoAllocateSelectedEmployee(row);
            }
        }
    }

    public static class ObjectExtensions
    {
        public static AllocationStatus ToAllocationStatus(this Object obj)
        {
            AllocationStatus result = AllocationStatus.NotAllocated;
            if (obj.ToString() != "")
                result = (AllocationStatus)int.Parse(obj.ToString());

            return result;
        }

        public static int ToInt(this Object obj)
        {
            int result = 0;
            if (obj.ToString() != "")
                result = int.Parse(obj.ToString());
                
            return result;
        }
    }
}
