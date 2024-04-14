namespace CoolingGridManager.Models.Requests
{
    public class IGetGridConsumptionRequest
    {
        public int GridID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}