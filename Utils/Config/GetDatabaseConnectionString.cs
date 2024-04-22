
using CoolingGridManager.Exceptions;
using ILogger = Serilog.ILogger;

public class DatabaseConnection
{
    private readonly ILogger _logger;
    public DatabaseConnection(ILogger logger)
    {
        _logger = logger;
    }

    // Retrieve database connection string from settings
    public string GetDatabaseConnectionString()
    {
        try
        {
            // Retrieve environment information
            var envConfig = new Settings(_logger);
            var setting = envConfig.LoadSettings();
            var connectionString = setting.GetConnectionString("DefaultConnection");

            // Check if the connection string is null
            if (connectionString == null)
            {
                // Handle the case where the connection string is null
                // Stop program in this case!
                _logger.Error("DefaultConnection connection string is not found in the configuration.");
                throw new InvalidOperationException("DefaultConnection connection string is not found in the configuration.");
            }

            return connectionString;
        }
        catch (Exception ex)
        {
            string message = string.Format("Exception: {ex}", ex.ToString());
            _logger.Error(ex, message);
            throw new TryCatchException(message, "GetDatabaseConnectionString");
        }
    }
}