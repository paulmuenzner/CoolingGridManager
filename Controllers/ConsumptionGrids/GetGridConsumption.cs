using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using FluentValidation.Results;
using CoolingGridManager.Validators.ConsumptionGrids;

namespace CoolingGridManager.Controllers.ConsumptionGridController
{
    [Area("consumptiongrids")]
    [Route("api/consumptiongrids/[controller]")]
    public partial class AddConsumptionController : ControllerBase
    {
        private readonly AddConsumptionGridValidator _addConsumptionGridValidator;
        private readonly ConsumptionGridService _consumptionGridService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly AppDbContext _context;
        public AddConsumptionController(AppDbContext context, AddConsumptionGridValidator addConsumptionGridValidator, ExceptionResponse exceptionResponse, ConsumptionGridService consumptionGridService)
        {
            _consumptionGridService = consumptionGridService;
            _addConsumptionGridValidator = addConsumptionGridValidator;
            _exceptionResponse = exceptionResponse;
            _context = context;

        }
        [HttpPost]
        public async Task<IActionResult> GetGridConsumption([FromBody] GetGridConsumptionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Consumer ID not valid. Valid consumer ID must be provided.", "Related consumer not found.", null);
                }

                // Validate
                AddConsumptionGridValidator validator = new AddConsumptionGridValidator(_context);
                ValidationResult result = validator.Validate(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var consumptionId = await _consumptionGridService.AddGridConsumption(request);
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