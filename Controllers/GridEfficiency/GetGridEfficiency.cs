using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.ResponseHandler;
using FluentValidation.Results;
using CoolingGridManager.Validators.GridEnergyTransfer;
using Microsoft.AspNetCore.RateLimiting;
using CoolingGridManager.Services;
using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.Controllers.GridEfficiencyController
{
    [Area("GridEfficiency")]
    [Route("api/GridEfficiency/[controller]")]
    [EnableRateLimiting("fixed")]
    public partial class GetController : ControllerBase
    {
        private readonly GetGridEfficiencyValidator _getGridEfficiencyValidator;
        private readonly GridEfficiencyService _gridEfficiencyService;
        private readonly ExceptionResponse _exceptionResponse;

        public GetController(GetGridEfficiencyValidator getGridEfficiencyValidator, ExceptionResponse exceptionResponse, GridEfficiencyService gridEfficiencyService)
        {
            _gridEfficiencyService = gridEfficiencyService;
            _getGridEfficiencyValidator = getGridEfficiencyValidator;
            _exceptionResponse = exceptionResponse;
        }

        /// <summary>
        /// Retrieve grid's efficiency for certain month
        /// </summary>
        /// <remarks>
        /// Retrieve efficiency value of a grid for a certain month from GridEfficiency table. 
        /// This GridEnergyTransfer table is filled automatically by a cron job based on consumer consumption and and grid parameter logs.
        ///
        /// ```json
        /// Sample request:
        /// Get /api/GridEfficiency/get
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
        [Tags("GridEfficiency")]
        public async Task<IActionResult> GetGridEfficiency([FromBody] IGetGridDataRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Valid request must be provided.", "Requested data not valid.", null);
                }

                // Validate
                ValidationResult result = _getGridEfficiencyValidator.Validate(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                GridEfficiency gridEfficiency = await _gridEfficiencyService.GetGridEfficiency(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { GridEfficiency = gridEfficiency }, $"Grid efficiency: {gridEfficiency.EfficiencyRelative}. Losses: {1 - gridEfficiency.EfficiencyRelative}");
            }
            catch (FormatException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "FormatException when retrieving grid efficiency.", "Requesting grid efficiency currently not poosible. Please retry later.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred.", "Requesting grid efficiency currently not possible. Please retry later.", ExceptionType.General);
            }
        }
    }
}