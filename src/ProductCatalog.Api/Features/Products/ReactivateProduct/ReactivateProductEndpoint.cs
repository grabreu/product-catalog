namespace ProductCatalog.Api.Features.Products.ReactivateProduct;

public static class ReactivateProductEndpoint
{
    public static void MapReactivateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPatch("/products/{id}/reactivate", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new ReactivateProductCommand(id);
            var result = await sender.Send(command, cancellationToken);
            return result.ToNoContent();
        })
        .WithTags("Products")
        .WithName("ReactivateProduct")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
