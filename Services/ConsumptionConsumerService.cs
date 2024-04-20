using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using CoolingGridManager.IRequests;
using Microsoft.EntityFrameworkCore;
using Serilog;
using FormatException = CoolingGridManager.Exceptions.FormatException;
using CoolingGridManager.IServices;

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
        public async Task<ConsumptionConsumer> CreateConsumerConsumptionRecord(ICreateConsumerConsumptionRequest request)
        {
            try
            {
                var consumptionRecord = new ConsumptionConsumer
                {
                    ConsumerID = request.ConsumerID,
                    ConsumptionValue = request.ConsumptionValue,
                    LogDate = DateTime.UtcNow,
                    DateTimeStart = request.DateTimeStart,
                    DateTimeEnd = request.DateTimeEnd
                };

                // Consider related consumer (foreign key relation)
                var existingConsumer = await _context.Consumers.FindAsync(request.ConsumerID);

                if (existingConsumer == null)
                {
                    _logger.Warning($"Consumer with ID {request.ConsumerID} does not exist.");
                    throw new FormatException($"Consumer with ID {request.ConsumerID} does not exist.", "AddCoolingGridParameterLog");
                }
                // Associate the existing grid with the new grid section
                consumptionRecord.Consumer = existingConsumer;

                _context.ConsumptionConsumers.Add(consumptionRecord);
                await _context.SaveChangesAsync();

                return consumptionRecord;
            }
            catch (Exception ex)
            {
                var message = $"**Full details:** {ex}";
                Log.Error("AddConsumption try catch error", message);
                throw new TryCatchException(message, "AddConsumption");
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
                var message = string.Format($"Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "GetConsumptionForUserByMonth");
            }
        }
    }
}