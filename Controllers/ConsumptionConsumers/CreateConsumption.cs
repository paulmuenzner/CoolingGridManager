using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Validators.ConsumptionConsumers;
using FluentValidation.Results;
using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.Controllers.ConsumptionConsumerController
{
    [Area("consumptionconsumers")]
    [Route("api/consumptionconsumers/[controller]")]
    public partial class CreateConsumerConsumptionController : ControllerBase
    {
        private readonly CreateConsumptionRecordValidator _createConsumptionRecordValidator;
        private readonly ConsumptionConsumerService _consumptionConsumerService;
        private readonly ExceptionResponse _exceptionResponse;

        public CreateConsumerConsumptionController(CreateConsumptionRecordValidator createConsumptionRecordValidator, ExceptionResponse exceptionResponse, ConsumptionConsumerService consumptionConsumerService)
        {
            _consumptionConsumerService = consumptionConsumerService;
            _createConsumptionRecordValidator = createConsumptionRecordValidator;
            _exceptionResponse = exceptionResponse;
        }
        [HttpPost]
        public async Task<IActionResult> CreateConsumptionRecord([FromBody] ICreateConsumerConsumptionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Request not valid.", "Related consumer not found.", null);
                }

                // Validate
                ValidationResult result = await _createConsumptionRecordValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                ConsumptionConsumer consumptionRecord = await _consumptionConsumerService.CreateConsumerConsumptionRecord(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { ConsumptionID = consumptionRecord }, $"New consumption entry with id {consumptionRecord.LogId} added.");
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