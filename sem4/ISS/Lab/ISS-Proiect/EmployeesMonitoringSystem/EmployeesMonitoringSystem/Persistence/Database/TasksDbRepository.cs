using EmployeesMonitoringSystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesMonitoringSystem.Persistence.Database
{
    public class TasksDbRepository : ITasksRepository
    {
        public void Add(Task elem)
        {
            Console.WriteLine("In Add TasksRepo, elem = ", elem);
            try
            {
                using (DataContext context = new DataContext())
                {
                    var task = new Task { Title = elem.Title, Description = elem.Description, StartTime = elem.StartTime, Status = elem.Status };
                    var employee = context.Employees.Find(elem.Employee.Id);
                    employee.Tasks.Add(task);
                    int r = context.SaveChanges();
                    if (r == 0)
                        throw new RepoException("Task was not added!");
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Out Add TasksRepo");
            }
        }

        public List<Task> FindAll()
        {
            throw new NotImplementedException();
        }

        public Task FindById(int id)
        {
            Console.WriteLine("In FindById TasksRepo, Id = ", id);
            Task task = null;
            try
            {
                using (DataContext context = new DataContext())
                {
                    task = context.Tasks.Find(id);
                    return task;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                return task;
            }
            finally
            {
                Console.WriteLine("Out FindById TasksRepo");
            }
        }

        public List<Task> FindTasksByEmployee(int employeeId)
        {
            Console.WriteLine("In FindTasksByEmployee TasksRepo");
            List<Task> tasks = new List<Task>();
            try
            {
                using (DataContext context = new DataContext())
                {
                    tasks = context.Tasks.Where(x => x.Employee.Id == employeeId).ToList();
                    return tasks;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return tasks;
            }
            finally
            {
                Console.WriteLine("Out FindTasksByEmployee TasksRepo");
            }
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Task elem, int id)
        {
            Console.WriteLine("In Update TasksRepo, id = ", id, ", elem = ", elem);
            try
            {
                using (DataContext context = new DataContext())
                {
                    var task = context.Tasks.Find(id);
                    if (task == null)
                        throw new RepoException("No task to update!");
                    task.Title = elem.Title;
                    task.Description = elem.Description;
                    task.Status = elem.Status;
                    task.StartTime = elem.StartTime;
                    task.EndTime = elem.EndTime;
                    context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Out Update TasksRepo");
            }
        }
    }
}
