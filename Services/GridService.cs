using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using CoolingGridManager.IRequests;

namespace CoolingGridManager.Services
{
    public class GridService
    {
        private readonly AppDbContext _context;
        private readonly Serilog.ILogger _logger;
        public GridService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        /////////////////////////////////////
        // CREATE GRID RECORD
        public async Task<Grid> CreateGridRecord(ICreateGridRequest request)
        {
            try
            {
                var grid = new Grid { GridName = request.GridName };
                _context.Grids.Add(grid);
                await _context.SaveChangesAsync();
                return grid;
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "CreateGridRecord");
            }
        }
    }
}