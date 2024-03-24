using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.EntityFrameworkCore;
using static ConfigurationHelper;
using CoolingGridManager.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


// using CoolingGridManager.Services;


var builder = WebApplication.CreateBuilder(args);

// Load Configuration File 
var configuration = ConfigurationHelper.LoadConfiguration();

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Register the logger as a service
builder.Services.AddSingleton<Serilog.ILogger>(_ => Log.Logger);

builder.Services.AddSingleton<ExceptionResponse>();


// Configure database
var connectionString = ConfigurationHelper.GetDatabaseConnectionString();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add support for controllers
builder.Services.AddControllers();

builder.Services.AddScoped<ConsumerService>();

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

app.MapAreaRoute("customer", "{controller}/{action=Index}/{consumerId?}");

// app.MapAreaControllerRoute(
//     name: "consumption",
//     areaName: "consumption",
//     pattern: "api/consumption/{controller}/{action=Index}/{id?}");

// app.MapAreaControllerRoute(
//     name: "billing",
//     areaName: "billing",
//     pattern: "api/billing/{controller}/{action=Index}/{id?}");

// app.MapAreaControllerRoute(
//     name: "grids",
//     areaName: "grids",
//     pattern: "api/grids/{controller}/{action=Index}/{id?}");

// app.MapAreaControllerRoute(
//     name: "gridsections",
//     areaName: "gridsections",
//     pattern: "api/gridsections/{controller}/{action=Index}/{id?}");

// app.MapAreaControllerRoute(
//     name: "ticketsystem",
//     areaName: "ticketsystem",
//     pattern: "api/ticketsystem/{controller}/{action=Index}/{id?}");

app.MapGet("/{**slug}", async (context) =>
{
    // Return a 404 Not Found response for any unmatched route
    context.Response.StatusCode = StatusCodes.Status404NotFound;
    await context.Response.WriteAsync("404 - Not Found");
});


app.Run();

Log.CloseAndFlush();