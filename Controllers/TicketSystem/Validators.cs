using System.Text.RegularExpressions;
using CoolingGridManager.Controllers.TicketsController;
using CoolingGridManager.Models;
using FluentValidation;



namespace CoolingGridManager.Validators.Tickets
{

    // Get Ticket Validator
    public class TicketGetByIdValidator : AbstractValidator<TicketRequest>
    {
        // Get Ticket Validator
        public TicketGetByIdValidator()
        {
            RuleFor(ticketRequest => ticketRequest.TicketId)
               .NotEmpty().WithMessage("Valid ticket ID must be provided.")
               .NotNull().WithMessage("Valid ticket ID must be provided.")
               .GreaterThan(0).WithMessage("Ticket ID must be greater than 0.");
        }
    }

    // Add ticket validation
    public class TicketAddValidator : AbstractValidator<TicketModel>
    {
        // Add ticket validation
        public TicketAddValidator()
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
               .Must(BeValidCategory).WithMessage("Invalid category value. Can only be technical, billing or miscellaneous.");

            RuleFor(ticketModel => ticketModel.Priority)
                .NotEmpty().WithMessage("Priority is required.").Must(BeValidPriority)
                .WithMessage("Invalid priority value. Can only be high, medium or low.");

            RuleFor(ticketModel => ticketModel.Status)
                .NotEmpty().WithMessage("Status is required.").Must(BeValidStatus)
                .WithMessage("Invalid status value. Can only be open, solved or onhold.");
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
}