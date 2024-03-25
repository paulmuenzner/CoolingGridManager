using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

internal sealed class NotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<NotFoundExceptionHandler> _logger;
    private readonly string _developmentMessage;
    private readonly string _productionMessage;

    public NotFoundExceptionHandler(
        ILogger<NotFoundExceptionHandler> logger,
        string developmentMessage,
        string productionMessage)
    {
        _logger = logger;
        _developmentMessage = developmentMessage;
        _productionMessage = productionMessage;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFoundException)
        {
            return false;
        }

        _logger.LogError(
            notFoundException,
            "Exception occurred: {Message}",
            notFoundException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not Found",
            Detail = GetMessage(notFoundException)
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private string GetMessage(NotFoundException exception)
    {
        bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        return isDevelopment ? _developmentMessage : _productionMessage;
    }
}
