using CoolingGridManager.IRequests;
using CoolingGridManager.Exceptions;
using CoolingGridManager.Models.Data;
using Microsoft.EntityFrameworkCore;
using CoolingGridManager.IServices;

namespace CoolingGridManager.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly AppDbContext _context;
        private readonly Serilog.ILogger _logger;

        public ConsumerService(AppDbContext context, Serilog.ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        ///////////////////////////////////////////
        // CREATE NEW CONSUMER
        public async Task<Consumer> CreateConsumerRecord(ICreateConsumerRecordRequest request)
        {
            try
            { // Prepare as validation
                // Retrieve an existing grid section from the context
                var existingGridSection = await _context.GridSections.FirstOrDefaultAsync();
                if (existingGridSection == null)
                {
                    // Handle the case where no existing grid section is found
                    throw new InvalidOperationException("No existing grid section found.");
                }
                // Assign the existing grid section to the consumer
                request.GridSection = existingGridSection;
                _context.Consumers.Add(request);
                await _context.SaveChangesAsync();
                return request;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating user"); // Prepare
                throw;
            }
        }

        ///////////////////////////////////////////
        // GET CONSUMER
        public async Task<Consumer> GetConsumerDetails(int consumerId)
        {
            try
            {
                var consumer = await _context.Consumers.FindAsync(consumerId);
                if (consumer == null)
                {
                    // return null; // prepare
                    var message = string.Format($"No consumer found with id {consumerId}. Result is null.");
                    throw new NotFoundException(message, "Consumer", consumerId);
                }
                if (consumer != null)
                {
                    return consumer;
                }
                else
                {
                    _logger.Error($"With GetConsumerDetails requested consumer null. Non-existing consumer requested. Consumer ID: {consumerId}.");
                    throw new NotFoundException($"Requested consumer not found", "GetConsumerDetails", consumerId);
                }


            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"With GetConsumerDetails requested consumer null. Consumer ID: {consumerId}.");
                throw new TryCatchException($"Bill for consumer ID {consumerId} not found", "GetConsumerDetails");
            }
        }

        ///////////////////////////////////////////
        // Get Consumers in Batches
        public async Task<List<Consumer>> GetConsumerBatch(IGetConsumerBatchRecord request)
        {
            try
            {
                var consumers = await _context.Consumers
                        .OrderBy(c => c.ConsumerID) // Order by primary key or other unique column
                        .Skip(request.Skip * request.Size)
                        .Take(request.Size)
                        .ToListAsync();
                if (consumers != null)
                {
                    return consumers;
                }
                else
                {
                    var message = $"Non-existing users requested in batches. Error retrieving consumers in batches. Size: {request.Size}, Skip: {request.Skip}";
                    _logger.Error(message);
                    throw new Exception(message);
                }

            }
            catch (Exception ex)
            {
                var message = $"Error retrieving consumers in batches. Size: {request.Size}, Skip: {request.Skip}";
                _logger.Error(ex, message);
                throw new Exception(message);
            }
        }

        ///////////////////////////////////////////
        // GET CONSUMER WITH RELATED GRID SECTION
        public async Task<Consumer> GetConsumerWithGridSection(int id)
        {
            try
            {
                var consumer = await _context.Consumers
                .Include(c => c.GridSection) // Include related GridSection data
                .FirstOrDefaultAsync(c => c.ConsumerID == id);
                if (consumer != null)
                {
                    return consumer;
                }
                else
                {
                    _logger.Error($"Non-existing user requested. User ID: {id}");
                    throw new Exception($"Consumer with ID: {id} not found");
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error retrieving consumer with ID: {id}");
                throw new Exception($"Consumer with ID: {id} not found");
            }
        }
    }
}



