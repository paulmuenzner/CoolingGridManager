using Microsoft.AspNetCore.Mvc;
// using CoolingGridManager.Services;
using CoolingGridManager.Models;

using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CoolingGridManager.Controllers.Consumers
{
    [Area("consumers")]
    [Route("consumers")]
    public class CreateConsumersController : Controller
    {
        private readonly ILogger<CreateConsumersController> _logger;
        private readonly IHostEnvironment _env;
        private readonly AppDbContext _context;
        public CreateConsumersController(ILogger<CreateConsumersController> logger, AppDbContext context, /*ConsumerService consumerService,*/ IHostEnvironment env)
        {
            _context = context;
            _logger = logger;
            // _consumerService = consumerService;
            _env = env;
        }
        public async Task<ActionResult<IEnumerable<Consumer>>> Index()
        {
            return await _context.Consumers.ToListAsync();
            ///////////////////////////////
            // if (ModelState.IsValid)
            // {
            //     try
            //     {
            //         var user = await _consumerService.CreateUserAsync(model);
            //         if (user != null)
            //         {
            //             return RedirectToAction("Index", new { message = "User created successfully!" });
            //         }
            //         else
            //         {
            //             // Handle creation failure (e.g., model state errors)
            //             ModelState.AddModelError("", "Error creating user. Please try again.");
            //         }
            //     }
            //     catch (Exception ex)
            //     {
            //         // Handle unexpected errors
            //         ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
            //         _logger.LogError(ex, "Error creating user");
            //     }
            // }

            // return View(model);
        }
        // Customer-related actions (e.g., GetCustomerById, CreateCustomer)
        // public IActionResult GetCustomerById(int id)
        // {
        //     /// ... your logic to retrieve customer
        //     return Ok($"Retrieved customer with ID: {id}");
        // }

    }

}