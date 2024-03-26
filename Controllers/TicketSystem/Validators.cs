using CoolingGridManager.Models;
using FluentValidation;
using static CoolingGridManager.Controllers.TicketsController.UpdateStatusController;



public class TicketSolveRequestValidator : AbstractValidator<TicketModel>
{

    public TicketSolveRequestValidator()
    {

        // RuleFor(request => request.TicketId)
        //     .NotNull().WithMessage("Ticket ID is required.")
        //     .GreaterThan(0).WithMessage("Ticket ID must be greater than 0.");

        RuleFor(ticketModel => ticketModel.Description)
            .NotEmpty().WithMessage("Status is required.");
    }
}