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

builder
    .Services.AddMarten(config =>
        config.Connection(builder.Configuration.GetConnectionString("Database")!)
    )
    .UseLightweightSessions(); // Allows use of lightweight sessions for read/write operations

builder.Services.AddLogging();

builder.Services.AddExceptionHandler<eShopExceptionHandler>();

var app = builder.Build();

// Configure HTTP request pipeline.
app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();
