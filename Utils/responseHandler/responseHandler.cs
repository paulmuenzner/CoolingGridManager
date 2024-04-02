using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CoolingGridManager.ResponseHandler
{
    public enum HttpStatusPositive
    {
        OK = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
    }

    /////////////////////////////////////
    // Fail
    public enum HttpStatusNegative
    {
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        UnprocessableEntity = 422, // Used for validation error
        InternalServerError = 500
    }

    public static class ResponseFormatter
    {
        public static IActionResult Success(HttpStatusPositive statusCode, object data, string message)
        {
            switch (statusCode)
            {
                case HttpStatusPositive.OK:
                    return new OkObjectResult(new { StatusCode = (int)statusCode, Message = message, Data = data });
                case HttpStatusPositive.Created:
                    return new CreatedResult("", new { StatusCode = (int)statusCode, Message = message, Data = data });
                case HttpStatusPositive.Accepted:
                    return new AcceptedResult("", new { StatusCode = (int)statusCode, Message = message, Data = data });
                case HttpStatusPositive.NoContent:
                    return new NoContentResult();
                default:
                    return new ObjectResult(new { StatusCode = (int)statusCode, Message = message, Data = data })
                    {
                        StatusCode = (int)statusCode
                    };
            }
        }


        public static IActionResult Negative(HttpStatusNegative statusCode, object data, string internalMessage, string officialMessage, Exception? ex)
        {
            object? exceptionInformation = null; // Specify the type explicitly
            if (ex != null)
            {
                exceptionInformation = ExceptionParser.ParseException(ex);
            }
            var internalResponseMessage = new { exception = exceptionInformation, InternalMessage = internalMessage, OfficialMessage = officialMessage };
            switch (statusCode)
            {
                case HttpStatusNegative.BadRequest:
                    Log.Warning("BadRequest! Internal Message: " + internalResponseMessage);
                    return new BadRequestObjectResult(new { StatusCode = (int)statusCode, Message = internalResponseMessage, Data = data });

                default:
                    return new BadRequestObjectResult(new { StatusCode = (int)statusCode, Message = internalResponseMessage, Data = data })
                    {
                        StatusCode = (int)statusCode
                    };
            }
        }
    }
}
