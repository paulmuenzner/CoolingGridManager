using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Validators.ConsumptionConsumers;
using FluentValidation.Results;
using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;
using CoolingGridManager.IResponse;
using Microsoft.AspNetCore.RateLimiting;

namespace CoolingGridManager.Controllers.ConsumptionConsumerController
{
    [Area("consumptionconsumers")]
    [Route("api/consumptionconsumers/[controller]")]
    [EnableRateLimiting("fixed")]
    public partial class CreateController : ControllerBase
    {
        private readonly CreateConsumptionRecordValidator _createConsumptionRecordValidator;
        private readonly ConsumptionConsumerService _consumptionConsumerService;
        private readonly ExceptionResponse _exceptionResponse;

        public CreateController(CreateConsumptionRecordValidator createConsumptionRecordValidator, ExceptionResponse exceptionResponse, ConsumptionConsumerService consumptionConsumerService)
        {
            _consumptionConsumerService = consumptionConsumerService;
            _createConsumptionRecordValidator = createConsumptionRecordValidator;
            _exceptionResponse = exceptionResponse;
        }

        /// <summary>
        /// Add consumer consumption
        /// </summary>
        /// <remarks>
        /// Add a batch of consumer consumption records, such as those collected at a meter station over the course of a day, in list format.
        /// </remarks>
        [HttpPost]
        [Tags("ConsumerConsumption")]
        public async Task<IActionResult> CreateConsumptionRecord([FromBody] ICreateConsumerConsumptionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Request not valid.", "Related consumer not found.", null);
                }

                // Validate
                ValidationResult result = await _createConsumptionRecordValidator.ValidateAsync(request.ConsumptionDataList);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                ICreateConsumerConsumptionRecordResponse store = await _consumptionConsumerService.CreateConsumerConsumptionRecord(request.ConsumptionDataList);
                string success = $"All {store.Count} provided consumption records were successfully added.";
                string fail = $"Error. Not all of the {store.Count} provided consumption records are not successfully added.";

                IActionResult responseFail = ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, fail, fail, null);
                IActionResult responseSuccess = ResponseFormatter.Success(HttpStatusPositive.Created, new { }, success);

                IActionResult response = store.Success ? responseSuccess : responseFail;

                return response;
            }
            catch (FormatException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "Logging consumption results in FormatException.", "Adding consumption entry currently not poosible. Please retry later.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred.", "Adding consumption entry currently not poosible. Please retry later.", ExceptionType.General);
            }

        }
    }
}