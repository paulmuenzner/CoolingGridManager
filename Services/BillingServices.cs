using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;

namespace CoolingGridManager.Services
{
    public class BillingService
    {
        private readonly AppDbContext _context;

        public BillingService(AppDbContext context)
        {
            _context = context;
        }

        // ADD BILL
        public async Task<int> AddBill(string gridName)
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
                throw new TryCatchException(message, "AddBill");
            }
        }
    }
}