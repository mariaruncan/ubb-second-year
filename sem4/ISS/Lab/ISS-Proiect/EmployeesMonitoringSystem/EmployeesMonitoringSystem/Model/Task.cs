using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeesMonitoringSystem.Model
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Navigation properties
        public virtual Employee Employee { get; set; }

        public override string ToString()
        {
            return $"Task: id = {Id}, title = {Title},\n description = {Description},\n status = {Status.ToString()}," +
                $" startTime = {StartTime}, endTime = {EndTime},\n assigned to {Employee.Username}";
        }
    }

    public enum TaskStatus { PENDING, COMPLETED }
}
