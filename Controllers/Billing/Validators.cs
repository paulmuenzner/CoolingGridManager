using FluentValidation;
using CoolingGridManager.Models.Requests;
using CoolingGridManager.Models.Data;


namespace CoolingGridManager.Validators.Consumers
{

    // Add Consumer Validator
    public class GetBillValidator : AbstractValidator<GetBillRequest>
    {
        private readonly AppDbContext _context;
        public GetBillValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(bill => bill.ConsumerID)
                .NotEmpty().WithMessage("Consumer ID is required.")
                .GreaterThan(0).WithMessage("Consumer ID must be greater than 0.")
                .MustAsync(ExistingConsumer).WithMessage("Consumer not found with provided consumer ID.");

            RuleFor(bill => bill.Month)
                .NotNull().WithMessage("Month is required.")
                .InclusiveBetween(1, 12).WithMessage("Month must be between January and December.");

            RuleFor(bill => bill.Year)
                .NotNull().WithMessage("Year is required.")
                .GreaterThanOrEqualTo(2020).WithMessage("Year must be greater than or equal to 2020.");
        }

        private async Task<bool> ExistingConsumer(int? consumerID, CancellationToken cancellationToken)
        {
            if (consumerID == null) return false;
            var existingConsumer = await _context.Consumers.FindAsync(new object[] { consumerID }, cancellationToken);
            return existingConsumer != null;
        }
    }
}