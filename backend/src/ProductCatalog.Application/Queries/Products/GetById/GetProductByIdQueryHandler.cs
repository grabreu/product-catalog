using ProductCatalog.Application.Models.Products;

namespace ProductCatalog.Application.Queries.Products.GetById;

public sealed class GetProductByIdQueryHandler(IProductQueries queries) : IQueryHandler<GetProductByIdQuery, ErrorOr<ProductDto>>
{
    public async ValueTask<ErrorOr<ProductDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await queries.GetByIdAsync(query.Id, cancellationToken);

        if (product is null)
        {
            return Error.NotFound(description: $"No product was found with ID '{query.Id}'.");
        }

        return product;
    }
}
