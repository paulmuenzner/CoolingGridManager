using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Models.Data;
using CoolingGridManager.ResponseHandler;
using FluentValidation.Results;
using CoolingGridManager.Validators.Tickets;


namespace CoolingGridManager.Controllers.TicketsController
{
    [Area("tickets")]
    [Route("api/tickets/[controller]")]
    public partial class AddTicketController : ControllerBase
    {
        private readonly TicketAddValidator _ticketAddValidator;
        private readonly TicketService _ticketService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly Serilog.ILogger _logger;
        public AddTicketController(TicketAddValidator ticketAddValidator, ExceptionResponse exceptionResponse, Serilog.ILogger logger, TicketService ticketService)
        {
            _ticketService = ticketService;
            _ticketAddValidator = ticketAddValidator;
            _exceptionResponse = exceptionResponse;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> AddTicket([FromBody] TicketModel ticket)
        {
            try
            {
                // Validate
                TicketAddValidator validator = new();
                ValidationResult result = validator.Validate(ticket);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                // Retrieve data
                var newTicket = await _ticketService.AddTicket(ticket);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Ticket = newTicket }, $"New ticket with id {newTicket.TicketId} added");
            }
            catch (FormatException ex)
            {
                _logger.Error(ex, "FormatException occurred while adding a ticket. Ticket: {@ticket}", ticket);
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "Check the format of the input data.", "Input string was not in the correct format.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "An unexpected error occurred.", "Adding new ticket currently not possible.", ex);
            }

        }

    }
}