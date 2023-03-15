using EmployeesMonitoringSystem.Model;
using System.Collections.Generic;

namespace EmployeesMonitoringSystem.Persistence
{
    public interface IEmployeesRepositoy : IRepository<Employee>
    {
        Employee FindByUsername(string username);
        List<Employee> FindLoggedEmployees();
    }
}
