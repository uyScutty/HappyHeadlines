using ArticleCacheWorker.Data;
using ArticleShared;
using Enyim.Caching;
using Microsoft.EntityFrameworkCore;

namespace ArticleCacheWorker;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMemcachedClient _cache;

    public Worker(
        IServiceScopeFactory scopeFactory,
        IMemcachedClient cache)
    {
        _scopeFactory = scopeFactory;
        _cache = cache;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<ArticleReadDbContext>();

            var cutoff = DateTime.UtcNow.AddDays(-14);

            var articles = await db.Articles
                .Where(a => a.PublishedAt >= cutoff)
                .ToListAsync(stoppingToken);

            foreach (var article in articles)
            {
                var cachedArticle = new CachedArticleDto
                {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    PublishedAt = article.PublishedAt
                };

                await _cache.SetAsync(
                    $"article:{article.Id}",
                    cachedArticle,
                    21600
                );
            }
        } 
            await Task.Delay(
                TimeSpan.FromHours(1),
                stoppingToken);
        
    }
}