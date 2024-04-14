namespace CoolingGridManager.Models.Requests
{
    public class IGetGridParameterRequest
    {
        public int GridID { get; set; }
        public int Year { get; set; }
    }


    public class IGetParameterLogsRequest
    {
        public int GridID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}