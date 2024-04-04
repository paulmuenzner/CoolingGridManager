using FluentValidation;
using CoolingGridManager.Models.Requests;


namespace CoolingGridManager.Validators.ConsumptionConsumers
{

    // Add Consumption Validator
    public class AddConsumptionValidator : AbstractValidator<AddConsumerConsumptionRequest>
    {
        private readonly AppDbContext _context;
        public AddConsumptionValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(consumption => consumption.ConsumptionDate)
                .NotNull().WithMessage("Consumption date is required.")
                .Must(date => date != default(DateTime)).WithMessage("Consumption date is required.")
                // If sender and receiver in same time zone, LessThan is recommended
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Consumption date must not be in the future.");

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
        }

    }
}