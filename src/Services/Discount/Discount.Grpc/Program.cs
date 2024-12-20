using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

string connectionString = builder.Configuration.GetConnectionString("Database")!;
builder.Services.AddDbContext<DiscountContext>(options => options.UseSqlite(connectionString));

builder.Services.AddLogging();

var app = builder.Build();

// Apply missing migrations
app.UseMigration();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet(
    "/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909"
);

app.Run();
