namespace CoolingGridManager.IRequests
{
    //////////////////////////////////////////////////////////////////////////////////////////////
    // Create Consumer Consumption for logging external data lists collected by meter over time
    public class ICreateConsumerConsumptionRequest
    {
        public required List<ConsumptionData> ConsumptionDataList { get; set; }
    }

    public class ConsumptionData
    {
        public int ConsumerID { get; set; }
        public required string ElementID { get; set; }
        public decimal ConsumptionValue { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
    }
}