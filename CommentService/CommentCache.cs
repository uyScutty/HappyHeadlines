using StackExchange.Redis;
using System.Text.Json;

namespace CommentService;

public class CommentCache : ICommentCache
{
    private readonly IDatabase _cache;
    private readonly CacheMetrics _metrics;

    public CommentCache(IConnectionMultiplexer redis, CacheMetrics metrics)
    {
        _cache = redis.GetDatabase();
        _metrics = metrics;
    }
    public async Task<List<Comment>?> GetCommentsAsync(int articleId)
{
    var value = await _cache.StringGetAsync(
        $"comments:article:{articleId}");

    if (value.IsNullOrEmpty)
    {
        _metrics.RecordMiss("comments");
        return null;
    }
    _metrics.RecordHit("comments");

    return JsonSerializer.Deserialize<List<Comment>>(value!);
}
public async Task SetCommentsAsync(int articleId, List<Comment> comments)
{
    var json = JsonSerializer.Serialize(comments);

    await _cache.StringSetAsync(
        $"comments:article:{articleId}",
        json
    );
}
public async Task TouchArticleAsync(int articleId)
{
    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    await _cache.SortedSetAddAsync(
        "comments:recent_articles",
        articleId,
        timestamp);
}
public async Task EnforceLimitAsync()
{
    var count = await _cache.SortedSetLengthAsync(
        "comments:recent_articles");

    if (count <= 30)
        return;

    var oldest = await _cache.SortedSetRangeByRankAsync(
        "comments:recent_articles",
        0,
        0);

    if (oldest.Length == 0)
        return;

    var articleId = oldest[0];

    await _cache.KeyDeleteAsync(
        $"comments:article:{articleId}");

    await _cache.SortedSetRemoveAsync(
        "comments:recent_articles",
        articleId);
}

}