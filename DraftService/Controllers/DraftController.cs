namespace DraftService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using DraftService.Interfaces;

[ApiController]
[Route("api/drafts")]

public class DraftsController : ControllerBase
{
    private readonly IDraftService _draftService;

    public DraftsController(IDraftService draftService)
    {
        _draftService = draftService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Draft>>> GetAll()
    {
        var drafts =
            await _draftService.GetAllDraftsAsync();

        return Ok(drafts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Draft>> GetById(int id)
    {
        var draft =
            await _draftService.GetDraftByIdAsync(id);

        if (draft == null)
        {
            return NotFound();
        }

        return Ok(draft);
    }

    [HttpPost]
    public async Task<ActionResult<Draft>> Create(
        Draft draft)
    {   
        Log.Information("Creating a new draft.");
        var createdDraft =
            await _draftService.CreateDraftAsync(draft);
        Log.Information("Draft created successfully.");
        return CreatedAtAction(
            nameof(GetById),
            new { id = createdDraft.Id },
            createdDraft);
            
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        Draft draft)
    {
        var success =
            await _draftService.UpdateDraftAsync(draft);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) 
    {   
        Log.Information("Deleting draft with ID: {DraftId}", id);
        var success =
            await _draftService.DeleteDraftAsync(id);
        Log 
        if (!success)
        {
            Log.Warning("Draft with ID: {DraftId} not found.", id);    
            return NotFound();
        }

        return NoContent();
    }
}