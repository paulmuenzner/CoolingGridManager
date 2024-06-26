using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Exceptions;
using CoolingGridManager.Validators.Consumers;
using FluentValidation.Results;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.RateLimiting;
using CoolingGridManager.IRequests;

namespace CoolingGridManager.Controllers.Consumers
{
    [ApiController]
    [Area("consumers")]
    [Route("api/consumers/[controller]")]
    [EnableRateLimiting("fixed")]
    public partial class GetConsumerController : ControllerBase
    {
        private readonly GetConsumerDetailsValidator _getConsumerDetailsValidator;
        private readonly ConsumerService _consumerService;
        public GetConsumerController(GetConsumerDetailsValidator getConsumerDetailsValidator, ConsumerService consumerService)
        {
            _consumerService = consumerService;
            _getConsumerDetailsValidator = getConsumerDetailsValidator;
        }


        /// <summary>
        /// Retrieve consumer details
        /// </summary>
        /// <remarks>
        /// Retrieve consumer details of existing consumers by providing the consumer ID (consumerID) only.
        /// </remarks>
        [HttpGet]
        [Tags("Consumers")]
        public async Task<IActionResult> GetConsumerDetails([FromBody] IGetConsumerRequest request)
        {
            try
            {
                // Validate
                ValidationResult result = await _getConsumerDetailsValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                Consumer consumer = await _consumerService.GetConsumerDetails(request);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Consumer = consumer }, $"New consumer with name {consumer.LastName} created.");
            }
            catch (NotFoundException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.NotFound, new { }, "No consumer found.", "No consumer found.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "An unexpected error occurred.", "Action currently not possible.", ex);
            }
        }
    }
}