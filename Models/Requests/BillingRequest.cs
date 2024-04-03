namespace CoolingGridManager.Models.Requests
{
    public class GetBillRequest
    {
        public required int? ConsumerID { get; set; }
        public required int? Month { get; set; }
        public required int? Year { get; set; }
    }

    public class DeleteBillRequest
    {
        public int? BillingId { get; set; }
    }

    public class BillStatusRequest
    {
        public int? BillingId { get; set; }
        public bool? IsPaid { get; set; }
    }


}