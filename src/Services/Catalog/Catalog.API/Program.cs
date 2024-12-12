var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.AddCarter();

string connectionString = builder.Configuration.GetConnectionString("Database")!;

builder.Services.AddMarten(config => config.Connection(connectionString)).UseLightweightSessions(); // Allows use of lightweight sessions for read/write operations

if (builder.Environment.IsDevelopment())
{
    // Seed DB
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddLogging();

builder.Services.AddExceptionHandler<eShopExceptionHandler>();

builder.Services.AddHealthChecks().AddNpgSql(connectionString);

var app = builder.Build();

// Configure HTTP request pipeline.
app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks(
    "/health",
    new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse }
);

app.Run();
