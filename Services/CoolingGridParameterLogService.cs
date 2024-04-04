using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using CoolingGridManager.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace CoolingGridManager.Services
{
    public class CoolingGridParameterLogService
    {
        private readonly AppDbContext _context;

        private readonly Serilog.ILogger _logger;
        public CoolingGridParameterLogService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        // ADD CONSUMPTION VALUE
        public async Task<CoolingGridParameterLog> AddCoolingGridParameterLog(CoolingGridParameterLog log)
        {
            try
            {
                _context.CoolingGridParameterLogs.Add(log);
                await _context.SaveChangesAsync();

                return log;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "AddConsumption");
            }
        }

        // GET ALL CONSUMPTION ENTRIES PER USER AND MONTH
        public async Task<List<ConsumptionLog>> GetConsumptionForUserByMonth(int consumerId, int month)
        {
            try
            {
                // All entries of current month
                var startDate = new DateTimeOffset(DateTime.Now.Year, month, 1, 0, 0, 0, TimeSpan.Zero);
                var endDate = startDate.AddMonths(1).AddTicks(-1);

                var logs = await _context.ConsumptionLogs
                    .Where(log =>
                        log.ConsumerID == consumerId &&
                        log.ConsumptionDate >= startDate &&
                        log.ConsumptionDate <= endDate).ToListAsync();

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
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "GetConsumptionForUserByMonth");
            }
        }
    }
}