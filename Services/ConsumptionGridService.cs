using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using Microsoft.EntityFrameworkCore;
using CoolingGridManager.IServices;

namespace CoolingGridManager.Services
{
    public class ConsumptionGridService : IConsumptionGridService
    {
        private readonly AppDbContext _context;

        private readonly Serilog.ILogger _logger;
        public ConsumptionGridService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        ///////////////////////////////////////////
        // ADD CONSUMPTION VALUE
        public async Task<ConsumptionGrid> CreateGridConsumptionRecord(ICreateGridConsumptionRecordRequest request)
        {
            try
            {
                // Retrieve an existing grid from the context<
                var gridID = request.GridID;
                var grid = await _context.Grids.FindAsync(gridID);
                if (grid == null)
                {
                    _logger.Warning($"Cannot find grid with ID {gridID}.");
                    throw new InvalidOperationException($"Cannot find grid with ID {gridID}.");
                }

                var consumptionLog = new ConsumptionGrid
                {
                    Grid = grid,
                    Month = request.Month,
                    Year = request.Year,
                    Consumption = request.Consumption
                };
                _context.ConsumptionGrids.Add(consumptionLog);
                await _context.SaveChangesAsync();

                return consumptionLog;
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "CreateGridConsumptionRecord");
            }
        }

        ///////////////////////////////
        // Consumption Entry Exists?
        public async Task<bool> DoesGridConsumptionEntryExist(IGetGridDataRequest request)
        {
            return await _context.ConsumptionGrids
                .AnyAsync(g => g.GridID == request.GridID && g.Month == request.Month && g.Year == request.Year);
        }

        //////////////////////////////////////////////////
        // GET ALL CONSUMPTION ENTRIES PER USER AND MONTH
        public async Task<ConsumptionGrid> GetGridConsumptionDetails(IGetGridDataRequest request)
        {
            try
            {
                // All entries of current month
                var logs = await _context.ConsumptionGrids
                    .FirstOrDefaultAsync(log =>
                        log.GridID == request.GridID &&
                        log.Month == request.Month &&
                        log.Year == request.Year);

                if (logs != null)
                {
                    return logs;
                }
                else
                {
                    var message = $"Not possible to retrieve grid consumption logs with 'GetGridConsumptionDetails'. Month: {request.Month}, Year: {request.Year}, GridID: {request.GridID}";
                    _logger.Error(message);
                    throw new Exception(message);
                }

            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetGridConsumptionDetails");
            }
        }
    }
}