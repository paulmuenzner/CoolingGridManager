namespace CoolingGridManager.Models.Requests
{
    public class IGetBillRequest
    {
        public required int? ConsumerID { get; set; }
        public required int? Month { get; set; }
        public required int? Year { get; set; }
    }

    public class IDeleteBillRequest
    {
        public int? BillingId { get; set; }
    }

    public class IBillStatusRequest
    {
        public int? billingId { get; set; }
        public bool? isPaid { get; set; }
    }
}