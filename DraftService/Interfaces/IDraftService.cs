namespace DraftService.Interfaces;
using DraftService.Models;
public interface IDraftService
{
    Task<Draft> CreateDraftAsync(Draft draft);
    Task<Draft> GetDraftByIdAsync(int id);
    Task<IEnumerable<Draft>> GetAllDraftsAsync();
    Task<Draft> UpdateDraftAsync(Draft draft);
    Task<bool> DeleteDraftAsync(int id);
}