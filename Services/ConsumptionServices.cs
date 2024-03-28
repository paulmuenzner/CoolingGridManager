using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models;
using CoolingGridManager.Exceptions;

namespace CoolingGridManager.Services
{
    public class ConsumptionServices
    {
        private readonly AppDbContext _context;

        private readonly Serilog.ILogger _logger;
        public ConsumptionServices(AppDbContext context, Serilog.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }

        // ADD CONSUMPTION
        public async Task<int> AddConsumption(ConsumptionLog consumptionLog)
        {
            try
            {
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