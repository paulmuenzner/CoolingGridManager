using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Exceptions;

namespace CoolingGridManager.Controllers.Consumers
{
    [Area("consumers")]
    [Route("api/consumers/[controller]/{consumerId}")]
    public partial class GetConsumerByIdController : Controller
    {
        private readonly ILogger<GetConsumerByIdController> _logger;
        private readonly ConsumerService _consumerService;
        private readonly ExceptionResponse _exceptionResponse;
        public GetConsumerByIdController(ExceptionResponse exceptionResponse, ILogger<GetConsumerByIdController> logger, ConsumerService consumerService)
        {
            _consumerService = consumerService;
            _exceptionResponse = exceptionResponse;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetConsumerById(int consumerId)
        {
            try
            {
                var consumer = await _consumerService.GetConsumerById(consumerId);
                return Ok(consumer);
            }
            catch (NotFoundException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "No consumer found.", "No consumer found.", ExceptionType.NotFound);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred.", "Action currently not possible.", ExceptionType.General);
            }
            // return Ok(consumer);
            // return Ok($"Retrieved customer with ID: {consumerId}");
            // try
            // {
            //     var consumer = await _consumerService.GetConsumerById(consumerId);
            //     return Ok(consumer);
            // }
            // catch (Exception ex)
            // {
            //     // Log or handle the exception
            //     _logger.LogError(ex, $"Error retrieving consumer with consumerId: {consumerId}");
            //     throw new Exception($"Consumer with ID: {consumerId} not found");
            //     // return StatusCode(500, "An error occurred while retrieving the consumer.");
            // }
        }
    }
}