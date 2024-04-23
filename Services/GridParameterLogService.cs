using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using Microsoft.EntityFrameworkCore;
using FormatException = CoolingGridManager.Exceptions.FormatException;
using CoolingGridManager.IRequests;
using CoolingGridManager.IResponse;

namespace CoolingGridManager.Services
{
    public class GridParameterLogService
    {
        private readonly AppDbContext _context;
        private readonly Serilog.ILogger _logger;
        public GridParameterLogService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        ///////////////////////////////////////////
        // CREATE GRID PARAMETER ENTRY
        public async Task<IParameterLogResponse> CreateGridParameterLogRecord(ICreateGridParameterLogRecordRequest request)
        {
            try
            {
                foreach (var data in request.GridParameterData)
                {
                    var GridID = data.GridID;
                    var existingGrid = await _context.Grids.FindAsync(GridID);

                    if (existingGrid == null)
                    {
                        _logger.Warning($"Grid with ID {GridID} does not exist.");
                        throw new FormatException($"Grid with ID {GridID} does not exist.", "CreateGridParameterLogRecord");
                    }
                    // Associate the existing grid with the new grid section
                    var input = new GridParameterLog
                    {
                        ElementID = data.ElementID,
                        MassFlowRate = data.MassFlowRate,
                        SpecificHeatCapacity = data.SpecificHeatCapacity,
                        MeanTemperatureIn = data.MeanTemperatureIn,
                        MeanTemperatureOut = data.MeanTemperatureOut,
                        DateTimeStart = data.DateTimeStart,
                        DateTimeEnd = data.DateTimeEnd,
                        Grid = existingGrid
                    };
                    _context.GridParameterLog.Add(input);
                    await _context.SaveChangesAsync();
                }
                IParameterLogResponse response = new IParameterLogResponse { Success = true, Count = request.GridParameterData.Count };
                return response;
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "CreateGridParameterLogRecord");
            }
        }

        ////////////////////////////////////////////////////
        // GET GRID PARAMETER ENTRIES BY MONTH
        public async Task<List<GridParameterLog>> GetMonthlyGridParameterDetails(IGetGridDataRequest request)
        {
            try
            {
                // All entries of current month
                var startDate = new DateTimeOffset(request.Year, request.Month, 1, 0, 0, 0, TimeSpan.Zero);
                var endDate = startDate.AddMonths(1).AddTicks(-1);

                var logs = await _context.GridParameterLog
                    .Where(log =>
                        log.GridID == request.GridID &&
                        log.DateTimeStart >= startDate &&
                        log.DateTimeEnd <= endDate).ToListAsync();

                if (logs != null)
                {
                    return logs;
                }
                else
                {
                    var message = $"Not possible to retrieve consumption logs with 'GetMonthlyGridParameterDetails'. Month: {request.Month}, Skip: {request.GridID}";
                    _logger.Error(message);
                    throw new Exception(message);
                }

            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetMonthlyGridParameterDetails");
            }
        }
    }
}