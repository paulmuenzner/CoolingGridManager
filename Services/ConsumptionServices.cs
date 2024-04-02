using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using CoolingGridManager.Models.Requests;

namespace CoolingGridManager.Services
{
    public class ConsumptionService
    {
        private readonly AppDbContext _context;

        private readonly Serilog.ILogger _logger;
        public ConsumptionService(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        // ADD CONSUMPTION VALUE
        public async Task<int> AddConsumption(AddConsumptionRequest request)
        {
            try
            {
                var consumptionLog = new CoolingGridManager.Models.Data.ConsumptionLog
                {
                    ConsumerID = request.ConsumerID ?? 0,
                    ConsumptionValue = request.ConsumptionValue ?? 0m,
                    LogDate = DateTime.Today,
                    ConsumptionDate = request.ConsumptionDate ?? DateTime.Today
                };
                _context.ConsumptionLogs.Add(consumptionLog);
                await _context.SaveChangesAsync();

                return consumptionLog.LogId;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "AddConsumption");
            }
        }
    }
}