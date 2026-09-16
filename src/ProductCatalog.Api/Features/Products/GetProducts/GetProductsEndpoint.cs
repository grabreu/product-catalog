using ProductCatalog.Api.Common.Models;

namespace ProductCatalog.Api.Features.Products.GetProducts;

public static class GetProductsEndpoint
{
    public static void MapGetProductsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (int page, int pageSize, bool? isActive, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetProductsQuery(page, pageSize, isActive);
            var result = await sender.Send(query, cancellationToken);
            return result.ToOk();

        })
        .WithTags("Products")
        .WithName("GetProducts")
        .Produces<PagedResult<ProductDto>>(StatusCodes.Status200OK)
        .ProducesValidationProblem();
    }
}
