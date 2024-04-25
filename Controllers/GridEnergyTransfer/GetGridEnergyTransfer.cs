using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.ResponseHandler;
using FluentValidation.Results;
using CoolingGridManager.Validators.GridEnergyTransfer;
using Microsoft.AspNetCore.RateLimiting;
using CoolingGridManager.Services;
using CoolingGridManager.IRequests;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.Controllers.GridEnergyTransferController
{
    [Area("GridEnergyTransfer")]
    [Route("api/GridEnergyTransfer/[controller]")]
    [EnableRateLimiting("fixed")]
    public partial class GetController : ControllerBase
    {
        private readonly GetGridEnergyTransferValidator _getGridEnergyTransferValidator;
        private readonly GridEnergyTransferService _gridEnergyTransferService;
        private readonly ExceptionResponse _exceptionResponse;

        public GetController(GetGridEnergyTransferValidator getGridEnergyTransferValidator, ExceptionResponse exceptionResponse, GridEnergyTransferService gridEnergyTransferService)
        {
            _gridEnergyTransferService = gridEnergyTransferService;
            _getGridEnergyTransferValidator = getGridEnergyTransferValidator;
            _exceptionResponse = exceptionResponse;
        }

        /// <summary>
        /// Retrieve grid's energy transfer per month
        /// </summary>
        /// <remarks>
        /// Retrieve energy transfer of an entire grid for a certain month from GridEnergyTransfer table. Feel free to revise and customize this controller as per your needs (eg. daily instead of monthly).
        /// This GridEnergyTransfer table is filled automatically by a cron job using the data from the more detailed GridParameterLogs table.
        ///
        /// ```json
        /// Sample request:
        /// Get /api/GridEnergyTransfer/get
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
        [Tags("GridEnergyTransfer")]
        public async Task<IActionResult> GetGridEnergyTransfer([FromBody] IGetGridDataRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Valid request must be provided.", "Requested data not valid.", null);
                }

                // Validate
                ValidationResult result = _getGridEnergyTransferValidator.Validate(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                GridEnergyTransfer energyTransfer = await _gridEnergyTransferService.GetGridEnergyTransferDetails(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { EnergyTransfer = energyTransfer }, "Energy transfer for grid found.");
            }
            catch (FormatException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "FormatException when retrieving energy transfer for grid.", "Requesting grid's energy transfer currently not poosible. Please retry later.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred.", "Requesting grid's energy transfer currently not possible. Please retry later.", ExceptionType.General);
            }

        }
    }
}