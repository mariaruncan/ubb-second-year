using EmployeesMonitoringSystem.Model;
using EmployeesMonitoringSystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmployeesMonitoringSystem.Ui.Controllers
{
    public class BossController
    {
        public ServiceImpl Service { get; set; }
        public Employee CrtEmployee { get; set; }

        public BossController(ServiceImpl service, Employee crtEmployee)
        {
            Service = service;
            CrtEmployee = crtEmployee;
        }

        public IList<Employee> FindLoggedWorkers()
        {
            return Service.FindLoggedEmployees();
        }

        public void LogOut()
        {
            Service.LogOut(CrtEmployee.Username);
        }

        public void AssignTask(string title, string description, int empId)
        {
            Service.AssignTask(title, description, empId);
        }

        public IList<Task> FindEmployeesTasks(int id)
        {
            return Service.FindEmployeesTasks(id);
        }
    }
}
