using CoolingGridManager.Services;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtension
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ConsumerService>();
        services.AddScoped<GridService>();
        services.AddScoped<GridSectionService>();
        services.AddScoped<TicketService>();
        services.AddScoped<ConsumptionConsumerService>();
        services.AddScoped<ConsumptionGridService>();
        services.AddScoped<GridParameterLogService>();
        services.AddScoped<BillingService>();
    }
}