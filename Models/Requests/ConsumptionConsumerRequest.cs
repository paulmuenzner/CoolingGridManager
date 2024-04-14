namespace CoolingGridManager.Models.Requests
{
    public class IAddConsumerConsumptionRequest
    {
        public int ConsumerID { get; set; }
        public decimal ConsumptionValue { get; set; }

        // Defining the related time period where the value was measured for
        // The associated data is not tied to a specific time frame. This maintains the flexibility of the table. 
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
    }
}