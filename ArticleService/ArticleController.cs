using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/articles")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleManager _articleManager;

    public ArticlesController(IArticleManager articleManager)
    {
        _articleManager = articleManager;
    }
     

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Article>> GetArticle(int id)
    {
        var article = await _articleManager.GetArticleAsync(id);

        if (article == null)
        {
            return NotFound();
        }

        return Ok(article);
    }

    [HttpGet]
    public async Task<ActionResult<List<Article>>> GetAll()
    {
        var articles =
            await _articleManager.GetAllAsync();

        return Ok(articles);
    }

    [HttpPost]
    public async Task<ActionResult<Article>> Create(
        Article article)
    {
        var createdArticle =
            await _articleManager.CreateAsync(article);

        return CreatedAtAction(
            nameof(GetArticle),
            new { id = createdArticle.Id },
            createdArticle);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        Article article)
    {
        var success =
            await _articleManager.UpdateAsync(id, article);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success =
            await _articleManager.DeleteAsync(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}