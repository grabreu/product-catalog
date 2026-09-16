namespace ProductCatalog.Api.Features.Products.DeactivateProduct;

public static class DeactivateProductEndpoint
{
    public static void MapDeactivateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPatch("/products/{id}/deactivate", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeactivateProductCommand(id);
            var result = await sender.Send(command, cancellationToken);
            return result.ToNoContent();
        })
        .WithTags("Products")
        .WithName("DeactivateProduct")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
