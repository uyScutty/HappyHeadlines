/*
using ArticleService;
using Microsoft.EntityFrameworkCore;

public class ArticleCacheRefreshService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ArticleCacheRefreshService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<ArticleDbContext>();

            var cutoff = DateTime.UtcNow.AddDays(-14);

            var articles = await db.Articles
                .Where(a => a.PublishedAt >= cutoff)
                .ToListAsync(stoppingToken);

            var cache = scope.ServiceProvider
                .GetRequiredService<ArticleCache>();

            foreach (var article in articles)
           {
                await cache.SetArticleAsync(article);
            }    

            await Task.Delay(
                TimeSpan.FromHours(1),
                stoppingToken);
        }
    }
}
*/