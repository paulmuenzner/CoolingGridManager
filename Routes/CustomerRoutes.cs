using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;


namespace CoolingGridManager.Routes
{
    public static class CustomerRouteConfig
    {
        public static void ConfigureCustomerRoutes(IEndpointRouteBuilder app)
        {
            // app.MapControllerRoute(
            //      name: "customersApiRoute", // Define a name for the route
            //     pattern: "api/customers/{action=index}", // Specify the URL pattern
            //     defaults: new { controller = "Customer" } // Set default controller
            // );

            // app.MapControllerRoute(
            //      name: "GetCustomerByIdAction",
            //     pattern: "api/customers/{action=index}/{id}",
            //     defaults: new { controller = "Customer" }
            // );
            // ... other sub-routes for customers
        }
    }
}
