namespace ArticleService;
using Microsoft.EntityFrameworkCore;
using ArticleShared;

public class ArticleManager : IArticleManager
{
    private readonly ArticleDbContext _dbContext;

    private readonly ArticleCache _articleCache;

    public ArticleManager(ArticleDbContext dbContext, ArticleCache articleCache)
    {
        _dbContext = dbContext;
        _articleCache = articleCache;
    }
 
    public async Task<Article?> GetArticleAsync(int id)
{
    var cachedArticle = await _articleCache.GetArticleAsync(id);

    if (cachedArticle != null)
    {
         return new Article
        {
            Id = cachedArticle.Id,
            Title = cachedArticle.Title,
            Content = cachedArticle.Content,
            PublishedAt = cachedArticle.PublishedAt
        };
    }

    return await _dbContext.Articles.FindAsync(id);
}

    public async Task<List<Article>> GetAllAsync()
    {
        return await _dbContext.Articles.ToListAsync();
    }


    public async Task<Article?> GetByIdAsync(int id)
    {
        return await _dbContext.Articles.FindAsync(id);
    }


    public async Task<Article> CreateAsync(Article article)
    {
        _dbContext.Articles.Add(article);

        await _dbContext.SaveChangesAsync();

        return article;
    }


    public async Task<bool> UpdateAsync(
        int id,
        Article article)
    {
        var existingArticle =
            await _dbContext.Articles.FindAsync(id);

        if (existingArticle == null)
        {
            return false;
        }

        existingArticle.Title = article.Title;
        existingArticle.Content = article.Content;
        existingArticle.PublishedAt = article.PublishedAt;

        await _dbContext.SaveChangesAsync();

        return true;
    }


    public async Task<bool> DeleteAsync(int id)
    {
        var article =
            await _dbContext.Articles.FindAsync(id);

        if (article == null)
        {
            return false;
        }

        _dbContext.Articles.Remove(article);

        await _dbContext.SaveChangesAsync();

        return true;
    }
}