using ProductCatalog.Application.Models;
using ProductCatalog.Application.Models.Products;

namespace ProductCatalog.Application.Queries.Products.List;

public sealed record GetProductsQuery(int Page, int PageSize) : IQuery<ErrorOr<PagedResult<ProductDto>>>;
