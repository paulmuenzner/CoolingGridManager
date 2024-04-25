using FluentValidation;
using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;
using Microsoft.EntityFrameworkCore;



namespace CoolingGridManager.Validators.Bills
{
    // Update Bill Payment Status
    public class BillStatusValidator : AbstractValidator<IUpdateStatusRequest>
    {
        private readonly AppDbContext _context;
        public BillStatusValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(bill => bill.BillingId)
                .NotEmpty().WithMessage("Billing ID is required.")
                .GreaterThan(0).WithMessage("Billing ID must be greater than 0.")
                .MustAsync(ExistingBill).WithMessage("Bill not found with provided bill ID.");

            RuleFor(bill => bill.IsPaid)
                .NotNull().WithMessage("Payment status is required.");
        }

        private async Task<bool> ExistingBill(int billingId, CancellationToken cancellationToken)
        {
            var existingBill = await _context.Bills.FindAsync(new object[] { billingId }, cancellationToken);
            return existingBill != null;
        }
    }

    // Delete Bill Validator
    public class DeleteBillValidator : AbstractValidator<int>
    {
        private readonly AppDbContext _context;
        public DeleteBillValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(billingId => billingId)
                .NotEmpty().WithMessage("Billing ID is required.")
                .GreaterThan(0).WithMessage("Billing ID must be greater than 0.")
                .MustAsync(ExistingBill).WithMessage("Bill not found with provided bill ID.");
        }

        private async Task<bool> ExistingBill(int billingId, CancellationToken cancellationToken)
        {
            var existingBill = await _context.Bills.FindAsync(new object[] { billingId }, cancellationToken);
            return existingBill != null;
        }
    }

    // Add Bill Validator
    public class GetBillValidator : AbstractValidator<IGetBillByConsumerRequest>
    {
        private readonly AppDbContext _context;
        public GetBillValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(bill => bill.ConsumerID)
                .NotEmpty().WithMessage("Consumer ID is required.")
                .GreaterThan(0).WithMessage("Consumer ID must be greater than 0.")
                .MustAsync(ExistingConsumer).WithMessage("Consumer not found with provided consumer ID.");

            RuleFor(bill => bill.BillingMonth)
                .NotNull().WithMessage("Month is required.")
                .InclusiveBetween(1, 12).WithMessage("No valid month selected.");

            RuleFor(bill => bill.BillingYear)
                .NotNull().WithMessage("Year is required.")
                .GreaterThanOrEqualTo(2020).WithMessage("Year must be greater than or equal to 2020.")
                .LessThan(2035).WithMessage("No valid year selected.");
        }

        private async Task<bool> ExistingConsumer(int consumerID, CancellationToken cancellationToken)
        {
            var existingConsumer = await _context.Consumers.FindAsync(new object[] { consumerID }, cancellationToken);
            return existingConsumer != null;
        }
    }

    // Add Bill Validator
    public class CreateBillRecordValidator : AbstractValidator<Billing>
    {
        private readonly AppDbContext _context;
        public CreateBillRecordValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(billing => billing.ConsumerID)
                .NotEmpty().WithMessage("Consumer ID is required.")
                .GreaterThan(0).WithMessage("Consumer ID must be greater than 0.")
                .MustAsync(ConsumerExists).WithMessage("Consumer not found. You can only add bills to existing consumers.");

            RuleFor(billing => billing.BillingMonth)
                .NotEmpty().WithMessage("Month is required.")
                .InclusiveBetween(1, 12).WithMessage("Valid month must be provided.");

            RuleFor(billing => billing.BillingYear)
                .NotEmpty().WithMessage("Year is required.")
                .InclusiveBetween(AppData.TimeFrameYearMin, AppData.TimeFrameYearMax).WithMessage($"Year must be between or equal to {AppData.TimeFrameYearMin} and {AppData.TimeFrameYearMax}.");

            RuleFor(billing => billing.TotalConsumption)
                .NotEmpty().WithMessage("Total consumption is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Total consumption cannot be negative.");

            RuleFor(billing => billing.IsPaid)
                .NotNull().WithMessage("Provide information if bill is paid.");

            RuleFor(billing => billing.BillingAmount)
                .NotEmpty().WithMessage("Billing amount is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Billing amount cannot be negative.");

            RuleFor(billing => billing)
                .MustAsync(BeUniqueBilling).WithMessage("A bill already exists for the same user, month, and year combination.");
        }

        private async Task<bool> ConsumerExists(int consumerID, CancellationToken cancellationToken)
        {
            var consumer = await _context.Consumers.FindAsync(consumerID);

            return consumer != null;
        }

        private async Task<bool> BeUniqueBilling(Billing billing, CancellationToken cancellationToken)
        {
            var existingBill = await _context.Bills
                .Where(b => b.ConsumerID == billing.ConsumerID && b.BillingMonth == billing.BillingMonth && b.BillingYear == billing.BillingYear)
                .FirstOrDefaultAsync(cancellationToken);

            return existingBill == null || existingBill.BillingId == billing.BillingId;
        }
    }
}