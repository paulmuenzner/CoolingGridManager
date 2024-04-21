using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using Quartz;
using ILogger = Serilog.ILogger;

namespace CoolingGridManager.Utils.CronJobs
{

    [DisallowConcurrentExecution]
    public class CreateBills : IJob
    {
        private const int PageSize = 100;
        private readonly ConsumptionConsumerService _consumptionConsumerService;
        private readonly ILogger _logger;
        private readonly BillingService _billingService;
        private readonly ConsumerService _consumerService;

        public CreateBills(ILogger logger, BillingService billingService, ConsumerService consumerService, ConsumptionConsumerService consumptionConsumerService)
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
                // Define billing month -> previous calender month
                // Current date
                DateTime date = DateTime.Now;
                // Get the billing month (previous month)
                int billingMonth = date.Month - 1;
                int billingYear = date.Year;

                // Adjust the year if the previous month is December
                if (billingMonth == 0)
                {
                    billingMonth = 12;
                    billingYear--;
                }
                var pageNumber = 1;
                bool hasNextPage = true;

                while (hasNextPage)
                {
                    // Retrieve consumers for the current page (pageNumber)
                    var skip = (pageNumber - 1) * PageSize;
                    // Create the request object
                    var batchRequest = new IGetConsumerBatchRequest
                    {
                        Skip = skip,
                        Size = PageSize
                    };
                    List<Consumer> consumers = await _consumerService.GetConsumerBatch(batchRequest);


                    if (consumers.Any())
                    {
                        // Handle each consumer in the batch
                        foreach (var consumer in consumers)
                        {

                            // Create the request object
                            var consumptionRequest = new IGetConsumptionForUserByMonthRequest
                            {
                                ConsumerID = consumer.ConsumerID,
                                BillingMonth = billingMonth,
                                BillingYear = billingYear
                            };
                            List<ConsumptionConsumer> userConsumptionsByMonth = await _consumptionConsumerService.GetConsumptionForUserByMonth(consumptionRequest);
                            // Sum up all ConsumptionValue properties
                            decimal totalConsumption = userConsumptionsByMonth.Sum(log => log.ConsumptionValue);

                            // Get consumer contract data
                            decimal unitPrice = consumer.UnitPrice;
                            decimal monthlyBaseFee = consumer.MonthlyBaseFee;

                            // Calculate billing amount 
                            decimal billingAmount = totalConsumption * unitPrice + monthlyBaseFee;
                            _logger.Information($"Total consumption value: {billingAmount}");

                            var bill = new Billing
                            {
                                ConsumerID = consumer.ConsumerID,
                                BillingMonth = billingMonth,
                                BillingYear = billingYear,
                                TotalConsumption = totalConsumption,
                                IsPaid = false,
                                BillingAmount = billingAmount,
                                Consumer = consumer
                            };
                            var consumptionId = await _billingService.CreateBillingRecord(bill);
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