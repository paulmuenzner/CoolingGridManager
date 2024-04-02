using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Validators.Tickets;
using CoolingGridManager.ResponseHandler;
using FluentValidation.Results;
using CoolingGridManager.Models.Requests;



namespace CoolingGridManager.Controllers.TicketsController
{
    [Area("tickets")]
    [Route("api/tickets/[controller]")]
    public partial class UpdateStatusController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly Serilog.ILogger _logger;

        private readonly AppDbContext _context;
        public UpdateStatusController(AppDbContext context, ExceptionResponse exceptionResponse, Serilog.ILogger logger, TicketService ticketService)
        {
            _ticketService = ticketService;
            _exceptionResponse = exceptionResponse;
            _logger = logger;
            _context = context;

        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateTicketStatusRequest ticketStatusRequest)
        {
            try
            {
                // Validate
                UpdateTicketStatusValidator validator = new UpdateTicketStatusValidator(_context);
                ValidationResult result = await validator.ValidateAsync(ticketStatusRequest);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                // Update the ticket in the database
                var updatedTicket = await _ticketService.UpdateStatusTicket(ticketStatusRequest.TicketId, ticketStatusRequest.Status);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { TicketUpdate = updatedTicket }, $"Ticket status updated to '{ticketStatusRequest.Status}'.");
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Update currently not possible.", $"An error occurred while marking the ticket as solved: {ex.Message}", ex);
            }
        }

    }
}