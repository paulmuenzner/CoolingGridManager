using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Models;
using CoolingGridManager.ResponseHandler;


namespace CoolingGridManager.Controllers.GridSectionController
{
    [Area("gridsections")]
    [Route("api/gridsections/[controller]")]
    public partial class AddGridSectionController : ControllerBase
    {
        private readonly GridSectionService _gridSectionService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly Serilog.ILogger _logger;
        public AddGridSectionController(ExceptionResponse exceptionResponse, Serilog.ILogger logger, GridSectionService gridSectionService)
        {
            _gridSectionService = gridSectionService;
            _exceptionResponse = exceptionResponse;
            _logger = logger;

        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GridSection gridSection)
        {
            try
            {
                var gridSectionId = await _gridSectionService.AddGridSecion(gridSection);
                return ResponseFormatter.FormatSuccessResponse(HttpStatus.OK, new { GridSectionId = gridSectionId }, $"New grid section with ID {gridSectionId} added.");
            }
            catch (FormatException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "Error. Grid section name already exists your provided grid ID is not valid.", "Error. Grid section name already exists your provided grid ID is not valid.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, $"An unexpected error occurred. {ex.GetType().Name}", "Acction currently not possible.", ExceptionType.General);
            }

        }
    }
}