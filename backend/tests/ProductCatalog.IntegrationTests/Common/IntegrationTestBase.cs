using ProductCatalog.Domain.Products;
using ProductCatalog.Infrastructure.Persistence;

namespace ProductCatalog.IntegrationTests.Common;

[Collection(IntegrationTestCollection.Name)]
public abstract class IntegrationTestBase(ProductCatalogApiFactory factory) : IAsyncLifetime
{
    protected static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    protected HttpClient Client { get; } = factory.CreateClient();

    public Task InitializeAsync() => factory.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    protected async Task<Product> SeedProductAsync(Action<Product>? configure = null)
    {
        var product = Product.Create("Widget", $"SKU-{Guid.NewGuid():N}", 10m, ProductCategory.Other);
        configure?.Invoke(product);

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductCatalogDbContext>();

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        return product;
    }
}
