using Enyim.Caching;
using ArticleShared;

namespace ArticleService;

public class ArticleCache
{
    
private readonly IMemcachedClient _memcachedClient;

    public ArticleCache(IMemcachedClient memcachedClient)
    {
        _memcachedClient = memcachedClient;
    }

public async Task<CachedArticleDto?> GetArticleAsync(int articleId)
{
    var result = await _memcachedClient.GetAsync<CachedArticleDto>(
        $"article:{articleId}");

    return result.Success ? result.Value : null;
}    

/*
public async Task SetArticleAsync(CachedArticleDto article)
{
    await _memcachedClient.SetAsync(
        $"article:{article.Id}",
        article,
        1200);
}
*/
}
