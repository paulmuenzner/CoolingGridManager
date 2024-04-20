using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IServices
{
    /// <summary>
    /// Represents a service for managing grid section operations.
    /// </summary>
    public interface IGridSectionService
    {
        Task<GridSection> CreateGridSectionRecord(ICreateGridSectionRecordRequest request);
    }
}