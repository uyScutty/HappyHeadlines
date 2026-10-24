using Enyim.Caching;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cache")]
public class CacheTestController : ControllerBase
{
    private readonly IMemcachedClient _cache;

    public CacheTestController(IMemcachedClient cache)
    {
        _cache = cache;
    }

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var value = await _cache.GetValueOrCreateAsync(
            "test-key",
            600,
            () => Task.FromResult("Memcached virker!")
        );

        return Ok(value);
    }
}