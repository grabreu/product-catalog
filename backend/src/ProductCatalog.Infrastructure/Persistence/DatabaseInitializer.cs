using ProductCatalog.Domain.Products;

namespace ProductCatalog.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductCatalogDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProductCatalogDbContext>>();

        try
        {
            await dbContext.Database.MigrateAsync();
            await SeedAsync(dbContext, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public static async Task SeedAsync(ProductCatalogDbContext dbContext, ILogger logger)
    {
        if (await dbContext.Products.AnyAsync())
        {
            logger.LogInformation("Database already has data - seeding skipped.");
            return;
        }

        logger.LogInformation("Seeding database with sample data.");

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
