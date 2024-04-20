using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IServices
{
    /// <summary>
    /// Represents a service for managing consumer consumption operations.
    /// </summary>
    public interface IConsumptionConsumerService
    {
        Task<ConsumptionConsumer> CreateConsumerConsumptionRecord(ICreateConsumerConsumptionRequest request);
        Task<List<ConsumptionConsumer>> GetConsumptionForUserByMonth(IGetConsumptionForUserByMonthRequest request);

    }
}