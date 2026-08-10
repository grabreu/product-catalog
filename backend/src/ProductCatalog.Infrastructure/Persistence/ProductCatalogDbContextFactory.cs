namespace ProductCatalog.Infrastructure.Persistence;

public class ProductCatalogDbContextFactory : IDesignTimeDbContextFactory<ProductCatalogDbContext>
{
    public ProductCatalogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductCatalogDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProductCatalogDb;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new ProductCatalogDbContext(optionsBuilder.Options);
    }
}
