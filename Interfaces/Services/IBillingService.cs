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
        Task<Billing> GetBillingDetails(IGetBillByConsumerRequest request);
        Task<bool> DoesBillingEntryExist(IGetBillByConsumerRequest request);
        Task<bool> DeleteBillingEntry(int request);
        Task<bool> UpdatePaymentStatus(IUpdateStatusRequest request);
    }
}