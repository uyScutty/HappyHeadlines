namespace CommentService;

using Microsoft.EntityFrameworkCore;
 
 

public class CommentManager : ICommentService
{
    private readonly CommentDbContext _dbContext;
    private readonly HttpClient _httpClient;

    private readonly ICommentCache _commentCache;

    public CommentManager(
        CommentDbContext dbContext,
        HttpClient httpClient,
        ICommentCache commentCache)
    {
        _dbContext = dbContext;
        _httpClient = httpClient;
        _commentCache = commentCache;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _dbContext.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _dbContext.Comments.FindAsync(id);
    }

    public async Task<Comment> CreateAsync(Comment comment)
{

    var response = await _httpClient.PostAsJsonAsync(
    "api/profanities/check",
    new { Text = comment.Content });
    // 1. Tjek comment.Text hos ProfanityService
    var containsProfanity = await response.Content.ReadFromJsonAsync<bool>();

    // 2. Hvis profanity -> afvis
    if (containsProfanity)
    {
        throw new InvalidOperationException("Comment contains profanity.");
    }

    // 3. Hvis OK -> gem
    
    {
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();
        return comment;
    }
    }

    public async Task<bool> UpdateAsync(
        int id,
        Comment comment)
    {
        var existingComment =
            await _dbContext.Comments.FindAsync(id);

        if (existingComment == null)
        {
            return false;
        }

        existingComment.Content = comment.Content;
        existingComment.Id = comment.Id;
        existingComment.CreatedAt = comment.CreatedAt;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var comment =
            await _dbContext.Comments.FindAsync(id);

        if (comment == null)
        {
            return false;
        }

        _dbContext.Comments.Remove(comment);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<List<Comment>> GetCommentsForArticleAsync(int articleId)
{
    var cachedComments = await _commentCache.GetCommentsAsync(articleId);

    if (cachedComments != null)
    {
        await _commentCache.TouchArticleAsync(articleId);

        return cachedComments;
    }

    var comments = await _dbContext.Comments
        .Where(c => c.ArticleId == articleId)
        .ToListAsync();

    await _commentCache.SetCommentsAsync(articleId, comments);

    await _commentCache.TouchArticleAsync(articleId);

    await _commentCache.EnforceLimitAsync();

    return comments;
}

}