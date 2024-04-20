using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Models.Data;
using CoolingGridManager.ResponseHandler;
using FluentValidation.Results;
using CoolingGridManager.Validators.Tickets;
using CoolingGridManager.IRequests;


namespace CoolingGridManager.Controllers.TicketsController
{
    [Area("tickets")]
    [Route("api/tickets/[controller]")]
    public partial class AddTicketController : ControllerBase
    {
        private readonly CreateTicketValidator _createTicketValidator;
        private readonly TicketService _ticketService;
        private readonly Serilog.ILogger _logger;
        public AddTicketController(CreateTicketValidator createTicketValidator, Serilog.ILogger logger, TicketService ticketService)
        {
            _ticketService = ticketService;
            _createTicketValidator = createTicketValidator;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> AddTicket([FromBody] ICreateTicketRecordRequest request)
        {
            try
            {
                // Validate
                ValidationResult result = _createTicketValidator.Validate(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                // Retrieve data
                TicketModel newTicket = await _ticketService.CreateTicketRecord(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Ticket = newTicket }, $"New ticket with id {newTicket.TicketId} added");
            }
            catch (FormatException ex)
            {
                _logger.Error(ex, "FormatException occurred while adding a ticket. Ticket: {@ticket}", request);
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "Check the format of the input data.", "Input string was not in the correct format.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "An unexpected error occurred.", "Adding new ticket currently not possible.", ex);
            }

        }

    }
}