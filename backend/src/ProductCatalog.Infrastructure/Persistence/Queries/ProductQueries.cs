using ProductCatalog.Application.Models.Products;
using ProductCatalog.Application.Queries.Products;

namespace ProductCatalog.Infrastructure.Persistence.Queries;

public sealed class ProductQueries(ProductCatalogDbContext dbContext) : IProductQueries
{
    public Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Sku,
                p.Description,
                p.Price,
                p.Category,
                p.StockQuantity,
                p.IsActive,
                p.CreatedAt,
                p.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
