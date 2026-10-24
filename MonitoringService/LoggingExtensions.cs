using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;

namespace MonitoringService;

public static class LoggingExtensions
{
    public static void ConfigureSharedLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithProperty("ServiceName", builder.Environment.ApplicationName)      
            .CreateLogger();

        builder.Host.UseSerilog(); 
    }
}    