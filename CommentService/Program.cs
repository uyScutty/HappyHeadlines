using Microsoft.Extensions.Http;
using Microsoft.Extensions.Http.Resilience;
using StackExchange.Redis;
using Polly;
using CommentService;
using MonitoringService;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Logging
builder.ConfigureSharedLogging();

builder.Services.AddMonitoring("CommentService");

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(
        builder.Configuration["Cache:Redis"] ?? "localhost:6379"));

builder.Services.AddDbContext<CommentDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CommentDatabase")));

builder.Services.AddScoped<ICommentCache, CommentCache>();




builder.Services
    .AddHttpClient<ICommentService, CommentManager>(client =>
    {
        client.BaseAddress = new Uri("http://profanity-service");
    })
    .AddResilienceHandler("profanity-pipeline", pipeline =>
    {
        // 1. CIRCUIT BREAKER
        pipeline.AddCircuitBreaker(
            new HttpCircuitBreakerStrategyOptions
            {
                FailureRatio = 1.0,
                MinimumThroughput = 3,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(30),

                OnOpened = args =>
                {
                    Console.WriteLine(
                        "Circuit breaker OPEN - ProfanityService unavailable.");

                    return ValueTask.CompletedTask;
                },

                OnHalfOpened = args =>
                {
                    Console.WriteLine(
                        "Circuit breaker HALF-OPEN - testing ProfanityService.");

                    return ValueTask.CompletedTask;
                },

                OnClosed = args =>
                {
                    Console.WriteLine(
                        "Circuit breaker CLOSED - ProfanityService available.");

                    return ValueTask.CompletedTask;
                }
            });

        // 2. RETRY
        pipeline.AddRetry(
            new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 2,

                Delay = TimeSpan.FromMilliseconds(500),

                BackoffType = DelayBackoffType.Exponential,

                OnRetry = args =>
                {
                    Console.WriteLine(
                        $"Retry #{args.AttemptNumber + 1}");

                    return ValueTask.CompletedTask;
                }
            });

        // 3. TIMEOUT PER ATTEMPT
        pipeline.AddTimeout(
            TimeSpan.FromSeconds(2));
    });

var app = builder.Build();

app.MapControllers();

app.Run();