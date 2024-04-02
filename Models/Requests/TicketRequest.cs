namespace CoolingGridManager.Models.Requests
{
    public class GetTicketByIDRequest
    {
        public required int? TicketId { get; set; }
    }

    public class UpdateTicketStatusRequest
    {
        public required int TicketId { get; set; }
        public required string Status { get; set; }
    }
}