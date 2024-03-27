using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CoolingGridManager.Routes
{
    public class GridSectionController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Hello World from GridSection API!"); // Replace with desired response
        }

        // ... other actions
    }
    public static class GridSectionRouteConfig
    {
        public static void ConfigureGridSectionRoutes(IEndpointRouteBuilder app)
        {
            app.MapControllerRoute(
                name: "customersApiRoute",
                pattern: "api/gridsection/{action=index}",
                defaults: new { controller = "Customer" }
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
