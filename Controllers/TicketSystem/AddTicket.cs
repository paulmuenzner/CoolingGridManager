using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Models;
using CoolingGridManager.ResponseHandler;
using FluentValidation.Results;


namespace CoolingGridManager.Controllers.TicketsController
{
    [Area("tickets")]
    [Route("api/tickets/[controller]")]
    public partial class AddTicketController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly Serilog.ILogger _logger;
        public AddTicketController(ExceptionResponse exceptionResponse, Serilog.ILogger logger, TicketService ticketService)
        {
            _ticketService = ticketService;
            _exceptionResponse = exceptionResponse;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> AddTicket([FromBody] TicketModel ticket)
        {
            try
            {
                TicketSolveRequestValidator validator = new();
                ValidationResult result = validator.Validate(ticket);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.FormatSuccessResponse(HttpStatus.BadRequest, new { Error = error }, $"{error.ErrorMessage}");
                    }
                }
                var newTicket = await _ticketService.AddTicket(ticket);
                return ResponseFormatter.FormatSuccessResponse(HttpStatus.OK, new { Ticket = newTicket }, $"New ticket with id {newTicket.TicketId} added");
            }
            catch (FormatException ex)
            {
                // Log the exception details
                _logger.Error(ex, "FormatException occurred while adding a ticket. Ticket: {@ticket}", ticket);

                // Return a more specific error message
                return _exceptionResponse.ExceptionResponseHandle(ex, "Input string was not in the correct format.", "Check the format of the input data.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred.", "Adding new ticket currently not possible.", ExceptionType.General);
            }

        }

    }
}