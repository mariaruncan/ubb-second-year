using EmployeesMonitoringSystem.Model;
using EmployeesMonitoringSystem.Ui.Controllers;
using EmployeesMonitoringSystem.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EmployeesMonitoringSystem.Ui.Forms
{
    public partial class BossForm : Form, Observer
    {
        private readonly BossController Ctrl;
        private IList<Employee> workersData;
        private IList<Task> tasksData;
        private int empId = -1;

        public BossForm(BossController ctrl)
        {
            InitializeComponent();
            Ctrl = ctrl;
            workersData = Ctrl.FindLoggedWorkers();
            dataGridViewWorkers.DataSource = workersData;
            foreach (DataGridViewColumn column in dataGridViewWorkers.Columns)
                column.Visible = false;
            dataGridViewWorkers.Columns["FirstName"].Visible = true;
            dataGridViewWorkers.Columns["LastName"].Visible = true;

            if(workersData.Count > 0)
            {
                tasksData = Ctrl.FindEmployeesTasks(workersData[0].Id);
                dataGridViewTasks.DataSource = tasksData;
                foreach (DataGridViewColumn column in dataGridViewTasks.Columns)
                    column.Visible = false;
                dataGridViewTasks.Columns["Id"].Visible = true;
                dataGridViewTasks.Columns["Id"].Width = 50;
                dataGridViewTasks.Columns["Title"].Visible = true;
                dataGridViewTasks.Columns["Status"].Visible = true;
                dataGridViewTasks.Columns["StartTime"].Visible = true;
                dataGridViewTasks.Columns["EndTime"].Visible = true;
            }
        }

        private void BossForm_Load(object sender, EventArgs e) { }

        public void update(Utils.Message data)
        {
            if(data.Type == "Log in")
            {
                MessageBox.Show((string)data.Data + " logged in!");
                updateDataGridEmployees();
            }
            else if(data.Type == "Log out")
            {
                MessageBox.Show((string)data.Data + " logged out!");
                updateDataGridEmployees();
            }
        }

        private void updateDataGridEmployees()
        {
            workersData = Ctrl.FindLoggedWorkers();
            dataGridViewWorkers.DataSource = null;
            dataGridViewWorkers.DataSource = workersData;
            foreach (DataGridViewColumn column in dataGridViewWorkers.Columns)
                column.Visible = false;
            dataGridViewWorkers.Columns["FirstName"].Visible = true;
            dataGridViewWorkers.Columns["LastName"].Visible = true;
        }

        private void BossForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Ctrl.LogOut();
                Ctrl.Service.RemoveObserverBosses(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void buttonAssign_Click(object sender, EventArgs e)
        {
            try
            {
                if (empId == -1)
                    throw new Exception("Please select a worker!");
                Ctrl.AssignTask(textBoxTitle.Text, textBoxDescription.Text, empId);
                MessageBox.Show(this, "Task assigned!");
                buttonClear_Click(sender, e);
                updateDataGridTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void updateDataGridTasks()
        {
            if (empId != -1)
            {
                tasksData = Ctrl.FindEmployeesTasks(empId);
                dataGridViewTasks.DataSource = tasksData;
                foreach (DataGridViewColumn column in dataGridViewTasks.Columns)
                    column.Visible = false;
                dataGridViewTasks.Columns["Id"].Visible = true;
                dataGridViewTasks.Columns["Id"].Width = 50;
                dataGridViewTasks.Columns["Title"].Visible = true;
                dataGridViewTasks.Columns["Status"].Visible = true;
                dataGridViewTasks.Columns["StartTime"].Visible = true;
                dataGridViewTasks.Columns["EndTime"].Visible = true;
            }
        }

        private void dataGridViewWorkers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0 || index >= workersData.Count)
                empId = -1;
            else
                empId = workersData[index].Id;
            updateDataGridTasks();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxTitle.Text = "";
            textBoxDescription.Text = "";
        }
    }
}
