namespace ArticleCacheWorker.Data;

public class ArticleReadModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime PublishedAt { get; set; }
}