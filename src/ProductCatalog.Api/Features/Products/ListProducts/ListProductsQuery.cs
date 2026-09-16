using ProductCatalog.Api.Common.Models;

namespace ProductCatalog.Api.Features.Products.ListProducts;

public record ListProductsQuery(int Page, int PageSize, bool? IsActive) : IQuery<Result<PagedResult<ProductDto>>>;
