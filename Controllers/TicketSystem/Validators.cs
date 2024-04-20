using Azure.Core;
using CoolingGridManager.IRequests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;



namespace CoolingGridManager.Validators.Tickets
{

    // Validation helper
    public static class TicketValidatorHelper
    {
        public static bool BeValidStatus(string description)
        {
            return description == "open" || description == "solved" || description == "onhold";
        }

        public static bool BeValidPriority(string description)
        {
            return description == "high" || description == "medium" || description == "low";
        }

        public static bool BeValidCategory(string description)
        {
            return description == "billing" || description == "technical" || description == "miscellaneous";
        }
    }

    // Get Ticket Validator
    public class GetTicketByIdValidator : AbstractValidator<int>
    {
        // Get Ticket Validator
        public GetTicketByIdValidator()
        {
            RuleFor(request => request)
               .NotEmpty().WithMessage("Valid ticket ID must be provided.")
               .NotNull().WithMessage("Valid ticket ID must be provided.")
               .GreaterThan(0).WithMessage("Ticket ID must be greater than 0.");
        }
    }

    // Add ticket validation
    public class CreateTicketValidator : AbstractValidator<ICreateTicketRecordRequest>
    {
        // Add ticket validation
        public CreateTicketValidator()
        {
            RuleFor(ticketModel => ticketModel.ReportedBy)
               .NotEmpty().WithMessage("Name for reporting person is required.")
               .Length(3, 100).WithMessage("Name for reporting person must be between 3 and 100 characters.")
               .Matches("^[a-zA-Z ]+$").WithMessage("Name can only contain alphabetical characters.");

            RuleFor(ticketModel => ticketModel.Responsible)
               .NotEmpty().WithMessage("Name of responsible person is required.")
               .Length(3, 100).WithMessage("Name of responsible person must be between 3 and 100 characters.")
               .Matches("^[a-zA-Z]+$").WithMessage("Name of responsible person can only contain alphabetical characters.");

            RuleFor(ticketModel => ticketModel.Description)
               .NotEmpty().WithMessage("Description is required.")
               .Length(50, 1000).WithMessage("Description must be between 50 and 1000 characters.")
               .Matches("^[a-zA-Z0-9., -]+$").WithMessage("Description can only contain alphanumeric characters, '.', ',', and '-'.");

            RuleFor(ticketModel => ticketModel.Title)
               .NotEmpty().WithMessage("Title is required.")
               .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.")
               .Matches("^[a-zA-Z0-9., -]+$").WithMessage("Title can only contain alphanumeric characters, '.', ',', and '-'.");

            RuleFor(ticketModel => ticketModel.Category)
               .NotEmpty().WithMessage("Category is required.")
               .Must(TicketValidatorHelper.BeValidCategory).WithMessage("Invalid category value. Can only be technical, billing or miscellaneous.");

            RuleFor(ticketModel => ticketModel.Priority)
                .NotEmpty().WithMessage("Priority is required.").Must(BeValidPriority)
                .WithMessage("Invalid priority value. Can only be high, medium or low.");

            RuleFor(ticketModel => ticketModel.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(TicketValidatorHelper.BeValidStatus).WithMessage("Invalid status value. Can only be open, solved or onhold.");
        }

        private bool BeValidStatus(string description)
        {
            return description == "open" || description == "solved" || description == "onhold";
        }

        private bool BeValidPriority(string description)
        {
            return description == "high" || description == "medium" || description == "low";
        }

        private bool BeValidCategory(string description)
        {
            return description == "billing" || description == "technical" || description == "miscellaneous";
        }
    }

    //////////////////////////////////////
    // Update ticket validation
    public class UpdateTicketStatusValidator : AbstractValidator<IUpdateTicketStatusRequest>
    {
        private readonly AppDbContext _context;
        public UpdateTicketStatusValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(request => request.TicketID)
                .NotEmpty().WithMessage("Ticket ID is required.")
                .GreaterThan(0).WithMessage("Ticket ID must be greater than 0.")
                .MustAsync(ExistingTicket).WithMessage("Requested ticket not found.");

            RuleFor(request => request.NewStatus)
                .NotEmpty().WithMessage("Status is required.")
                .Must(TicketValidatorHelper.BeValidStatus).WithMessage("Invalid status value. Can only be open, solved or onhold.")
                .MustAsync(StatusAlreadySet).WithMessage("Ticket status already set.");
        }


        private async Task<bool> ExistingTicket(int ticketId, CancellationToken cancellationToken)
        {
            var existingTicket = await _context.Tickets.FindAsync(new object[] { ticketId }, cancellationToken);
            return existingTicket != null;
        }

        // Validate if the requested status is equal and has been set already
        private async Task<bool> StatusAlreadySet(IUpdateTicketStatusRequest request, string newStatus, CancellationToken cancellationToken)
        {
            var existingTicketStatus = await _context.Tickets
                .Where(t => t.TicketId == request.TicketID)
                .Select(t => t.Status)
                .SingleOrDefaultAsync(cancellationToken);

            return existingTicketStatus != request.NewStatus;
        }

    }

}