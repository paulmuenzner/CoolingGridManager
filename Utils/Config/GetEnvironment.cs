
using CoolingGridManager.Exceptions;
using ILogger = Serilog.ILogger;


public class EnvConfig
{
    private readonly ILogger _logger;
    public EnvConfig(ILogger logger)
    {
        _logger = logger;
    }

    // Retrieve information about environment setting (eg. Development or Production)
    public string GetEnvironment()
    {
        try
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(env))
            {
                _logger.Error("ASPNETCORE_ENVIRONMENT is not set.");
                throw new InvalidOperationException("ASPNETCORE_ENVIRONMENT is not set.");
            }
            else
            {
                return env;
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.Error(ex, "Invalid operation occurred while retrieving environment.");
            throw;
        }
        catch (Exception ex)
        {
            string message = string.Format("Exception: {ex}", ex.ToString());
            _logger.Error(ex, message);
            throw new TryCatchException(message, "GetEnvironment");
        }
    }
}

