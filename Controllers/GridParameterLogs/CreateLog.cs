using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using FluentValidation.Results;
using CoolingGridManager.Validators.GridParameterLogs;
using CoolingGridManager.IRequests;
using Microsoft.AspNetCore.RateLimiting;
using CoolingGridManager.IResponse;

namespace CoolingGridManager.Controllers.GridParameters
{
    [Area("gridparameters")]
    [Route("api/gridparameters/[controller]")]
    [EnableRateLimiting("fixed")]
    public partial class CreateLogController : ControllerBase
    {
        private readonly CreateGridParameterLogValidator _createGridParameterLogValidator;
        private readonly GridParameterLogService _gridParameterLogService;
        private readonly ExceptionResponse _exceptionResponse;
        public CreateLogController(CreateGridParameterLogValidator createGridParameterLogValidator, ExceptionResponse exceptionResponse, GridParameterLogService gridParameterLogService)
        {
            _gridParameterLogService = gridParameterLogService;
            _createGridParameterLogValidator = createGridParameterLogValidator;
            _exceptionResponse = exceptionResponse;
        }

        /// <summary>
        /// Retrieve grid consumption per month
        /// </summary>
        /// <remarks>
        /// Log bunch of parameter data of the grid for variable time frame. Adopt this database table per your needs and add more parameter.
        /// Take care when changing: This table's parameters are used to calculate the monthly grid comsumption at the beginning of each month for the past month using a cron job.
        /// 
        /// Logging should take place automatically of course.
        /// 
        /// Feel free to protect route, for example by adding an IP whitelist, a mandatory key and secret, a small time frame each day where logging is possible only, etc. 
        /// </remarks>
        [HttpPost]
        [Tags("GridParameters")]
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

                IParameterLogResponse addLogs = await _gridParameterLogService.CreateGridParameterLogRecord(request);

                string success = $"{addLogs.Success} new parameter logs for grid added.";
                string fail = $"Error. Not all of the {addLogs.Success} new parameter logs for grid added.";

                IActionResult responseFail = ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, fail, fail, null);
                IActionResult responseSuccess = ResponseFormatter.Success(HttpStatusPositive.Created, new { }, success);

                IActionResult response = addLogs.Success ? responseSuccess : responseFail;

                return response;
            }
            catch (FormatException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "Logging grid parameter results in FormatException.", "Creating parameter log for grid currently not poosible. Please retry later.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "An unexpected error occurred when Creating parameter log for grid.", "Creating parameter log for grid currently not poosible. Please retry later.", ex);
            }

        }
    }
}