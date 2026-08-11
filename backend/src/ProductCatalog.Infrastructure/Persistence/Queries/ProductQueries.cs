using ProductCatalog.Application.Models;
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

    public async Task<PagedResult<ProductDto>> GetPagedAsync(int page, int pageSize, bool? isActive, CancellationToken cancellationToken)
    {
        var query = dbContext.Products.AsNoTracking();

        if (isActive.HasValue)
        {
            query = query.Where(p => p.IsActive == isActive.Value);
        }

        query = query.OrderBy(p => p.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductDto>(items, page, pageSize, totalCount);
    }
}
