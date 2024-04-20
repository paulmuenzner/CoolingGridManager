using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Services;
using FluentValidation.Results;
using CoolingGridManager.Validators.Bills;
using CoolingGridManager.IRequests;
using Microsoft.EntityFrameworkCore;


namespace CoolingGridManager.Controllers.Bills
{
    [Area("billing")]
    [Route("api/billing/[controller]")]
    public class UpdateBillStatusController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly BillingService _billingService;
        private readonly AppDbContext _context;

        public UpdateBillStatusController(AppDbContext context, Serilog.ILogger logger, BillingService billingService)
        {
            _logger = logger;
            _context = context;
            _billingService = billingService;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBillStatus([FromBody] IUpdateStatusRequest body)
        {
            try
            {
                // Validate
                if (body == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, $"Not all required information provided. Billing ID and/or Payment Status not provided or wrong type.", "Request currently not possible.", null);
                }

                BillStatusValidator validator = new BillStatusValidator(_context);
                ValidationResult result = await validator.ValidateAsync(body);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var bill = await _billingService.UpdatePaymentStatus(body);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Bill = bill }, "Bill updated.");
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "ArgumentNullException occurred. The provided data is not valid or incomplete.", "Your provided data is not valid or incomplete.", ex);
            }
            catch (DbUpdateException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Internal database error. Bill cannot be updated.", "The system encountered an issue. Our team is working on resolving it.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Internal exception. Bill cannot be updated.", "The system is currently undergoing updates. Our team is working diligently to complete this task as quickly as possible.", ex);
            }
        }
    }

}