using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models;
using Microsoft.EntityFrameworkCore;
using CoolingGridManager.Exceptions;
using FormatException = System.FormatException;

namespace CoolingGridManager.Services
{
    public class GridSectionService
    {
        private readonly AppDbContext _context;

        public GridSectionService(AppDbContext context)
        {
            _context = context;
        }

        // ADD GRID
        public async Task<int> AddGridSecion(GridSection gridSection)
        {
            try
            {
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