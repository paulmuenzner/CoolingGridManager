using FluentValidation;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Models.Requests;


namespace CoolingGridManager.Validators.GridParameterLogs
{

    // Add Consumption Validator
    public class AddGridParameterLogValidator : AbstractValidator<GridParameterLog>
    {
        private readonly AppDbContext _context;
        public AddGridParameterLogValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(consumption => consumption.MassFlowRate)
                .NotEmpty().WithMessage("Month is required.")
                .InclusiveBetween(1, 12).WithMessage("Valid month must be selected.");

            RuleFor(consumption => consumption.SpecificHeatCapacity)
                .NotEmpty().WithMessage("Year value is required.")
                .InclusiveBetween(2020, 2040).WithMessage("Year must be between 2020 and 2040.");

            RuleFor(consumption => consumption.TemperatureIn)
                .NotEmpty().WithMessage("Valid grid ID must be provided.")
                .NotNull().WithMessage("Valid grid ID must be provided.")
                .GreaterThan(0).WithMessage("Grid ID must be greater than 0.");

            RuleFor(consumption => consumption.TemperatureOut)
                .NotEmpty().WithMessage("Valid grid ID must be provided.")
                .NotNull().WithMessage("Valid grid ID must be provided.")
                .GreaterThan(0).WithMessage("Grid ID must be greater than 0.");

            RuleFor(consumption => consumption.TimeStart)
                .NotEmpty().WithMessage("Valid grid ID must be provided.")
                .NotNull().WithMessage("Valid grid ID must be provided.")
                .GreaterThan(0).WithMessage("Grid ID must be greater than 0.");

            RuleFor(consumption => consumption.TimeEnd)
                .NotEmpty().WithMessage("Valid grid ID must be provided.")
                .NotNull().WithMessage("Valid grid ID must be provided.")
                .GreaterThan(0).WithMessage("Grid ID must be greater than 0.");

            RuleFor(consumption => consumption.TemperatureOut)
                .NotEmpty().WithMessage("Valid grid ID must be provided.")
                .NotNull().WithMessage("Valid grid ID must be provided.")
                .GreaterThan(0).WithMessage("Grid ID must be greater than 0.");
        }

    }
}