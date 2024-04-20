using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CoolingGridManager.IRequests;


namespace CoolingGridManager.Validators.GridParameterLogs
{
    // GetGridParameterLog Validator
    public class GetGridParameterLogValidator : AbstractValidator<IGetMonthlyGridParameterDetailsRequest>
    {
        private readonly AppDbContext _context;
        public GetGridParameterLogValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(request => request.Month)
                .NotEmpty().WithMessage("Valid month for requested period required.")
                .InclusiveBetween(1, 12).WithMessage("Valid month needed.");

            RuleFor(request => request.Year)
                .NotEmpty().WithMessage("Valid year for requested period required.")
                .InclusiveBetween(AppData.TimeFrameYearMin, AppData.TimeFrameYearMax).WithMessage($"Year must be between or equal to {AppData.TimeFrameYearMin} and {AppData.TimeFrameYearMax}.");


            RuleFor(request => request.GridID)
                .NotEmpty().WithMessage("Grid ID is required.")
                .GreaterThan(0).WithMessage("Grid ID cannot be 0 or smaller.")
                .MustAsync(async (gridId, cancellationToken) =>
                {
                    return await GridExists(gridId);
                })
                .WithMessage("Requested grid does not exist.");

        }

        public async Task<bool> GridExists(int gridId)
        {
            var existingGrid = await _context.Grids.FindAsync(gridId);
            return existingGrid != null;
        }

    }

    // CreateGridParameterLog Validator
    public class CreateGridParameterLogValidator : AbstractValidator<ICreateGridParameterLogRecordRequest>
    {
        private readonly AppDbContext _context;
        public CreateGridParameterLogValidator(AppDbContext context)
        {
            _context = context;
            // Prepare must be same month
            RuleFor(request => request.MassFlowRate)
                .NotEmpty().WithMessage("Mass flow is required.")
                .GreaterThan(0).WithMessage("Mass flow cannot be 0 or smaller.");

            RuleFor(request => request.SpecificHeatCapacity)
                .NotEmpty().WithMessage("Specific heat capacity is required.")
                .GreaterThan(0).WithMessage("Specific heat capacity cannot be 0 or smaller.")
                .LessThan(5).WithMessage("Specific heat capacity cannot be 5 or larger.")
                .Must(value => BeAValidPrecision(value, 3)).WithMessage("Specific heat capacity must have a maximum of 3 numbers after the decimal point.");

            RuleFor(request => request.MeanTemperatureIn)
                .NotEmpty().WithMessage("Mean fluid temperature entering the grid must be provided.")
                .NotNull().WithMessage("Mean fluid temperature entering the grid must be provided.")
                .InclusiveBetween(-10, 30).WithMessage("Mean fluid temperature entering the grid must be between -10 and 30°C.")
                .Must(value => BeAValidPrecision(value, 2)).WithMessage("Temperature must have a maximum of 2 decimal places.");

            RuleFor(request => request.MeanTemperatureOut)
                .NotEmpty().WithMessage("Mean fluid temperature leaving the grid must be provided.")
                .NotNull().WithMessage("Mean fluid temperature leaving the grid must be provided.")
                .InclusiveBetween(-10, 30).WithMessage("Mean fluid temperature leaving the grid must be between -10 and 30°C.")
                .Must(value => BeAValidPrecision(value, 2)).WithMessage("Temperature must have a maximum of 2 decimal places.");

            RuleFor(request => request.DateTimeStart)
                .NotEmpty().WithMessage("Valid date time for period start required.")
                .Must(date => date != default(DateTime)).WithMessage("Valid date time for period start required.")
                // Assumption that sender and receiver recide in same time zone. Otherwise LessThanOrEqualTo may lead to false negative results.
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Valid date time for period start required.");

            RuleFor(request => request.DateTimeEnd)
                .NotEmpty().WithMessage("Valid date time for period end required.")
                .Must(date => date != default(DateTime)).WithMessage("Valid date time for period end required.")
                // Assumption that sender and receiver recide in same time zone. Otherwise LessThanOrEqualTo may lead to false negative results.
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Valid date time for period end required.");

            RuleFor(request => request.GridID)
                .NotEmpty().WithMessage("Grid ID is required.")
                .GreaterThan(0).WithMessage("Grid ID cannot be 0 or smaller.")
                .MustAsync(async (gridId, cancellationToken) =>
                {
                    return await GridExists(gridId);
                })
                .WithMessage("Requested grid does not exist.");

            RuleFor(request => request)
               .MustAsync(async (request, cancellationToken) =>
                {
                    return await ValidateTimeOverlap(request.DateTimeStart, request.DateTimeEnd, request.GridID);
                }).WithMessage("The time frame you provided overlaps with existing data. We cannot store this log data as it would compromise data consistency.");
        }

        public async Task<bool> ValidateTimeOverlap(DateTime DateTimeStart, DateTime DateTimeEnd, int gridID)
        {
            var hasOverlap = await _context.GridParameterLog
                    .Where(log => log.GridID == gridID)
                    .AnyAsync(log =>
                        (log.DateTimeStart < DateTimeEnd && log.DateTimeEnd > DateTimeEnd) ||
                        (DateTimeStart < log.DateTimeEnd && DateTimeStart > log.DateTimeStart)
                    );

            return !hasOverlap;
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

        public async Task<bool> GridExists(int gridId)
        {
            var existingGrid = await _context.Grids.FindAsync(gridId);
            return existingGrid != null;
        }

    }
}