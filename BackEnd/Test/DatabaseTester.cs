using Microsoft.Data.SqlClient;
using log4net;

namespace SportingStatsBackEnd.Test
{
    public static class DatabaseTester
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DatabaseTester));
        
        public static void TestDatabaseConnection(string connectionString)
        {
            try
            {
                logger.Info("Attempting to connect to the database...");
                logger.Info($"Connection String being passed is: {connectionString}");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    logger.Info("Database connection successful!");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Database connection failed.", ex);
            }
        }
    }
}
