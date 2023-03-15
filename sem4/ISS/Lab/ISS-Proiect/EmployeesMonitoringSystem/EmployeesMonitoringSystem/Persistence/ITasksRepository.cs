using EmployeesMonitoringSystem.Model;
using System.Collections.Generic;

namespace EmployeesMonitoringSystem.Persistence
{
    public interface ITasksRepository : IRepository<Task>
    {
        List<Task> FindTasksByEmployee(int employeeId);
    }
}
