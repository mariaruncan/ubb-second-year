using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.db.ef
{
    public class EmployeesDbEfRepository : EmployeesRepository
    {

        public void add(Employee elem)
        {
            using (DataContext context = new DataContext())
            {
                context.Employees.Add(new Emp { firstname = elem.Firstname, lastname = elem.Lastname, username = elem.Username, hashedPassword = elem.HashedPassword });
                context.SaveChanges();
            }
        }

        public void delete(long id)
        {
            using(DataContext context = new DataContext())
            {
                var emp = context.Employees.Find(id);
                if (emp != null)
                {
                    context.Remove(emp);
                    context.SaveChanges();
                }
            }
        }

        public IEnumerable<Employee> findAll()
        {
            using (DataContext context = new DataContext())
            {
                List<Employee> rez = new List<Employee>();
                context.Employees.ToList().ForEach(emp =>
                {
                    Employee e = new Employee(emp.firstname, emp.lastname, emp.username, emp.hashedPassword);
                    e.Id = emp.id;
                    rez.Add(e);
                });
                return rez;
            }
        }

        public Employee findById(long id)
        {
            using (DataContext context = new DataContext())
            {
                var emp = context.Employees.Where(e => e.id == id);
                if (emp != null)
                {
                    var e = emp.First();
                    Employee rez = new Employee(e.firstname, e.lastname, e.username, e.hashedPassword);
                    rez.Id = e.id;
                    return rez;
                }
            }
            return null;
        }

        public Employee FindByUsername(string username)
        {
            using (DataContext context = new DataContext())
            {
                var emp = context.Employees.Where(e => e.username == username).FirstOrDefault();
                if (emp != null)
                {
                    Employee rez = new Employee(emp.firstname, emp.lastname, emp.username, emp.hashedPassword);
                    rez.Id = emp.id;
                    return rez;
                }
            }
            return null;
        }

        public void update(Employee elem, long id)
        {
            using(DataContext context = new DataContext())
            {
                var emp = context.Employees.Find(id);
                if (emp != null)
                {
                    emp.firstname = elem.Firstname;
                    emp.lastname = elem.Lastname;
                    emp.username = elem.Username;
                    emp.hashedPassword = elem.HashedPassword;
                    context.SaveChanges();
                }
            }
        }
    }
}
