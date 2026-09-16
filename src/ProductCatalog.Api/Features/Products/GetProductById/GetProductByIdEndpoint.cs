namespace ProductCatalog.Api.Features.Products.GetProductById;

public static class GetProductByIdEndpoint
{
    public static void MapGetProductByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetProductByIdQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return result.ToOk();

        })
        .WithTags("Products")
        .WithName("GetProductById")
        .Produces<ProductDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
