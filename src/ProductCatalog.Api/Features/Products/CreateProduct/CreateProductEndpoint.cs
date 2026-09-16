using ProductCatalog.Api.Domain.Products;

namespace ProductCatalog.Api.Features.Products.CreateProduct;

public static class CreateProductEndpoint
{
    public static void MapCreateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateProductCommand(request.Name, request.Sku, request.Price, request.Category);
            var result = await sender.Send(command, cancellationToken);
            return result.ToCreated(value => $"/product/{value.Id}");

        })
        .WithTags("Products")
        .WithName("CreateProduct")
        .Produces<ProductDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict);
    }

    public record CreateProductRequest(string Name, string Sku, decimal Price, ProductCategory Category);
}
