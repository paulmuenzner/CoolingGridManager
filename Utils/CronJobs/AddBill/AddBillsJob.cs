using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using Quartz;
using ILogger = Serilog.ILogger;

namespace CoolingGridManager.Utils.CronJobs
{

    [DisallowConcurrentExecution]
    public class AddBills : IJob
    {
        private const int PageSize = 100;
        private readonly ConsumptionConsumerService _consumptionConsumerService;
        private readonly ILogger _logger;
        private readonly BillingService _billingService;
        private readonly ConsumerService _consumerService;

        public AddBills(ILogger logger, BillingService billingService, ConsumerService consumerService, ConsumptionConsumerService consumptionConsumerService)
        {
            _logger = logger;
            _billingService = billingService;
            _consumerService = consumerService;
            _consumptionConsumerService = consumptionConsumerService;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // Billing period is previous month
                // Define month and year of previous calender month
                // Current date
                DateTime date = DateTime.Now;
                // Get the previous month and year
                int previousMonth = date.Month - 1;
                int previousYear = date.Year;

                // Adjust the year if the previous month is December
                if (previousMonth == 0)
                {
                    previousMonth = 12;
                    previousYear--;
                }
                var pageNumber = 1;
                bool hasNextPage = true;

                while (hasNextPage)
                {
                    // Retrieve consumers for the current page (pageNumber)
                    var skip = (pageNumber - 1) * PageSize;
                    var consumers = await _consumerService.GetConsumerBatch(skip, PageSize);


                    if (consumers.Any())
                    {
                        // Handle each consumer in the batch
                        foreach (var consumer in consumers)
                        {

                            List<ConsumptionConsumer> logs = await _consumptionConsumerService.GetConsumptionForUserByMonth(consumer.ConsumerID, previousMonth, previousYear);
                            // Sum up all ConsumptionValue properties
                            decimal totalConsumption = logs.Sum(log => log.ConsumptionValue);
                            _logger.Information($"Total consumption value: {totalConsumption}");
                        }

                        pageNumber++;
                    }
                    else
                    {
                        hasNextPage = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving and logging consumers in batches");
                throw;
            }
        }
    }
}