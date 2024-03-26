using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.EntityFrameworkCore;
using static ConfigurationHelper;
using CoolingGridManager.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;


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
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

// Add validation service
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddScoped<ConsumerService>();
builder.Services.AddScoped<GridService>();
builder.Services.AddScoped<GridSectionService>();
builder.Services.AddScoped<TicketService>();

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

app.MapAreaRoute("consumers", "{controller}/{index}");
app.MapAreaRoute("gridsections", "{controller}/{index}");
app.MapAreaRoute("grids", "{controller}/{action=Index}/{consumerId?}");
app.MapAreaRoute("tickets", "{controller}/{index}");


// app.MapAreaControllerRoute(
//     name: "consumption",
//     areaName: "consumption",
//     pattern: "api/consumption/{controller}/{action=Index}/{id?}");

// app.MapAreaControllerRoute(
//     name: "billing",
//     areaName: "billing",
//     pattern: "api/billing/{controller}/{action=Index}/{id?}");


// app.MapAreaControllerRoute(
//     name: "gridsections",
//     areaName: "gridsections",
//     pattern: "api/gridsections/{controller}/{action=Index}/{id?}");



app.MapGet("/{**slug}", async (context) =>
{
    // Return a 404 Not Found response for any unmatched route
    context.Response.StatusCode = StatusCodes.Status404NotFound;
    await context.Response.WriteAsync("404 - Not Found");
});


app.Run();

Log.CloseAndFlush();