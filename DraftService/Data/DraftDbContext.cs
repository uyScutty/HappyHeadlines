namespace DraftService.Data;
using DraftService.Models;
using Microsoft.EntityFrameworkCore;

public class DraftDbContext : DbContext
{
    public DraftDbContext(DbContextOptions<DraftDbContext> options) : base(options)
    {
    }

    public DbSet<Draft> Drafts { get; } = new List<Draft>();
}
