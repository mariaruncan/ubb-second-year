using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Model;

namespace Persistence.db
{
    public class GamesDBRepository : GamesRepository
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(EmployeesDBRepository));
        DBUtils dbUtils;

        public GamesDBRepository()
        {
            logger.Info("Initializing GamesDBRepository");
            dbUtils = new DBUtils();
        }

        public void add(Game elem)
        {
            logger.Info($"saving game {elem}");
            IDbConnection con = dbUtils.GetConnection();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Games (team1Id, team2Id, description, totalNoOfTickets, soldTickets, pricePerTicket, date) VALUES (@team1Id, @team2Id, @description, @totalNoOfTickets, @soldTickets, @pricePerTicket, @date);";

                    IDbDataParameter paramTeam1Id = cmd.CreateParameter();
                    paramTeam1Id.ParameterName = "@team1Id";
                    paramTeam1Id.Value = elem.Team1.Id;
                    cmd.Parameters.Add(paramTeam1Id);

                    IDbDataParameter paramTeam2Id = cmd.CreateParameter();
                    paramTeam2Id.ParameterName = "@team2Id";
                    paramTeam2Id.Value = elem.Team2.Id;
                    cmd.Parameters.Add(paramTeam2Id);

                    IDbDataParameter paramDescription = cmd.CreateParameter();
                    paramDescription.ParameterName = "@description";
                    paramDescription.Value = elem.Description;
                    cmd.Parameters.Add(paramDescription);

                    IDbDataParameter paramTotalNoOfTickets = cmd.CreateParameter();
                    paramTotalNoOfTickets.ParameterName = "@totalNoOfTickets";
                    paramTotalNoOfTickets.Value = elem.TotalNoOfTickets;
                    cmd.Parameters.Add(paramTotalNoOfTickets);

                    IDbDataParameter paramSoldTickets = cmd.CreateParameter();
                    paramSoldTickets.ParameterName = "@soldTickets";
                    paramSoldTickets.Value = elem.SoldTickets;
                    cmd.Parameters.Add(paramSoldTickets);

                    IDbDataParameter paramPricePerTicket = cmd.CreateParameter();
                    paramPricePerTicket.ParameterName = "@pricePerTicket";
                    paramPricePerTicket.Value = elem.PricePerTicket;
                    cmd.Parameters.Add(paramPricePerTicket);
                    
                    IDbDataParameter paramDate = cmd.CreateParameter();
                    paramDate.ParameterName = "@date";
                    paramDate.Value = elem.Date.ToString();
                    cmd.Parameters.Add(paramDate);

                    var result = cmd.ExecuteNonQuery();
                    if (result == 0)
                        throw new RepositoryException("No game added!");
                    logger.Info($"saved {result} instances");
                }
            }
            catch (SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
            }
        }

        public void delete(long id)
        {
            logger.Info($"deleting game {id}");
            IDbConnection con = dbUtils.GetConnection();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Games WHERE id = @id;";
                    IDbDataParameter paramId = cmd.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;
                    cmd.Parameters.Add(paramId);

                    var dataR = cmd.ExecuteNonQuery();
                    if (dataR == 0)
                        throw new RepositoryException("No game deleted!");
                    logger.Info("exit delete EmployeeRepo");
                }
            }
            catch (SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
            }
        }

        public IEnumerable<Game> findAll()
        {
            logger.Info("enter findAll");
            IDbConnection con = dbUtils.GetConnection();
            IList<Game> games = new List<Game>();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT G.id, G.description, G.totalNoOfTickets, G.soldTickets, " +
                        "G.pricePerTicket, G.date, T1.id, T1.name, T2.id, T2.name " +
                        "FROM Games G " +
                        "INNER JOIN Teams T1 on G.team1Id = T1.id " +
                        "INNER JOIN Teams T2 on G.team2Id = T2.id; ";
                    
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            long id = result.GetInt64(0);
                            String description = result.GetString(1);
                            int totalNoOfTickets = result.GetInt32(2);
                            int soldTickets = result.GetInt32(3);
                            float pricePerTicket = result.GetFloat(4);
                            DateTime date = DateTime.Parse(result.GetString(5));
                            long team1Id = result.GetInt64(6);
                            String team1Name = result.GetString(7);
                            long team2Id = result.GetInt64(8);
                            String team2Name = result.GetString(9);
                            Team team1 = new Team(team1Name);
                            team1.Id = team1Id;
                            Team team2 = new Team(team2Name);
                            team2.Id = team2Id;
                            Game g = new Game(team1, team2, date, description, totalNoOfTickets, soldTickets, pricePerTicket);
                            g.Id = id;
                            games.Add(g);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            logger.Info("exit findAll");
            return games;
        }

        public Game findById(long id)
        {
            logger.Info("enter findById");
            IDbConnection con = dbUtils.GetConnection();
            Game game = null;

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT G.id, G.description, G.totalNoOfTickets, G.soldTickets, " +
                            "G.pricePerTicket, G.date, T1.id, T1.name, T2.id, T2.name " +
                            "FROM Games G " +
                            "INNER JOIN Teams T1 on G.team1Id = T1.id " +
                            "INNER JOIN Teams T2 on G.team2Id = T2.id " +
                            "WHERE G.id = @id;";
                    IDbDataParameter paramId = cmd.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;
                    cmd.Parameters.Add(paramId);

                    using (var result = cmd.ExecuteReader())
                    {
                        if (result.Read())
                        {
                            long idG = result.GetInt64(0);
                            String description = result.GetString(1);
                            int totalNoOfTickets = result.GetInt32(2);
                            int soldTickets = result.GetInt32(3);
                            float pricePerTicket = result.GetFloat(4);
                            DateTime date = DateTime.Parse(result.GetString(5));
                            long team1Id = result.GetInt64(6);
                            String team1Name = result.GetString(7);
                            long team2Id = result.GetInt64(8);
                            String team2Name = result.GetString(9);
                            Team team1 = new Team(team1Name);
                            team1.Id = team1Id;
                            Team team2 = new Team(team2Name);
                            team2.Id = team2Id;
                            game = new Game(team1, team2, date, description, totalNoOfTickets, soldTickets, pricePerTicket);
                            game.Id = id;
                        }
                    }
                    logger.InfoFormat("exit findById");
                    return game;
                }
            }
            catch(SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
                return game;
            }
        }

        public void update(Game elem, long id)
        {
            logger.Info($"updating game {elem}");
            IDbConnection con = dbUtils.GetConnection();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Games SET team1Id = @team1Id, team2Id = @team2Id, " +
                        "description = @description, totalNoOfTickets = @totalNoOfTickets, " +
                        "soldTickets = @soldTickets, pricePerTicket = @pricePerTicket , date = @date " +
                        "WHERE id = @id;";
                    IDbDataParameter paramTeam1Id = cmd.CreateParameter();
                    paramTeam1Id.ParameterName = "@team1Id";
                    paramTeam1Id.Value = elem.Team1.Id;
                    cmd.Parameters.Add(paramTeam1Id);

                    IDbDataParameter paramTeam2Id = cmd.CreateParameter();
                    paramTeam2Id.ParameterName = "@team2Id";
                    paramTeam2Id.Value = elem.Team2.Id;
                    cmd.Parameters.Add(paramTeam2Id);

                    IDbDataParameter paramDescription = cmd.CreateParameter();
                    paramDescription.ParameterName = "@description";
                    paramDescription.Value = elem.Description;
                    cmd.Parameters.Add(paramDescription);

                    IDbDataParameter paramTotalNoOfTickets = cmd.CreateParameter();
                    paramTotalNoOfTickets.ParameterName = "@totalNoOfTickets";
                    paramTotalNoOfTickets.Value = elem.TotalNoOfTickets;
                    cmd.Parameters.Add(paramTotalNoOfTickets);

                    IDbDataParameter paramsoldTickets = cmd.CreateParameter();
                    paramsoldTickets.ParameterName = "@soldTickets";
                    paramsoldTickets.Value = elem.SoldTickets;
                    cmd.Parameters.Add(paramsoldTickets);

                    IDbDataParameter paramPricePerTicket = cmd.CreateParameter();
                    paramPricePerTicket.ParameterName = "@pricePerTicket";
                    paramPricePerTicket.Value = elem.PricePerTicket;
                    cmd.Parameters.Add(paramPricePerTicket);
                    
                    IDbDataParameter paramDate = cmd.CreateParameter();
                    paramDate.ParameterName = "@date";
                    paramDate.Value = elem.Date.ToString();
                    cmd.Parameters.Add(paramDate);

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
