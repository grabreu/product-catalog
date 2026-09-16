using ProductCatalog.Api.Common.Models;
using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products.GetProducts;

public class GetProductsHandler(ApplicationDbContext dbContext) : IQueryHandler<GetProductsQuery, Result<PagedResult<ProductDto>>>
{
    public async ValueTask<Result<PagedResult<ProductDto>>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var queryable = dbContext.Products.AsNoTracking();

        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(p => p.IsActive == query.IsActive.Value);
        }

        var totalCount = await queryable.CountAsync(cancellationToken);

        var items = await queryable
            .OrderBy(p => p.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
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

        return new PagedResult<ProductDto>(items, query.Page, query.PageSize, totalCount);
    }
}
