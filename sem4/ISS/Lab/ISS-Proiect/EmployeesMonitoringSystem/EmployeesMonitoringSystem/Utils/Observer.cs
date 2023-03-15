using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesMonitoringSystem.Utils
{
    public interface Observer
    {
        void update(Message data);
    }
}
