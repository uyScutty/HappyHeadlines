namespace CommentService{
public interface ICommentService
{
    Task<List<Comment>> GetAllAsync();

    Task<Comment?> GetByIdAsync(int id);

    Task<Comment> CreateAsync(Comment comment);

    Task<bool> UpdateAsync(int id, Comment comment);

    Task<bool> DeleteAsync(int id);

    Task<List<Comment>> GetCommentsForArticleAsync(int articleId);


}}