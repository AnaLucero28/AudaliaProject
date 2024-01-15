using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.ComponentModel.DataAnnotations;
using System.IO;
using DevExpress.XtraLayout.Helpers;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors.Controls;
using Audalia.DataHUBClient;
using Audalia.DataHUBCommon;
using System.Globalization;

namespace WindowsApplication
{
    public partial class ProjectForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string projectID;
        bool modified;

        //AvailabilityUserControl.AvailabilityUserControl auditoresControl;
        AllocationUserControl auditoresControl;

        DataTable budgetDataTable;
        DateTime allocationStartDate;
        DateTime allocationFinishDate;

        static List<string> branchOfficeList = new List<string>() { "Madrid", "Barcelona" };
        static int branchOfficeIx = 0;

        public ProjectForm()
        {
            InitializeComponent();

            if (DateTime.Now.Month < 9)
            {
                allocationStartDate = new DateTime(DateTime.Now.Year - 1, 9, 1);
                allocationFinishDate = new DateTime(DateTime.Now.Year + 1, 8, 31);
            }
            else
            {
                allocationStartDate = new DateTime(DateTime.Now.Year, 9, 1);
                allocationFinishDate = new DateTime(DateTime.Now.Year + 2, 8, 31);
            }

            InitComboBoxes();            
        }

        private void SetPermissions()
        {
            if (Data.Audit.UserPermissions <= UserPermissions.Read)
            {
                bbiSave.Enabled = false;
                bbiSaveAndClose.Enabled = false;
                bbiSaveAndNew.Enabled = false;
                bbiReset.Enabled = false;
                bbiDelete.Enabled = false;

                //layoutControl1.Enabled = false;
                layoutControl1.OptionsView.IsReadOnly = DevExpress.Utils.DefaultBoolean.True;
                budgetGridView.OptionsBehavior.ReadOnly = true;

                //auditoresControl.GanttControl.OptionsBehavior.ReadOnly = true;
                auditoresControl.GridView.OptionsBehavior.ReadOnly = true;
            }
        }

        void InitComboBoxes()
        {         
            
            branchOfficeComboBoxEdit.Properties.Items.AddRange(branchOfficeList);
            branchOfficeComboBoxEdit.SelectedItem = branchOfficeComboBoxEdit.Properties.Items[branchOfficeIx];
        }

        public static void CreateProject()
        {         
            var form = new ProjectForm();            
            form.Owner = Application.OpenForms[0];
            form.StartPosition = FormStartPosition.CenterParent;
            form.WindowState = FormWindowState.Maximized;

            form.projectID = "";

            form.ejercicioTextEdit.Text = DateTime.Today.Month <= 8 ? (DateTime.Today.Year - 1).ToString() : DateTime.Today.Year.ToString();
            form.startDateDateEdit.DateTime = DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday); //DateTime.Today.AddDays(((int)DateTime.Today.DayOfWeek - (int)DayOfWeek.Monday) + 7);
            //form.startDateDateEdit.Properties.MinValue = form.allocationStartDate;
            //form.startDateDateEdit.Properties.MaxValue = form.allocationFinishDate;

            form.finishDateDateEdit.DateTime = form.startDateDateEdit.DateTime.AddDays(4);
            form.finishDateDateEdit.Properties.MinValue = form.startDateDateEdit.DateTime;
            form.finishDateDateEdit.Properties.MaxValue = form.allocationFinishDate;
                        
            //form.durationSpinEdit.Value = 5;

            form.PresupuestoControlInit();
            form.AuditoresControlInit();
            form.auditoresControl.SetDates(form.startDateDateEdit.DateTime, form.finishDateDateEdit.DateTime, null, null, null, null);

            form.SetPermissions();
            form.modified = true;

