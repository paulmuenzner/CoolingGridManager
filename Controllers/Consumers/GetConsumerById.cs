using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Services;
using CoolingGridManager.Exceptions;
using CoolingGridManager.Validators.Consumers;
using FluentValidation.Results;
using CoolingGridManager.ResponseHandler;

namespace CoolingGridManager.Controllers.Consumers
{
    [Area("consumers")]
    [Route("api/consumers/[controller]/{consumerId}")]
    public partial class GetConsumerController : Controller
    {
        private readonly GetConsumerDetailsValidator _getConsumerDetailsValidator;
        private readonly ConsumerService _consumerService;
        private readonly ExceptionResponse _exceptionResponse;
        public GetConsumerController(GetConsumerDetailsValidator getConsumerDetailsValidator, ExceptionResponse exceptionResponse, ConsumerService consumerService)
        {
            _consumerService = consumerService;
            _getConsumerDetailsValidator = getConsumerDetailsValidator;
            _exceptionResponse = exceptionResponse;
        }

        [HttpGet]
        public async Task<IActionResult> GetConsumerDetails(int consumerId)
        {
            try
            {

                // Validate
                ValidationResult result = await _getConsumerDetailsValidator.ValidateAsync(consumerId);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        return ResponseFormatter.Negative(HttpStatusNegative.UnprocessableEntity, new { Error = error }, $"{error.ErrorMessage}", $"{error.ErrorMessage}", null);
                    }
                }

                var consumer = await _consumerService.GetConsumerDetails(consumerId);
                return Ok(consumer); // Prepeare formatted response
            }
            catch (NotFoundException ex)
            {// Prepeare formatter
                return _exceptionResponse.ExceptionResponseHandle(ex, "No consumer found.", "No consumer found.", ExceptionType.NotFound);
            }
            catch (Exception ex)
            {// Prepeare formatter
                return _exceptionResponse.ExceptionResponseHandle(ex, "An unexpected error occurred.", "Action currently not possible.", ExceptionType.General);
            }
        }
    }
}