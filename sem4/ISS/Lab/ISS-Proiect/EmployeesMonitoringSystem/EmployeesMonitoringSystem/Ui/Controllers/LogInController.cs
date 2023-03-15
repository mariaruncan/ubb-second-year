using EmployeesMonitoringSystem.Model;
using EmployeesMonitoringSystem.Service;

namespace EmployeesMonitoringSystem.Ui.Controllers
{
    public class LogInController
    {
        public ServiceImpl Service { get; set; }
        public Employee CrtEmployee { get; set; }

        public LogInController(ServiceImpl service)
        {
            Service = service;
        }

        public void LogIn(string username, string password)
        {
            Service.LogIn(username, password);
            CrtEmployee = Service.FindEmployeeByUsername(username);
        }
    }
}
