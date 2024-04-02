using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models.Data;
using Microsoft.EntityFrameworkCore;
using CoolingGridManager.Exceptions;
using FormatException = System.FormatException;

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

        // ADD GRID
        public async Task<int> AddGridSection(GridSection gridSection)
        {
            try
            {
                var existingGrid = await _context.Grids.FindAsync(gridSection.GridID);

                if (existingGrid == null)
                {
                    _logger.Information($"Grid with ID {gridSection.GridID} does not exist.");
                    // Handle the case where the provided grid ID does not exist
                    throw new FormatException($"Grid with ID {gridSection.GridID} does not exist.");
                }
                // Associate the existing grid with the new grid section
                gridSection.Grid = existingGrid;

                _context.GridSections.Add(gridSection);
                await _context.SaveChangesAsync();
                return gridSection.GridSectionID;
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