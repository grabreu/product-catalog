using ProductCatalog.Application.Models;
using ProductCatalog.Application.Models.Products;

namespace ProductCatalog.Application.Queries.Products.List;

public sealed class GetProductsQueryHandler(IProductQueries queries) : IQueryHandler<GetProductsQuery, ErrorOr<PagedResult<ProductDto>>>
{
    public async ValueTask<ErrorOr<PagedResult<ProductDto>>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        return await queries.GetPagedAsync(query.Page, query.PageSize, query.IsActive, cancellationToken);
    }
}
