using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using FluentValidation.Results;
using CoolingGridManager.Validators.Bills;
using CoolingGridManager.IRequests;
using Microsoft.EntityFrameworkCore;


namespace CoolingGridManager.Controllers.Bills
{
    [Area("billing")]
    [Route("api/billing/[controller]")]
    public class GetBillController : ControllerBase
    {
        private readonly BillingService _billingService;
        private readonly GetBillValidator _getBillValidator;
        public GetBillController(BillingService billingService, GetBillValidator getBillValidator)
        {
            _billingService = billingService;
            _getBillValidator = getBillValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBillDetails([FromBody] IGetBillRequest request)
        {
            try
            {
                // Validate
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Consumer ID, month, and year must be provided.", "Consumer ID, month, and year must be provided.", null);
                }

                ValidationResult result = await _getBillValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var bill = await _billingService.GetBillingDetails(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Bill = bill }, "Requested bill retrieved.");
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "Your provided data is not valid or incomplete.", "Your provided data is not valid or incomplete.", ex);
            }
            catch (DbUpdateException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Internal database error. Bill cannot be retrieved.", "The system encountered an issue. Our team is working on resolving it.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Internal exception. Bill cannot be added currently.", "The system is currently undergoing updates. Our team is working diligently to complete this task as quickly as possible.", ex);
            }


        }
    }

}