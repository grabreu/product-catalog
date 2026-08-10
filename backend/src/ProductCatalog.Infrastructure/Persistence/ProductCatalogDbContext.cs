namespace ProductCatalog.Infrastructure.Persistence;

public class ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : DbContext(options)
{
}
