using CoolingGridManager.IRequests;
using CoolingGridManager.IResponse;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IServices
{
    /// <summary>
    /// Represents a service for managing consumer consumption operations.
    /// </summary>
    public interface IConsumptionConsumerService
    {
        Task<decimal> GetEntireConsumerConsumptionForGrid(IGetGridDataRequest request);
        Task<ICreateConsumerConsumptionRecordResponse> CreateConsumerConsumptionRecord(List<ConsumptionData> request);
        Task<List<ConsumptionConsumer>> GetConsumptionForUserByMonth(IGetBillByConsumerRequest request);

    }
}