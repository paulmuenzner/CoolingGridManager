using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Models.Data;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Validators.GridSections;
using FluentValidation.Results;
using CoolingGridManager.IRequests;


namespace CoolingGridManager.Controllers.GridSectionController
{
    [Area("gridsections")]
    [Route("api/gridsections/[controller]")]
    public partial class CreateController : ControllerBase
    {
        private readonly AddGridSectionValidator _addGridSectionValidator;
        private readonly GridSectionService _gridSectionService;
        private readonly ExceptionResponse _exceptionResponse;
        private readonly Serilog.ILogger _logger;

        public CreateController(AddGridSectionValidator addGridSectionValidator, ExceptionResponse exceptionResponse, Serilog.ILogger logger, GridSectionService gridSectionService)
        {
            _addGridSectionValidator = addGridSectionValidator;
            _gridSectionService = gridSectionService;
            _exceptionResponse = exceptionResponse;
            _logger = logger;
        }

        [HttpPost]
        [Tags("GridSections")]
        public async Task<IActionResult> Create([FromBody] ICreateGridSectionRecordRequest request)
        {
            try
            {
                // Validate
                ValidationResult result = await _addGridSectionValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var gridSectionId = await _gridSectionService.CreateGridSectionRecord(request);
                return ResponseFormatter.Success(HttpStatusPositive.Created, new { GridSectionId = gridSectionId }, $"New grid section with ID {gridSectionId} added.");
            }
            catch (FormatException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "FormatException occurred while creating a new grid section. Check the format of the input data.", "Provided data was not in the correct format.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, $"An unexpected error occurred. {ex.GetType().Name}", "Acction currently not possible.", ex);
            }

        }
    }
}