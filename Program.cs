using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.EntityFrameworkCore;
using CoolingGridManager.Services;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using CoolingGridManager.Validators.Tickets;
using CoolingGridManager.Validators.Grids;
using CoolingGridManager.Validators.GridSections;
using CoolingGridManager.Validators.Consumptions;
using CoolingGridManager.Validators.Consumers;
// using CoolingGridManager.Validators.Billing;


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

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();


builder.Services.AddScoped<TicketAddValidator>();
builder.Services.AddScoped<TicketGetByIdValidator>();
builder.Services.AddScoped<AddGridValidator>();
builder.Services.AddScoped<AddGridSectionValidator>();
builder.Services.AddScoped<AddConsumerValidator>();

// Add services
builder.Services.AddScoped<ConsumerService>();
builder.Services.AddScoped<GridService>();
builder.Services.AddScoped<GridSectionService>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<ConsumptionService>();
builder.Services.AddScoped<BillingService>();

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
app.MapAreaRoute("grids", "{controller}/{action=Index}/{consumerId?}"); // prepare
app.MapAreaRoute("tickets", "{controller}/{index}");
app.MapAreaRoute("consumptions", "{controller}/{index}");
app.MapAreaRoute("billing", "{controller}/{index}");


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