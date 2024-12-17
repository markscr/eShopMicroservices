using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter();

Assembly assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    // Register the Validation Behavior pipeline
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    // Register the Logging Behavior pipeline
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

string dbConnectionString = builder.Configuration.GetConnectionString("Database")!;

builder
    .Services.AddMarten(config =>
    {
        config.Connection(dbConnectionString);
        config.Schema.For<ShoppingCart>().Identity(x => x.UserName); // Modifying the schema definition from Marten
    })
    .UseLightweightSessions(); // Allows use of lightweight sessions for read/write operations

builder.Services.AddLogging();

// Adding repositories
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>(); // Using Scrutor lib to decorate the repository with caching

// Adding Redis
string redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = redisConnectionString
);

// Adding global exception handler
builder.Services.AddExceptionHandler<eShopExceptionHandler>();

// Adding Health checks
builder.Services.AddHealthChecks().AddNpgSql(dbConnectionString).AddRedis(redisConnectionString);

var app = builder.Build();

// Configure HTTP request pipeline
app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks(
    "/health",
    new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse }
);

app.Run();
