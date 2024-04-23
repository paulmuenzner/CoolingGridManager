using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IServices
{
    /// <summary>
    /// Represents a service for managing grid consumption operations.
    /// </summary>
    public interface IConsumptionGridService
    {
        Task<ConsumptionGrid> CreateGridConsumptionRecord(ICreateGridConsumptionRecordRequest request);
        Task<ConsumptionGrid> GetGridConsumptionDetails(IGetGridDataRequest request);
        Task<bool> DoesGridConsumptionEntryExist(IGetGridDataRequest request);
    }
}