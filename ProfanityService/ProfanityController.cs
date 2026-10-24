 using Microsoft.AspNetCore.Mvc;
 

[ApiController]
[Route("api/profanities")]

public class ProfanityController : ControllerBase
{
 private readonly IProfanityService _profanityService;

    public ProfanityController(IProfanityService profanityService)
    {
        _profanityService = profanityService;
    }

    [HttpPost("check")]
    public async Task<ActionResult<bool>> Check(ProfanityCheckRequest request)
    {
        var containsProfanity =
            await _profanityService.ContainsProfanityAsync(request.Text);

        return Ok(containsProfanity);
  }
}
 
