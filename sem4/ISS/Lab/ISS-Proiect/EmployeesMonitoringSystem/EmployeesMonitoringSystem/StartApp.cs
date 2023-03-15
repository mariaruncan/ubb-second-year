using EmployeesMonitoringSystem.Model;
using EmployeesMonitoringSystem.Persistence;
using EmployeesMonitoringSystem.Persistence.Database;
using EmployeesMonitoringSystem.Service;
using EmployeesMonitoringSystem.Ui.Controllers;
using EmployeesMonitoringSystem.Ui.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeesMonitoringSystem
{
    internal static class StartApp
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
                maria, ionut - Workers
                andrei, alina - Bosses
                admin
            */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            IEmployeesRepositoy employeesRepo = new EmployeesDbRepository();
            ITasksRepository tasksRepo = new TasksDbRepository();
            ServiceImpl service = new ServiceImpl(employeesRepo, tasksRepo);

            LogInController ctrl = new LogInController(service);
            LogInForm form = new LogInForm(ctrl);
            Application.Run(form);
        }
    }
}
