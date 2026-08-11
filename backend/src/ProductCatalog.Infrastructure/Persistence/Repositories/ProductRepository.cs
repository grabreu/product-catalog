using ProductCatalog.Domain.Products;

namespace ProductCatalog.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository(ProductCatalogDbContext dbContext) : IProductRepository
{
    public async Task AddAsync(Product product, CancellationToken cancellationToken) => await dbContext.Products.AddAsync(product, cancellationToken);

    public Task<bool> SkuExistsAsync(string sku, CancellationToken cancellationToken) => dbContext.Products.AnyAsync(p => p.Sku == sku, cancellationToken);
}
