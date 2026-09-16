using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products.GetProductById;

public class GetProductByIdHandler(ApplicationDbContext dbContext) : IQueryHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async ValueTask<Result<ProductDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Id == query.ProductId)
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

        if (product is null)
        {
            return Error.NotFound("Products.NotFound", $"No product was found with ID '{query.ProductId}'.");
        }

        return product;
    }
}
