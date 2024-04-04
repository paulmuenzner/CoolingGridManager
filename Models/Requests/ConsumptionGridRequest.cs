namespace CoolingGridManager.Models.Requests
{
    public class GetGridConsumptionRequest
    {
        public int GridID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}