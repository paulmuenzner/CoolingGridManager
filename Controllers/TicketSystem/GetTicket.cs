using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Exceptions;
using FluentValidation.Results;
using CoolingGridManager.Validators.Tickets;
using CoolingGridManager.Models.Data;


namespace CoolingGridManager.Controllers.TicketsController
{
    [Area("tickets")]
    [Route("api/tickets/[controller]")]
    public partial class GetController : ControllerBase
    {
        private readonly GetTicketByIdValidator _getTicketByIdValidator;
        private readonly TicketService _ticketService;
        public GetController(GetTicketByIdValidator getTicketByIdValidator, TicketService ticketService)
        {
            _ticketService = ticketService;
            _getTicketByIdValidator = getTicketByIdValidator;
        }

        [HttpGet]
        [Tags("Tickets")]
        public async Task<IActionResult> GetTicket([FromBody] int request)
        {
            try
            {
                // Validate
                ValidationResult result = _getTicketByIdValidator.Validate(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                TicketModel ticket = await _ticketService.GetTicketDetails(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Ticket = ticket }, $"Ticket with ID {request} found.");
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "ArgumentNullException. No valid ticket ID registered.", "No ticket found.", ex);
            }
            catch (NotFoundException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, $"NotFoundException. No ticket found for ID {request}", $"No ticket found for ID {request}", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "Exception. No valid ticket ID found.", "No ticket found.", ex);
            }
        }

    }

}