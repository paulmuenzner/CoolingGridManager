using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IServices
{
    /// <summary>
    /// Represents a service for managing grid energy transfer operations
    /// </summary>
    public interface IGridEnergyTransferService
    {
        Task<GridEnergyTransfer> CreateGridEnergyTransferRecord(ICreateGridEnergyTransferRecordRequest request);
        Task<GridEnergyTransfer> GetGridEnergyTransferDetails(IGetGridDataRequest request);
        Task<bool> DoesGridEnergyTransferEntryExist(IGetGridDataRequest request);
    }
}