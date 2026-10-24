using Microsoft.AspNetCore.Mvc;
using CommentService;


[ApiController]
[Route("api/comments")]


public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }



    [HttpGet("article/{articleId:int}")]
    public async Task<ActionResult<List<Comment>>> GetCommentsForArticle(int articleId)
    {
        var comments = await _commentService.GetCommentsForArticleAsync(articleId);

    return Ok(comments);
   }

    [HttpGet]
    public async Task<ActionResult<List<Comment>>> GetAll()
    {
        var comments =
            await _commentService.GetAllAsync();

        return Ok(comments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetById(int id)
    {
        var comment =
            await _commentService.GetByIdAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment);
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> Create(
        Comment comment)
    {
        var createdComment =
            await _commentService.CreateAsync(comment);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdComment.Id },
            createdComment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        Comment comment)
    {
        var success =
            await _commentService.UpdateAsync(id, comment);

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
            await _commentService.DeleteAsync(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}