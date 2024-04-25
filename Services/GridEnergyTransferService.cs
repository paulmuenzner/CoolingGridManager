using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using Microsoft.EntityFrameworkCore;
using CoolingGridManager.IServices;

namespace CoolingGridManager.Services
{
    public class GridEnergyTransferService : IGridEnergyTransferService
    {
        private readonly AppDbContext _context;
        private readonly Serilog.ILogger _logger;
        public GridEnergyTransferService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        ///////////////////////////////////////////
        // ADD VALUE FOR ENERGY TRANSFER
        public async Task<GridEnergyTransfer> CreateGridEnergyTransferRecord(ICreateGridEnergyTransferRecordRequest request)
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

                var log = new GridEnergyTransfer
                {
                    Grid = grid,
                    Month = request.Month,
                    Year = request.Year,
                    EnergyTransfer = request.EnergyTransfer
                };
                _context.GridEnergyTransfers.Add(log);
                await _context.SaveChangesAsync();

                return log;
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "CreateGridEnergyTransferRecord");
            }
        }

        ///////////////////////////////
        // ENTRY EXISTS?
        public async Task<bool> DoesGridEnergyTransferEntryExist(IGetGridDataRequest request)
        {
            return await _context.GridEnergyTransfers
                .AnyAsync(g => g.GridID == request.GridID && g.Month == request.Month && g.Year == request.Year);
        }


        //////////////////////////////////////////////////
        // GET ENERGY TRANSFER DETAILS FOR GRID 
        public async Task<GridEnergyTransfer> GetGridEnergyTransferDetails(IGetGridDataRequest request)
        {
            try
            {
                var logs = await _context.GridEnergyTransfers
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
                    var message = $"Not possible to retrieve the grid's energy transfer logs with 'GetGridEnergyTransferDetails'. Month: {request.Month}, Year: {request.Year}, GridID: {request.GridID}";
                    _logger.Error(message);
                    throw new Exception(message);
                }

            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetGridEnergyTransferDetails");
            }
        }

        internal async Task CreateGridEfficiencyRecord(GridEfficiency record)
        {
            throw new NotImplementedException();
        }
    }
}