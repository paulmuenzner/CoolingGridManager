using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IRequests
{
    public class IGetGridConsumptionRequest
    {
        public int GridID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }


    public class ICreateGridConsumptionRecordRequest : ConsumptionGrid
    {
    }
}