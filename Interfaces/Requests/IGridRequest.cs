
namespace CoolingGridManager.IRequests
{
    public class ICreateGridRequest
    {
        public required string GridName { get; set; }
    }

    public class IGetGridBatchRequest
    {
        public required int Skip { get; set; }
        public required int Size { get; set; }
    }
}