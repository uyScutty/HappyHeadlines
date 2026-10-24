using Microsoft.EntityFrameworkCore;


public class ProfanityManager : IProfanityService
{
    private readonly ProfanityDbContext _context;

    public ProfanityManager(ProfanityDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ContainsProfanityAsync(string text)
    {
        var profanityWords = await _context.ProfanityWords
            .Select(p => p.Word.ToLower())
            .ToListAsync();

        var wordsInText = text
            .ToLower()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return wordsInText.Any(word => profanityWords.Contains(word));
    }
}