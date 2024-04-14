using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using Microsoft.EntityFrameworkCore;
using FormatException = CoolingGridManager.Exceptions.FormatException;

namespace CoolingGridManager.Services
{
    public class GridParameterLogService
    {
        private readonly AppDbContext _context;

        private readonly Serilog.ILogger _logger;
        public GridParameterLogService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        // ADD CONSUMPTION VALUE
        public async Task<GridParameterLog> AddCoolingGridParameterLog(GridParameterLog log)
        {
            try
            {

                var existingGrid = await _context.Grids.FindAsync(log.GridID);

                if (existingGrid == null)
                {
                    _logger.Warning($"Grid with ID {log.GridID} does not exist.");
                    throw new FormatException($"Grid with ID {log.GridID} does not exist.", "AddCoolingGridParameterLog");
                }
                // Associate the existing grid with the new grid section
                log.Grid = existingGrid;

                _context.GridParameterLog.Add(log);
                await _context.SaveChangesAsync();

                return log;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "AddCoolingGridParameterLog");
            }
        }

        // GET ALL CONSUMPTION ENTRIES PER USER AND MONTH
        public async Task<List<GridParameterLog>> GetGridParameterByMonth(int gridId, int month, int year)
        {
            try
            {
                // All entries of current month
                var startDate = new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero);
                var endDate = startDate.AddMonths(1).AddTicks(-1);

                var logs = await _context.GridParameterLog
                    .Where(log =>
                        log.GridID == gridId &&
                        log.DateTimeStart >= startDate &&
                        log.DateTimeEnd <= endDate).ToListAsync();

                if (logs != null)
                {
                    return logs;
                }
                else
                {
                    var message = $"Not possible to retrieve consumption logs with 'GetConsumptionForUserByMonth'. Month: {month}, Skip: {gridId}";
                    _logger.Error(message);
                    throw new Exception(message);
                }

            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "GetGridParameterByMonth");
            }
        }
    }
}