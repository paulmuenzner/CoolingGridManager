using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using CoolingGridManager.IRequests;
using Microsoft.EntityFrameworkCore;
using Serilog;
using FormatException = CoolingGridManager.Exceptions.FormatException;
using CoolingGridManager.IServices;
using CoolingGridManager.IResponse;

namespace CoolingGridManager.Services
{
    public class ConsumptionConsumerService : IConsumptionConsumerService
    {
        private readonly AppDbContext _context;

        private readonly Serilog.ILogger _logger;
        public ConsumptionConsumerService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
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

        ///////////////////////////////////////////
        // GET ALL CONSUMPTION ENTRIES PER USER AND MONTH
        public async Task<List<ConsumptionConsumer>> GetConsumptionForUserByMonth(IGetConsumptionForUserByMonthRequest request)
        {
            try
            {
                // All entries of current month
                var startDate = new DateTimeOffset(request.BillingYear, request.BillingMonth, 1, 0, 0, 0, TimeSpan.Zero);
                var endDate = startDate.AddMonths(1).AddTicks(-1);

                var logs = await _context.ConsumptionConsumers
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