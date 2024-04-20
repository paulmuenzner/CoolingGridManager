using ILogger = Serilog.ILogger;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using FormatException = System.FormatException;
using CoolingGridManager.IRequests;

namespace CoolingGridManager.Services
{
    public class GridSectionService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GridSectionService> _logger;

        public GridSectionService(AppDbContext context, ILogger<GridSectionService> logger)
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
                    _logger.LogInformation($"Grid with ID {request.GridID} does not exist.");
                    throw new FormatException($"Grid with ID {request.GridID} does not exist.");
                }
                // Associate the existing grid with the new grid section
                request.Grid = existingGrid;

                _context.GridSections.Add(request);
                await _context.SaveChangesAsync();
                return request;
            }
            catch (FormatException ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new FormatException(message);
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "AddGridSection");
            }
        }
    }
}