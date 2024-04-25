using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IServices
{
    /// <summary>
    /// Represents a service for managing grid parameter log operations.
    /// </summary>
    public interface IGridParameterLogService
    {
        Task<GridParameterLog> CreateGridParameterLogRecord(ICreateGridParameterLogRecordRequest request);
        Task<GridEnergyTransfer> GetMonthlyGridParameterDetails(IGetMonthlyGridParameterDetailsRequest request);

    }
}