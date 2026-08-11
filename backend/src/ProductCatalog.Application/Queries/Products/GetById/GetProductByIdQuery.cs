using ProductCatalog.Application.Models.Products;

namespace ProductCatalog.Application.Queries.Products.GetById;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<ErrorOr<ProductDto>>;
