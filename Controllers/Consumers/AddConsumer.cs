using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models;
using CoolingGridManager.Services;
using Microsoft.EntityFrameworkCore;



namespace CoolingGridManager.Controllers.Consumers
{
    [Area("consumers")]
    [Route("api/consumers/[controller]")]
    public class AddConsumerController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IExceptionHandlingService _exceptionHandlingService;
        private readonly ConsumerService _consumerService;

        private readonly ExceptionResponse _exceptionResponse;
        public AddConsumerController(IExceptionHandlingService exceptionHandlingService, ExceptionResponse exceptionResponse, Serilog.ILogger logger, ConsumerService consumerService, IHostEnvironment env)
        {
            _logger = logger;
            _consumerService = consumerService;
            _exceptionHandlingService = exceptionHandlingService;
            _exceptionResponse = exceptionResponse;
        }

        [HttpPost]
        public async Task<IActionResult> AddConsumer([FromBody] Consumer consumer)
        {
            try
            {
                var newConsumer = await _consumerService.Add(consumer);
                return ResponseFormatter.Success(HttpStatusPositive.OK, new { Consumer = newConsumer }, $"New consumer with name {newConsumer.LastName} and id {newConsumer.ConsumerID} added");
            }
            catch (Exception ex)
            {
                return _exceptionHandlingService.HandleException(ex, "The system is currently undergoing updates. Our team is working diligently to complete this task as quickly as possible.", "Internal exception. Consumer cannot be added currently.", ExceptionType.General);
            }


        }


    }

}