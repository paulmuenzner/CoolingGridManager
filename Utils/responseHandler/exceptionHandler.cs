using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public interface IExceptionHandlingService
{
    IActionResult HandleException(Exception ex, string internalMessage, string officialMessage, ExceptionType exceptionType);
}

public class ExceptionHandlingService : IExceptionHandlingService
{
    private readonly ExceptionResponse _exceptionResponse;

    public ExceptionHandlingService(ExceptionResponse exceptionResponse)
    {
        _exceptionResponse = exceptionResponse;
    }

    public IActionResult HandleException(Exception ex, string internalMessage, string officialMessage, ExceptionType exceptionType)
    {
        if (ex is System.FormatException formatException)
        {
            return _exceptionResponse.ExceptionResponseHandle(formatException, internalMessage, officialMessage, exceptionType);
        }
        else if (ex is DbUpdateException dbUpdateException)
        {
            if (dbUpdateException.InnerException is Npgsql.PostgresException postgresException)
            {
                return _exceptionResponse.ExceptionResponseHandle(postgresException, internalMessage, officialMessage, exceptionType);
            }
            return _exceptionResponse.ExceptionResponseHandle(dbUpdateException, internalMessage, officialMessage, exceptionType);
        }
        else
        {
            return _exceptionResponse.ExceptionResponseHandle(ex, internalMessage, officialMessage, exceptionType);
        }
    }
}
