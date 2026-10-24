namespace CommentService
    {
    public interface ICommentCache
    {


    Task<List<Comment>?> GetCommentsAsync(int articleId);

    Task SetCommentsAsync(int articleId, List<Comment> comments);

    Task TouchArticleAsync(int articleId);
    Task EnforceLimitAsync();
    }
    }