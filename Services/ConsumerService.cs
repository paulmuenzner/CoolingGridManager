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
            {
                // Retrieve an existing grid section from the context
                var existingGridSection = await _context.GridSections.FirstOrDefaultAsync();
                if (existingGridSection == null)
                {
                    throw new InvalidOperationException("No existing grid section found.");
                }

                var consumer = new Consumer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CompanyName = request.CompanyName,
                    Email = request.Email,
                    Phone = request.Phone,
                    MonthlyBaseFee = request.MonthlyBaseFee,
                    UnitPrice = request.UnitPrice,
                    GridSection = existingGridSection,
                    GridSectionID = request.GridSectionID
                };

                _context.Consumers.Add(consumer);
                await _context.SaveChangesAsync();
                return consumer;
            }
            catch (Exception ex)
            {
                string message = string.Format($"Error using CreateConsumerRecord. Consumer ID: {request.ConsumerID}. Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "CreateConsumerRecord");
            }
        }

        ///////////////////////////////////////////
        // GET CONSUMER
        public async Task<Consumer> GetConsumerDetails(IGetConsumerRequest request)
        {
            try
            {
                int consumerID = request.ConsumerID;
                var consumer = await _context.Consumers.FindAsync(consumerID);
                if (consumer == null)
                {
                    var message = string.Format($"No consumer found with id {consumerID}. Result is null.");
                    throw new NotFoundException(message, "GetConsumerDetails", consumerID);
                }
                if (consumer != null)
                {
                    return consumer;
                }
                else
                {
                    _logger.Error($"With GetConsumerDetails requested consumer null. Non-existing consumer requested. Consumer ID: {consumerID}.");
                    throw new NotFoundException($"Requested consumer not found", "GetConsumerDetails", consumerID);
                }


            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString()) + $"Consumer ID: {request.ConsumerID}.";
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetConsumerDetails");
            }
        }

        ///////////////////////////////////////////
        // Get Consumers in Batches
        public async Task<List<Consumer>> GetConsumerBatch(IGetConsumerBatchRequest request)
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
                string message = string.Format("Exception: {ex}", ex.ToString()) + $"Error retrieving consumers in batches. Size: {request.Size}, Skip: {request.Skip}";
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetConsumerBatch");
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
                    string message = $"Non-existing user requested in GetConsumerWithGridSection. User ID: {id}";
                    _logger.Error(message);
                    throw new Exception(message);
                }

            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetConsumerWithGridSection");
            }
        }
    }
}



