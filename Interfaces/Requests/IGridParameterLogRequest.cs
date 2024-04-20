using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IRequests
{
    public class ICreateGridParameterLogRecordRequest : GridParameterLog
    {
    }


    public class IGetMonthlyGridParameterDetailsRequest
    {
        public int GridID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}