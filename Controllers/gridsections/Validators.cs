using FluentValidation;
using CoolingGridManager.Models.Data;


namespace CoolingGridManager.Validators.GridSections
{

    // Add Grid Section Validator
    public class AddGridSectionValidator : AbstractValidator<GridSection>
    {
        private readonly AppDbContext _context;
        public AddGridSectionValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(gridSection => gridSection.GridSectionName)
               .NotEmpty().WithMessage("Grid section name is required.")
               .Length(3, 100).WithMessage("Grid section name length must be between 3 and 100 characters.")
               .Matches("^[a-zA-Z0-9 ]+$").WithMessage("Grid section name can only contain alphanumeric characters.");
            // Prepare if already existing, instead of service validation ?

            RuleFor(gridSection => gridSection.GridID)
                .NotEmpty().WithMessage("Valid grid ID must be provided.")
                .NotNull().WithMessage("Valid grid ID must be provided.")
                .GreaterThan(0).WithMessage("Grid ID must be greater than 0.")
                .MustAsync(async (gridId, cancellationToken) =>
            {
                var existingGrid = await _context.Grids.FindAsync(gridId);
                return existingGrid != null;
            })
            .WithMessage("Related grid does not exist.");
        }

    }
}