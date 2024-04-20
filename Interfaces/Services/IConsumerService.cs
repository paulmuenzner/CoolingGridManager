using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IServices
{
    /// <summary>
    /// Represents a service for managing consumer operations.
    /// </summary>
    public interface IConsumerService
    {
        Task<Consumer> CreateConsumerRecord(Consumer request);
        Task<Consumer> GetConsumerDetails(int consumerId);
        Task<List<Consumer>> GetConsumerBatch(IGetConsumerBatch request);
        Task<Consumer> GetConsumerWithGridSection(int id);
    }
}