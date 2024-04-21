
// Centralized provision of app data
public static class AppData
{
    // Permitted time frame data for year validation 
    public static int TimeFrameYearMin { get; } = 2020;
    public static int TimeFrameYearMax { get; } = 2035;

    // Regex
    public static readonly string UuidPattern = @"^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$";

}