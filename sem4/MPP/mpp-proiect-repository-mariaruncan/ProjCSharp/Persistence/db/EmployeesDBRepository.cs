using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Model;

namespace Persistence.db
{
    public class EmployeesDBRepository : EmployeesRepository
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(EmployeesDBRepository));
        IDictionary<String, string> props;
        DBUtils dbUtils;

        public EmployeesDBRepository()
        {
            logger.Info("Initializing EmployeesDBRepository");
            dbUtils = new DBUtils();
        }

        public void add(Employee elem)
        {
            logger.Info($"saving employee {elem}");
            IDbConnection con = dbUtils.GetConnection();
            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Employees (firstname, lastname, username, hashedPassword) VALUES (@firstname, @lastname, @username, @hashedPassword);";
                    IDbDataParameter paramFirstname = cmd.CreateParameter();
                    paramFirstname.ParameterName = "@firstname";
                    paramFirstname.Value = elem.Firstname;
                    cmd.Parameters.Add(paramFirstname);

                    IDbDataParameter paramLastname = cmd.CreateParameter();
                    paramLastname.ParameterName = "@lastname";
                    paramLastname.Value = elem.Lastname;
                    cmd.Parameters.Add(paramLastname);

                    IDbDataParameter paramUsername = cmd.CreateParameter();
                    paramUsername.ParameterName = "@username";
                    paramUsername.Value = elem.Username;
                    cmd.Parameters.Add(paramUsername);

                    IDbDataParameter paramHashedPass = cmd.CreateParameter();
                    paramHashedPass.ParameterName = "@hashedPassword";
                    paramHashedPass.Value = elem.HashedPassword;
                    cmd.Parameters.Add(paramHashedPass);

                    var result = cmd.ExecuteNonQuery();
                    if (result == 0)
                        throw new RepositoryException("No employee added!");
                    logger.Info($"saved {result} instances");
                }
            }
            catch(SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
            }
        }

        public void delete(long id)
        {
            logger.Info($"deleting employee {id}");
            IDbConnection con = dbUtils.GetConnection();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Employees WHERE id = @id;";
                    IDbDataParameter paramId = cmd.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;
                    cmd.Parameters.Add(paramId);

                    var dataR = cmd.ExecuteNonQuery();
                    if (dataR == 0)
                        throw new RepositoryException("No employee deleted!");
                    logger.Info("exit delete EmployeeRepo");
                }
            }
            catch (SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
            }
        }

        public IEnumerable<Employee> findAll()
        {
            logger.Info("enter findAll");
            IDbConnection con = dbUtils.GetConnection();
            IList<Employee> employees = new List<Employee>();
            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, firstname, lastname, username, hashedPassword FROM Employees;";

                    using (var result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            long id = result.GetInt64(0);
                            String firstname = result.GetString(1);
                            String lastname = result.GetString(2);
                            String username = result.GetString(3);
                            String hashedPassword = result.GetString(4);
                            Employee e = new Employee(firstname, lastname, username, hashedPassword);
                            e.Id = id;
                            employees.Add(e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
            }
            logger.Info("exit findAll");
            return employees;
        }

        public Employee findById(long id)
        {
            logger.Info("enter findById");
            IDbConnection con = dbUtils.GetConnection();
            Employee employee = null;

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, firstname, lastname, username, hashedPassword FROM Employees WHERE id = @id LIMIT 1;";
                    IDbDataParameter paramId = cmd.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;
                    cmd.Parameters.Add(paramId);

                    using (var result = cmd.ExecuteReader())
                    {
                        if (result.Read())
                        {
                            long idE = result.GetInt64(0);
                            String firstname = result.GetString(1);
                            String lastname = result.GetString(2);
                            String username = result.GetString(3);
                            String hashedPassword = result.GetString(4);
                            employee = new Employee(firstname, lastname, username, hashedPassword);
                            employee.Id = idE;
                        }
                    }
                    logger.InfoFormat("exit findById");
                    return employee;
                }
            }
            catch(SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
                return employee;
            }
        }

        public Employee FindByUsername(string username)
        {
            logger.Info("enter findByUsername");
            IDbConnection con = dbUtils.GetConnection();
            Employee employee = null;

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, firstname, lastname, username, hashedPassword FROM Employees WHERE username = @username LIMIT 1;";
                    IDbDataParameter paramUsername= cmd.CreateParameter();
                    paramUsername.ParameterName = "@username";
                    paramUsername.Value = username;
                    cmd.Parameters.Add(paramUsername);

                    using (var result = cmd.ExecuteReader())
                    {
                        if (result.Read())
                        {
                            long id = result.GetInt64(0);
                            String firstname = result.GetString(1);
                            String lastname = result.GetString(2);
                            String usernameE = result.GetString(3);
                            String hashedPassword = result.GetString(4);
                            employee = new Employee(firstname, lastname, usernameE, hashedPassword);
                            employee.Id = id;
                        }
                    }
                    logger.InfoFormat("exit findById");
                    return employee;
                }
            }
            catch (SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
                return employee;
            }
        }

        public void update(Employee elem, long id)
        {
            logger.Info($"updating employee {elem}");
            IDbConnection con = dbUtils.GetConnection();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Employees SET firstname = @firstname, lastname = @lastname, username = @username, hashedPassword = @hashedPassword WHERE id = @id;";
                    IDbDataParameter paramFirstname = cmd.CreateParameter();
                    paramFirstname.ParameterName = "@firstname";
                    paramFirstname.Value = elem.Firstname;
                    cmd.Parameters.Add(paramFirstname);

                    IDbDataParameter paramLastname = cmd.CreateParameter();
                    paramLastname.ParameterName = "@lastname";
                    paramLastname.Value = elem.Lastname;
                    cmd.Parameters.Add(paramLastname);

                    IDbDataParameter paramUsername = cmd.CreateParameter();
                    paramUsername.ParameterName = "@username";
                    paramUsername.Value = elem.Username;
                    cmd.Parameters.Add(paramUsername);

                    IDbDataParameter paramHashedPass = cmd.CreateParameter();
                    paramHashedPass.ParameterName = "@hashedPassword";
                    paramHashedPass.Value = elem.HashedPassword;
                    cmd.Parameters.Add(paramHashedPass);

                    IDbDataParameter paramId = cmd.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = elem.Id;
                    cmd.Parameters.Add(paramId);

                    var result = cmd.ExecuteNonQuery();
                    logger.Info($"updated {result} instances");
                }
            }
            catch(SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
            }
        }
    }
}
