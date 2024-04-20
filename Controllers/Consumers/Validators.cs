using FluentValidation;
using CoolingGridManager.IRequests;
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

            RuleFor(consumer => consumer.MonthlyBaseFee)
                .NotEmpty().WithMessage("Monthly base fee for grid connection required.")
                .GreaterThanOrEqualTo(0).WithMessage("Grid connection is not for free. Monthly base fee must be greater than 0.")
                .Must(value => BeAValidPrecision(value, 2)).WithMessage("The monthly base fee must be rounded to a maximum of two decimal places.");

            RuleFor(consumer => consumer.UnitPrice)
                .NotEmpty().WithMessage("Unit price per kWh cooling energy required.")
                .GreaterThanOrEqualTo(0).WithMessage("Unit price per kWh cooling energy must be greater than 0.")
                .Must(value => BeAValidPrecision(value, 2)).WithMessage("The unit price for cooling energy per kWh must be rounded to a maximum of two decimal places.");

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

        private bool BeAValidPrecision(decimal value, int maxDecimalPlaces)
        {
            // Convert the decimal to a string
            var valueAsString = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            // Check if the string contains a decimal point
            if (valueAsString.Contains('.'))
            {
                // Get the number of digits after the decimal point
                var digitsAfterDecimalPoint = valueAsString.Split('.')[1].Length;

                // Return true if there are the maximum allowed or fewer digits after the decimal point, otherwise false
                return digitsAfterDecimalPoint <= maxDecimalPlaces;
            }

            return true;
        }

    }
}