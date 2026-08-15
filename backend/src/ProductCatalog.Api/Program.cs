using ProductCatalog.Api.Endpoints.Products;
using ProductCatalog.Api.ExceptionHandling;
using ProductCatalog.Application;
using ProductCatalog.Infrastructure;
using ProductCatalog.Infrastructure.Persistence;
using ProductCatalog.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Servers = [];
        return Task.CompletedTask;
    });
});

builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapProductEndpoints();

await app.Services.MigrateDatabaseAsync();
await app.Services.SeedDatabaseAsync();

await app.RunAsync();
