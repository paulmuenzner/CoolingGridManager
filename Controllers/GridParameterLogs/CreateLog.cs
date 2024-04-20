using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using FluentValidation.Results;
using CoolingGridManager.Validators.GridParameterLogs;
using CoolingGridManager.IRequests;

namespace CoolingGridManager.Controllers.GridParameters
{
    [Area("gridparameters")]
    [Route("api/gridparameters/[controller]")]
    public partial class CreateParameterLogController : ControllerBase
    {
        private readonly CreateGridParameterLogValidator _createGridParameterLogValidator;
        private readonly GridParameterLogService _gridParameterLogService;
        private readonly ExceptionResponse _exceptionResponse;
        public CreateParameterLogController(CreateGridParameterLogValidator createGridParameterLogValidator, ExceptionResponse exceptionResponse, GridParameterLogService gridParameterLogService)
        {
            _gridParameterLogService = gridParameterLogService;
            _createGridParameterLogValidator = createGridParameterLogValidator;
            _exceptionResponse = exceptionResponse;
        }
        [HttpPost]
        public async Task<IActionResult> CreateParameterLogEntry([FromBody] ICreateGridParameterLogRecordRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Request data not provided correctly.", "Action currently not possible.", null);
                }

                // Validate
                ValidationResult result = await _createGridParameterLogValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                GridParameterLog newLog = await _gridParameterLogService.CreateGridParameterLogRecord(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { ConsumptionID = newLog }, $"New parameter log for grid created.");
            }
            catch (FormatException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "Logging grid parameter results in FormatException.", "Creating parameter log for grid currently not poosible. Please retry later.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred when Creating parameter log for grid.", "Creating parameter log for grid currently not poosible. Please retry later.", ExceptionType.General);
            }

        }
    }
}