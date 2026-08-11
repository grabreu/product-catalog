using ProductCatalog.Domain.Products;

namespace ProductCatalog.Infrastructure.Persistence;

public sealed class SeedData(ProductCatalogDbContext dbContext)
{
    public async Task InitializeAsync() => await dbContext.Database.MigrateAsync();

    public async Task SeedAsync()
    {
        if (await dbContext.Products.AnyAsync())
        {
            return;
        }

        var categories = Enum.GetValues<ProductCategory>();

        var products = Enumerable.Range(1, 12)
            .Select(i => Product.Create(
                name: $"Sample Product {i}",
                sku: $"SKU-{i:0000}",
                price: 10m + i,
                category: categories[i % categories.Length]))
            .ToArray();

        foreach (var product in products)
        {
            product.AdjustStock(50);
        }

        dbContext.Products.AddRange(products);

        await dbContext.SaveChangesAsync();
    }
}

public static class InitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<SeedData>();

        await initializer.InitializeAsync();
        await initializer.SeedAsync();
    }
}
