using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.ResponseHandler;
using CoolingGridManager.Models;
using CoolingGridManager.Services;


namespace CoolingGridManager.Controllers.Consumers
{
    [Area("consumers")]
    [Route("api/consumers/[controller]")]
    public class AddConsumerController : ControllerBase
    {
        private readonly ILogger<AddConsumerController> _logger;

        private readonly ConsumerService _consumerService;

        private readonly ExceptionResponse _exceptionResponse;
        public AddConsumerController(ExceptionResponse exceptionResponse, ILogger<AddConsumerController> logger, ConsumerService consumerService, IHostEnvironment env)
        {
            _logger = logger;
            _consumerService = consumerService;
            _exceptionResponse = exceptionResponse;
        }

        [HttpPost]
        public async Task<IActionResult> AddConsumer([FromBody] Consumer consumer)
        {
            try
            {
                var newConsumer = await _consumerService.Add(consumer);
                return ResponseFormatter.FormatSuccessResponse(HttpStatus.OK, new { Consumer = newConsumer }, $"New consumer with name {newConsumer.LastName} and id {newConsumer.ConsumerID} added");
            }
            catch (System.FormatException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "Grid name already exists. Choose a different name.", "Choose a different name.", ExceptionType.Format);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, $"An unexpected error occurred. {ex.GetType().Name}", "Acction currently not possible.", ExceptionType.General);
            }


        }


    }

}