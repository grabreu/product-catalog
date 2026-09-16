using ProductCatalog.Api.Common.Behaviors;
using ProductCatalog.Api.Common.ExceptionHandling;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Data.Interceptors;
using ProductCatalog.Api.Features.Products.AdjustProductStock;
using ProductCatalog.Api.Features.Products.ChangeProductPrice;
using ProductCatalog.Api.Features.Products.CreateProduct;
using ProductCatalog.Api.Features.Products.DeactivateProduct;
using ProductCatalog.Api.Features.Products.GetProductById;
using ProductCatalog.Api.Features.Products.ListProducts;
using ProductCatalog.Api.Features.Products.ReactivateProduct;
using ProductCatalog.Api.Features.Products.UpdateProduct;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"])
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
    options.PipelineBehaviors =
    [
        typeof(ValidationBehavior<,>)
    ];
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<DispatchDomainEventsInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(sp.GetRequiredService<DispatchDomainEventsInterceptor>());
});

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseExceptionHandler();

app.MapOpenApi();
app.MapScalarApiReference();
app.Map("/", () => Results.Redirect("/scalar"));

app.MapHealthChecks("/health");
app.MapHealthChecks("/alive", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("live")
});

app.MapAdjustProductStockEndpoint();
app.MapChangeProductPriceEndpoint();
app.MapCreateProductEndpoint();
app.MapDeactivateProductEndpoint();
app.MapGetProductByIdEndpoint();
app.MapListProductsEndpoint();
app.MapReactivateProductEndpoint();
app.MapUpdateProductEndpoint();

if (app.Environment.IsDevelopment())
{
    await app.Services.InitialiseDatabaseAsync();
}

await app.RunAsync();
