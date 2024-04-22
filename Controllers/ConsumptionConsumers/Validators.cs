using FluentValidation;
using CoolingGridManager.IRequests;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Utility.ValidatorHelpers;


namespace CoolingGridManager.Validators.ConsumptionConsumers
{
    public class CreateConsumptionRecordValidator : AbstractValidator<List<ConsumptionData>>
    {
        private readonly AppDbContext _context;
        public CreateConsumptionRecordValidator(AppDbContext context)
        {
            _context = context;

            RuleForEach(consumptionList => consumptionList)
                .Must(consumptionList => DateHelpers.HaveSameMonth(consumptionList.DateTimeStart, consumptionList.DateTimeEnd))
                .WithMessage("For data consistency, start and end dates must be in the same month.");

            RuleForEach(list => list)
                .ChildRules(consumption =>
                {
                    consumption.RuleFor(c => c)
                        .NotNull().WithMessage("Element ID not provided for all records.")
                        .Must(id => Regex.IsMatch(id.ElementID, AppData.UuidPattern, RegexOptions.IgnoreCase)).WithMessage("Element ID not provided for all records.")
                        .MustAsync(async (c, cancellationToken) =>
                        {
                            // No usage of ConsumerService here. Validation logic, business logic, and data access logic should ideally be separated. Using a service for data access within a validation rule may violate this principle, as it introduces data access logic into the validation layer.
                            var elementID = c.ElementID;
                            var existingElementID = await _context.Consumers.FindAsync(elementID);
                            return existingElementID == null;
                        })
                        .WithMessage(c => $"Consumption entry with element ID {c.ElementID} already existing.");

                    consumption.RuleFor(c => c.DateTimeStart)
                        .NotNull().WithMessage($"Date time for time frame start is required but missing.")
                        .Must(date => date != default(DateTime)).WithMessage("Date time for time frame start is required.")
                        // Assumption that sender and receiver recide in same time zone. Otherwise LessThanOrEqualTo may lead to false negative results.
                        .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date time for time frame start must not be in the future.");

                    consumption.RuleFor(c => c.DateTimeEnd)
                        .NotNull().WithMessage("Date time for time frame end is required.")
                        .Must(date => date != default(DateTime)).WithMessage("Date time for time frame end is required.")
                        // Assumption that sender and receiver recide in same time zone. Otherwise LessThanOrEqualTo may lead to false negative results.
                        .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date time for time frame end must not be in the future.");

                    consumption.RuleFor(c => c.ConsumptionValue)
                        .NotNull().WithMessage("Consumption value is required.")
                        .GreaterThanOrEqualTo(0).WithMessage("Consumption value must be zero or positive.");

                    consumption.RuleFor(c => c.ConsumerID)
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

                    consumption.RuleFor(c => c)
                       .MustAsync(async (c, cancellationToken) =>
                        {
                            return await ValidateTimeOverlap(c.DateTimeStart, c.DateTimeEnd, c.ConsumerID);
                        }).WithMessage("Your provided time frame overlaps with existing data. Log data cannot be stored under these conditions as it would compromise data consistency.");
                });
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
    }
}