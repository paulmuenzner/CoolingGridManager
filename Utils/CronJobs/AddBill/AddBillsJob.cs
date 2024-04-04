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
        private readonly ConsumptionService _consumptionService;
        private readonly ILogger _logger;
        private readonly BillingService _billingService;
        private readonly ConsumerService _consumerService;

        public AddBills(ILogger logger, BillingService billingService, ConsumerService consumerService, ConsumptionService consumptionService)
        {
            _logger = logger;
            _billingService = billingService;
            _consumerService = consumerService;
            _consumptionService = consumptionService;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                int currentMonth = DateTime.Now.Month;
                var pageNumber = 1;
                bool hasNextPage = true;

                while (hasNextPage)
                {
                    // Retrieve consumers for the current page
                    var skip = (pageNumber - 1) * PageSize;
                    var consumers = await _consumerService.GetConsumerBatch(skip, PageSize);


                    if (consumers.Any())
                    {
                        // Log each consumer in the batch
                        foreach (var consumer in consumers)
                        {

                            var logs = await _consumptionService.GetConsumptionForUserByMonth(consumer.ConsumerID, currentMonth);
                            _logger.Information("Batch Consumer: {@Consumer}", logs);
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