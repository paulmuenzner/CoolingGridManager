using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;

namespace CoolingGridManager.Services
{
    public class GridService
    {
        private readonly AppDbContext _context;

        public GridService(AppDbContext context)
        {
            _context = context;
        }

        /////////////////////////////////////
        // CREATE GRID RECORD
        public async Task<Grid> CreateGridRecord(string gridName)
        {
            try
            {
                var grid = new Grid { GridName = gridName };
                _context.Grids.Add(grid);
                await _context.SaveChangesAsync();
                return grid;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "CreateGridRecord");
            }
        }
    }
}