namespace DraftService.Services;
using DraftService.Interfaces;
using DraftService.Models;
using DraftService.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Serilog;


public class DraftApplicationService : IDraftService
{
    private readonly DraftDbContext _dbContext;
    private readonly HttpClient _httpClient;

    private readonly ILogger<DraftApplicationService> _logger;

    public DraftApplicationService(DraftDbContext dbContext)
    {
        _dbContext = dbContext;
        _httpClient = new HttpClient();
        _logger = Log.ForContext<DraftApplicationService>();
    }

    public async Task<Draft> CreateDraftAsync(Draft draft)
    {
        _logger.Information("Creating a new draft.");
        _dbContext.Drafts.Add(draft);
        await _dbContext.SaveChangesAsync();
        _logger.Information("Draft created successfully.");
        return draft;
    }

    public async Task<Draft> GetDraftByIdAsync(int id)
    {
        _logger.Information("Fetching draft by ID.");
        return await _dbContext.Drafts.FindAsync(id);
    }

    public async Task<IEnumerable<Draft>> GetAllDraftsAsync()
    {
        _logger.Information("Fetching all drafts.");
        return await _dbContext.Drafts.ToListAsync();
    }

    public async Task<Draft> UpdateDraftAsync(Draft draft)
    {
        _logger.Information("Updating draft.");
        _dbContext.Drafts.Update(draft);
        await _dbContext.SaveChangesAsync();
        _logger.Information("Draft updated successfully.");
        return draft;
    }

    public async Task<bool> DeleteDraftAsync(int id)
    {
        _logger.Information("Deleting draft.");
        var draft = await _dbContext.Drafts.FindAsync(id);
        if (draft == null)
            return false;

        _logger.Information("Draft found. Proceeding with deletion.");
        _dbContext.Drafts.Remove(draft);
        await _dbContext.SaveChangesAsync();
        _logger.Information("Draft deleted successfully.");
        return true;
    }
}