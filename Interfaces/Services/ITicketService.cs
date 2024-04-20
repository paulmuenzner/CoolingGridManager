using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.IServices
{
    /// <summary>
    /// Represents a service for managing ticket operations.
    /// </summary>
    public interface ITicketService
    {
        Task<TicketModel> CreateTicketRecord(ICreateTicketRecordRequest request);
        Task<TicketModel> GetTicketDetails(int ticketId);
        Task<TicketModel> UpdateTicketStatus(IUpdateTicketStatusRequest request);
    }
}