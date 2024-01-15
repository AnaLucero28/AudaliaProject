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

namespace WindowsApplication
{
    public partial class EmployeeForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string employeeID;
        bool modified;
        static List<string> branchOfficeList = new List<string>() { "Madrid", "Barcelona" };
        static int branchOfficeIx = 0;

        public EmployeeForm()
        {
            InitializeComponent();

            InitComboBoxes();

            if (Data.Audit.UserPermissions <= UserPermissions.Read)
            {
                bbiSave.Enabled = false;
                bbiSaveAndClose.Enabled = false;
                bbiSaveAndNew.Enabled = false;
                bbiReset.Enabled = false;
                bbiDelete.Enabled = false;

                //layoutControl1.Enabled = false;
                layoutControl1.OptionsView.IsReadOnly = DevExpress.Utils.DefaultBoolean.True;
            }
        }

        void InitComboBoxes()
        {
            var categoryList = new List<string>() { "Socio", "Director", "Gerente", "Senior", "Asistente", "Becario" };
            categoryComboBoxEdit.Properties.Items.AddRange(categoryList);
            categoryComboBoxEdit.SelectedItem = categoryComboBoxEdit.Properties.Items[5];
            
            branchOfficeComboBoxEdit.Properties.Items.AddRange(branchOfficeList);
            branchOfficeComboBoxEdit.SelectedItem = branchOfficeComboBoxEdit.Properties.Items[branchOfficeIx];
        }

        public static void CreateEmployee()
        {
            var form = new EmployeeForm();
            form.Owner = Application.OpenForms[0];
            form.StartPosition = FormStartPosition.CenterParent;

            form.employeeID = "";
            form.modified = true;

            form.ShowDialog();
            form.Dispose();
        }

        public static void EditEmployee(string employeeID)
        {            
            var form = new EmployeeForm();
            form.Owner = Application.OpenForms[0];
            form.StartPosition = FormStartPosition.CenterParent;

            form.employeeID = employeeID;
            var employee = Data.Audit.Employees[form.employeeID];
            form.nameTextEdit.Text = employee.Name;

            //form.categoryTextEdit.Text = employee.Category;
            form.categoryComboBoxEdit.SelectedItem = employee.Category;

            //form.branchOfficeTextEdit.Text = employee.BranchOffice;
            form.branchOfficeComboBoxEdit.SelectedItem = employee.BranchOffice;

            //form.statusCheckEdit.Checked = employee.Status != "Inactive";
            if (employee.LeavingDate != null)
            {
                form.leavingDateDateEdit.DateTime = employee.LeavingDate.Value;
            }
            else
            {
                form.leavingDateDateEdit.Text = "";
            }

            form.modified = false;

            form.ShowDialog();
            form.Dispose();
        }

        void Save()
        {
            if (this.employeeID == "")
            {
                var employee = new Employee();
                employee.Name = this.nameTextEdit.Text;

                //employee.BranchOffice = this.branchOfficeTextEdit.Text;
                employee.BranchOffice = this.branchOfficeComboBoxEdit.SelectedItem.ToString();

                //employee.Category = this.categoryTextEdit.Text;
                employee.Category = this.categoryComboBoxEdit.SelectedItem.ToString();

                //employee.Status = this.statusCheckEdit.Checked ? "Active" : "Inactive";
                employee.LeavingDate = (leavingDateDateEdit.Text != "") ? leavingDateDateEdit.DateTime : (DateTime?)null;
                
                var employeeBase = DataHUBClient.ServiceContract.PostEmployee(employee);
                employee.DBID = employeeBase.DBID;
                this.employeeID = employeeBase.DBID;

                Data.Audit.Employees.Add(employee.DBID, employee);                
            }
            else
            {
                var employee = Data.Audit.Employees[this.employeeID];

                employee.Name = this.nameTextEdit.Text;

                //employee.BranchOffice = this.branchOfficeTextEdit.Text;
                employee.BranchOffice = this.branchOfficeComboBoxEdit.SelectedItem.ToString();

                //employee.Category = this.categoryTextEdit.Text;
                employee.Category = this.categoryComboBoxEdit.SelectedItem.ToString();

                //employee.Status = this.statusCheckEdit.Checked ? "Active" : "Inactive";
                employee.LeavingDate = (this.leavingDateDateEdit.Text != "") ? this.leavingDateDateEdit.DateTime : (DateTime?)null;

                var employeeBase = DataHUBClient.ServiceContract.PutEmployee(employee);                
            }

            modified = false;
        }

        void New()
        {
            Close();
            Dispose();
            CreateEmployee();
        }

        void Reset()
        {
            //TODO:...
        }

        private void bbiSaveAndClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
            this.Close();
        }

        private void bbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }

        private void bbiSaveAndNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
            New();
        }

        private void bbiReset_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Reset();
        }

        private void bbiClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void nameTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            modified = true;
        }

        private void EmployeeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modified && MessageBox.Show(this, "¿Desea salir sin guardar los cambios?", "Auditor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void branchOfficeComboBoxEdit_CloseUp(object sender, CloseUpEventArgs e)
        {
            branchOfficeIx = branchOfficeList.IndexOf(e.Value.ToString());
        }
    }
}
