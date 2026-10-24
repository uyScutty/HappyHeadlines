
using ArticleService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEnyimMemcached(options =>
{
    options.AddServer(
        builder.Configuration["Cache:Memcached:Host"] ?? "localhost",
        builder.Configuration.GetValue("Cache:Memcached:Port", 11211));
});

//builder.Services.AddHostedService<ArticleCacheRefreshService>();

builder.Services.AddDbContext<ArticleDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("GlobalArticleDatabase")));

builder.Services.AddScoped<ArticleCache, ArticleCache>();
builder.Services.AddScoped<IArticleManager, ArticleManager>();

var app = builder.Build();

app.UseEnyimMemcached();

app.MapControllers();

app.Run();
