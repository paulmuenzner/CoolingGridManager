namespace CoolingGridManager.Models.Requests
{
    public class GetBillRequest
    {
        public required int? ConsumerID { get; set; }
        public required int? Month { get; set; }
        public required int? Year { get; set; }
    }

}