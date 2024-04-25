using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

public static class RouteExtension
{
    public static void ConfigureRoutes(WebApplication app)
    {
        app.MapAreaRoute("consumers", "{controller}/{index}");
        app.MapAreaRoute("gridsections", "{controller}/{index}");
        app.MapAreaRoute("grids", "{controller}/{index}");
        app.MapAreaRoute("tickets", "{controller}/{index}");
        app.MapAreaRoute("consumptionconsumers", "{controller}/{index}");
        app.MapAreaRoute("billing", "{controller}/{index}");
        app.MapAreaRoute("gridparameters", "{controller}/{index}");
        app.MapAreaRoute("consumptiongrid", "{controller}/{index}");
        app.MapAreaRoute("gridefficiency", "{controller}/{index}");
    }
}
