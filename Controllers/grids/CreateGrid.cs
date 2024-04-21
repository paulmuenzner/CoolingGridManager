using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Validators.Grids;
using FluentValidation.Results;
using CoolingGridManager.Models.Data;
using CoolingGridManager.IRequests;


namespace CoolingGridManager.Controllers.GridController
{
    [Area("grids")]
    [Route("api/grids/[controller]")]
    public partial class CreateController : ControllerBase
    {
        private readonly CreateGridValidator _createGridValidator;
        private readonly GridService _gridService;

        public CreateController(CreateGridValidator createGridValidator, GridService gridService)
        {
            _gridService = gridService;
            _createGridValidator = createGridValidator;
        }
        [HttpPost]
        [Tags("Grid")]
        public async Task<IActionResult> CreateGridRecord([FromBody] ICreateGridRequest request)
        {
            try
            {
                // Validate
                ValidationResult result = await _createGridValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                Grid grid = await _gridService.CreateGridRecord(request);
                return ResponseFormatter.Success(HttpStatusPositive.Created, new { GridID = grid.GridID }, $"New grid with name {grid.GridName} and id {grid.GridID} added");
            }
            catch (FormatException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "FormatException occurred while creating a new grid. Check the format of the input data.", "Grid service currently not available.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "An unexpected error occurred when creating new grid.", "Adding new grid currently not possible.", ex);
            }

        }
    }
}