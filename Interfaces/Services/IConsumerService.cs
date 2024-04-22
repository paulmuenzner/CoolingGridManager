using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IServices
{
    /// <summary>
    /// Represents a service for managing consumer operations.
    /// </summary>
    public interface IConsumerService
    {
        Task<Consumer> CreateConsumerRecord(ICreateConsumerRecordRequest request);
        Task<Consumer> GetConsumerDetails(IGetConsumerRequest request);
        Task<List<Consumer>> GetConsumerBatch(IGetConsumerBatchRequest request);
        Task<Consumer> GetConsumerWithGridSection(int id);
    }
}