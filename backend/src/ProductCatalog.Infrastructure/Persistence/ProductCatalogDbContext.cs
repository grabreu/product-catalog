using ProductCatalog.Domain.Products;
using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.Infrastructure.Persistence;

public sealed class ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductCatalogDbContext).Assembly);
    }
}
