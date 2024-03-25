using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CoolingGridManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolingGridManager.Services
{
    public class ConsumerService
    {
        private readonly AppDbContext _context;

        private readonly ILogger<ConsumerService> _logger;

        public ConsumerService(AppDbContext context, ILogger<ConsumerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // CREATE CONSUMER
        public async Task<Consumer> CreateConsumer(Consumer model)
        {
            try
            {
                _context.Consumers.Add(model);
                await _context.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                throw;
            }
        }


        // GET CONSUMER

        public async Task<Consumer> GetConsumerById(int consumerId)
        {
            var consumer = await _context.Consumers.FindAsync(consumerId);
            if (consumer == null)
            {

                var message = string.Format($"No consumer found with id {consumerId}. Result is null.");
                throw new NotFoundException(message, "Consumer", consumerId);

            }
            return consumer;
            // try
            // {
            //     // Use FindAsync to retrieve the consumer by ID
            //     var consumer = await _context.Consumers.FindAsync(consumerId);

            //     if (consumer == null)
            //     {
            //         // Handle the case where the consumer with the specified ID is not found
            //         throw new Exception("Consumer not found");
            //     }

            //     return consumer;
            // }
            // catch (Exception ex)
            // {
            //     _logger.LogError(ex, $"Error retrieving consumer with consumerId: {consumerId}");
            //     throw new Exception($"Consumer with ID: {consumerId} not found");
            // }
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
                    _logger.LogError($"Non-existing user requested. User ID: {id}");
                    throw new Exception($"Consumer with ID: {id} not found");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving consumer with ID: {id}");
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
