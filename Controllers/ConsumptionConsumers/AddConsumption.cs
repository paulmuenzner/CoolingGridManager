using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Validators.ConsumptionConsumers;
using FluentValidation.Results;
using CoolingGridManager.IRequests;

namespace CoolingGridManager.Controllers.ConsumptionConsumerController
{
    [Area("consumptionconsumers")]
    [Route("api/consumptionconsumers/[controller]")]
    public partial class AddConsumptionController : ControllerBase
    {
        private readonly AddConsumptionValidator _addConsumptionValidator;
        private readonly ConsumptionConsumerService _consumptionConsumerService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly AppDbContext _context;
        public AddConsumptionController(AppDbContext context, AddConsumptionValidator addConsumptionValidator, ExceptionResponse exceptionResponse, ConsumptionConsumerService consumptionConsumerService)
        {
            _consumptionConsumerService = consumptionConsumerService;
            _addConsumptionValidator = addConsumptionValidator;
            _exceptionResponse = exceptionResponse;
            _context = context;

        }
        [HttpPost]
        public async Task<IActionResult> AddConsumption([FromBody] IAddConsumerConsumptionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Consumer ID not valid. Valid consumer ID must be provided.", "Related consumer not found.", null);
                }

                // Validate
                AddConsumptionValidator validator = new AddConsumptionValidator(_context);
                ValidationResult result = await validator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var consumptionId = await _consumptionConsumerService.AddConsumption(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { ConsumptionID = consumptionId }, $"New consumption entry with id {consumptionId} added.");
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