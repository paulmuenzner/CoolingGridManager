using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using FluentValidation.Results;
using CoolingGridManager.Validators.GridParameterLogs;

namespace CoolingGridManager.Controllers.ConsumptionController
{
    [Area("gridparameters")]
    [Route("api/gridparameters/[controller]")]
    public partial class AddParameterLogController : ControllerBase
    {
        private readonly AddGridParameterLogValidator _addGridParameterLogValidator;
        private readonly GridParameterLogService _gridParameterLogService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly AppDbContext _context;
        public AddParameterLogController(AppDbContext context, AddGridParameterLogValidator addGridParameterLogValidator, ExceptionResponse exceptionResponse, GridParameterLogService gridParameterLogService)
        {
            _gridParameterLogService = gridParameterLogService;
            _addGridParameterLogValidator = addGridParameterLogValidator;
            _exceptionResponse = exceptionResponse;
            _context = context;

        }
        [HttpPost]
        public async Task<IActionResult> AddParameterLog([FromBody] GridParameterLog request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Consumer ID not valid. Valid consumer ID must be provided.", "Related consumer not found.", null);
                }

                // Validate
                AddGridParameterLogValidator validator = new AddGridParameterLogValidator(_context);
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