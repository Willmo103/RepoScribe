using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using RepoScribe.Core.Utilities;
using Serilog;

namespace RepoScribe.Core.Services
{
    public class LocalDatabaseService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public LocalDatabaseService(IConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
            // get the environment variable for Environment
            string environment = Environment.GetEnvironmentVariable("Environment") ?? "DEV";
            _logger.Information($"Environment: {environment}");
            // set the connection string based on the environment
            if (environment == "PROD")
            {
                _connectionString = _config.GetConnectionString("ProdDatabase") ?? throw new ArgumentNullException("Connection string is null or empty");
            }
            else
            {
                _connectionString = _config.GetConnectionString("DevDatabase") ?? throw new ArgumentNullException("Connection string is null or empty");
            }
            if (string.IsNullOrEmpty(_connectionString))
            {
                _logger.Error("Connection string is null or empty");
                throw new ArgumentNullException("Connection string is null or empty");
            }
        }


        public Boolean TestConnection()
        {
            if (_connectionString.Contains("Data Source"))
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                _logger.Information("Connection to the database was successful");
                return true;

            }
            else
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                _logger.Information("Connection to the database was successful");
                return true;
            }
        }

        // Example method to open a connection
        public void OpenConnection()
        {
            try
            {
                if (_connectionString.Contains("Data Source"))
                {
                    using var connection = new SqliteConnection(_connectionString);
                    connection.Open();
                    // Perform database operations
                }
                else
                {
                    using var connection = new NpgsqlConnection(_connectionString);
                    connection.Open();
                    // Perform database operations
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error opening connection to the database");
            }
        }

        public void InitializeDatabase()
        {
            if (_connectionString.Contains("Data Source"))
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                // Perform database operations


                // Create the table if it does not exist
                // Run migrations from DevDatabase to ProdDatabase
                // etc... 
            }
            else
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                // Perform database operations

                // Create the table if it does not exist
                // Run migrations from DevDatabase to ProdDatabase
                // etc... 
            }

            while (!TestConnection())
            {
                Thread.Sleep(1000);


            }
        }
    }
}
