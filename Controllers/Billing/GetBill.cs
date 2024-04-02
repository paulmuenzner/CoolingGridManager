using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using FluentValidation.Results;
using CoolingGridManager.Validators.Bills;
using CoolingGridManager.Models.Requests;


namespace CoolingGridManager.Controllers.Bills
{
    [Area("billing")]
    [Route("api/billing/[controller]")]
    public class GetBillController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly BillingService _billingService;
        private readonly AppDbContext _context;

        public GetBillController(AppDbContext context, Serilog.ILogger logger, BillingService billingService)
        {
            _logger = logger;
            _context = context;
            _billingService = billingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBill([FromBody] GetBillRequest getBillRequest)
        {
            try
            {
                // Validate
                if (getBillRequest.ConsumerID == null || getBillRequest.Month == null || getBillRequest.Year == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Consumer ID, month, and year must be provided.", "Consumer ID, month, and year must be provided.", null);
                }

                GetBillValidator validator = new GetBillValidator(_context);
                ValidationResult result = await validator.ValidateAsync(getBillRequest);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var bill = await _billingService.GetBill(getBillRequest);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Bill = bill }, $"Requested bill retrieved.");
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "Your provided data is not valid or incomplete.", "Your provided data is not valid or incomplete.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Internal exception. Bill cannot be added currently.", "The system is currently undergoing updates. Our team is working diligently to complete this task as quickly as possible.", ex);
            }


        }
    }

}