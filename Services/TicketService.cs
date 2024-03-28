using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models;
using CoolingGridManager.Exceptions;
using CoolingGridManager.ResponseHandler;
using Microsoft.EntityFrameworkCore;

namespace CoolingGridManager.Services
{
    public class TicketService
    {
        private readonly AppDbContext _context;

        public TicketService(AppDbContext context)
        {
            _context = context;
        }

        // ADD Ticket
        public async Task<TicketModel> AddTicket(TicketModel ticket)
        {
            try
            {
                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();
                return ticket;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "AddTicket");
            }
        }

        // GET TICKET
        public async Task<TicketModel> GetTicketById(int ticketId)
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

        // CHANGE STATUS 
        public async Task<TicketModel> UpdateStatusTicket(int ticketId, string status)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(ticketId);

                if (ticket == null)
                {
                    throw new NotFoundException($"Ticket with ID {ticketId} not found.", "UpdateStatusTicket", ticketId);
                }

                // Update the ticket status
                ticket.Status = status;

                // Add a new status change object to the status history array
                ticket.StatusHistory.Add(new StatusChange
                {
                    Status = "Solved",
                    ChangedDate = DateTime.UtcNow
                });

                // Update the ticket in the database
                await _context.SaveChangesAsync();
                return ticket;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "UpdateStatusTicket");
            }
        }
    }
}