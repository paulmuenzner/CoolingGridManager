using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using CoolingGridManager.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace CoolingGridManager.Services
{
    public class ConsumptionGridService
    {
        private readonly AppDbContext _context;

        private readonly Serilog.ILogger _logger;
        public ConsumptionGridService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        // ADD CONSUMPTION VALUE
        public async Task<ConsumptionGrid> AddGridConsumption(ConsumptionGrid request)
        {
            try
            {
                var consumptionLog = new ConsumptionGrid
                {
                    Month = request.Month,
                    Year = request.Year,
                    Consumption = request.Consumption
                };
                _context.ConsumptionGrids.Add(request);
                await _context.SaveChangesAsync();

                return consumptionLog;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "AddGridConsumption");
            }
        }

        // GET ALL CONSUMPTION ENTRIES PER USER AND MONTH
        public async Task<ConsumptionGrid> GetConsumptionForGridByDate(int gridID, int month, int year)
        {
            try
            {
                // All entries of current month
                var logs = await _context.ConsumptionGrids
                    .FirstOrDefaultAsync(log =>
                        log.GridID == gridID &&
                        log.Month == month &&
                        log.Year == year);

                if (logs != null)
                {
                    return logs;
                }
                else
                {
                    var message = $"Not possible to retrieve grid consumption logs with 'GetConsumptionForGridByDate'. Month: {month}, Year: {year}, GridID: {gridID}";
                    _logger.Error(message);
                    throw new Exception(message);
                }

            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "GetConsumptionForGridByDate");
            }
        }
    }
}