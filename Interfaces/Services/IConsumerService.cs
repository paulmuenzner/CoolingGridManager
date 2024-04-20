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
        Task<Consumer> GetConsumerDetails(int consumerId);
        Task<List<Consumer>> GetConsumerBatch(IGetConsumerBatchrequest request);
        Task<Consumer> GetConsumerWithGridSection(int id);
    }
}