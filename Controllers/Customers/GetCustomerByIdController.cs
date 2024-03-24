using Microsoft.AspNetCore.Mvc;

namespace CoolingGridManager.Controllers.Customer
{
    [Area("customer")]
    [Route("api/customer/[controller]/{id}")]
    public partial class GetCustomerByIdController : Controller
    {
        private readonly ILogger<GetCustomerByIdController> _logger;
        public GetCustomerByIdController(ILogger<GetCustomerByIdController> logger)
        {
            _logger = logger;
        }
        // Customer-related actions (e.g., GetCustomerById, CreateCustomer)
        public IActionResult GetCustomerById(int id)
        {
            // ... your logic to retrieve customer
            return Ok($"Retrieved customer with ID: {id}");
        }
    }
}