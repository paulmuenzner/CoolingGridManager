using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using Quartz;
using Utility.Functions;
using ILogger = Serilog.ILogger;

namespace CoolingGridManager.Utils.CronJobs
{
    public class CalculateEfficiency : IJob
    {
        private const int PageSize = 100;
        private readonly ConsumptionConsumerService _consumptionConsumerService;
        private readonly ILogger _logger;
        private readonly GridEnergyTransferService _gridEnergyTransferService;
        private readonly GridEfficiencyService _gridEfficiencyService;
        private readonly GridService _gridService;

        public CalculateEfficiency(ILogger logger, GridEnergyTransferService gridEnergyTransferService, GridEfficiencyService gridEfficiencyService, ConsumptionConsumerService consumptionConsumerService, GridService gridService)
        {
            _logger = logger;
            _gridEfficiencyService = gridEfficiencyService;
            _consumptionConsumerService = consumptionConsumerService;
            _gridEnergyTransferService = gridEnergyTransferService;
            _gridService = gridService;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // Period is previous month
                // Defining month -> previous calendar month
                // Current date
                DateTime date = DateTime.Now;
                // Get the month (previous month)
                int month = date.Month - 1;
                int year = date.Year;

                // Adjust the year if the previous month is December
                if (month == 0)
                {
                    month = 12;
                    year--;
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
                            var gridRequest = new IGetGridDataRequest
                            {
                                GridID = grid.GridID,
                                Month = month,
                                Year = year
                            };

                            var gridEnergyTransferEntryExists = await _gridEnergyTransferService.DoesGridEnergyTransferEntryExist(gridRequest);

                            // Skip to next grid if entry already existing
                            if (gridEnergyTransferEntryExists)
                            {
                                _logger.Warning($"Energy transfer entry for grid with ID {grid.GridID} for month {month} and year {year} already existing. Date: {date}.");
                                continue;
                            }

                            Models.Data.GridEnergyTransfer gridEnergyTransfer = await _gridEnergyTransferService.GetGridEnergyTransferDetails(gridRequest);
                            decimal totalGridEnergyTransferByMonth = gridEnergyTransfer.EnergyTransfer;

                            // Get measured/logged of all consumer consumption values by grid and month
                            decimal totalConsumerConsumptionByGridAndMonth = await _consumptionConsumerService.GetEntireConsumerConsumptionForGrid(gridRequest);

                            // Validation if energy transfer of grid is greater than total consumer consumption for same time frame
                            if (totalConsumerConsumptionByGridAndMonth > totalGridEnergyTransferByMonth)
                            {
                                _logger.Warning($"Total consumer consumption of {totalConsumerConsumptionByGridAndMonth}kWh cannot be greater than the entire grid energy transfer of {totalGridEnergyTransferByMonth} (Grid ID: {grid.GridID}, month: {month}, year: {year} ).");
                                continue;
                            }

                            decimal efficiency = totalConsumerConsumptionByGridAndMonth / totalGridEnergyTransferByMonth;
                            decimal efficiency_formatted = Numbers.RoundDecimal(efficiency, 10);

                            var record = new ICreateGridEfficiencyRequest
                            {
                                GridID = grid.GridID,
                                EfficiencyRelative = efficiency_formatted,
                                Month = month,
                                Year = year
                            };
                            await _gridEfficiencyService.CreateGridEfficiencyRecord(record);
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
                _logger.Error(ex, "Error determining and storing grid efficiencies.");
                throw;
            }
        }
    }
}