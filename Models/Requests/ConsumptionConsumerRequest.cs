namespace CoolingGridManager.Models.Requests
{
    public class IAddConsumerConsumptionRequest
    {
        public int ConsumerID { get; set; }
        public decimal ConsumptionValue { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
    }
}