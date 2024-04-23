using static CoolingGridManager.Models.Data.ConsumptionGrid;

namespace CoolingGridManager.IRequests
{
    public class IGetGridDataRequest
    {
        public int GridID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class ICreateGridConsumptionRecordRequest : CreateConsumptionGridDto
    {
    }

}