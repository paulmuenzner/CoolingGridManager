using Serilog;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CoolingGridManager.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

// Load Configuration File 
var configuration = ConfigurationHelper.LoadConfiguration();

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Register the logger as a service
builder.Services.AddSingleton<Serilog.ILogger>(_ => Log.Logger);

// Register Exception responses
builder.Services.AddSingleton<ExceptionResponse>();

// Configure database
var connectionString = ConfigurationHelper.GetDatabaseConnectionString();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add support for controllers
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

// Add Api Versioning 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cooling Grid Manager API", Version = "v1" });

    // Add support for XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});


// Register Cron Jobs
builder.Services.AddCustomCronJobs();

// Register Validators
builder.Services.AddValidators();

// Rate Limiter
builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(15);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));

// Register Services
ServiceExtension.AddServices(builder.Services);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // Sets Swagger UI at the root URL
        options.DocumentTitle = "Cooling Grid Manager API";
    });
}

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

app.UseRouting();

app.MapHealthChecks("/health");

app.UseRateLimiter();


// Register routes
RouteExtension.ConfigureRoutes(app);

app.MapGet("/{**slug}", async (context) =>
{
    // Return a 404 Not Found response for any unmatched route
    context.Response.StatusCode = StatusCodes.Status404NotFound;
    await context.Response.WriteAsync("404 - Not Found");
});


app.Run();

Log.CloseAndFlush();