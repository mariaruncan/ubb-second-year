using System;
using System.Data;
using System.Data.SQLite;
using log4net;
using System.Configuration;

namespace Persistence.db
{
    public class DBUtils
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DBUtils));
        private IDbConnection instance = null;

        private IDbConnection getNewConnection()
        {
            logger.Info("enter get new connection");
            try
            {
                string connectionString = "Data Source=D:\\Facultate\\Semestrul 4\\MPP\\mpp-proiect-repository-mariaruncan\\proiect.db";
                Console.WriteLine(connectionString);
                return new SQLiteConnection(connectionString);
            }
            catch (SQLiteException ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine("Error getting connection " + ex.Message);
            }
            return null;
        }

        public IDbConnection GetConnection()
        {
            logger.Info("enter get connection");
            try
            {
                if (instance == null || instance.State == ConnectionState.Closed)
                {
                    instance = getNewConnection();
                    instance.Open();
                }
            }
            catch (SQLiteException e)
            {
                logger.Error(e.Message);
                Console.WriteLine("Error DB " + e);
            }
            logger.Info("exit get connection");
            return instance;
        }
    }
}
