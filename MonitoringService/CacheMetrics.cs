using System.Diagnostics.Metrics;


public class CacheMetrics
{
    public const string MeterName = "HappyHeadlines.Cache";

    private static readonly Meter Meter = new(MeterName);

  

    private readonly Counter<long> _hits =
        Meter.CreateCounter<long>("cache_hits");

    private readonly Counter<long> _misses =
        Meter.CreateCounter<long>("cache_misses");

    public void RecordHit(string cacheName)
{
    _hits.Add(1,
        new KeyValuePair<string, object?>("cache", cacheName));
}

public void RecordMiss(string cacheName)
{
    _misses.Add(1,
        new KeyValuePair<string, object?>("cache", cacheName));
}
}