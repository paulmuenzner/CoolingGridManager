using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using FluentValidation.Results;
using CoolingGridManager.Validators.Consumers;
using CoolingGridManager.IRequests;
using Microsoft.AspNetCore.RateLimiting;


namespace CoolingGridManager.Controllers.Consumers
{
    [ApiController]
    [Area("consumers")]
    [Route("api/consumers/[controller]")]
    [EnableRateLimiting("fixed")]
    public class CreateConsumerController : ControllerBase
    {

        private readonly CreateConsumerRecordValidator _createConsumerRecordValidator;
        private readonly ConsumerService _consumerService;

        public CreateConsumerController(CreateConsumerRecordValidator createConsumerRecordValidator, ConsumerService consumerService)
        {
            _createConsumerRecordValidator = createConsumerRecordValidator;
            _consumerService = consumerService;
        }

        /// <summary>
        /// Create new consumer
        /// </summary>
        /// <remarks>
        /// Create new consumers for the cooling grid. Amongst others, a grid section ID (gridSectionID) is required for each new consumer. So first create a grid and structure it in grid sections.
        /// </remarks>
        [HttpPost]
        [Tags("Consumers")]
        public async Task<IActionResult> CreateConsumer([FromBody] ICreateConsumerRecordRequest request)
        {
            try
            {
                // Validate
                ValidationResult result = await _createConsumerRecordValidator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                Consumer newConsumer = await _consumerService.CreateConsumerRecord(request);
                return ResponseFormatter.Success(HttpStatusPositive.Created, new { Consumer = newConsumer }, $"New consumer with name {newConsumer.LastName} and id {newConsumer.ConsumerID} created.");
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.BadRequest, new { }, "Your provided data is not valid or incomplete.", "Your provided data is not valid or incomplete.", ex);
            }
            catch (Exception ex)
            {
                return ResponseFormatter.Negative(HttpStatusNegative.InternalServerError, new { }, "Internal exception. Consumer cannot be added currently.", "The system is currently undergoing updates. Our team is working diligently to complete this task as quickly as possible.", ex);
            }


        }


    }

}