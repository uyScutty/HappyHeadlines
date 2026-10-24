using ArticleCacheWorker.Data;
using Microsoft.EntityFrameworkCore;


var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddEnyimMemcached(options =>
{
    options.AddServer(
        builder.Configuration["Cache:Memcached:Host"] ?? "localhost",
        builder.Configuration.GetValue("Cache:Memcached:Port", 11211));
});

//builder.Services.AddHostedService<ArticleCacheRefreshService>();

builder.Services.AddDbContext<ArticleReadDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("GlobalArticleDatabase")));


var app = builder.Build();


app.Run();
