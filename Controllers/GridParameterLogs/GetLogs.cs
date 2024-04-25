using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using FluentValidation.Results;
using CoolingGridManager.Validators.GridParameterLogs;
using CoolingGridManager.IRequests;
using Microsoft.AspNetCore.RateLimiting;

namespace CoolingGridManager.Controllers.GridParameters
{
    [Area("gridparameters")]
    [Route("api/gridparameters/[controller]")]
    [EnableRateLimiting("fixed")]
    public partial class GetLogsController : ControllerBase
    {
        private readonly GetGridParameterLogValidator _getGridParameterLogValidator;
        private readonly GridParameterLogService _gridParameterLogService;
        private readonly ExceptionResponse _exceptionResponse;
        public GetLogsController(GetGridParameterLogValidator getGridParameterLogValidator, ExceptionResponse exceptionResponse, GridParameterLogService gridParameterLogService)
        {
            _gridParameterLogService = gridParameterLogService;
            _getGridParameterLogValidator = getGridParameterLogValidator;
            _exceptionResponse = exceptionResponse;
        }

        /// <summary>
        /// Retrieve grid parameter per month
        /// </summary>
        /// <remarks>
        /// Retrieve parameter logs of a grid for a certain month. Feel free to revise and customize this controller as per your needs (eg. daily instead of monthly).
        ///
        /// ```json
        /// Sample request:
        /// Get /api/gridparameters/getlogs
        /// 
        /// {
        ///   "gridID": 1,
        ///   "month": 3,
        ///   "year": 2023
        /// }
        /// ```
        ///
        /// </remarks>
        [HttpGet]
        [Tags("GridParameters")]
        public async Task<IActionResult> GetParameterLogs([FromBody] IGetGridDataRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Request not valid. Valid grid ID, year and month must be provided.", "Requested parameter not valid.", null);
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
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "Logging consumption results in FormatException.", "Adding consumption entry currently not poosible. Please retry later.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "An unexpected error occurred.", "Adding consumption entry currently not poosible. Please retry later.", ex);
            }

        }
    }
}