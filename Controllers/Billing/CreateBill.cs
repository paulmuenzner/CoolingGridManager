using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using FluentValidation.Results;
using CoolingGridManager.Validators.Bills;
using Microsoft.EntityFrameworkCore;


namespace CoolingGridManager.Controllers.Bills
{
    [Area("billing")]
    [Route("api/billing/[controller]")]
    public class AddController : ControllerBase
    {
        private readonly CreateBillRecordValidator _createBillRecordValidator;
        private readonly BillingService _billingService;

        public AddController(CreateBillRecordValidator createBillRecordValidator, BillingService billingService)
        {
            _createBillRecordValidator = createBillRecordValidator;
            _billingService = billingService;
        }

        [HttpPost]
        [Tags("Billing")]
        public async Task<IActionResult> CreateBillingRecord([FromBody] Billing request)
        {
            try
            {
                // Validate
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Request data is either improperly formatted or contains missing data. No new billing record created.", "Creating new billing records currently not possible.", null);
                }

                ValidationResult result = await _createBillRecordValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                int newBillID = await _billingService.CreateBillingRecord(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { BillID = newBillID }, $"New bill with ID {newBillID} added");
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "Your provided data is not valid or incomplete.", "Your provided data is not valid or incomplete.", ex);
            }
            catch (DbUpdateException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Internal database error. Bill cannot be added.", "The system encountered an issue. Our team is working on resolving it.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Internal exception. Bill cannot be added currently.", "The system is currently undergoing updates. Our team is working diligently to complete this task as quickly as possible.", ex);
            }


        }
    }

}