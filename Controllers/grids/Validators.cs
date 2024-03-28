using CoolingGridManager.Controllers.TicketsController;
using FluentValidation;



namespace CoolingGridManager.Validators.Grids
{

    // Add Grid Validator
    public class AddGridValidator : AbstractValidator<GridModel>
    {
        public AddGridValidator()
        {
            RuleFor(grid => grid.GridName)
               .NotEmpty().WithMessage("Grid name is required.")
               .Length(3, 100).WithMessage("Grid name length must be between 3 and 100 characters.")
               .Matches("^[a-zA-Z0-9]+$").WithMessage("Grid name can only contain alphanumeric characters.");
        }

    }
}