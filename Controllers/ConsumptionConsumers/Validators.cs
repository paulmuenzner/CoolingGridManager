using FluentValidation;
using CoolingGridManager.Models.Requests;
using Microsoft.EntityFrameworkCore;


namespace CoolingGridManager.Validators.ConsumptionConsumers
{

    // Add Consumption Validator
    public class AddConsumptionValidator : AbstractValidator<IAddConsumerConsumptionRequest>
    {
        private readonly AppDbContext _context;
        public AddConsumptionValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(consumption => consumption)
                .Must(consumption => HaveSameMonth(consumption.DateTimeStart, consumption.DateTimeEnd))
                .WithMessage("For data consistency, start and end dates must be in the same month.");

            RuleFor(consumption => consumption.DateTimeStart)
                .NotNull().WithMessage("Date time for time frame start is required.")
                .Must(date => date != default(DateTime)).WithMessage("Date time for time frame start is required.")
                // Assumption that sender and receiver recide in same time zone. Otherwise LessThanOrEqualTo may lead to false negative results.
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date time for time frame start must not be in the future.");

            RuleFor(consumption => consumption.DateTimeEnd)
                .NotNull().WithMessage("Date time for time frame end is required.")
                .Must(date => date != default(DateTime)).WithMessage("Date time for time frame end is required.")
                // Assumption that sender and receiver recide in same time zone. Otherwise LessThanOrEqualTo may lead to false negative results.
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date time for time frame end must not be in the future.");

            RuleFor(consumption => consumption.ConsumptionValue)
                .NotNull().WithMessage("Consumption value is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Consumption value must be zero or positive.");

            RuleFor(consumption => consumption.ConsumerID)
                .NotEmpty().WithMessage("Valid consumer ID must be provided.")
                .NotNull().WithMessage("Valid consumer ID must be provided.")
                .GreaterThan(0).WithMessage("Consumer ID must be greater than 0.")
                .MustAsync(async (consumerID, cancellationToken) =>
            {
                // No usage of Consumer Service here. Validation logic, business logic, and data access logic should ideally be separated. Using a service for data access within a validation rule may violate this principle, as it introduces data access logic into the validation layer.
                var existingConsumer = await _context.Consumers.FindAsync(consumerID);
                return existingConsumer != null;
            })
            .WithMessage("Requested consumer does not exist.");

            RuleFor(consumption => consumption)
               .MustAsync(async (consumption, cancellationToken) =>
                {
                    return await ValidateTimeOverlap(consumption.DateTimeStart, consumption.DateTimeEnd, consumption.ConsumerID);
                }).WithMessage("Your provided time frame overlaps with existing data. Log data cannot be stored under these conditions as it would compromise data consistency.");
        }

        public async Task<bool> ValidateTimeOverlap(DateTime DateTimeStart, DateTime DateTimeEnd, int consumerID)
        {
            var hasOverlap = await _context.ConsumptionConsumers
                    .Where(log => log.ConsumerID == consumerID)
                    .AnyAsync(log =>
                        (log.DateTimeStart < DateTimeEnd && log.DateTimeEnd > DateTimeEnd) ||
                        (DateTimeStart < log.DateTimeEnd && DateTimeStart > log.DateTimeStart)
                    );

            return !hasOverlap;
        }

        private bool HaveSameMonth(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            // Extract month and year components from the start and end dates
            var startMonthYear = new DateTime(dateTimeStart.Year, dateTimeStart.Month, 1);
            var endMonthYear = new DateTime(dateTimeEnd.Year, dateTimeEnd.Month, 1);

            // Compare if the month and year components are the same
            return startMonthYear == endMonthYear;
        }

    }
}