namespace CoolingGridManager.IRequests
{
    public class IGetBillRequest
    {
        public required int ConsumerID { get; set; }
        public required int BillingMonth { get; set; }
        public required int BillingYear { get; set; }
    }


    public class IDeleteBillRequest
    {
        public int BillingId { get; set; }
    }


    public class IUpdateStatusRequest
    {
        public int BillingId { get; set; }
        public bool IsPaid { get; set; }
    }


    public class IBillStatusRequest
    {
        public int billingId { get; set; }
        public bool isPaid { get; set; }
    }
}