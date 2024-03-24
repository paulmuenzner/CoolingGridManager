using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CoolingGridManager.Models;

public class ConsumerService
{
    private readonly AppDbContext _context; // Assuming you have a DbContext for data access
    private readonly ILogger<ConsumerService> _logger;

    public ConsumerService(AppDbContext context, ILogger<ConsumerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Consumer> CreateUserAsync(Consumer model)
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
