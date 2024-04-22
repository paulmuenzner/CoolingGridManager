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
                    var existingGrid = await _context.Grids.FindAsync(data.GridID);

                    if (existingGrid == null)
                    {
                        _logger.Warning($"Grid with ID {data.GridID} does not exist.");
                        throw new FormatException($"Grid with ID {data.GridID} does not exist.", "CreateGridParameterLogRecord");
                    }
                    // Associate the existing grid with the new grid section
                    data.Grid = existingGrid;

                    _context.GridParameterLog.Add(data);
                    await _context.SaveChangesAsync();
                }
                IParameterLogResponse response = new IParameterLogResponse { Success = true, Count = request.GridParameterData.Count };
                return response;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "CreateGridParameterLogRecord"); //prepare update function names everywhere
            }
        }

        ////////////////////////////////////////////////////
        // GET GRID PARAMETER ENTRIES BY MONTH
        public async Task<List<GridParameterLog>> GetMonthlyGridParameterDetails(IGetMonthlyGridParameterDetailsRequest request)
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
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "GetMonthlyGridParameterDetails");
            }
        }
    }
}