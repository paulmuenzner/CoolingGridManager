using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using CoolingGridManager.IRequests;
using Microsoft.EntityFrameworkCore;

namespace CoolingGridManager.Services
{
    public class GridEfficiencyService
    {
        private readonly AppDbContext _context;
        private readonly Serilog.ILogger _logger;
        public GridEfficiencyService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        /////////////////////////////////////
        // CREATE GRID EFFICIENCY RECORD
        public async Task<Grid> CreateGridEfficiencyRecord(ICreateGridRequest request)
        {
            try
            {
                var grid = new GridEfficiency { GridName = request.GridName };
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

        ///////////////////////////////////////////
        // Get Grids in Batches
        public async Task<List<Grid>> GetGridBatch(IGetGridBatchRequest request)
        {
            try
            {
                var grids = await _context.Grids
                        .OrderBy(g => g.GridID)
                        .Skip(request.Skip * request.Size)
                        .Take(request.Size)
                        .ToListAsync();
                if (grids != null)
                {
                    return grids;
                }
                else
                {
                    var message = $"Non-existing grids requested in batches using GetGridBatch. Error retrieving grids in batches. Size: {request.Size}, Skip: {request.Skip}";
                    _logger.Error(message);
                    throw new Exception(message);
                }

            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString()) + $"Error retrieving grids in batches. Size: {request.Size}, Skip: {request.Skip}";
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetGridBatch");
            }
        }

    }
}