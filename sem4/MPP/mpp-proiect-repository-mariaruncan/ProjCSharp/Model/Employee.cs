using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Employee : Entity<long>
    {
        public String Firstname { get; set; }
        public String Lastname { get; set; }
        public String Username { get; set; }
        public String HashedPassword { get; set; }

        public Employee(): base(0)
        {
            Firstname = "";
            Lastname = "";
            Username = "";
            HashedPassword = "";
        }

        public Employee(string username, string pass): base(0)
        {
            Firstname = "";
            Lastname = "";
            Username = username;
            HashedPassword = pass;
        }

        public Employee(string firstname, string lastname, string username, string hashedPassword): base(0)
        {
            Firstname = firstname;
            Lastname = lastname;
            Username = username;
            HashedPassword = hashedPassword;
        }

        public override string ToString()
        {
            return "Employee: " + base.Id + " " + Firstname + " " + Lastname + " " + Username;
        }

        public override bool Equals(object obj)
        {
            return obj is Employee employee &&
                   Id == employee.Id &&
                   Firstname == employee.Firstname &&
                   Lastname == employee.Lastname &&
                   Username == employee.Username &&
                   HashedPassword == employee.HashedPassword;
        }

        public override int GetHashCode()
        {
            int hashCode = -374713858;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Firstname);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Lastname);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(HashedPassword);
            return hashCode;
        }
    }
}
