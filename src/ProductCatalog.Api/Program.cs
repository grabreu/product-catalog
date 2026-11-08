using ProductCatalog.Api.Common.Behaviors;
using ProductCatalog.Api.Common.ExceptionHandling;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Data.Interceptors;
using ProductCatalog.Api.Features.Products;
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
builder.Services.AddExceptionHandler<ConcurrencyExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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

builder.EnrichSqlServerDbContext<ApplicationDbContext>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.MapOpenApi();
app.MapScalarApiReference();
app.Map("/", () => Results.Redirect("/scalar"));

app.MapAdjustProductStockEndpoint();
app.MapChangeProductPriceEndpoint();
app.MapCreateProductEndpoint();
app.MapDeactivateProductEndpoint();
app.MapGetProductByIdEndpoint();
app.MapListProductsEndpoint();
app.MapReactivateProductEndpoint();
app.MapUpdateProductEndpoint();

await app.Services.InitialiseDatabaseAsync();

await app.RunAsync();
