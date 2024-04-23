using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using Quartz;
using Utility.Functions;
using ILogger = Serilog.ILogger;

namespace CoolingGridManager.Utils.CronJobs
{

    [DisallowConcurrentExecution]
    public class GridConsumption : IJob
    {
        private const int PageSize = 10;
        private readonly GridParameterLogService _gridParameterLogService;
        private readonly ConsumptionGridService _consumptionGridService;
        private readonly GridService _gridService;
        private readonly ILogger _logger;

        public GridConsumption(ILogger logger, GridParameterLogService gridParameterLogService, ConsumptionGridService consumptionGridService, GridService gridService)
        {
            _logger = logger;
            _gridParameterLogService = gridParameterLogService;
            _consumptionGridService = consumptionGridService;
            _gridService = gridService;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // Relevant period to calculate monthly grid consumption for is previous month
                // Current date
                DateTime date = DateTime.Now;

                // Get the month (previous month)
                int consumptionMonth = date.Month - 1;
                int consumptionYear = date.Year;

                // Adjust the year if the previous month is December
                if (consumptionMonth == 0)
                {
                    consumptionMonth = 12;
                    consumptionYear--;
                }
                var pageNumber = 1;
                bool hasNextPage = true;

                while (hasNextPage)
                {
                    // Retrieve grids for the current page (pageNumber)
                    var skip = (pageNumber - 1) * PageSize;
                    // Create the request object
                    var batchRequest = new IGetGridBatchRequest
                    {
                        Skip = skip,
                        Size = PageSize
                    };
                    List<Grid> grids = await _gridService.GetGridBatch(batchRequest);


                    if (grids.Any())
                    {
                        // Handle each grid in the batch
                        foreach (var grid in grids)
                        {
                            // First: It shouldn't but validate if consumption entry already exists avoiding duplication
                            var gridRequest = new IGetGridDataRequest
                            {
                                GridID = grid.GridID,
                                Month = consumptionMonth,
                                Year = consumptionYear
                            };
                            var consumptionEntryExists = await _consumptionGridService.DoesGridConsumptionEntryExist(gridRequest);

                            // Skip to next grid if entry already existing
                            if (consumptionEntryExists)
                            {
                                _logger.Warning($"Consumption entry for grid with ID {grid.GridID} for month {consumptionMonth} and year {consumptionYear} already existing. Date: {date}.");
                                continue;
                            }

                            // Get all relevant data from grid parameter logs 
                            List<GridParameterLog> gridConsumptionsByMonth = await _gridParameterLogService.GetMonthlyGridParameterDetails(gridRequest);

                            // Calculate consumed cooling energy for entire month
                            decimal totalConsumption = Energy.CaluclateGridConsumption(gridConsumptionsByMonth);

                            var record = new ICreateGridConsumptionRecordRequest
                            {
                                GridID = grid.GridID,
                                Consumption = totalConsumption,
                                Month = consumptionMonth,
                                Year = consumptionYear
                            };
                            var consumptionId = await _consumptionGridService.CreateGridConsumptionRecord(record);
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