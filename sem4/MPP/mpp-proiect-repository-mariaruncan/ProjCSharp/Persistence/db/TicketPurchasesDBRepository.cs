using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Model;

namespace Persistence.db
{
    public class TicketPurchasesDBRepository : TicketPurchesesRepository
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(EmployeesDBRepository));
        DBUtils dbUtils;

        public TicketPurchasesDBRepository()
        {
            logger.Info("Initializing TicketPurchasesDBRepository");
            dbUtils = new DBUtils();
        }

        public void add(TicketPurchase elem)
        {
            logger.Info($"saving ticket purchase {elem}");
            IDbConnection con = dbUtils.GetConnection();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO TicketPurchases (gameId, noOfTickets, totalPrice, clientName) " +
                        "VALUES (@gameId, @noOfTickets, @totalPrice, @clientName);";

                    IDbDataParameter paramGameId = cmd.CreateParameter();
                    paramGameId.ParameterName = "@gameId";
                    paramGameId.Value = elem.Game.Id;
                    cmd.Parameters.Add(paramGameId);

                    IDbDataParameter paramNoOfTickets = cmd.CreateParameter();
                    paramNoOfTickets.ParameterName = "@noOfTickets";
                    paramNoOfTickets.Value = elem.NoOfTickets;
                    cmd.Parameters.Add(paramNoOfTickets);

                    IDbDataParameter paramTotalPrice = cmd.CreateParameter();
                    paramTotalPrice.ParameterName = "@totalPrice";
                    paramTotalPrice.Value = elem.TotalPrice;
                    cmd.Parameters.Add(paramTotalPrice);

                    IDbDataParameter paramClientName = cmd.CreateParameter();
                    paramClientName.ParameterName = "@clientName";
                    paramClientName.Value = elem.ClientName;
                    cmd.Parameters.Add(paramClientName);

                    var result = cmd.ExecuteNonQuery();
                    if (result == 0)
                        throw new RepositoryException("No ticket purchase added!");
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
            logger.Info($"deleting ticket purchase {id}");
            IDbConnection con = dbUtils.GetConnection();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM TicketPurchases WHERE id = @id;";
                    IDbDataParameter paramId = cmd.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;
                    cmd.Parameters.Add(paramId);

                    var dataR = cmd.ExecuteNonQuery();
                    if (dataR == 0)
                        throw new RepositoryException("No ticket purchase deleted!");
                    logger.Info("exit delete EmployeeRepo");
                }
            }
            catch (SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
            }
        }

        public IEnumerable<TicketPurchase> findAll()
        {
            logger.Info("enter findAll");
            IDbConnection con = dbUtils.GetConnection();
            IList<TicketPurchase> ticketPurchases = new List<TicketPurchase>();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT TP.id, TP.gameId, TP.noOfTickets, TP.totalPrice, " +
                        "TP.clientName " +
                        "FROM TicketPurchases TP; ";

                    using (var result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            long tpId = result.GetInt64(0);
                            long gameId = result.GetInt64(1);
                            int noOfTickets = result.GetInt32(2);
                            float totalPrice = result.GetFloat(3);
                            String clientName = result.GetString(4);

                            Game game = null;

                            using (IDbCommand cmd1 = con.CreateCommand())
                            {
                                cmd1.CommandText = "SELECT G.id, G.description, G.totalNoOfTickets, G.soldTickets, " +
                                        "G.pricePerTicket, G.date, T1.id, T1.name, T2.id, T2.name " +
                                        "FROM Games G " +
                                        "INNER JOIN Teams T1 on G.team1Id = T1.id " +
                                        "INNER JOIN Teams T2 on G.team2Id = T2.id " +
                                        "WHERE G.id = @id " +
                                        "LIMIT 1;";
                                IDbDataParameter paramId = cmd1.CreateParameter();
                                paramId.ParameterName = "@id";
                                paramId.Value = gameId;
                                cmd1.Parameters.Add(paramId);

                                using (var result1 = cmd1.ExecuteReader())
                                {
                                    if (result1.Read())
                                    {
                                        long idG = result1.GetInt64(0);
                                        String description = result1.GetString(1);
                                        int totalNoOfTickets = result1.GetInt32(2);
                                        int soldTickets = result1.GetInt32(3);
                                        float pricePerTicket = result1.GetFloat(4);
                                        DateTime date = result1.GetDateTime(5);
                                        long team1Id = result1.GetInt64(6);
                                        String team1Name = result1.GetString(7);
                                        long team2Id = result1.GetInt64(8);
                                        String team2Name = result1.GetString(9);
                                        Team team1 = new Team(team1Name);
                                        team1.Id = team1Id;
                                        Team team2 = new Team(team2Name);
                                        team2.Id = team2Id;
                                        game = new Game(team1, team2, date, description, totalNoOfTickets, soldTickets, pricePerTicket);
                                        game.Id = gameId;
                                    }
                                }
                            }

                            TicketPurchase tp = new TicketPurchase(game, noOfTickets, totalPrice, clientName);
                            tp.Id = tpId;
                            ticketPurchases.Add(tp);
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
            return ticketPurchases;
        }

        public TicketPurchase findById(long id)
        {
            logger.Info("enter findById");
            IDbConnection con = dbUtils.GetConnection();
            TicketPurchase tp = null;

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT TP.id, TP.gameId, TP.noOfTickets, TP.totalPrice, " +
                        "TP.clientName " +
                        "FROM TicketPurchases TP " +
                        "LIMIT 1;";

                    using (var result = cmd.ExecuteReader())
                    {
                        if (result.Read())
                        {
                            long tpId = result.GetInt64(0);
                            long gameId = result.GetInt64(1);
                            int noOfTickets = result.GetInt32(2);
                            float totalPrice = result.GetFloat(3);
                            String clientName = result.GetString(4);

                            Game game = null;

                            using (IDbCommand cmd1 = con.CreateCommand())
                            {
                                cmd1.CommandText = "SELECT G.id, G.description, G.totalNoOfTickets, G.soldTickets, " +
                                        "G.pricePerTicket, G.date, T1.id, T1.name, T2.id, T2.name " +
                                        "FROM Games G " +
                                        "INNER JOIN Teams T1 on G.team1Id = T1.id " +
                                        "INNER JOIN Teams T2 on G.team2Id = T2.id " +
                                        "WHERE G.id = @id;";
                                IDbDataParameter paramId = cmd1.CreateParameter();
                                paramId.ParameterName = "@id";
                                paramId.Value = gameId;
                                cmd1.Parameters.Add(paramId);

                                using (var result1 = cmd1.ExecuteReader())
                                {
                                    if (result1.Read())
                                    {
                                        long idG = result1.GetInt64(0);
                                        String description = result1.GetString(1);
                                        int totalNoOfTickets = result1.GetInt32(2);
                                        int soldTickets = result1.GetInt32(3);
                                        float pricePerTicket = result1.GetFloat(4);
                                        DateTime date = result1.GetDateTime(5);
                                        long team1Id = result1.GetInt64(6);
                                        String team1Name = result1.GetString(7);
                                        long team2Id = result1.GetInt64(8);
                                        String team2Name = result1.GetString(9);
                                        Team team1 = new Team(team1Name);
                                        team1.Id = team1Id;
                                        Team team2 = new Team(team2Name);
                                        team2.Id = team2Id;
                                        game = new Game(team1, team2, date, description, totalNoOfTickets, soldTickets, pricePerTicket);
                                        game.Id = gameId;
                                    }
                                }
                            }
                            tp = new TicketPurchase(game, noOfTickets, totalPrice, clientName);
                            tp.Id = tpId;
                        }
                    }
                }
            }
            catch(SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error DB " + ex.Message);
            }
            logger.InfoFormat("exit findById");
            return tp;
        }


        public void update(TicketPurchase elem, long id)
        {
            logger.Info($"updating ticket purchase {elem}");
            IDbConnection con = dbUtils.GetConnection();

            try
            {
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE TicketPurchases SET gameId = @gameId, noOfTickets = @noOfTickets," +
                    "totalPrice = @totalPrice, clientName = @clientName WHERE id = @id;";

                    IDbDataParameter paramGameId = cmd.CreateParameter();
                    paramGameId.ParameterName = "@gameId";
                    paramGameId.Value = elem.Game.Id;
                    cmd.Parameters.Add(paramGameId);

                    IDbDataParameter paramNoOfTickets = cmd.CreateParameter();
                    paramNoOfTickets.ParameterName = "@noOfTickets";
                    paramNoOfTickets.Value = elem.NoOfTickets;
                    cmd.Parameters.Add(paramNoOfTickets);

                    IDbDataParameter paramTotalPrice = cmd.CreateParameter();
                    paramTotalPrice.ParameterName = "@totalPrice";
                    paramTotalPrice.Value = elem.TotalPrice;
                    cmd.Parameters.Add(paramTotalPrice);

                    IDbDataParameter paramClientName = cmd.CreateParameter();
                    paramClientName.ParameterName = "@clientName";
                    paramClientName.Value = elem.ClientName;
                    cmd.Parameters.Add(paramClientName);

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
