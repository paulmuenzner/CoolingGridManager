using FluentValidation;
using CoolingGridManager.Models.Requests;
using CoolingGridManager.Models.Data;


namespace CoolingGridManager.Validators.Consumers
{

    // Add Consumer Validator
    public class AddConsumerValidator : AbstractValidator<Consumer>
    {
        private readonly AppDbContext _context;
        public AddConsumerValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(consumer => consumer.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Matches(@"^\p{L}+$").WithMessage("First name must contain only letters.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

            RuleFor(consumer => consumer.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Matches(@"^\p{L}+$").WithMessage("Last name must contain only letters.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

            RuleFor(consumer => consumer.CompanyName)
                .NotEmpty().WithMessage("Company name is required.")
                .Matches(@"^[\p{L}\.,']+").WithMessage("Company name can only contain letters, points, commas, and single quotes.")
                .Length(2, 50).WithMessage("Company name must be between 2 and 50 characters.");

            RuleFor(consumer => consumer.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Length(6, 30).WithMessage("Email address number must be between 6 and 30 characters.");

            RuleFor(consumer => consumer.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d+$").WithMessage("Phone number must contain only numbers.")
                .Length(5, 18).WithMessage("Phone number must be between 5 and 18 characters.");

            RuleFor(consumer => consumer.GridSectionID)
                .NotEmpty().WithMessage("Grid section ID is required.")
                .GreaterThan(0).WithMessage("Grid Section ID must be greater than 0.")
                .MustAsync(async (gridSectionId, cancellationToken) =>
            {
                var existingGrid = await _context.GridSections.FindAsync(gridSectionId);
                return existingGrid != null;
            })
            .WithMessage("Declared grid section does not exist.");
        }

    }
}