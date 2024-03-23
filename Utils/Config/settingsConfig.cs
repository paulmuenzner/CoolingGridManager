
using Serilog;


public static class ConfigurationHelper
{

    // Retrieve information about environment setting (eg. Development or Production)
    public static string GetEnvironment()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(env))
        {
            Log.Logger.Error("ASPNETCORE_ENVIRONMENT is not set.");
            throw new InvalidOperationException("ASPNETCORE_ENVIRONMENT is not set.");
        }
        else
        {
            return env; // Missing semicolon was added here
        }
    }

    // Load configuration based on environment
    public static IConfiguration LoadConfiguration()
    {
        var environment = GetEnvironment();
        var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());

        if (environment == "Development")
        {
            configurationBuilder.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
        }
        else if (environment == "Production")
        {
            configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
        else
        {
            Log.Logger.Error("Unknown environment: {environment}.", environment);
            throw new InvalidOperationException("Unknown environment.");
        }

        var configuration = configurationBuilder.Build();

        return configuration;
    }

    // Retrieve database connection string from settings
    public static string GetDatabaseConnectionString()
    {
        // Retrieve environment information
        var connectionString = LoadConfiguration().GetConnectionString("DefaultConnection");

        // Check if the connection string is null
        if (connectionString == null)
        {
            // Handle the case where the connection string is null
            // Stop program in this case!
            Log.Logger.Error("DefaultConnection connection string is not found in the configuration.");
            throw new InvalidOperationException("DefaultConnection connection string is not found in the configuration.");
        }

        return connectionString;
    }
}