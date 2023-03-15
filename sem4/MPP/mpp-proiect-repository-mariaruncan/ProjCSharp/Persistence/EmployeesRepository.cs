using System;
namespace Persistence
{
    public interface EmployeesRepository : Repository<long, Model.Employee>
    {
        Model.Employee FindByUsername(String username);
    }
}
