using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace MonitoringService;

public static class MonitoringExtensions
{
    public static IServiceCollection AddMonitoring(
        this IServiceCollection services,
        string serviceName)
    {
        services.AddSingleton<CacheMetrics>();

        services.AddOpenTelemetry()
            .ConfigureResource(resource =>
                resource.AddService(serviceName))
            .WithMetrics(metrics =>
            {
                metrics.AddMeter(CacheMetrics.MeterName);

                metrics.AddOtlpExporter((options, readerOptions) =>
                {
                    options.Endpoint =
                        new Uri("http://seq:5341/ingest/otlp/v1/metrics");

                    options.Protocol =
                        OtlpExportProtocol.HttpProtobuf;

                    readerOptions.TemporalityPreference =
                        MetricReaderTemporalityPreference.Delta;
                });
            });

        return services;
    }
}