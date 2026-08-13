using ProductCatalog.Api.Endpoints.Products;
using ProductCatalog.Api.ExceptionHandling;
using ProductCatalog.Application;
using ProductCatalog.Infrastructure;
using ProductCatalog.Infrastructure.Persistence;
using ProductCatalog.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapProductEndpoints();

if (app.Environment.IsDevelopment())
{
    await app.Services.InitializeDatabaseAsync();
}

await app.RunAsync();
