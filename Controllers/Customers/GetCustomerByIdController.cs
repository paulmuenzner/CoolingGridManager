using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;


namespace CoolingGridManager.Controllers.Customer
{
    [Area("customer")]
    [Route("api/customer/[controller]/{consumerId}")]
    public partial class GetCustomerByIdController : Controller
    {
        private readonly ILogger<GetCustomerByIdController> _logger;
        private readonly ConsumerService _consumerService;
        private readonly ExceptionResponse _exceptionResponse;
        public GetCustomerByIdController(ExceptionResponse exceptionResponse, ILogger<GetCustomerByIdController> logger, ConsumerService consumerService)
        {
            _consumerService = consumerService;
            _exceptionResponse = exceptionResponse;
            _logger = logger;
        }
        public async Task<IActionResult> GetConsumerById(int consumerId)
        {
            try
            {
                var consumer = await _consumerService.GetConsumerById(consumerId);
                return Ok(consumer);
            }
            catch (NotFoundException ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex.ToString(), "No consumer found.", ExceptionType.NotFound);
            }
            catch (Exception ex)
            {
                return _exceptionResponse.ExceptionResponseHandle(ex.ToString(), "An unexpected error occurred.", ExceptionType.General);
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