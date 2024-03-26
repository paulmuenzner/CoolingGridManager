using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Models;
using CoolingGridManager.ResponseHandler;


namespace CoolingGridManager.Controllers.TicketsController
{
    [Area("tickets")]
    [Route("api/tickets/[controller]")]
    public partial class UpdateStatusController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly Serilog.ILogger _logger;
        public UpdateStatusController(ExceptionResponse exceptionResponse, Serilog.ILogger logger, TicketService ticketService)
        {
            _ticketService = ticketService;
            _exceptionResponse = exceptionResponse;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] TicketSolveRequest request)
        {
            try
            {
                if (request == null || request.TicketId == 0 || request.Status == "")
                {
                    return BadRequest("Invalid request. Ticket ID and a valid status is required.");
                }

                var ticket = await _ticketService.GetTicketById(request.TicketId);

                if (ticket == null)
                {
                    return NotFound($"Ticket with ID {request.TicketId} not found.");
                }

                // Update the ticket in the database
                var updatedTicket = await _ticketService.UpdateStatusTicket(request.TicketId, request.Status);

                return ResponseFormatter.FormatSuccessResponse(HttpStatus.OK, new { TicketUpdate = updatedTicket }, $"Ticket status updated to '{request.Status}'.");
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, $"An error occurred while marking the ticket as solved: {ex.Message}", "Update currently not possible.", ExceptionType.General);
            }
        }
        public class TicketSolveRequest
        {
            public required int TicketId { get; set; }
            public required string Status { get; set; }
        }
    }
}