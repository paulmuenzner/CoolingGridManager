using CoolingGridManager.IRequests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;



namespace CoolingGridManager.Validators.Grids
{

    // Add Grid Validator
    public class CreateGridValidator : AbstractValidator<ICreateGridRequest>
    {
        private readonly AppDbContext _context;
        public CreateGridValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(grid => grid.GridName)
               .NotEmpty().WithMessage("Grid name is required.")
               .Length(3, 100).WithMessage("Grid name length must be between 3 and 100 characters.")
               .Matches("^[a-zA-Z0-9 ]+$").WithMessage("Grid name can only contain alphanumeric characters.")
               .MustAsync(async (gridName, cancellationToken) =>
               {
                   var existingGrid = await _context.Grids.FirstOrDefaultAsync(g => g.GridName == gridName);
                   return existingGrid == null;
               })
               .WithMessage("Grid name already existing. Choose another name.");
        }
    }
}