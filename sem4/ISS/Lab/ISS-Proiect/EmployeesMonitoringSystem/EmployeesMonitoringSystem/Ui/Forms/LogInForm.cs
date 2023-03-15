using EmployeesMonitoringSystem.Ui.Controllers;
using System;
using System.Windows.Forms;

namespace EmployeesMonitoringSystem.Ui.Forms
{
    public partial class LogInForm : Form
    {
        private LogInController Ctrl;

        public LogInForm(LogInController ctrl)
        {
            Ctrl = ctrl;
            InitializeComponent();
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            try
            {
                Ctrl.LogIn(username, password);
                switch (Ctrl.CrtEmployee.Jobtitle)
                {
                    case Model.JobTitle.ADMIN:
                        AdminController ctrl = new AdminController(Ctrl.Service, Ctrl.CrtEmployee);
                        AdminForm form = new AdminForm(ctrl);
                        form.Text = "Admin";
                        form.Show();
                        break;
                    case Model.JobTitle.BOSS:
                        BossController ctrl1 = new BossController(Ctrl.Service, Ctrl.CrtEmployee);
                        BossForm form1 = new BossForm(ctrl1);
                        Ctrl.Service.AddObserverBoss(form1);
                        form1.Text = "Boss";
                        form1.Show();
                        break;
                    case Model.JobTitle.WORKER:
                        WorkerController ctrl2 = new WorkerController(Ctrl.Service, Ctrl.CrtEmployee);
                        WorkerForm form2 = new WorkerForm(ctrl2);
                        Ctrl.Service.AddObserverWorker(form2);
                        form2.Text = "Worker";
                        form2.Show();
                        break;
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Log in error " + ex.Message);
                return;
            }
        }

        private void LogInForm_Load(object sender, EventArgs e) { }
    }
}
