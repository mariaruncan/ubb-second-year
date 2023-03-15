using EmployeesMonitoringSystem.Model;
using EmployeesMonitoringSystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesMonitoringSystem.Ui.Controllers
{
    public class WorkerController
    {
        public ServiceImpl Service { get; set; }
        public Employee CrtEmployee { get; set; }

        public WorkerController(ServiceImpl service, Employee crtEmployee)
        {
            Service = service;
            CrtEmployee = crtEmployee;
        }

        public void LogOut()
        {
            Service.LogOut(CrtEmployee.Username);
        }

        public IList<Model.Task> FindEmployeesTasks()
        {
            return Service.FindEmployeesTasks(CrtEmployee.Id);
        }

        public void CompleteTask(int taskId, DateTime value)
        {
            Service.CompleteTask(taskId, value);
        }
    }
}
