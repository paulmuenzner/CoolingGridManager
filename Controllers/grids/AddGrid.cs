using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using System.ComponentModel.DataAnnotations;
using CoolingGridManager.ResponseHandler;

public class GridModel
{
    [Required]
    public required string GridName { get; set; }
}

namespace CoolingGridManager.Controllers.GridController
{
    [Area("grids")]
    [Route("api/grids/[controller]")]
    public partial class AddGridController : ControllerBase
    {
        private readonly GridService _gridService;
        private readonly ExceptionResponse _exceptionResponse;
        public AddGridController(ExceptionResponse exceptionResponse, GridService gridService)
        {
            _gridService = gridService;
            _exceptionResponse = exceptionResponse;

        }
        [HttpPost]
        public async Task<IActionResult> AddGrid([FromBody] GridModel model)
        {
            try
            {
                var gridId = await _gridService.AddGrid(model.GridName);
                return ResponseFormatter.FormatSuccessResponse(HttpStatus.OK, new { GridID = gridId }, $"New grid with name {model.GridName} and id {gridId} added");
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