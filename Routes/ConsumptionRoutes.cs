using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CoolingGridManager.Routes
{
    public class ConsumptionController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Hello World from Consumption API!"); // Replace with desired response
        }

        // ... other actions
    }
    public static class ConsumptionRouteConfig
    {
        public static void ConfigureConsumptionRoutes(IEndpointRouteBuilder app)
        {
            app.MapControllerRoute(
                name: "customersApiRoute", // Define a name for the route
                pattern: "api/customers/{action=index}", // Specify the URL pattern
                defaults: new { controller = "Customer" } // Set default controller
            );

            // ... other sub-routes for customers
        }
        // public static void ConfigureCustomerRoutes(IEndpointRouteBuilder app)
        // {
        //     // Define sub-routes for customers here (e.g., GET /api/customers, POST /api/customers)
        //     app.MapControllerRoute(
        //         name: "customersApiRoute",
        //         pattern: "api/customers/{action=index}",
        //         defaults: new { controller = "Customer" }
        //     );

        //     // Add additional sub-routes for specific customer functionalities
        //     // (e.g., GET /api/customers/{id}, PUT /api/customers/{id})
        // }
    }
}
