using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using FormatException = CoolingGridManager.Exceptions.FormatException;
using CoolingGridManager.IRequests;
using Microsoft.EntityFrameworkCore;

namespace CoolingGridManager.Services
{
    public class GridSectionService
    {
        private readonly AppDbContext _context;
        private readonly Serilog.ILogger _logger;

        public GridSectionService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        ////////////////////////////////////////////////////
        // CREATE GRID SECTION RECORD
        public async Task<GridSection> CreateGridSectionRecord(ICreateGridSectionRecordRequest request)
        {
            try
            {
                // Retrieve the grid associated with the new grid section entry, as it is essential for maintaining data integrity.
                var existingGrid = await _context.Grids.FindAsync(request.GridID);

                if (existingGrid == null)
                {
                    _logger.Information($"Grid with ID {request.GridID} does not exist.");
                    throw new FormatException($"Grid with ID {request.GridID} does not exist.", "CreateGridSectionRecord");
                }
                // Associate the existing grid with the new grid section
                request.Grid = existingGrid;

                _context.GridSections.Add(request);
                await _context.SaveChangesAsync();
                return request;
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "CreateGridSectionRecord");
            }
        }

        ////////////////////////////////////////////////////
        // GET GRID SECTION RECORDS BY GRID ID
        public async Task<List<GridSection>> GetGridSectionRecords(int gridID)
        {
            try
            {
                // Retrieve the grid sections associated with GridID
                List<GridSection> existingGrid = await _context.GridSections.Where(gs => gs.GridID == gridID).ToListAsync();

                if (existingGrid == null)
                {
                    _logger.Information($"Grid sections with Grid ID {gridID} not found with GetGridSectionRecords.");
                    return new List<GridSection>();
                }

                return existingGrid;
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetGridSectionRecords");
            }
        }
    }
}