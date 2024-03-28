using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Models;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Validators.GridSections;
using FluentValidation.Results;


namespace CoolingGridManager.Controllers.GridSectionController
{
    [Area("gridsections")]
    [Route("api/gridsections/[controller]")]
    public partial class AddGridSectionController : ControllerBase
    {
        private readonly AddGridSectionValidator _addGridSectionValidator;
        private readonly GridSectionService _gridSectionService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly Serilog.ILogger _logger;

        private readonly AppDbContext _context;
        public AddGridSectionController(AppDbContext context, AddGridSectionValidator addGridSectionValidator, ExceptionResponse exceptionResponse, Serilog.ILogger logger, GridSectionService gridSectionService)
        {
            _addGridSectionValidator = addGridSectionValidator;
            _gridSectionService = gridSectionService;
            _exceptionResponse = exceptionResponse;
            _logger = logger;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GridSection gridSection)
        {
            try
            {
                // Validate
                AddGridSectionValidator validator = new AddGridSectionValidator(_context);
                ValidationResult result = await validator.ValidateAsync(gridSection);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var gridSectionId = await _gridSectionService.AddGridSection(gridSection);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { GridSectionId = gridSectionId }, $"New grid section with ID {gridSectionId} added.");
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