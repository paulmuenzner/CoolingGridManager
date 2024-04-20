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
    public partial class GetParameterLogsController : ControllerBase
    {
        private readonly GetGridParameterLogValidator _getGridParameterLogValidator;
        private readonly GridParameterLogService _gridParameterLogService;
        private readonly ExceptionResponse _exceptionResponse;
        public GetParameterLogsController(GetGridParameterLogValidator getGridParameterLogValidator, ExceptionResponse exceptionResponse, GridParameterLogService gridParameterLogService)
        {
            _gridParameterLogService = gridParameterLogService;
            _getGridParameterLogValidator = getGridParameterLogValidator;
            _exceptionResponse = exceptionResponse;
        }
        [HttpGet]
        public async Task<IActionResult> GetParameterLogs([FromBody] IGetMonthlyGridParameterDetailsRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Request not valid. Valid grid ID, year and month must be provided.", "Requested paramter logs not found.", null);
                }

                // Validate
                ValidationResult result = await _getGridParameterLogValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                List<GridParameterLog> gridParameter = await _gridParameterLogService.GetMonthlyGridParameterDetails(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { GridParameter = gridParameter }, $"Found {gridParameter.Count} entries for grid id {request.GridID} in selected time frame.");
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