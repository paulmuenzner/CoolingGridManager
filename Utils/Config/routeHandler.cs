public static class CustomRouteExtensions
{
    public static void MapAreaRoute(this WebApplication app, string areaName, string pattern)
    {
        app.MapAreaControllerRoute(
            name: areaName,
            areaName: areaName,
            pattern: $"api/{areaName}/{pattern}").RequireRateLimiting("fixed");
    }
}