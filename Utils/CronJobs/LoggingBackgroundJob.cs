using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure;

[DisallowConcurrentExecution]
public class LoggingBackgroundJob : IJob
{
    // private readonly ILogger<LoggingBackgroundJob> _logger;
    private readonly Serilog.ILogger _logger;

    public LoggingBackgroundJob(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.Information("{UtcNow}", DateTime.UtcNow);

        return Task.CompletedTask;
    }
}