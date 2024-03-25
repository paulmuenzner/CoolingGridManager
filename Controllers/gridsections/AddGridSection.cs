using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Models;


namespace CoolingGridManager.Controllers.GridSectionController
{
    [Area("gridsections")]
    [Route("api/gridsections/[controller]")]
    public partial class AddGridSectionController : ControllerBase
    {
        private readonly GridSectionService _gridSectionService;
        private readonly ExceptionResponse _exceptionResponse;
        public AddGridSectionController(ExceptionResponse exceptionResponse, GridSectionService gridSectionService)
        {
            _gridSectionService = gridSectionService;
            _exceptionResponse = exceptionResponse;

        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GridSection gridSection)
        {
            try
            {
                var gridSectionId = await _gridSectionService.AddGridSecion(gridSection);
                return Ok(new { GridSectionId = gridSectionId });
            }
            catch (FormatException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "Grid name already exists. Choose a different name.", "Choose a different name.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, $"An unexpected error occurred. {ex.GetType().Name}", "Acction currently not possible.", ExceptionType.General);
            }

        }
    }
}