            form.ShowDialog();
            form.Dispose();
        }


        public static void EditProject(string projectID)
        {            
            var form = new ProjectForm();
            form.Owner = Application.OpenForms[0];
            form.StartPosition = FormStartPosition.CenterParent;
            form.WindowState = FormWindowState.Maximized;

            form.projectID = projectID;            
            var project = Data.Audit.Projects[form.projectID];
            
            form.nameTextEdit.Text = project.Name;

            //form.branchOfficeTextEdit.Text = project.BranchOffice;
            form.branchOfficeComboBoxEdit.SelectedItem = project.BranchOffice;

            form.ejercicioTextEdit.Text = project.TaxYear;
            form.idGextorTextEdit.Text = project.IdGextor;

            form.startDateDateEdit.DateTime = project.StartDate1;
            //form.startDateDateEdit.Properties.MinValue = form.allocationStartDate;
            //form.startDateDateEdit.Properties.MaxValue = form.allocationFinishDate;

            form.finishDateDateEdit.DateTime = project.FinishDate1;
            form.finishDateDateEdit.Properties.MinValue = form.startDateDateEdit.DateTime;
            form.finishDateDateEdit.Properties.MaxValue = form.allocationFinishDate;

            /*
            if (project.StartDate2 != null)
                form.startDateDateEdit2.DateTime = (DateTime)project.StartDate2;
            */

            //DateTimeFormatInfo.CurrentInfo.ShortDatePattern

            form.startDateDateEdit2.Text = project.StartDate2.ToShortDateString();
            //form.startDateDateEdit2.Properties.MinValue = form.finishDateDateEdit.DateTime;
            //form.startDateDateEdit2.Properties.MaxValue = form.allocationFinishDate;

            //form.finishDateDateEdit2.DateTime = project.FinishDate2;
            form.finishDateDateEdit2.Text = project.FinishDate2.ToShortDateString();
            //form.finishDateDateEdit2.Properties.MinValue = form.startDateDateEdit2.DateTime;
            //form.finishDateDateEdit2.Properties.MaxValue = form.allocationFinishDate;

            //form.startDateDateEdit3.DateTime = project.StartDate3;
            form.startDateDateEdit3.Text = project.StartDate3.ToShortDateString();
            //form.startDateDateEdit3.Properties.MinValue = form.finishDateDateEdit2.DateTime;
            //form.startDateDateEdit3.Properties.MaxValue = form.allocationFinishDate;

            //form.finishDateDateEdit3.DateTime = project.FinishDate3;
            form.finishDateDateEdit3.Text = project.FinishDate3.ToShortDateString();
            //form.finishDateDateEdit3.Properties.MinValue = form.startDateDateEdit3.DateTime;
            //form.finishDateDateEdit3.Properties.MaxValue = form.allocationFinishDate;

            /*
            form.durationSpinEdit.Value = 1 + (project.FinishDate1 - project.StartDate1).Days;
            form.baselineStartDateDateEdit.DateTime = project.BaselineStartDate;                        
            form.baselineFinishDateDateEdit.DateTime = project.BaselineFinishDate;            
            */

            form.colorPickEdit.Color = project.Color.StringToColor();
            form.comentsMemoEdit.Text = project.Comments;

            form.PresupuestoControlInit();
            form.AuditoresControlInit();
            form.auditoresControl.SetDates(project.StartDate1, project.FinishDate1, project.StartDate2, project.FinishDate2, project.StartDate3, project.FinishDate3);

            form.SetPermissions();
            
            /*
            foreach (var employee in project.Employees)
            {
                foreach (var node in form.auditoresControl.GanttNodes.Where(p => p.EmployeeID == employee.Employee.DBID))
                    node.Selected = true;
            }
            */

            form.modified = false;

            form.ShowDialog();
            form.Dispose();
        }

        bool Save()
        {
            var result = true;

            try
            {
                mainRibbonControl.Enabled = false;


                //Valida las fechas

                if (this.startDateDateEdit.Text == "")
                {
                    MessageBox.Show("La fecha de inicio es obligatoria");
                    return false;
                }

                if (this.finishDateDateEdit.Text == "")
                {
                    MessageBox.Show("La fecha de finalización es obligatoria");
                    return false;
                }

                if ((this.startDateDateEdit2.Text != "" && this.finishDateDateEdit2.Text == "") ||
                    (this.startDateDateEdit2.Text == "" && this.finishDateDateEdit2.Text != "") ||
                    (this.startDateDateEdit2.DateTime > this.finishDateDateEdit2.DateTime))
                {
                    MessageBox.Show("Las fechas de la segunda fase son incorectas");
                    return false;
                }

                if ((this.startDateDateEdit3.Text != "" && this.finishDateDateEdit3.Text == "") ||
                    (this.startDateDateEdit3.Text == "" && this.finishDateDateEdit3.Text != "") ||
                    (this.startDateDateEdit3.DateTime > this.finishDateDateEdit3.DateTime))
                {
                    MessageBox.Show("Las fechas de la tercera fase son incorectas");
                    return false;
                }

                //

                if (this.projectID == "")
                {
                    var project = new Project();
                    project.Name = this.nameTextEdit.Text;
                    project.TaxYear = this.ejercicioTextEdit.Text;
                    project.IdGextor = this.idGextorTextEdit.Text;

                    //project.BranchOffice = this.branchOfficeTextEdit.Text;
                    project.BranchOffice = this.branchOfficeComboBoxEdit.SelectedItem.ToString();

                    project.StartDate1 = this.startDateDateEdit.DateTime;
                    project.FinishDate1 = this.finishDateDateEdit.DateTime;

                    project.StartDate2 = null;
                    if (this.startDateDateEdit2.DateTime.Ticks > 0)
                        project.StartDate2 = this.startDateDateEdit2.DateTime;

                    project.FinishDate2 = null;
                    if (this.finishDateDateEdit2.DateTime.Ticks > 0)
                        project.FinishDate2 = this.finishDateDateEdit2.DateTime;

                    project.StartDate3 = null;
                    if (this.startDateDateEdit3.DateTime.Ticks > 0)
                        project.StartDate3 = this.startDateDateEdit3.DateTime;

                    project.FinishDate3 = null;
                    if (this.finishDateDateEdit3.DateTime.Ticks > 0)
                        project.FinishDate3 = this.finishDateDateEdit3.DateTime;


                    //int projectDuration = (project.FinishDate - project.StartDate).Days;
                    /*
                    if (this.baselineFinishDateDateEdit.Text == "")
                        this.baselineFinishDateDateEdit.DateTime = this.finishDateDateEdit.DateTime;

                    if (this.baselineStartDateDateEdit.Text == "")
                        this.baselineStartDateDateEdit.DateTime = this.startDateDateEdit.DateTime;

                    project.BaselineStartDate = this.baselineStartDateDateEdit.DateTime;
                    project.BaselineFinishDate = this.baselineFinishDateDateEdit.DateTime;
                    */

                    project.Color = colorPickEdit.Color.ColorToString();
                    project.Comments = comentsMemoEdit.Text;

                    var projectBase = DataHUBClient.ServiceContract.PostProject(project);
                    project.DBID = projectBase.DBID;
                    this.projectID = projectBase.DBID;
                    Data.Audit.Projects.Add(project.DBID, project);

                    foreach (DataRow row in auditoresControl.AllocationDataTable.Rows)
                    {
                        if ((bool)row["Checked"])
                        {
                            var employee = Data.Audit.Employees[row["EmployeeId"].ToString()];
                            var role = (employee.Category == "Socio") ? "SOCIO" : "RESPONSABLE";
                            var projectEmployeeAssoc = new ProjectEmployeeAssoc() { Employee = employee, Project = project, Role = role };

                            var projectEmployeeAssocBase = DataHUBClient.ServiceContract.PostProjectEmployeeAssoc(projectEmployeeAssoc);
                            projectEmployeeAssoc.DBID = projectEmployeeAssocBase.DBID;

                            Data.Audit.ProjectEmployeeAssocs.Add(projectEmployeeAssoc.DBID, projectEmployeeAssoc);

                            for (DateTime date = auditoresControl.AllocationStartDate; date < auditoresControl.AllocationFinishDate; date = date.AddDays(1.0))
                            {
                                var fieldName = date.ToString("yyyyMMdd");
                                switch (row["CurrentProjectStatus" + fieldName].ToAllocationStatus())
                                {
                                    case AllocationStatus.NewAllocation:
                                        DataHUBClient.ServiceContract.PostProjectEmployeeAlloc(new ProjectEmployeeAllocBase() { ProjectId = project.DBID, EmployeeId = employee.DBID, AllocDate = date, Quantity = row[fieldName].ToInt() });
                                        break;

                                    case AllocationStatus.DeletedAllocation:
                                        var alloc = Data.Audit.ProjectEmployeeAllocs.FirstOrDefault(p => p.Value.ProjectId == project.DBID && p.Value.EmployeeId == employee.DBID && p.Value.AllocDate == date);
                                        DataHUBClient.ServiceContract.DeleteProjectEmployeeAlloc(new ProjectEmployeeAllocBase() { DBID = alloc.Value.DBID });
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    var project = Data.Audit.Projects[this.projectID];

                    project.Name = this.nameTextEdit.Text;

                    //project.BranchOffice = this.branchOfficeTextEdit.Text;
                    project.BranchOffice = this.branchOfficeComboBoxEdit.SelectedItem.ToString();
                    project.TaxYear = this.ejercicioTextEdit.Text;
                    project.IdGextor = this.idGextorTextEdit.Text;

                    project.StartDate1 = this.startDateDateEdit.DateTime;
                    project.FinishDate1 = this.finishDateDateEdit.DateTime;

                    project.StartDate2 = null;
                    if (this.startDateDateEdit2.DateTime.Ticks > 0)
                        project.StartDate2 = this.startDateDateEdit2.DateTime;

                    project.FinishDate2 = null;
                    if (this.finishDateDateEdit2.DateTime.Ticks > 0)
                        project.FinishDate2 = this.finishDateDateEdit2.DateTime;

                    project.StartDate3 = null;
                    if (this.startDateDateEdit3.DateTime.Ticks > 0)
                        project.StartDate3 = this.startDateDateEdit3.DateTime;

                    project.FinishDate3 = null;
                    if (this.finishDateDateEdit3.DateTime.Ticks > 0)
                        project.FinishDate3 = this.finishDateDateEdit3.DateTime;

                    /*
                    project.BaselineStartDate = this.baselineStartDateDateEdit.DateTime;
                    project.BaselineFinishDate = this.baselineFinishDateDateEdit.DateTime;
                    */

                    project.Color = this.colorPickEdit.Color.ColorToString();
                    project.Comments = this.comentsMemoEdit.Text;

                    var projectBase = DataHUBClient.ServiceContract.PutProject(project);

                    var task = project;

                    var selectedEmployees = new List<string>();

                    List<ProjectEmployeeAllocBase> allocListToPost = new List<ProjectEmployeeAllocBase>();
                    List<ProjectEmployeeAllocBase> allocListToDelete = new List<ProjectEmployeeAllocBase>();

                    foreach (DataRow row in auditoresControl.AllocationDataTable.Rows)
                    {
                        if ((bool)row["Checked"])
                        {
                            var employee = Data.Audit.Employees[row["EmployeeId"].ToString()];
                            selectedEmployees.Add(employee.DBID);

                            if (!Data.Audit.ProjectEmployeeAssocs.Values.ToList().Exists(p => p.ProjectId == task.DBID && p.EmployeeId == employee.DBID))
                            {
                                var role = (employee.Category == "Socio") ? "SOCIO" : "RESPONSABLE";
                                var projectEmployeeAssoc = new ProjectEmployeeAssoc() { Employee = employee, Project = task, Role = role };

                                var projectEmployeeAssocBase = DataHUBClient.ServiceContract.PostProjectEmployeeAssoc(projectEmployeeAssoc);
                                projectEmployeeAssoc.DBID = projectEmployeeAssocBase.DBID;

                                Data.Audit.ProjectEmployeeAssocs.Add(projectEmployeeAssoc.DBID, projectEmployeeAssoc);
                            }

                            for (DateTime date = auditoresControl.AllocationStartDate; date < auditoresControl.AllocationFinishDate; date = date.AddDays(1.0))
                            {
                                var fieldName = date.ToString("yyyyMMdd");
                                switch (row["CurrentProjectStatus" + fieldName].ToAllocationStatus())
                                {
                                    case AllocationStatus.NewAllocation:
                                        //DataHUBClient.ServiceContract.PostProjectEmployeeAlloc(new ProjectEmployeeAllocBase() { ProjectId = project.DBID, EmployeeId = employee.DBID, AllocDate = date, Quantity = row[fieldName].ToInt() });
                                        allocListToPost.Add(new ProjectEmployeeAllocBase() { ProjectId = project.DBID, EmployeeId = employee.DBID, AllocDate = date, Quantity = row[fieldName].ToInt() });
                                        break;

                                    case AllocationStatus.DeletedAllocation:
                                        var alloc = Data.Audit.ProjectEmployeeAllocs.FirstOrDefault(p => p.Value.ProjectId == project.DBID && p.Value.EmployeeId == employee.DBID && p.Value.AllocDate == date);
                                        if (alloc.Key != null)
                                        {
                                            //DataHUBClient.ServiceContract.DeleteProjectEmployeeAlloc(new ProjectEmployeeAllocBase() { DBID = alloc.Value.DBID });
                                            allocListToDelete.Add(new ProjectEmployeeAllocBase() { DBID = alloc.Value.DBID });
                                        }

                                        break;
                                }
                            }

                        }
                    }

                    if (allocListToPost.Count > 0)
                    {
                        DataHUBClient.ServiceContract.PostProjectEmployeeAllocList(allocListToPost);
                    }

                    if (allocListToDelete.Count > 0)
                    {
                        DataHUBClient.ServiceContract.DeleteProjectEmployeeAllocList(allocListToDelete);
                    }

                    /////////

                    foreach (var projectEmployeeAssoc in Data.Audit.ProjectEmployeeAssocs.Where(p => p.Value.ProjectId == task.DBID))
                    {
                        if (!selectedEmployees.Contains(projectEmployeeAssoc.Value.EmployeeId))
                        {
                            DataHUBClient.ServiceContract.DeleteProjectEmployeeAssoc(projectEmployeeAssoc.Value);
                        }
                    }


                }

                //Budget

                foreach (DataRow row in budgetDataTable.Rows)
                {
                    if (row["BudgetId"].ToString() == "-1")
                    {
                        if (int.Parse(row["Budgeted"].ToString()) > 0)
                        {
                            var budget = new BudgetBase()
                            {
                                ProjectId = projectID,
                                Category = row["Category"].ToString(),
                                Quantity = int.Parse(row["Budgeted"].ToString())
                            };

                            budget = Data.Audit.PostBudget(budget);

                            row.BeginEdit();
                            row["BudgetId"] = budget.DBID;
                            row.EndEdit();
                        }
                    }
                    else
                    if (row["BudgetId"].ToString() != "")
                    {
                        var budget = new BudgetBase()
                        {
                            DBID = row["BudgetId"].ToString(),
                            ProjectId = projectID,
                            Category = row["Category"].ToString(),
                            Quantity = int.Parse(row["Budgeted"].ToString())
                        };

                        budget = Data.Audit.PutBudget(budget);
                    }
                }

                Application.DoEvents();
                auditoresControl.ReloadGrid(projectID);

                modified = false;
            }
            finally
            {
                mainRibbonControl.Enabled = true;
            }

            return result;
        }

        void New()
        {
            Close();
            Dispose();
            CreateProject();            
        }

        void Reset()
        {
            if (projectID == "")
            {
                Close();
                Dispose();
                CreateProject();
            }
            else
            {
                Close();
                Dispose();
                EditProject(projectID);
            }
        }

        private void bbiSaveAndClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Save())
                this.Close();
        }

        private void bbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }

        private void bbiSaveAndNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Save())
                New();
        }

        private void bbiReset_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PresupuestoControlInit();
            AuditoresControlInit();                       
            Reset();
        }

        private void bbiClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        public void PresupuestoControlInit()
        {
            budgetDataTable = new DataTable();

            budgetDataTable.Columns.Add(new DataColumn($"BudgetId", typeof(string)));
            budgetDataTable.Columns.Add(new DataColumn($"Category", typeof(string)));
            budgetDataTable.Columns.Add(new DataColumn($"Budgeted", typeof(int)));
            budgetDataTable.Columns.Add(new DataColumn($"Allocated", typeof(int)));

            List<string> categories = new List<string>() { "Socio", "Director", "Gerente", "Senior", "Asistente", "Becario" };

            //Budgeted

            List<BudgetBase> budget = Data.Audit.GetBudget(projectID);
            int totalBudgeted = 0;
            foreach (var category in categories)
            {
                var categoryBudget = budget.FirstOrDefault(p => p.Category == category);
                string dbid = categoryBudget != null ? categoryBudget.DBID : "-1";
                int qty = categoryBudget != null ? categoryBudget.Quantity : 0;
                totalBudgeted += qty;

                var row = budgetDataTable.NewRow();
                row["BudgetId"] = dbid;
                row["Category"] = category;
                row["Budgeted"] = qty;
                row["Allocated"] = 0;
                budgetDataTable.Rows.Add(row);
            }

            budgetedTextEdit.Text = totalBudgeted.ToString();
            budgetGridControl.DataSource = budgetDataTable;
            budgetGridControl.DataMember = budgetDataTable.TableName;

            //Allocated

            int totalAllocated = 0;
            foreach (DataRow row in budgetDataTable.Rows)
            {
                if (row["BudgetId"].ToString() != "0")                
                {
                    var category = row["Category"].ToString();
                    int categoryAllocated = 0;
                    foreach (var alloc in Data.Audit.ProjectEmployeeAllocs.Values.Where(p => p.ProjectId == projectID && p.Employee.Category == category))
                    {
                        categoryAllocated += alloc.Quantity;
                        totalAllocated += alloc.Quantity;
                    }

                    row.BeginEdit();
                    row["Allocated"] = categoryAllocated;
                    row.EndEdit();
                }                
            }

            allocatedTextEdit.Text = totalAllocated.ToString();
        }

        public void AuditoresControlInit()
        {
            /*
            if (auditoresControl == null)
            {
                auditoresControl = new AvailabilityUserControl.AvailabilityUserControl();
                auditoresControl.Parent = panelControl1;
                auditoresControl.Dock = DockStyle.Fill;
            }

            auditoresControl.Init(startDateDateEdit.DateTime, finishDateDateEdit.DateTime);
            auditoresControl.GanttControl.CellValueChanged += (sender, e) =>
            {
                modified = true;
            };
            */

            if (auditoresControl == null)
            {
                auditoresControl = new AllocationUserControl(this.projectID);                                 
                auditoresControl.Parent = panelControl1;
                auditoresControl.Dock = DockStyle.Fill;
                auditoresControl.OnModify += AuditoresControl_OnModify;

            }
        }

        private void AuditoresControl_OnModify(object sender, EventArgs e)
        {
            modified = true;

            foreach (DataRow budgetRow in budgetDataTable.Rows)
            {
                if (budgetRow["BudgetId"].ToString() != "0")
                {            
                    budgetRow.BeginEdit();
                    budgetRow["Allocated"] = 0;
                    budgetRow.EndEdit();
                }
            }

            int totalAllocated = 0;

            foreach (DataRow allocRow in auditoresControl.AllocationDataTable.Rows)
            {
                if ((bool)allocRow["Checked"])
                {
                    for (DateTime date = allocationStartDate; date <= allocationFinishDate; date = date.AddDays(1.0))
                    {
                        var fieldName = date.ToString("yyyyMMdd");
                        if (allocRow["CurrentProjectStatus" + fieldName].ToAllocationStatus() == AllocationStatus.Allocated || 
                            allocRow["CurrentProjectStatus" + fieldName].ToAllocationStatus() == AllocationStatus.NewAllocation)
                        {
                            var budgetRow = budgetDataTable.Select("Category = '" + allocRow["Category"] + "'").FirstOrDefault();
                            budgetRow.BeginEdit();
                            budgetRow["Allocated"] = budgetRow["Allocated"].ToInt() + allocRow[fieldName].ToInt();
                            budgetRow.EndEdit();

                            totalAllocated += allocRow[fieldName].ToInt();
                        }
                    }
                }
            }

            allocatedTextEdit.Text = totalAllocated.ToString();
        }

        
        private void durationSpinEdit_EditValueChanged(object sender, EventArgs e)
        {   
            /*
            var finishDate = this.startDateDateEdit.DateTime.AddDays((int)Math.Round(this.durationSpinEdit.Value) - 1);
            if (this.finishDateDateEdit.DateTime != finishDate)
                this.finishDateDateEdit.DateTime = finishDate;            
            */
        }
        
        private void budgetGridView_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            int totalQty = 0;

            foreach (DataRow row in budgetDataTable.Rows)
            {                
                if (row["BudgetId"].ToString() != "0")
                {
                    totalQty += int.Parse(row["Budgeted"].ToString());                    
                }                
            }

            budgetedTextEdit.Text = totalQty.ToString();
        }

        private void budgetGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.AbsoluteIndex > 0)
            {
                int budgeted = Convert.ToInt32(budgetGridView.GetRowCellValue(e.RowHandle, budgetGridView.Columns["Budgeted"]));
                int allocated = Convert.ToInt32(budgetGridView.GetRowCellValue(e.RowHandle, budgetGridView.Columns["Allocated"]));
                
                e.Appearance.ForeColor = Color.Black;
                if (allocated > budgeted)
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 192, 192);
                }
                else
                {
                    e.Appearance.BackColor = Color.FromArgb(192, 255, 192);
                }

                var rect = e.Bounds;
                rect.Inflate(1, 1);
                e.Cache.FillRectangle(e.Appearance.BackColor, rect);
            }
        }

        private void budgetedTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            int budgeted = 0;
            Int32.TryParse(budgetedTextEdit.Text, out budgeted);
            int allocated = 0;
            Int32.TryParse(allocatedTextEdit.Text, out allocated);

            if (allocated > budgeted)
            {
                budgetedTextEdit.BackColor = Color.FromArgb(255, 192, 192);
                allocatedTextEdit.BackColor = Color.FromArgb(255, 192, 192);
            }
            else
            {
                budgetedTextEdit.BackColor = Color.FromArgb(192, 255, 192);
                allocatedTextEdit.BackColor = Color.FromArgb(192, 255, 192);
            }
            
        }

        private void allocatedTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            int budgeted = 0;
            Int32.TryParse(budgetedTextEdit.Text, out budgeted);
            int allocated = 0;
            Int32.TryParse(allocatedTextEdit.Text, out allocated);

            if (allocated > budgeted)
            {
                budgetedTextEdit.BackColor = Color.FromArgb(255, 192, 192);
                allocatedTextEdit.BackColor = Color.FromArgb(255, 192, 192);
            }
            else
            {
                budgetedTextEdit.BackColor = Color.FromArgb(192, 255, 192);
                allocatedTextEdit.BackColor = Color.FromArgb(192, 255, 192);
            }
        }

        private void bbiDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            MessageBox.Show(this, "Esta funcionalidad no está implementada", "Data HUB", MessageBoxButtons.OK, MessageBoxIcon.Information);         
        }

        private void ProjectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modified && MessageBox.Show(this,"¿Desea salir sin guardar los cambios?", "Proyecto", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void nameTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            modified = true;
        }

        private void budgetGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            modified = true;
        }

        private void startDateDateEdit_EditValueChanged(object sender, EventArgs e)
        {
            modified = true;

            finishDateDateEdit.Properties.MinValue = startDateDateEdit.DateTime;

            SetAuditoresControlDates();
            /*
            if (auditoresControl != null)
                auditoresControl.SetDates(startDateDateEdit.DateTime, finishDateDateEdit.DateTime);
            */

            finishDateDateEdit.IsModified = true;
            finishDateDateEdit.DoValidate();
        }

        private void finishDateDateEdit_EditValueChanged(object sender, EventArgs e)
        {            
            modified = true;

            SetAuditoresControlDates();
            /*
            if (auditoresControl != null)
                auditoresControl.SetDates(startDateDateEdit.DateTime, finishDateDateEdit.DateTime);
            */

            /*
            int duration = 1 + (this.finishDateDateEdit.DateTime - this.startDateDateEdit.DateTime).Days;
            if (this.durationSpinEdit.Value != duration)
                this.durationSpinEdit.Value = duration;
            */

            finishDateDateEdit.IsModified = true;
            finishDateDateEdit.DoValidate();
        }

        private void finishDateDateEdit_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = false;

            if (this.finishDateDateEdit.DateTime < this.startDateDateEdit.DateTime)
            {
                e.Cancel = true;
            }
        }

        private void branchOfficeComboBoxEdit_CloseUp(object sender, CloseUpEventArgs e)
        {                        
            branchOfficeIx = branchOfficeList.IndexOf(e.Value.ToString());
        }

        private void startDateDateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            modified = true;

            /*
            if (startDateDateEdit2.Text != "")
                finishDateDateEdit2.Properties.MinValue = startDateDateEdit2.DateTime;
            else
                finishDateDateEdit2.Properties.MinValue = DateTime.MinValue;
            */

            SetAuditoresControlDates();
            /*
            if (auditoresControl != null)
                auditoresControl.SetDates(startDateDateEdit2.DateTime, finishDateDateEdit2.DateTime);
            */

            finishDateDateEdit2.IsModified = true;
            //finishDateDateEdit2.DoValidate();
        }

        private void finishDateDateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            modified = true;

            SetAuditoresControlDates();
            /*
            if (auditoresControl != null)
                auditoresControl.SetDates(startDateDateEdit2.DateTime, finishDateDateEdit2.DateTime);
            */

            finishDateDateEdit2.IsModified = true;
            //finishDateDateEdit2.DoValidate();
        }

        private void startDateDateEdit3_EditValueChanged(object sender, EventArgs e)
        {
            modified = true;

            /*
            if (startDateDateEdit3.Text != "")
                finishDateDateEdit3.Properties.MinValue = startDateDateEdit3.DateTime;
            else
                finishDateDateEdit3.Properties.MinValue = DateTime.MinValue;
            */

            SetAuditoresControlDates();            
            finishDateDateEdit3.IsModified = true;
        }

        private void finishDateDateEdit3_EditValueChanged(object sender, EventArgs e)
        {
            modified = true;
            SetAuditoresControlDates();            
            finishDateDateEdit3.IsModified = true;
        }

        private void SetAuditoresControlDates()
        {
            if (auditoresControl != null)
            {
                DateTime projectStartDate1 = startDateDateEdit.DateTime;
                DateTime projectFinishDate1 = finishDateDateEdit.DateTime;

                DateTime? projectStartDate2 = null;
                if (startDateDateEdit2.DateTime.Ticks > 0)
                    projectStartDate2 = startDateDateEdit2.DateTime;

                DateTime? projectFinishDate2 = null;
                if (finishDateDateEdit2.DateTime.Ticks > 0)
                    projectFinishDate2 = finishDateDateEdit2.DateTime;

                DateTime? projectStartDate3 = null;
                if (startDateDateEdit3.DateTime.Ticks > 0)
                    projectStartDate3 = startDateDateEdit3.DateTime;

                DateTime? projectFinishDate3 = null;
                if (finishDateDateEdit3.DateTime.Ticks > 0)
                    projectFinishDate3 = finishDateDateEdit3.DateTime;

                auditoresControl.SetDates(projectStartDate1, projectFinishDate1, projectStartDate2, projectFinishDate2, projectStartDate3, projectFinishDate3);
            }
        }

        private void startDateDateEdit2_QueryPopUp(object sender, CancelEventArgs e)
        {
            DateEdit dateEdit = (DateEdit)sender;
            var currentText = dateEdit.Text;

            if (finishDateDateEdit.Text != "")
                dateEdit.Properties.MinValue = finishDateDateEdit.DateTime;
            else
                dateEdit.Properties.MinValue = allocationStartDate;

            dateEdit.Properties.MaxValue = allocationFinishDate;

            dateEdit.Text = currentText;
        }

        private void finishDateDateEdit2_QueryPopUp(object sender, CancelEventArgs e)
        {
            DateEdit dateEdit = (DateEdit)sender;
            var currentText = dateEdit.Text;

            if (startDateDateEdit2.Text != "")
                dateEdit.Properties.MinValue = startDateDateEdit2.DateTime;
            else
                if (finishDateDateEdit.Text != "")
                dateEdit.Properties.MinValue = finishDateDateEdit.DateTime;
            else
                dateEdit.Properties.MinValue = allocationStartDate;

            dateEdit.Properties.MaxValue = allocationFinishDate;

            dateEdit.Text = currentText;
        }

        private void startDateDateEdit3_QueryPopUp(object sender, CancelEventArgs e)
        {
            DateEdit dateEdit = (DateEdit)sender;
            var currentText = dateEdit.Text;

            if (finishDateDateEdit2.Text != "")
                dateEdit.Properties.MinValue = finishDateDateEdit2.DateTime;
            else
                if (finishDateDateEdit.Text != "")
                dateEdit.Properties.MinValue = finishDateDateEdit.DateTime;
            else
                dateEdit.Properties.MinValue = allocationStartDate;

            dateEdit.Properties.MaxValue = allocationFinishDate;

            dateEdit.Text = currentText;
        }

        private void finishDateDateEdit3_QueryPopUp(object sender, CancelEventArgs e)
        {
            DateEdit dateEdit = (DateEdit)sender;
            var currentText = dateEdit.Text;

            if (startDateDateEdit3.Text != "")
                dateEdit.Properties.MinValue = startDateDateEdit3.DateTime;
            else
                if (finishDateDateEdit2.Text != "")
                dateEdit.Properties.MinValue = finishDateDateEdit2.DateTime;
            else
                    if (finishDateDateEdit.Text != "")
                dateEdit.Properties.MinValue = finishDateDateEdit.DateTime;
            else
                dateEdit.Properties.MinValue = allocationStartDate;

            dateEdit.Properties.MaxValue = allocationFinishDate;

            dateEdit.Text = currentText;
        }
    }
}
