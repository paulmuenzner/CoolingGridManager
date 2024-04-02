namespace CoolingGridManager.Models.Requests
{
    public class AddConsumptionRequest
    {
        public int? ConsumerID { get; set; }
        public decimal? ConsumptionValue { get; set; }
        public DateTime? ConsumptionDate { get; set; }
    }
}