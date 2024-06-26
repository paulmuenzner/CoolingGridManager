using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IRequests
{
    public class IGetConsumerBatchRequest
    {
        public required int Skip { get; set; }
        public required int Size { get; set; }
    }

    public class IGetConsumerRequest
    {
        public required int ConsumerID { get; set; }
    }

    public class ICreateConsumerRecordRequest : CreateConsumerDto
    {
    }

}