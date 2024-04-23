using CoolingGridManager.Models.Data;
using static CoolingGridManager.Models.Data.GridParameterLog;

namespace CoolingGridManager.IRequests
{
    public class ICreateGridParameterLogRecordRequest
    {
        public required List<CreateGridParameterLogDto> GridParameterData { get; set; }
    }


    public class IGetMonthlyGridParameterDetailsRequest
    {
        public int GridID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}