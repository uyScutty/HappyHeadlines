using Microsoft.EntityFrameworkCore;

namespace ArticleCacheWorker.Data;

public class ArticleReadDbContext : DbContext
{
    public ArticleReadDbContext(
        DbContextOptions<ArticleReadDbContext> options)
        : base(options)
    {
    }

    public DbSet<ArticleReadModel> Articles => Set<ArticleReadModel>();
}
