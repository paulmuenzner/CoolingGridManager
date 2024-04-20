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
    public class DeleteBillController : ControllerBase
    {
        private readonly DeleteBillValidator _deleteBillValidator;
        private readonly BillingService _billingService;
        private readonly AppDbContext _context;

        public DeleteBillController(AppDbContext context, DeleteBillValidator deleteBillValidator, BillingService billingService)
        {
            _context = context;
            _deleteBillValidator = deleteBillValidator;
            _billingService = billingService;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBill([FromBody] IDeleteBillRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "No billing ID provided.", "No billing ID provided.", null);
                }

                ValidationResult result = await _deleteBillValidator.ValidateAsync(request.BillingId);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var bill = await _billingService.DeleteBillingEntry((int)request.BillingId);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Bill = bill }, $"Bill with id {request.BillingId} deleted.");
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "Your provided data is not valid or incomplete.", "Your provided data is not valid or incomplete.", ex);
            }
            catch (DbUpdateException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Internal database error. Bill cannot be deleted.", "The system encountered an issue. Our team is working on resolving it.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Internal exception. Bill cannot be added currently.", "The system is currently undergoing updates. Our team is working diligently to complete this task as quickly as possible.", ex);
            }


        }

    }

}