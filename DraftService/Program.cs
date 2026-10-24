

using DraftService.Data;
using DraftService.Services;
using DraftService.Models;
using DraftService.Interfaces;
using MonitoringService.LoggingExtensions;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();


// Logging
builder.Services.ConfigureSharedLogging("DraftService");

// Services
builder.Services.AddScoped<IDraftService,DraftApplicationService >();


// Database
builder.Services.AddDbContext<DraftDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


//

// OpenTelemetry
builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddJaegerExporter();
});

var app = builder.Build();

app.MapControllers();

app.Run();