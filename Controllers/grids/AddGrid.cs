using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;


namespace CoolingGridManager.Controllers.Grid
{
    [Area("grid")]
    [Route("api/grid/[controller]/{consumerId}")]
    public partial class GetCustomerByIdController : Controller
    {
        private readonly GridService _gridService;
        private readonly ExceptionResponse _exceptionResponse;
        public GetCustomerByIdController(ExceptionResponse exceptionResponse, GridService gridService)
        {
            _gridService = gridService;
            _exceptionResponse = exceptionResponse;

        }
        [HttpPost]
        public async Task<IActionResult> AddGrid([FromBody] string gridName)
        {
            try
            {
                var gridId = await _gridService.AddGrid(gridName);
                return Ok(new { GridID = gridId }); // Return the primary key of the newly added grid
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex.ToString(), "An unexpected error occurred.", ExceptionType.General);
            }

        }
    }
}