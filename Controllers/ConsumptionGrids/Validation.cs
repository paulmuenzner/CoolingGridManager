using FluentValidation;
using CoolingGridManager.Models.Data;


namespace CoolingGridManager.Validators.ConsumptionGrids
{

    // Add Consumption Validator
    public class AddConsumptionGridValidator : AbstractValidator<ConsumptionGrid>
    {
        private readonly AppDbContext _context;
        public AddConsumptionGridValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(consumption => consumption.Month)
                .NotEmpty().WithMessage("Month is required.")
                .InclusiveBetween(1, 12).WithMessage("Valid month must be selected.");

            RuleFor(consumption => consumption.Year)
                .NotEmpty().WithMessage("Year value is required.")
                .InclusiveBetween(2020, 2040).WithMessage("Year must be between 2020 and 2040.");

            RuleFor(consumption => consumption.GridID)
                .NotEmpty().WithMessage("Valid grid ID must be provided.")
                .NotNull().WithMessage("Valid grid ID must be provided.")
                .GreaterThan(0).WithMessage("Grid ID must be greater than 0.")
                .MustAsync(async (gridID, cancellationToken) =>
            {
                // No usage of Grids Service here. Validation logic, business logic, and data access logic should ideally be separated. Using a service for data access within a validation rule may violate this principle, as it introduces data access logic into the validation layer.
                var existingGrid = await _context.Grids.FindAsync(gridID);
                return existingGrid != null;
            })
            .WithMessage("Requested grid does not exist.");
        }

    }
}