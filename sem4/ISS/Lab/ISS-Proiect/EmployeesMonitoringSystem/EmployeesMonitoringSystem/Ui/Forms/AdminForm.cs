using EmployeesMonitoringSystem.Model;
using EmployeesMonitoringSystem.Ui.Controllers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EmployeesMonitoringSystem.Ui.Forms
{
    public partial class AdminForm : Form
    {
        private readonly AdminController Ctrl;
        private IList<Employee> employeesData;
        public AdminForm(AdminController ctrl)
        {
            InitializeComponent();
            Ctrl = ctrl;
            employeesData = Ctrl.FindAllEmployees();
            dataGridViewEmployees.DataSource = employeesData;
            foreach (DataGridViewColumn column in dataGridViewEmployees.Columns)
                column.Visible = false;
            dataGridViewEmployees.Columns["Id"].Visible = true;
            dataGridViewEmployees.Columns["Id"].Width = 50;
            dataGridViewEmployees.Columns["Username"].Visible = true;
        }

        private void AdminForm_Load(object sender, EventArgs e) { }

        private void updateDataGrid()
        {
            dataGridViewEmployees.DataSource = null;
            employeesData = Ctrl.FindAllEmployees().FindAll(x => x.Username.StartsWith(textBoxSearch.Text));
            dataGridViewEmployees.DataSource = employeesData;
            foreach (DataGridViewColumn column in dataGridViewEmployees.Columns)
                column.Visible = false;
            dataGridViewEmployees.Columns["Id"].Visible = true;
            dataGridViewEmployees.Columns["Id"].Width = 50;
            dataGridViewEmployees.Columns["Username"].Visible = true;

            clearFields();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            updateDataGrid();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Ctrl.AddEmployee(textBoxFirstname.Text, textBoxLastname.Text, comboBoxJobTitle.Text, textBoxUsername.Text, textBoxPassword.Text);
                MessageBox.Show(this, "Employee added!");
                updateDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string id = textBoxId.Text;
                if (id == "")
                {
                    MessageBox.Show(this, "Select an employee!");
                    return;
                }
                Ctrl.UpdateEmployee(id, textBoxFirstname.Text, textBoxLastname.Text, comboBoxJobTitle.Text,
                    textBoxUsername.Text, textBoxPassword.Text);
                MessageBox.Show(this, "Employee updated!");
                updateDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string id = textBoxId.Text;
                if(id == "")
                {
                    MessageBox.Show(this, "Select an employee!");
                    return;
                }
                Ctrl.RemoveEmployee(id);
                MessageBox.Show(this, "Employee removed!");
                updateDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void clearFields()
        {
            textBoxId.Text = "";
            textBoxFirstname.Text = "";
            textBoxLastname.Text = "";
            textBoxUsername.Text = "";
            textBoxPassword.Text = "";
            comboBoxJobTitle.Text = "";
        }

        private void dataGridViewEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0 || index >= employeesData.Count)
            {
                clearFields();
                return;
            }
            textBoxId.Text = employeesData[index].Id.ToString();
            textBoxFirstname.Text = employeesData[index].FirstName;
            textBoxLastname.Text = employeesData[index].LastName;
            textBoxUsername.Text = employeesData[index].Username;
            textBoxPassword.Text = employeesData[index].Password;
            switch (employeesData[index].Jobtitle)
            {
                case JobTitle.BOSS:
                    comboBoxJobTitle.SelectedIndex = 1;
                    break;
                case JobTitle.WORKER:
                    comboBoxJobTitle.SelectedIndex = 2;
                    break;
                case JobTitle.ADMIN:
                    comboBoxJobTitle.SelectedIndex = 0;
                    break;
            }
        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Ctrl.LogOut();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
    }
}
