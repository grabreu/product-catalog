namespace ProductCatalog.Infrastructure.Data;

public class ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : DbContext(options)
{
}
