// Starts every third day in a month on midnight once per month to validate if a bill for 
// ... cooling energy billing has been created for each consumer and month 
// If no bill created 

using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using Quartz;

namespace CoolingGridManager.Utils.CronJobs
{

    [DisallowConcurrentExecution]
    public class ValidateBills : IJob
    {
        // private readonly ILogger<LoggingBackgroundJob> _logger;
        private readonly ILogger<ValidateBills> _logger;
        private readonly BillingService _billingService;

        public ValidateBills(ILogger<ValidateBills> logger, BillingService billingService)
        {
            _logger = logger;
            _billingService = billingService;
        }

        // public Task Execute(IJobExecutionContext context)
        // {
        //     _logger.Information("{UtcNow}", DateTime.UtcNow);

        //     return Task.CompletedTask;
        // }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var random = new Random();
                var doubles = random.NextDouble() * 1;
                // Generate arbitrary values for Billing
                var bill = new Billing
                {
                    ConsumerID = random.Next(9, 11), // Assuming you have consumers with IDs 1-100
                    BillingMonth = random.Next(1, 12),
                    BillingYear = DateTime.Now.Year,
                    TotalConsumption = (decimal)(doubles * 100.025), // Arbitrary value
                    IsPaid = false,
                    BillingAmount = (decimal)(doubles * 100.025 * 1.5) // Arbitrary value
                };


                // Call AddBill method with generated Billing object
                await _billingService.CreateBillingRecord(bill);
                _logger.LogInformation("Bill added successfully {UtcNow}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to validate bill using cron job: {ex.Message}");
            }
        }
    }
}