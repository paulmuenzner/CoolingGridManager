using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using Quartz;
using ILogger = Serilog.ILogger;

namespace CoolingGridManager.Utils.CronJobs
{

    [DisallowConcurrentExecution]
    public class AddBills2 : IJob
    {
        private readonly ILogger<AddBills2> _logger;
        private readonly BillingService _billingService;

        public AddBills2(ILogger<AddBills2> logger, BillingService billingService)
        {
            _logger = logger;
            _billingService = billingService;
        }


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
                await _billingService.AddBill(bill);
                _logger.LogInformation("Bill added successfully {UtcNow}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add bill using cron job: {ex.Message}");
            }
        }
    }
}