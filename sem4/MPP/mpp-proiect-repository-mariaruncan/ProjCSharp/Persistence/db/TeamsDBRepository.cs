using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Model;

namespace Persistence.db
{
    public class TeamsDBRepository : TeamsRepository
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(EmployeesDBRepository));
        DBUtils dbUtils;

        public TeamsDBRepository()
        {
            logger.Info("Initializing TeamsDBRepository");
            dbUtils = new DBUtils();
        }

        public void add(Team elem)
        {
            logger.Info($"saving employee {elem}");
            IDbConnection con = dbUtils.GetConnection();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Teams (name) VALUES (@fname);";
                    IDbDataParameter paramName = cmd.CreateParameter();
                    paramName.ParameterName = "@name";
                    paramName.Value = elem.Name;
                    cmd.Parameters.Add(paramName);

                    var result = cmd.ExecuteNonQuery();
                    if (result == 0)
                        throw new RepositoryException("No team added!");
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
            logger.Info($"deleting team {id}");
            IDbConnection con = dbUtils.GetConnection();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Teams WHERE id = @id;";
                    IDbDataParameter paramId = cmd.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;
                    cmd.Parameters.Add(paramId);

                    var dataR = cmd.ExecuteNonQuery();
                    if (dataR == 0)
                        throw new RepositoryException("No team deleted!");
                    logger.Info("exit delete TeamsRepo");
                }
            }
            catch (SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
            }
        }

        public IEnumerable<Team> findAll()
        {
            logger.Info("enter findAll");
            IDbConnection con = dbUtils.GetConnection();
            IList<Team> teams = new List<Team>();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, name FROM Teams;";

                    using (var result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            long id = result.GetInt64(0);
                            String name = result.GetString(1);
                            Team t = new Team(name);
                            t.Id = id;
                            teams.Add(t);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            logger.Info("exit findAll");
            return teams;
        }

        public Team findById(long id)
        {
            logger.Info("enter findById");
            IDbConnection con = dbUtils.GetConnection();
            Team team = null;

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, name FROM Teams WHERE id = @id LIMIT 1;";
                    IDbDataParameter paramId = cmd.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;
                    cmd.Parameters.Add(paramId);

                    using (var result = cmd.ExecuteReader())
                    {
                        if (result.Read())
                        {
                            long idT = result.GetInt64(0);
                            String name = result.GetString(1);
                            team = new Team(name);
                            team.Id = idT;
                        }
                    }
                    // log.InfoFormat("Exiting findOne with value {0}", null);
                    return team;
                }
            }
            catch(SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
                return team;
            }
        }

        public void update(Team elem, long id)
        {
            logger.Info($"updating employee {elem}");
            IDbConnection con = dbUtils.GetConnection();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Teams SET name = @name WHERE id = @id;";
                    IDbDataParameter paramName = cmd.CreateParameter();
                    paramName.ParameterName = "@name";
                    paramName.Value = elem.Name;
                    cmd.Parameters.Add(paramName);

                    IDbDataParameter paramId = cmd.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = elem.Id;
                    cmd.Parameters.Add(paramId);

                    var result = cmd.ExecuteNonQuery();
                    logger.Info($"updated {result} instances");
                }
            }
            catch (SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
            }
        }
    }
}
