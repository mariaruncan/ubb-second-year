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
    public partial class WorkerForm : Form, Observer
    {
        private readonly WorkerController Ctrl;
        private IList<Task> tasksData;
        private int taskId = -1;

        public WorkerForm(WorkerController ctrl)
        {
            InitializeComponent();
            dateTimePickerStart.CustomFormat = "MM/dd/yyyy hh:mm:ss";
            dateTimePickerEnd.CustomFormat = "MM/dd/yyyy hh:mm:ss";
            Ctrl = ctrl;
            tasksData = Ctrl.FindEmployeesTasks();
            dataGridViewTasks.DataSource = tasksData;
            foreach (DataGridViewColumn column in dataGridViewTasks.Columns)
                column.Visible = false;
            dataGridViewTasks.Columns["Id"].Visible = true;
            dataGridViewTasks.Columns["Id"].Width = 45;
            dataGridViewTasks.Columns["Title"].Visible = true;
            dataGridViewTasks.Columns["Status"].Visible = true;
            dataGridViewTasks.Columns["Status"].Width = 80;
        }

        private void WorkerForm_Load(object sender, EventArgs e) { }

        private void WorkerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Ctrl.LogOut();
                Ctrl.Service.RemoveObserverWorker(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void buttonComplete_Click(object sender, EventArgs e)
        {
            if (taskId == -1)
                MessageBox.Show(this, "Please select a task to complete!");
            else
            {
                try
                {
                    Ctrl.CompleteTask(taskId, dateTimePickerEnd.Value);
                    MessageBox.Show(this, "Task completed!");
                    UpdateDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message);
                }
            }
        }

        private void UpdateDataGrid()
        {
            dataGridViewTasks.DataSource = null;
            tasksData = Ctrl.FindEmployeesTasks();
            dataGridViewTasks.DataSource = tasksData;
            foreach (DataGridViewColumn column in dataGridViewTasks.Columns)
                column.Visible = false;
            dataGridViewTasks.Columns["Id"].Visible = true;
            dataGridViewTasks.Columns["Id"].Width = 45;
            dataGridViewTasks.Columns["Title"].Visible = true;
            dataGridViewTasks.Columns["Status"].Visible = true;
            dataGridViewTasks.Columns["Status"].Width = 80;
            taskId = -1;
        }

        public void update(Utils.Message data)
        {
            if(data.Type == "New task")
            {
                if((int)data.Data == Ctrl.CrtEmployee.Id)
                {
                    MessageBox.Show(this, "New task");
                    UpdateDataGrid();
                }
            }
        }

        private void dataGridViewTasks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0 || index >= tasksData.Count)
            {
                taskId = -1;
                dateTimePickerStart.Value = DateTime.Now;
                dateTimePickerEnd.Value = DateTime.Now;
                textBoxDescription.Text = "";
            }
            else
            {
                taskId = tasksData[index].Id;
                dateTimePickerStart.Value = tasksData[index].StartTime;
                try
                {
                    dateTimePickerEnd.Value = tasksData[index].EndTime;
                }
                catch
                {
                    dateTimePickerEnd.Value = DateTime.Now;
                }
                textBoxDescription.Text = tasksData[index].Description;
            }
        }
    }
}
