using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using Quartz;
using Utility.Functions;
using ILogger = Serilog.ILogger;

namespace CoolingGridManager.Utils.CronJobs
{

    [DisallowConcurrentExecution]
    /// <summary>
    /// Grid Energy Transfer
    /// </summary>
    /// <remarks>
    /// Calculate Grid Energy Transfer based on the grid's flow, return flow. 
    /// This value/result doesn't equal and must be greater than the metered total consumption of all consumers of this grid, which doesn't includes losses
    /// </remarks>
    public class GridEnergyTransfer : IJob
    {
        private const int PageSize = 10;
        private readonly GridParameterLogService _gridParameterLogService;
        private readonly GridEnergyTransferService _gridEnergyTransferService;
        private readonly GridService _gridService;
        private readonly ILogger _logger;

        public GridEnergyTransfer(ILogger logger, GridParameterLogService gridParameterLogService, GridEnergyTransferService gridEnergyTransferService, GridService gridService)
        {
            _logger = logger;
            _gridParameterLogService = gridParameterLogService;
            _gridEnergyTransferService = gridEnergyTransferService;
            _gridService = gridService;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // Relevant period to calculate monthly grid energy transfer is previous month
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
                            // First: It shouldn't but validate if entry for energy transfer already exists avoiding duplication
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
                                _logger.Warning($"Entry for energy transfer of grid with ID {grid.GridID} for month {month} and year {year} already existing. Date: {date}.");
                                continue;
                            }

                            // Get all relevant data from grid parameter logs 
                            List<GridParameterLog> gridParametersByMonth = await _gridParameterLogService.GetMonthlyGridParameterDetails(gridRequest);

                            // Calculate transfered cooling energy for entire month (also based on flow, return flow)
                            decimal totalEnergyTransfer_kWh = Energy.CalculateGridEnergyTransfer(gridParametersByMonth);

                            var record = new ICreateGridEnergyTransferRecordRequest
                            {
                                GridID = grid.GridID,
                                EnergyTransfer = totalEnergyTransfer_kWh,
                                Month = month,
                                Year = year
                            };
                            await _gridEnergyTransferService.CreateGridEnergyTransferRecord(record);
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
                _logger.Error(ex, "Error determining and storing energy transfer entry.");
                throw;
            }
        }
    }
}