using Microsoft.EntityFrameworkCore;


public class ProfanityDbContext : DbContext
{
    public ProfanityDbContext(DbContextOptions<ProfanityDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProfanityWord> ProfanityWords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<ProfanityWord>().HasData(
        new ProfanityWord { Id = 1, Word = "fuck" },
        new ProfanityWord { Id = 2, Word = "shit" },
        new ProfanityWord { Id = 3, Word = "asshole" },
        new ProfanityWord { Id = 4, Word = "bitch" },
        new ProfanityWord { Id = 5, Word = "idiot" },
        new ProfanityWord { Id = 6, Word = "moron" },
        new ProfanityWord { Id = 7, Word = "damn" },
        new ProfanityWord { Id = 8, Word = "crap" },
        new ProfanityWord { Id = 9, Word = "bastard" },
        new ProfanityWord { Id = 10, Word = "dick" }
    );
}
}