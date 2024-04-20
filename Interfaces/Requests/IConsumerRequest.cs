namespace CoolingGridManager.IRequests
{
    public class IGetConsumerBatch
    {
        public required int Skip { get; set; }
        public required int Size { get; set; }
    }

}