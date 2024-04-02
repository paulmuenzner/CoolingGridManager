using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models.Data;
using CoolingGridManager.Services;
using FluentValidation.Results;
using CoolingGridManager.Validators.Consumers;


namespace CoolingGridManager.Controllers.Consumers
{
    [Area("consumers")]
    [Route("api/consumers/[controller]")]
    public class AddConsumerController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly ConsumerService _consumerService;
        private readonly AppDbContext _context;

        public AddConsumerController(AppDbContext context, Serilog.ILogger logger, ConsumerService consumerService, IHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _consumerService = consumerService;
        }

        [HttpPost]
        public async Task<IActionResult> AddConsumer([FromBody] Consumer consumer)
        {
            try
            {
                // Validate
                AddConsumerValidator validator = new AddConsumerValidator(_context);
                ValidationResult result = await validator.ValidateAsync(consumer);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var newConsumer = await _consumerService.Add(consumer);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Consumer = newConsumer }, $"New consumer with name {newConsumer.LastName} and id {newConsumer.ConsumerID} added");
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