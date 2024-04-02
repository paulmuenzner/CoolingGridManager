using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Exceptions;
using FluentValidation.Results;
using CoolingGridManager.Validators.Tickets;
using CoolingGridManager.Models.Requests;

namespace CoolingGridManager.Controllers.TicketsController
{
    [Area("tickets")]
    [Route("api/tickets/[controller]")]
    public partial class GetTicketByIdController : ControllerBase
    {
        private readonly TicketGetByIdValidator _ticketGetByIdValidator;
        private readonly TicketService _ticketService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly Serilog.ILogger _logger;
        public GetTicketByIdController(ExceptionResponse exceptionResponse, TicketGetByIdValidator ticketGetByIdValidator, Serilog.ILogger logger, TicketService ticketService)
        {
            _ticketService = ticketService;
            _ticketGetByIdValidator = ticketGetByIdValidator;
            _exceptionResponse = exceptionResponse;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetTicketById([FromBody] GetTicketByIDRequest ticketRequest)
        {
            try
            {
                // Validate
                TicketGetByIdValidator validator = new();
                ValidationResult result = validator.Validate(ticketRequest);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }
                if (!ticketRequest.TicketId.HasValue)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Ticket ID not valid. Valid ticket ID must be provided.", "No ticket found.", null);
                }

                var ticket = await _ticketService.GetTicketById(ticketRequest.TicketId.Value);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Ticket = ticket }, $"Ticket with ID {ticketRequest.TicketId} found.");
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "ArgumentNullException. No valid ticket ID registered.", "No ticket found.", ex);
            }
            catch (NotFoundException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, $"NotFoundException. No ticket found for ID {ticketRequest.TicketId}", $"No ticket found for ID {ticketRequest.TicketId}", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "Exception. No valid ticket ID found.", "No ticket found.", ex);
            }
        }

    }

}