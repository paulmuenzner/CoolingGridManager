using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Exceptions;


namespace CoolingGridManager.Controllers.TicketsController
{
    [Area("tickets")]
    [Route("api/tickets/[controller]")]
    public partial class GetTicketByIdController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly Serilog.ILogger _logger;
        public GetTicketByIdController(ExceptionResponse exceptionResponse, Serilog.ILogger logger, TicketService ticketService)
        {
            _ticketService = ticketService;
            _exceptionResponse = exceptionResponse;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetTicketById([FromBody] TicketRequest ticketRequest)
        {
            try
            {
                if (ticketRequest == null || ticketRequest.TicketId == 0)
                {
                    return ResponseFormatter.FormatSuccessResponse(HttpStatus.BadRequest, new { }, $"Invalid request. Valid ticket ID is required.");
                }

                var ticket = await _ticketService.GetTicketById(ticketRequest.TicketId);

                return ResponseFormatter.FormatSuccessResponse(HttpStatus.OK, new { Ticket = ticket }, $"Ticket with ID {ticketRequest.TicketId} found.");
            }
            catch (NotFoundException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "No ticket found.", "No ticket found.", ExceptionType.NotFound);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred.", "Action currently not possible.", ExceptionType.General);
            }
        }

    }

    public class TicketRequest
    {
        public required int TicketId { get; set; }
    }
}