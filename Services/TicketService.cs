using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using Microsoft.EntityFrameworkCore;
using CoolingGridManager.IRequests;

namespace CoolingGridManager.Services
{
    public class TicketService
    {
        private readonly AppDbContext _context;
        private readonly Serilog.ILogger _logger;
        public TicketService(AppDbContext context, Serilog.ILogger logger)
        {
            _context = context;
            _logger = logger;
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
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
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
                    throw new NotFoundException(message, "GetTicketDetails", ticketId);
                }
                return ticket;
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetTicketDetails");
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
                string message = string.Format("Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "UpdateTicketStatus");
            }
        }
    }
}