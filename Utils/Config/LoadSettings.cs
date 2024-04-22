
using CoolingGridManager.Exceptions;
using Serilog;
using ILogger = Serilog.ILogger;

public class Settings
{
    private readonly ILogger _logger;
    public Settings(ILogger logger)
    {
        _logger = logger;
    }

    // Load configuration based on environment
    public IConfiguration LoadSettings()
    {
        try
        {
            ILogger logger = Log.Logger;
            var envConfig = new EnvConfig(logger);
            var environment = envConfig.GetEnvironment();

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
                _logger.Error("Unknown environment: {environment}.", environment);
                throw new InvalidOperationException("Unknown environment.");
            }

            var configuration = configurationBuilder.Build();

            return configuration;
        }
        catch (Exception ex)
        {
            string message = string.Format("Exception: {ex}", ex.ToString());
            _logger.Error(ex, message);
            throw new TryCatchException(message, "GetEnvironment");
        }
    }
}