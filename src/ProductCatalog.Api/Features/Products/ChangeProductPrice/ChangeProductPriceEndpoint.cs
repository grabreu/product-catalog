namespace ProductCatalog.Api.Features.Products.ChangeProductPrice;

public static class ChangeProductPriceEndpoint
{
    public static void MapChangeProductPriceEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPatch("/products/{id}/price", async (Guid id, ChangeProductPriceRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new ChangeProductPriceCommand(id, request.NewPrice);
            var result = await sender.Send(command, cancellationToken);
            return result.ToNoContent();
        })
        .WithTags("Products")
        .WithName("ChangeProductPrice")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }

    public record ChangeProductPriceRequest(decimal NewPrice);
}
