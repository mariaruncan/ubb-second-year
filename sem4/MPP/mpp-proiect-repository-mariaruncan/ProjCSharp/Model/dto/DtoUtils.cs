using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.dto
{
    public class DtoUtils
    {
        public static Employee GetFromDTO(EmployeeDto empDto)
        {
            string username = empDto.Username;
            string password = empDto.Password;
            return new Employee(username, password);
        }

        public static EmployeeDto getDTO(Employee emp)
        {
            string username = emp.Username;
            string pass = emp.HashedPassword;
            return new EmployeeDto(username, pass);
        }
    }
}
