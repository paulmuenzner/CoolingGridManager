using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Validators.Grids;
using FluentValidation.Results;
using CoolingGridManager.Models.Data;


namespace CoolingGridManager.Controllers.GridController
{
    [Area("grids")]
    [Route("api/grids/[controller]")]
    public partial class AddGridController : ControllerBase
    {
        private readonly CreateGridValidator _createGridValidator;
        private readonly GridService _gridService;
        private readonly ExceptionResponse _exceptionResponse;
        public AddGridController(CreateGridValidator createGridValidator, ExceptionResponse exceptionResponse, GridService gridService)
        {
            _gridService = gridService;
            _createGridValidator = createGridValidator;
            _exceptionResponse = exceptionResponse;
        }
        [HttpPost]
        public async Task<IActionResult> CreateGridRecord([FromBody] string request)
        {
            try
            {
                // Validate
                ValidationResult result = _createGridValidator.Validate(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                Grid grid = await _gridService.CreateGridRecord(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { GridID = grid.GridID }, $"New grid with name {grid.GridName} and id {grid.GridID} added");
            }
            catch (FormatException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "Grid name already exists. Choose a different name.", "Grid name already exists. Choose a different name.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred when creating new grid.", "Adding new grid currently not possible.", ExceptionType.General);
            }

        }
    }
}