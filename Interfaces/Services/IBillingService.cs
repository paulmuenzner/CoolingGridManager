using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IServices
{
    /// <summary>
    /// Represents a service for managing billing operations.
    /// </summary>
    public interface IBillingService
    {
        Task<int> CreateBillingRecord(Billing request);
        Task<Billing> GetBillingDetails(IGetBillRequest request);
        Task<bool> DoesBillingEntryExist(IGetBillRequest request);
        Task<bool> DeleteBillingEntry(int request);
        Task<bool> UpdatePaymentStatus(IUpdateStatusRequest request);
    }
}