using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.EntityFrameworkCore;
using static ConfigurationHelper;
using CoolingGridManager.Routes;



var builder = WebApplication.CreateBuilder(args);

// Load Configuration File 
var configuration = ConfigurationHelper.LoadConfiguration();

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Register the logger as a service
builder.Services.AddSingleton<Serilog.ILogger>(_ => Log.Logger);


// Configure database
var connectionString = ConfigurationHelper.GetDatabaseConnectionString();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add support for controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

app.UseRouting();

app.MapAreaControllerRoute(
    name: "customer",
    areaName: "customer",
    pattern: "api/customer/{controller}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "consumption",
    areaName: "consumption",
    pattern: "api/consumption/{controller}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "billing",
    areaName: "billing",
    pattern: "api/billing/{controller}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "grids",
    areaName: "grids",
    pattern: "api/grids/{controller}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "gridsections",
    areaName: "gridsections",
    pattern: "api/gridsections/{controller}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "ticketsystem",
    areaName: "ticketsystem",
    pattern: "api/ticketsystem/{controller}/{action=Index}/{id?}");

app.MapGet("/", () => connectionString);

app.Run();

Log.CloseAndFlush();