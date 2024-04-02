using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Validators.Grids;
using FluentValidation.Results;

public class GridModel
{
    public required string GridName { get; set; }
}

namespace CoolingGridManager.Controllers.GridController
{
    [Area("grids")]
    [Route("api/grids/[controller]")]
    public partial class AddGridController : ControllerBase
    {
        private readonly AddGridValidator _addGridValidator;
        private readonly GridService _gridService;
        private readonly ExceptionResponse _exceptionResponse;
        public AddGridController(AddGridValidator addGridValidator, ExceptionResponse exceptionResponse, GridService gridService)
        {
            _gridService = gridService;
            _addGridValidator = addGridValidator;
            _exceptionResponse = exceptionResponse;

        }
        [HttpPost]
        public async Task<IActionResult> AddGrid([FromBody] GridModel model)
        {
            try
            {
                // Validate
                AddGridValidator validator = new();
                ValidationResult result = validator.Validate(model);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var gridId = await _gridService.AddGrid(model.GridName);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { GridID = gridId }, $"New grid with name {model.GridName} and id {gridId} added");
            }
            catch (FormatException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "Grid name already exists. Choose a different name.", "Grid name already exists. Choose a different name.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred.", "Adding new grid currently not possible.", ExceptionType.General);
            }

        }
    }
}