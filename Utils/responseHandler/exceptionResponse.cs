using Serilog;
using Microsoft.AspNetCore.Mvc;


public enum ExceptionType
{
    NotFound,
    Unauthorized,
    BadRequest, Format, Argument,
    General
}

public class ExceptionResponse : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public ExceptionResponse(IWebHostEnvironment env)
    {
        _env = env;
    }

    public ActionResult ExceptionResponseHandle(Exception ex, string internalMessage, string officialMessage, ExceptionType exception)
    {
        var exceptionInformation = ExceptionParser.ParseException(ex);
        var internalResponseMessage = new { exception = exceptionInformation, InternalMessage = internalMessage, OfficialMessage = officialMessage };
        if (_env.IsDevelopment())
        {
            switch (exception)
            {
                case ExceptionType.NotFound:
                    Log.Warning("Exception: " + internalResponseMessage);
                    return NotFound(internalResponseMessage);
                case ExceptionType.Unauthorized:
                    Log.Warning("Exception: " + internalResponseMessage);
                    return Unauthorized(internalResponseMessage);
                case ExceptionType.BadRequest:
                case ExceptionType.Format:
                case ExceptionType.Argument:
                    Log.Warning("Exception: " + internalResponseMessage);
                    return BadRequest(internalResponseMessage);
                case ExceptionType.General:
                    Log.Warning("Exception: " + internalResponseMessage);
                    return StatusCode(500, new
                    {
                        message = internalResponseMessage
                    });
                default:
                    return StatusCode(500, new { message = internalResponseMessage });
            }
        }
        else
        {
            switch (exception)
            {
                case ExceptionType.NotFound:
                    Log.Warning("Exception: " + internalResponseMessage);
                    return NotFound(officialMessage);
                case ExceptionType.Unauthorized:
                    Log.Warning("Exception: " + internalResponseMessage);
                    return Unauthorized(officialMessage);
                case ExceptionType.BadRequest:
                case ExceptionType.Format:
                case ExceptionType.Argument:
                    Log.Warning("Exception: " + internalResponseMessage);
                    return BadRequest(officialMessage);
                case ExceptionType.General:
                    Log.Warning("Exception: " + internalResponseMessage);
                    return StatusCode(500, new { message = officialMessage });
                default:
                    Log.Warning("Exception: " + internalResponseMessage);
                    return StatusCode(500, new { message = officialMessage });
            }
        }
    }
}


