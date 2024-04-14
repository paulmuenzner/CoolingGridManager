using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Exceptions;
using CoolingGridManager.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace CoolingGridManager.Services
{
    public class BillingService
    {
        private readonly AppDbContext _context;
        private readonly Serilog.ILogger _logger;

        public BillingService(AppDbContext context, Serilog.ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        // ADD BILL
        public async Task<int> AddBill(Billing bill)
        {
            try
            {
                _context.Bills.Add(bill);
                await _context.SaveChangesAsync();
                return bill.BillingId;
            }
            catch (Exception ex)
            {
                var message = string.Format("Exception: {ex}", ex.ToString());
                throw new TryCatchException(message, "AddBill");
            }
        }

        // Get bill 
        public async Task<Billing> GetBill(IGetBillRequest getBillRequest)
        {
            try
            {
                var bill = await _context.Bills
                    .FirstOrDefaultAsync(b =>
                    b.ConsumerID == getBillRequest.ConsumerID &&
                    b.BillingMonth == getBillRequest.Month &&
                    b.BillingYear == getBillRequest.Year);

                if (bill != null)
                {
                    return bill;
                }
                else
                {
                    _logger.Error($"Requested bill null. Non-existing bill requested. Consumer ID: {getBillRequest.ConsumerID}, Month: {getBillRequest.Month}, Year: {getBillRequest.Year}");
                    throw new NotFoundException($"Requested bill not found", "GetBill", getBillRequest);
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception. Non-existing bill requested. Consumer ID: {getBillRequest.ConsumerID}, Month: {getBillRequest.Month}, Year: {getBillRequest.Year}");
                throw new TryCatchException($"Bill for consumer ID {getBillRequest.ConsumerID} not found", "GetBill");
            }
        }

        // Delete bill 
        public async Task<string> DeleteBill(int billingId)
        {
            try
            {
                var billing = await _context.Bills.FindAsync(billingId);
                if (billing != null)
                {
                    _context.Bills.Remove(billing);
                    await _context.SaveChangesAsync();
                    return "Bill deleted.";
                }
                _logger.Error($"Error deleting bill with ID: {billingId}. Bill not found.");
                return $"Error deleting bill with ID: {billingId}. Bill not found.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error deleting bill with ID: {billingId}");
                throw new TryCatchException($"Bill with ID {billingId} not deleted.", "DeleteBill");
            }
        }

        // Update payment status
        public async Task<bool> PaymentStatus(int billingId, bool isPaid)
        {
            try
            {
                var bill = await _context.Bills.FindAsync(billingId);

                if (bill == null)
                {
                    throw new NotFoundException($"Bill with ID {billingId} not found.", "PaymentStatus", billingId);
                }

                bill.IsPaid = isPaid;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error updating payment status to {isPaid} of bill with ID: {billingId}");
                throw new TryCatchException($"Update of payment status of bill ID {billingId} currently not possible.", "MarkBillAsPaid");
            }
        }
    }
}