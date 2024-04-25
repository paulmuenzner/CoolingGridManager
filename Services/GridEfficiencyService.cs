using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using CoolingGridManager.IRequests;
using Microsoft.EntityFrameworkCore;
using FormatException = CoolingGridManager.Exceptions.FormatException;

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
        public async Task<GridEfficiency> CreateGridEfficiencyRecord(ICreateGridEfficiencyRequest request)
        {
            try
            {
                // Retrieve an existing grid from the context<
                var GridID = request.GridID;
                var relatedGrid = await _context.Grids.FindAsync(GridID);

                if (relatedGrid == null)
                {
                    _logger.Warning($"Grid with ID {GridID} does not exist.");
                    throw new FormatException($"Grid with ID {GridID} does not exist.", "CreateGridEfficiencyRecord");
                }

                var record = new GridEfficiency
                {
                    EfficiencyRelative = request.EfficiencyRelative,
                    Month = request.Month,
                    Year = request.Year,
                    Grid = relatedGrid
                };
                _context.GridEfficiencies.Add(record);
                await _context.SaveChangesAsync();
                return record;
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "CreateGridEfficiencyRecord");
            }
        }


        /////////////////////////////////////
        // GET GRID EFFICIENCY RECORD
        public async Task<GridEfficiency> GetGridEfficiency(IGetGridDataRequest request)
        {
            try
            {
                // Retrieve data from GridEfficiency table
                var gridEfficiency = await _context.GridEfficiencies.FirstOrDefaultAsync(x =>
                        x.GridID == request.GridID &&
                        x.Month == request.Month &&
                        x.Year == request.Year);

                if (gridEfficiency == null)
                {
                    _logger.Warning($"Entry with grid ID {request.GridID} does not exist.");
                    throw new FormatException($"Entry with grid ID {request.GridID} does not exist.", "GetGridEfficiency");
                }
                return gridEfficiency;

            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetGridEfficiency");
            }
        }

    }
}


