using Microsoft.AspNetCore.Mvc;
using CoolingGridManager.Models;

namespace CoolingGridManager.Controllers.Customer
{
    [Area("Customer")]
    [Route("customer")]
    public class CreateCustomerController : Controller
    {
        private readonly ILogger<CreateCustomerController> _logger;

        public CreateCustomerController(ILogger<CreateCustomerController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {

            return Ok("Hello World from Customers ss PIv!"); // Replace with desired response
        }
        // Customer-related actions (e.g., GetCustomerById, CreateCustomer)
        // public IActionResult GetCustomerById(int id)
        // {
        //     /// ... your logic to retrieve customer
        //     return Ok($"Retrieved customer with ID: {id}");
        // }

    }

}