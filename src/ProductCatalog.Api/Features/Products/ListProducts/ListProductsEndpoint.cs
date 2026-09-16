using ProductCatalog.Api.Common.Models;

namespace ProductCatalog.Api.Features.Products.ListProducts;

public static class ListProductsEndpoint
{
    public static void MapListProductsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (int page, int pageSize, bool? isActive, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new ListProductsQuery(page, pageSize, isActive);
            var result = await sender.Send(query, cancellationToken);
            return result.ToOk();

        })
        .WithTags("Products")
        .WithName("ListProducts")
        .Produces<PagedResult<ProductDto>>(StatusCodes.Status200OK)
        .ProducesValidationProblem();
    }
}
