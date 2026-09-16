using ProductCatalog.Api.Common.Models;

namespace ProductCatalog.Api.Features.Products.GetProducts;

public record GetProductsQuery(int Page, int PageSize, bool? IsActive) : IQuery<Result<PagedResult<ProductDto>>>;
