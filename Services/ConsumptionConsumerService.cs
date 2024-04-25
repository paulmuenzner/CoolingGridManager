using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using CoolingGridManager.IRequests;
using Microsoft.EntityFrameworkCore;
using FormatException = CoolingGridManager.Exceptions.FormatException;
using CoolingGridManager.IServices;
using CoolingGridManager.IResponse;

namespace CoolingGridManager.Services
{
    public class ConsumptionConsumerService : IConsumptionConsumerService
    {
        private readonly AppDbContext _context;
        private readonly GridSectionService _gridSectionService;
        private readonly Serilog.ILogger _logger;
        public ConsumptionConsumerService(AppDbContext context, Serilog.ILogger logger, GridSectionService gridSectionService)
        {
            _logger = logger;
            _context = context;
            _gridSectionService = gridSectionService;
        }

        /////////////////////////////////////////////////////////////////
        // SUMP UP CONSUMPTION ENTRIES OF ALL USERS IN A GRID PER MONTH
        public async Task<decimal> GetEntireConsumerConsumptionForGrid(IGetGridDataRequest request)
        {
            try
            {
                // Retrieve all related grid sections of this grid
                List<GridSection> gridSections = await _gridSectionService.GetGridSectionRecords(request.GridID);

                // Define time span (start and end DateTime) based on provided year and month 
                var startDate = new DateTimeOffset(request.Year, request.Month, 1, 0, 0, 0, TimeSpan.Zero);
                var endDate = startDate.AddMonths(1).AddTicks(-1);

                if (gridSections.Any())
                {
                    // Collect users of all grid sections
                    List<int> allConsumerIds = new List<int>();
                    foreach (var gridSection in gridSections)
                    {
                        // Get all consumer IDs as list by gridsection ID
                        List<int> consumerIds = await _context.Consumers
                                    .Where(c => c.GridSectionID == gridSection.GridSectionID)
                                    .Select(c => c.ConsumerID)
                                    .ToListAsync();
                        allConsumerIds.AddRange(consumerIds);
                    }

                    // Get total consumption values for each Consumer ID in list and sum up
                    decimal totalConsumerConsumptionInGrid = 0m;
                    foreach (var consumerId in allConsumerIds)
                    {
                        var consumerConsumption = await _context.ConsumptionConsumers.
                    Where(c =>
                        c.ConsumerID == consumerId &&
                        c.DateTimeStart >= startDate &&
                        c.DateTimeEnd <= endDate).ToListAsync();

                        // Summing up the consumerConsumption for each consumer
                        decimal consumerTotal = consumerConsumption.Sum(c => c.ConsumptionValue);

                        // Accumulating the consumerTotal into totalConsumerConsumption
                        totalConsumerConsumptionInGrid += consumerTotal;
                    }
                    return totalConsumerConsumptionInGrid;

                }
                else return 0;
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetEntireConsumerConsumptionForGrid");
            }
        }

        ///////////////////////////////////////////
        // ADD CONSUMPTION VALUE
        public async Task<ICreateConsumerConsumptionRecordResponse> CreateConsumerConsumptionRecord(List<ConsumptionData> request)
        {
            try
            {
                foreach (var data in request)
                {
                    var consumptionRecord = new ConsumptionConsumer
                    {
                        ConsumerID = data.ConsumerID,
                        ConsumptionValue = data.ConsumptionValue,
                        LogDate = DateTime.UtcNow,
                        DateTimeStart = data.DateTimeStart,
                        DateTimeEnd = data.DateTimeEnd
                    };

                    // Consider related consumer (foreign key relation)
                    var existingConsumer = await _context.Consumers.FindAsync(data.ConsumerID);

                    if (existingConsumer == null)
                    {
                        _logger.Warning($"Consumer with ID {data.ConsumerID} does not exist.");
                        throw new FormatException($"Consumer with ID {data.ConsumerID} does not exist.", "CreateConsumerConsumptionRecord");
                    }
                    // Associate the existing grid with the new grid section
                    consumptionRecord.Consumer = existingConsumer;

                    _context.ConsumptionConsumers.Add(consumptionRecord);
                    await _context.SaveChangesAsync();
                }
                int length = request.Count;
                var response = new ICreateConsumerConsumptionRecordResponse { Success = true, Count = length };
                return response;
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "CreateConsumerConsumptionRecord");
            }
        }

        /////////////////////////////////////////// prepare name
        // GET ALL CONSUMPTION ENTRIES PER USER AND MONTH
        public async Task<List<ConsumptionConsumer>> GetConsumptionForUserByMonth(IGetBillByConsumerRequest request)
        {
            try
            {
                // All entries of current month
                var startDate = new DateTimeOffset(request.BillingYear, request.BillingMonth, 1, 0, 0, 0, TimeSpan.Zero);
                var endDate = startDate.AddMonths(1).AddTicks(-1);

                List<ConsumptionConsumer> logs = await _context.ConsumptionConsumers
                    .Where(log =>
                        log.ConsumerID == request.ConsumerID &&
                        log.DateTimeStart >= startDate &&
                        log.DateTimeEnd <= endDate).ToListAsync();

                if (logs != null)
                {
                    return logs;
                }
                else
                {
                    var message = $"Not possible to retrieve consumption logs with 'GetConsumptionForUserByMonth'. Month: {request.BillingMonth}, Skip: {request.ConsumerID}";
                    _logger.Error(message);
                    throw new Exception(message);
                }

            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetConsumptionForUserByMonth");
            }
        }



    }
}