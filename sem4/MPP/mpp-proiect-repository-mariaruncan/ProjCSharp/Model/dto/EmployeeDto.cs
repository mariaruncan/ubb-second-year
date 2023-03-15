using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.dto
{
    [Serializable]
    public class EmployeeDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public EmployeeDto(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public override bool Equals(object obj)
        {
            return obj is EmployeeDto dto &&
                   Username == dto.Username &&
                   Password == dto.Password;
        }

        public override int GetHashCode()
        {
            int hashCode = 568732665;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
            return hashCode;
        }
    }
}
