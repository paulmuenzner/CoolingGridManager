using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IServices
{
    /// <summary>
    /// Represents a service for managing grid operations.
    /// </summary>
    public interface IGridService
    {
        Task<Grid> CreateGridRecord(string gridName);
    }
}