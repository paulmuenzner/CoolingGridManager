using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using Microsoft.EntityFrameworkCore;
using CoolingGridManager.IServices;
using CoolingGridManager.IRequests;

namespace CoolingGridManager.Services
{
    public class BillingService : IBillingService
    {
        private readonly AppDbContext _context;
        private readonly Serilog.ILogger _logger;

        public BillingService(AppDbContext context, Serilog.ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        ////////////////////////////
        // ADD NEW BILL
        public async Task<int> CreateBillingRecord(Billing request)
        {
            try
            {
                _context.Bills.Add(request);
                await _context.SaveChangesAsync();
                return request.BillingId;
            }
            catch (Exception ex)
            {
                string message = string.Format("Error creating new bill. Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "CreateBillingRecord");
            }
        }

        ////////////////////////////
        // GET BILL DETAILS 
        public async Task<Billing> GetBillingDetails(IGetBillByConsumerRequest request)
        {
            try
            {
                var bill = await _context.Bills
                    .FirstOrDefaultAsync(b =>
                    b.ConsumerID == request.ConsumerID &&
                    b.BillingMonth == request.BillingMonth &&
                    b.BillingYear == request.BillingYear);

                if (bill != null)
                {
                    return bill;
                }
                else
                {
                    _logger.Error($"Requested bill null. Non-existing bill requested. Consumer ID: {request.ConsumerID}, Month: {request.BillingMonth}, Year: {request.BillingYear}");
                    throw new NotFoundException($"Requested bill not found", "GetBillingDetails", request);
                }

            }
            catch (Exception ex)
            {
                string message = string.Format($"Exception retrieving consumer bill. Consumer ID: {request.ConsumerID}, Month: {request.BillingMonth}, Year: {request.BillingYear}. Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "GetBillingDetails");
            }
        }

        ////////////////////////////
        // Bill Exists?
        public async Task<bool> DoesBillingEntryExist(IGetBillByConsumerRequest request)
        {
            return await _context.Bills
                .AnyAsync(b => b.ConsumerID == request.ConsumerID && b.BillingMonth == request.BillingMonth && b.BillingYear == request.BillingYear);
        }

        // Delete bill 
        public async Task<bool> DeleteBillingEntry(int billingId)
        {
            try
            {
                var billing = await _context.Bills.FindAsync(billingId);
                if (billing != null)
                {
                    _context.Bills.Remove(billing);
                    await _context.SaveChangesAsync();
                    return true;
                }
                _logger.Error($"Error deleting bill with ID: {billingId}. Bill not found.");
                return false;
            }
            catch (Exception ex)
            {
                string message = string.Format($"Error deleting bill with ID: {billingId}. Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "DeleteBillingEntry");
            }
        }

        // Update payment status
        public async Task<bool> UpdatePaymentStatus(IUpdateStatusRequest request)
        {
            try
            {
                var bill = await _context.Bills.FindAsync(request.BillingId);

                if (bill == null)
                {
                    throw new NotFoundException($"Bill with ID {request.BillingId} not found.", "Payment status", request.BillingId);
                }

                bill.IsPaid = request.IsPaid;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                string message = string.Format($"Error updating payment status to {request.IsPaid} of bill with ID: {request.BillingId}. Billing ID: {request.BillingId}. Exception: {ex}", ex.ToString());
                _logger.Error(ex, message);
                throw new TryCatchException(message, "CreateConsumerRecord");
            }
        }
    }
}