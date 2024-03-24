using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public enum ExceptionType
{
    NotFound,
    Unauthorized,
    General
}

public class ExceptionResponse : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public ExceptionResponse(IWebHostEnvironment env)
    {
        _env = env;
    }

    public ActionResult ExceptionResponseHandle(string officialResponse, string internalResponse, ExceptionType exception)
    {
        if (_env.IsDevelopment())
        {
            switch (exception)
            {
                case ExceptionType.NotFound:
                    Log.Warning("Exception: " + officialResponse);
                    return NotFound(officialResponse);
                case ExceptionType.Unauthorized:
                    Log.Warning("Exception: " + officialResponse);
                    return Unauthorized("Exception: " + officialResponse);
                case ExceptionType.General:
                    Log.Warning("Exception: " + officialResponse);
                    return StatusCode(500, new { message = officialResponse });
                default:
                    return StatusCode(500, new { message = officialResponse });
            }
        }
        else
        {
            switch (exception)
            {
                case ExceptionType.NotFound:
                    Log.Warning("Exception: " + officialResponse);
                    return NotFound(internalResponse);
                case ExceptionType.Unauthorized:
                    Log.Warning("Exception: " + officialResponse);
                    return Unauthorized(internalResponse);
                case ExceptionType.General:
                    Log.Warning("Exception: " + officialResponse);
                    return StatusCode(500, new { message = internalResponse });
                default:
                    return StatusCode(500, new { message = internalResponse });
            }
        }
    }
}


