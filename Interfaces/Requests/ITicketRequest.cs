using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IRequests
{
    public class ICreateTicketRecordRequest : TicketModel
    {
    }

    public class IUpdateTicketStatusRequest
    {
        public int TicketID { get; set; }
        public required string NewStatus { get; set; }
    }

}