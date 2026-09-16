using ProductCatalog.Api.Domain.Products;

namespace ProductCatalog.Api.Data;

public static class ApplicationDbContextInitialiser
{
    public static async Task InitialiseDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();
        await dbContext.SeedAsync();
    }

    private static async Task SeedAsync(this ApplicationDbContext dbContext)
    {
        if (await dbContext.Products.AnyAsync())
        {
            return;
        }

        var categories = Enum.GetValues<ProductCategory>();

        var products = Enumerable.Range(1, 12)
            .Select(i => new Product(
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
