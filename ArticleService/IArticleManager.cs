public interface IArticleManager
{
    Task<List<Article>> GetAllAsync();

    Task<Article?> GetArticleAsync(int id);

    Task<Article?> GetByIdAsync(int id);

    Task<Article> CreateAsync(Article article);

    Task<bool> UpdateAsync(int id, Article article);

    Task<bool> DeleteAsync(int id);
}