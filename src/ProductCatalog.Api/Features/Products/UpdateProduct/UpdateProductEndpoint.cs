using ProductCatalog.Api.Domain.Products;

namespace ProductCatalog.Api.Features.Products.UpdateProduct;

public static class UpdateProductEndpoint
{
    public static void MapUpdateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (Guid id, UpdateProductRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateProductCommand(id, request.Name, request.Description, request.Category);
            var result = await sender.Send(command, cancellationToken);
            return result.ToNoContent();
        })
        .WithTags("Products")
        .WithName("UpdateProduct")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }

    public record UpdateProductRequest(string Name, string Description, ProductCategory Category);
}
