using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Validators.Consumptions;
using CoolingGridManager.Models.Data;
using FluentValidation.Results;
using CoolingGridManager.Models.Requests;

namespace CoolingGridManager.Controllers.ConsumptionController
{
    [Area("gridparameters")]
    [Route("api/gridparameters/[controller]")]
    public partial class AddParameterLogController : ControllerBase
    {
        private readonly AddConsumptionValidator _addConsumptionValidator;
        private readonly ConsumptionService _consumptionService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly AppDbContext _context;
        public AddParameterLogController(AppDbContext context, AddConsumptionValidator addConsumptionValidator, ExceptionResponse exceptionResponse, ConsumptionService consumptionService)
        {
            _consumptionService = consumptionService;
            _addConsumptionValidator = addConsumptionValidator;
            _exceptionResponse = exceptionResponse;
            _context = context;

        }
        [HttpPost]
        public async Task<IActionResult> AddConsumption([FromBody] AddConsumptionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Consumer ID not valid. Valid consumer ID must be provided.", "Related consumer not found.", null);
                }

                // Validate
                AddConsumptionValidator validator = new AddConsumptionValidator(_context);
                ValidationResult result = validator.Validate(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var consumptionId = await _consumptionService.AddConsumption(request);
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