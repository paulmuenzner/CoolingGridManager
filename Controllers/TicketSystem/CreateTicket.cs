using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Models.Data;
using CoolingGridManager.ResponseHandler;
using FluentValidation.Results;
using CoolingGridManager.Validators.Tickets;
using CoolingGridManager.IRequests;
using Microsoft.AspNetCore.RateLimiting;


namespace CoolingGridManager.Controllers.TicketsController
{
    [Area("tickets")]
    [Route("api/tickets/[controller]")]
    [EnableRateLimiting("fixed")]
    public partial class CreateController : ControllerBase
    {
        private readonly CreateTicketValidator _createTicketValidator;
        private readonly TicketService _ticketService;
        public CreateController(CreateTicketValidator createTicketValidator, TicketService ticketService)
        {
            _ticketService = ticketService;
            _createTicketValidator = createTicketValidator;
        }

        [HttpPost]
        [Tags("Tickets")]
        public async Task<IActionResult> CreateTicket([FromBody] ICreateTicketRecordRequest request)
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
                return ResponseFormatter.Success(HttpStatusPositive.Created, new { Ticket = newTicket }, $"New ticket with id {newTicket.TicketId} added");
            }
            catch (FormatException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "FormatException occurred while adding a ticket. Check the format of the input data.", "Input string was not in the correct format.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "An unexpected error occurred.", "Adding new ticket currently not possible.", ex);
            }

        }

    }
}