using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Validators.Tickets;
using CoolingGridManager.ResponseHandler;
using FluentValidation.Results;
using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;



namespace CoolingGridManager.Controllers.TicketsController
{
    [Area("tickets")]
    [Route("api/tickets/[controller]")]
    public partial class UpdateStatusController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly UpdateTicketStatusValidator _updateTicketStatusValidator;
        public UpdateStatusController(UpdateTicketStatusValidator updateTicketStatusValidator, TicketService ticketService)
        {
            _ticketService = ticketService;
            _updateTicketStatusValidator = updateTicketStatusValidator;
        }

        [HttpPost]
        [Tags("Tickets")]
        public async Task<IActionResult> UpdateStatus([FromBody] IUpdateTicketStatusRequest request)
        {
            try
            {
                // Validate
                ValidationResult result = await _updateTicketStatusValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                // Update the ticket in the database
                TicketModel update = await _ticketService.UpdateTicketStatus(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { TicketUpdate = update }, $"Ticket status updated to '{update.Status}'.");
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Update currently not possible.", $"An error occurred while marking the ticket as solved: {ex.Message}", ex);
            }
        }

    }
}