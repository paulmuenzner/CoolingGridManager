using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using System.ComponentModel.DataAnnotations;

public class GridModel
{
    [Required]
    [MaxLength(1)]
    public required string GridName { get; set; }
}

namespace CoolingGridManager.Controllers.Grid
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
                return Ok(new { GridID = gridId }); // Return the primary key of the newly added grid
            }
            catch (FormatException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred.", ExceptionType.General);
            }

        }
    }
}