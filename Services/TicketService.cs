using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using Microsoft.EntityFrameworkCore;
using CoolingGridManager.IRequests;

namespace CoolingGridManager.Services
{
    public class TicketService
    {
        private readonly AppDbContext _context;

        public TicketService(AppDbContext context)
        {
            _context = context;
        }

        ////////////////////////////////
        // CREATE TICKET RECORD
        public async Task<TicketModel> CreateTicketRecord(ICreateTicketRecordRequest request)
        {
            try
            {
                _context.Tickets.Add(request);
                await _context.SaveChangesAsync();
                return request;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "CreateTicketRecord");
            }
        }

        ////////////////////////////////
        // GET TICKET
        public async Task<TicketModel> GetTicketDetails(int ticketId)
        {
            try
            {
                var ticket = await _context.Tickets
                    .Include(t => t.StatusHistory)
                    .FirstOrDefaultAsync(t => t.TicketId == ticketId);
                if (ticket == null)
                {
                    var message = string.Format($"No ticket found with ID {ticketId}.");
                    throw new NotFoundException(message, "GetTicketById", ticketId);
                }
                return ticket;
            }
            catch (ArgumentNullException ex)
            {
                throw new NotFoundException(ex.Message, "GetTicketById", ticketId);
            }
        }

        ///////////////////////////////////////////
        // UPDATE TICKET STATUS 
        public async Task<TicketModel> UpdateTicketStatus(IUpdateTicketStatusRequest request)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(request.TicketID);

                if (ticket == null)
                {
                    throw new NotFoundException($"Ticket with ID {request.TicketID} not found.", "UpdateTicketStatus", request.TicketID);
                }

                // Update the ticket status
                ticket.Status = request.NewStatus;

                // Add a new status change object to the status history array
                ticket.StatusHistory.Add(new StatusChange
                {
                    Status = request.NewStatus,
                    ChangedDate = DateTime.UtcNow
                });

                // Update the ticket in the database
                await _context.SaveChangesAsync();
                return ticket;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "UpdateTicketStatus");
            }
        }
    }
}