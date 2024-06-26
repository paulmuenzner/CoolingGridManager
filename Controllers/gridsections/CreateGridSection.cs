using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Models.Data;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Validators.GridSections;
using FluentValidation.Results;
using CoolingGridManager.IRequests;
using Microsoft.AspNetCore.RateLimiting;


namespace CoolingGridManager.Controllers.GridSectionController
{
    [Area("gridsections")]
    [Route("api/gridsections/[controller]")]
    [EnableRateLimiting("fixed")]
    public partial class CreateController : ControllerBase
    {
        private readonly AddGridSectionValidator _addGridSectionValidator;
        private readonly GridSectionService _gridSectionService;


        public CreateController(AddGridSectionValidator addGridSectionValidator, GridSectionService gridSectionService)
        {
            _addGridSectionValidator = addGridSectionValidator;
            _gridSectionService = gridSectionService;

        }

        /// <summary>
        /// Create new grid section
        /// </summary>
        /// <remarks>
        /// Create new grid section for a grid. This is one of the first steps using this cooling grid manager. 
        /// Divide the network into individual sections. 
        ///
        /// </remarks>
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