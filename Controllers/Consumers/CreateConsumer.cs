using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using FluentValidation.Results;
using CoolingGridManager.Validators.Consumers;
using CoolingGridManager.IRequests;


namespace CoolingGridManager.Controllers.Consumers
{
    [Area("consumers")]
    [Route("api/consumers/[controller]")]
    public class CreateConsumerRecordController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly CreateConsumerRecordValidator _createConsumerRecordValidator;
        private readonly ConsumerService _consumerService;

        public CreateConsumerRecordController(CreateConsumerRecordValidator createConsumerRecordValidator, Serilog.ILogger logger, ConsumerService consumerService)
        {
            _logger = logger;
            _createConsumerRecordValidator = createConsumerRecordValidator;
            _consumerService = consumerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateConsumerRecord([FromBody] ICreateConsumerRecordRequest consumer)
        {
            try
            {
                // Validate
                ValidationResult result = await _createConsumerRecordValidator.ValidateAsync(consumer);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                Consumer newConsumer = await _consumerService.CreateConsumerRecord(consumer);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Consumer = newConsumer }, $"New consumer with name {newConsumer.LastName} and id {newConsumer.ConsumerID} created.");
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