using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeesMonitoringSystem.Model
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public JobTitle Jobtitle { get; set; }
        public bool IsLogged { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // Navigation properties
        public virtual ICollection<Task> Tasks { get; set; } =  new List<Task>();

        public override string ToString()
        {
            return $"Employee: id = {Id}, firstname = {FirstName}, lastname = {LastName}, jobTitle = {Jobtitle}, isLogged = {IsLogged}" +
                $",\n username = {Username}, password = {Password}";
        }
    }

    public enum JobTitle { BOSS, ADMIN, WORKER }
}
