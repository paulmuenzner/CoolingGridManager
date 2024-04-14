using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using CoolingGridManager.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace CoolingGridManager.Services
{
    public class ConsumptionConsumerService
    {
        private readonly AppDbContext _context;

        private readonly Serilog.ILogger _logger;
        public ConsumptionConsumerService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        // ADD CONSUMPTION VALUE
        public async Task<int> AddConsumption(IAddConsumerConsumptionRequest request)
        {
            try
            {
                var consumptionLog = new CoolingGridManager.Models.Data.ConsumptionConsumer
                {
                    ConsumerID = request.ConsumerID,
                    ConsumptionValue = request.ConsumptionValue,
                    LogDate = DateTime.Today,
                    DateTimeStart = request.DateTimeStart,
                    DateTimeEnd = request.DateTimeEnd
                };
                _context.ConsumptionConsumers.Add(consumptionLog);
                await _context.SaveChangesAsync();

                return consumptionLog.LogId;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "AddConsumption");
            }
        }

        // GET ALL CONSUMPTION ENTRIES PER USER AND MONTH
        public async Task<List<ConsumptionConsumer>> GetConsumptionForUserByMonth(int consumerId, int month, int year)
        {
            try
            {
                // All entries of current month
                var startDate = new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero);
                var endDate = startDate.AddMonths(1).AddTicks(-1);

                var logs = await _context.ConsumptionConsumers
                    .Where(log =>
                        log.ConsumerID == consumerId &&
                        log.DateTimeStart >= startDate &&
                        log.DateTimeEnd <= endDate).ToListAsync();

                if (logs != null)
                {
                    return logs;
                }
                else
                {
                    var message = $"Not possible to retrieve consumption logs with 'GetConsumptionForUserByMonth'. Month: {month}, Skip: {consumerId}";
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