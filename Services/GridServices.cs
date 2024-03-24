using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolingGridManager.Services
{
    public class GridService
    {
        private readonly AppDbContext _context;

        public GridService(AppDbContext context)
        {
            _context = context;
        }

        // ADD GRID
        public async Task<int> AddGrid(string gridName)
        {
            try
            {
                var grid = new Grid { GridName = gridName };
                _context.Grids.Add(grid);
                await _context.SaveChangesAsync();
                return grid.GridID;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "AddGrid");
            }
        }
    }
}