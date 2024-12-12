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

string connectionString = builder.Configuration.GetConnectionString("Database")!;

//builder.Services.AddMarten(config => config.Connection(connectionString)).UseLightweightSessions(); // Allows use of lightweight sessions for read/write operations

builder.Services.AddLogging();

var app = builder.Build();

// Configure HTTP request pipeline
app.MapCarter();

app.Run();
