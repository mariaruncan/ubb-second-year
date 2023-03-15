using EmployeesMonitoringSystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesMonitoringSystem.Persistence.Database
{
    public class EmployeesDbRepository : IEmployeesRepositoy
    {
        public void Add(Employee elem)
        {
            Console.WriteLine("In Add EmployeesRepo, elem = ", elem);
            try
            {
                using (DataContext context = new DataContext())
                {
                    context.Employees.Add(elem);
                    int r = context.SaveChanges();
                    if (r == 0)
                        throw new RepoException("Employee was not added!");
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Out Add EmployeesRepo");
            }
        }

        public List<Employee> FindAll()
        {
            Console.WriteLine("In FindAll EmployeesRepo");
            List<Employee> employees = new List<Employee>();
            try
            {
                using (DataContext context = new DataContext())
                {
                    employees = context.Employees.ToList();
                    return employees;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return employees;
            }
            finally
            {
                Console.WriteLine("Out FindAll EmployeesRepo");
            }
        }

        public Employee FindById(int id)
        {
            Console.WriteLine("In FindById EmployeesRepo, Id = ", id);
            Employee employee = null;
            try
            {
                using (DataContext context = new DataContext())
                {
                    employee = context.Employees.Find(id);
                    return employee;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                return employee;
            }
            finally
            {
                Console.WriteLine("Out FindById EmployeesRepo");
            }
        }

        public Employee FindByUsername(string username)
        {
            Console.WriteLine("In FindByUsername EmployeesRepo, username = {0}", username);
            Employee employee = null;
            try
            {
                using (DataContext context = new DataContext())
                {
                    employee = context.Employees
                        .Where(x => x.Username == username)
                        .Include(x => x.Tasks)
                        .FirstOrDefault();
                    return employee;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                return employee;
            }
            finally
            {
                Console.WriteLine("Out FindByUsername EmployeesRepo");
            }
        }

        public List<Employee> FindLoggedEmployees()
        {
            Console.WriteLine("In FindLoggedEmployees EmployeesRepo");
            List<Employee> employees = new List<Employee>();
            try
            {
                using (DataContext context = new DataContext())
                {
                    employees = context.Employees.Where(x => x.IsLogged == true && x.Jobtitle == JobTitle.WORKER).ToList();
                    return employees;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return employees;
            }
            finally
            {
                Console.WriteLine("Out FindLoggedEmployees EmployeesRepo");
            }
        }

        public void Remove(int id)
        {
            Console.WriteLine("In Remove EmployeesRepo, id = ", id);
            try
            {
                using (DataContext context = new DataContext())
                {
                    var emp = context.Employees.Include("Tasks").Where(x => x.Id == id).First();
                    if (emp == null)
                        throw new RepoException("No employee deleted!");
                    context.Employees.Remove(emp);
                    context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Out Remove EmployeesRepo");
            }
        }

        public void Update(Employee elem, int id)
        {
            Console.WriteLine("In Update EmployeesRepo, id = ", id, ", elem = ", elem);
            try
            {
                using (DataContext context = new DataContext())
                {
                    var emp = context.Employees.Find(id);
                    if (emp == null)
                        throw new RepoException("No employee to update!");
                    emp.FirstName = elem.FirstName;
                    emp.LastName = elem.LastName;
                    emp.Jobtitle = elem.Jobtitle;
                    emp.Username = elem.Username;
                    emp.Password = elem.Password;
                    emp.IsLogged = elem.IsLogged;
                    context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Out Update EmployeesRepo");
            }
        }
    }
}
