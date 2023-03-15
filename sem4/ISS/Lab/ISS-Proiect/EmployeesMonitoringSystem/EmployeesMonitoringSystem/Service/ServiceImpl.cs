using EmployeesMonitoringSystem.Model;
using EmployeesMonitoringSystem.Persistence;
using EmployeesMonitoringSystem.Utils;
using System;
using System.Collections.Generic;

namespace EmployeesMonitoringSystem.Service
{
    public class ServiceImpl : Observable
    {
        private IEmployeesRepositoy EmployeesRepo;
        private ITasksRepository TasksRepo;

        public ServiceImpl(IEmployeesRepositoy employeesRepo, ITasksRepository tasksRepo)
        {
            EmployeesRepo = employeesRepo;
            TasksRepo = tasksRepo;
        }

        public void AddEmployee(string firstname, string lastname, string jobTitle, string username, string password)
        {
            string err = "";
            JobTitle j = JobTitle.BOSS;
            switch (jobTitle)
            {
                case "WORKER":
                    j = JobTitle.WORKER;
                    break;
                case "ADMIN":
                    j = JobTitle.ADMIN;
                    break;
                case "BOSS":
                    j = JobTitle.BOSS;
                    break;
                default:
                    err += "Invalid job title!\n";
                    break;
            }
            if (username.Length < 5)
                err += "Invalid username!\n";
            if (password.Length < 5)
                err += "Invalid password!\n";
            if (err != "")
                throw new ServiceException(err);
            EmployeesRepo.Add(new Employee
            {
                FirstName = firstname,
                LastName = lastname,
                Jobtitle = j,
                Username = username,
                Password = password,
                IsLogged = false
            });
        }

        public void AssignTask(string title, string description, int employeeId)
        {
            Employee emp = new Employee { Id = employeeId };
            Task task = new Task { Title = title, Description = description, Employee = emp, StartTime = DateTime.Now, Status = TaskStatus.PENDING };
            TasksRepo.Add(task);
            UpdateWorkers(new Message { Type = "New task", Data = employeeId });
        }

        public void CompleteTask(int taskId, DateTime end)
        {
            Task elem = TasksRepo.FindById(taskId);
            if (elem.Status != TaskStatus.PENDING)
                throw new Exception("The task is already completed!");
            elem.EndTime = end;
            elem.Status = TaskStatus.COMPLETED;
            TasksRepo.Update(elem, taskId);
        }

        public List<Employee> FindAllEmployees()
        {
            return EmployeesRepo.FindAll();
        }

        public Employee FindEmployeeByUsername(string username)
        {
            return EmployeesRepo.FindByUsername(username);
        }

        public List<Task> FindEmployeesTasks(int employeeId)
        {
            return TasksRepo.FindTasksByEmployee(employeeId);
        }
        
        public List<Employee> FindLoggedEmployees()
        {
            return EmployeesRepo.FindLoggedEmployees();
        }

        public void LogIn(string username, string password)
        {
            Employee employee = EmployeesRepo.FindByUsername(username);
            if(employee != null)
            {
                if (password != employee.Password)
                    throw new ServiceException("Authentication failed.");
                if (employee.IsLogged)
                    throw new ServiceException("Employee already logged in!");
                employee.IsLogged = true;
                EmployeesRepo.Update(employee, employee.Id);
                if(employee.Jobtitle == JobTitle.WORKER)
                {
                    UpdateBosses(new Message { Type = "Log in", Data = employee.Username });
                }
            }
            else
            {
                throw new ServiceException("Authentication failed.");
            }
        }

        public void LogOut(string username)
        {
            Employee employee = EmployeesRepo.FindByUsername(username);
            if (!employee.IsLogged)
                throw new ServiceException("Employee is not logged in!");
            employee.IsLogged = false;
            EmployeesRepo.Update(employee, employee.Id);
            if (employee.Jobtitle == JobTitle.WORKER)
                UpdateBosses(new Message { Type = "Log out", Data = employee.Username });
        }

        public void RemoveEmployee(int employeeId)
        {
            EmployeesRepo.Remove(employeeId);
        }

        public void UpdateEmployee(int id, string newFirstname, string newLastname, string newJobTitle, string newUsername, string newPassword)
        {
            string err = "";
            JobTitle j = JobTitle.BOSS;
            switch (newJobTitle)
            {
                case "WORKER":
                    j = JobTitle.WORKER;
                    break;
                case "ADMIN":
                    j = JobTitle.ADMIN;
                    break;
                case "BOSS":
                    j = JobTitle.BOSS;
                    break;
                default:
                    err += "Invalid job title!\n";
                    break;
            }
            if (newUsername.Length < 5)
                err += "Invalid username!\n";
            if (newPassword.Length < 5)
                err += "Invalid password!\n";
            if (err != "")
                throw new ServiceException(err);
            EmployeesRepo.Update(new Employee 
            {   
                FirstName = newFirstname, 
                LastName = newLastname, 
                Jobtitle = j, 
                Username = newUsername, 
                Password = newPassword 
            }, id);
        }
    }
}
