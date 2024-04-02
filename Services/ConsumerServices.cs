using Microsoft.AspNetCore.Identity;
using CoolingGridManager.Exceptions;
using CoolingGridManager.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CoolingGridManager.Services
{
    public class ConsumerService
    {
        private readonly AppDbContext _context;
        private readonly Serilog.ILogger _logger;

        public ConsumerService(AppDbContext context, Serilog.ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        // ADD CONSUMER
        public async Task<Consumer> Add(Consumer consumer)
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
                consumer.GridSection = existingGridSection;
                _context.Consumers.Add(consumer);
                await _context.SaveChangesAsync();
                return consumer;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating user");
                throw;
            }
        }


        // GET CONSUMER
        public async Task<Consumer?> GetConsumerById(int consumerId)
        {
            // Prepare try catch
            var consumer = await _context.Consumers.FindAsync(consumerId);
            if (consumer == null)
            {
                // return null; // prepare
                var message = string.Format($"No consumer found with id {consumerId}. Result is null.");
                throw new NotFoundException(message, "Consumer", consumerId);
            }
            return consumer;
        }

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


// using CoolingGridManager.Models;

// namespace CoolingGridManager.Services
// {
//     public class ConsumerService
//     {
//         private readonly AppDbContext _dbContext;
//         private readonly ILogger<AppDbContext> _logger;
//         public ConsumerService(AppDbContext dbContext, ILogger<AppDbContext> logger)
//         {
//             _dbContext = dbContext;
//             _logger = logger;
//         }

//         public int CreateConsumer(Consumer consumer)
//         {
//             _logger.LogInformation("Executing SomeMethod...");
//             _dbContext.Consumers.Add(consumer);
//             _dbContext.SaveChanges();
//             return consumer.ConsumerID; // Return the ID of the newly created consumer
//         }
//     }
// }
