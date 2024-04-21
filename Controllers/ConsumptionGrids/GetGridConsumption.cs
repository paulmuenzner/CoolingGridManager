using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using FluentValidation.Results;
using CoolingGridManager.IRequests;
using CoolingGridManager.Validators.GridConsumptions;
using CoolingGridManager.Models.Data;

namespace CoolingGridManager.Controllers.ConsumptionGridController
{
    [Area("consumptiongrids")]
    [Route("api/consumptiongrids/[controller]")]
    public partial class GetController : ControllerBase
    {
        private readonly GetGridConsumptionValidator _getGridConsumptionValidator;
        private readonly ConsumptionGridService _consumptionGridService;
        private readonly ExceptionResponse _exceptionResponse;

        public GetController(GetGridConsumptionValidator getGridConsumptionValidator, ExceptionResponse exceptionResponse, ConsumptionGridService consumptionGridService)
        {
            _consumptionGridService = consumptionGridService;
            _getGridConsumptionValidator = getGridConsumptionValidator;
            _exceptionResponse = exceptionResponse;
        }

        [HttpPost]
        [Tags("GridConsumption")]
        public async Task<IActionResult> GetGridConsumption([FromBody] IGetGridConsumptionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { }, "Valid request must be provided.", "Requested data not found.", null);
                }

                // Validate
                ValidationResult result = _getGridConsumptionValidator.Validate(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                ConsumptionGrid consumption = await _consumptionGridService.GetGridConsumptionDetails(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { ConsumptionID = consumption }, "Consumption for grid found.");
            }
            catch (FormatException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "FormatException when retrieving consumption for grid.", "Grid consumption request currently not poosible. Please retry later.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred.", "Grid consumption request currently not poosible. Please retry later.", ExceptionType.General);
            }

        }
    }
}