namespace ProductCatalog.Api.Features.Products.AdjustProductStock;

public static class AdjustProductStockEndpoint
{
    public static void MapAdjustProductStockEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPatch("/products/{id}/stock", async (Guid id, AdjustProductStockRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new AdjustProductStockCommand(id, request.QuantityDelta);
            var result = await sender.Send(command, cancellationToken);
            return result.ToNoContent();
        })
        .WithTags("Products")
        .WithName("AdjustProductStock")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }

    public record AdjustProductStockRequest(int QuantityDelta);
}
