namespace CoolingGridManager.Models.Requests
{
    public class IGetTicketByIDRequest
    {
        public required int? TicketId { get; set; }
    }

    public class IUpdateTicketStatusRequest
    {
        public required int TicketId { get; set; }
        public required string Status { get; set; }
    }
}