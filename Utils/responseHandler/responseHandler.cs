using Microsoft.AspNetCore.Mvc;

namespace CoolingGridManager.ResponseHandler
{
    public enum HttpStatus
    {
        OK = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        BadRequest = 400
    }

    public static class ResponseFormatter
    {
        public static IActionResult FormatSuccessResponse(HttpStatus statusCode, object data, string message)
        {
            switch (statusCode)
            {
                case HttpStatus.OK:
                    return new OkObjectResult(new { StatusCode = (int)statusCode, Message = message, Data = data });
                case HttpStatus.Created:
                    return new CreatedResult("", new { StatusCode = (int)statusCode, Message = message, Data = data });
                case HttpStatus.Accepted:
                    return new AcceptedResult("", new { StatusCode = (int)statusCode, Message = message, Data = data });
                case HttpStatus.BadRequest:
                    return new BadRequestObjectResult(new { StatusCode = (int)statusCode, Message = message, Data = data });
                case HttpStatus.NoContent:
                    return new NoContentResult();
                default:
                    return new ObjectResult(new { StatusCode = (int)statusCode, Message = message, Data = data })
                    {
                        StatusCode = (int)statusCode
                    };
            }
        }
    }
}
