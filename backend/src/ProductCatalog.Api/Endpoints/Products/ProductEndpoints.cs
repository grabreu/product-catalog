using ProductCatalog.Application.Commands.Products.Create;

namespace ProductCatalog.Api.Endpoints.Products;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products").WithTags("Products");

        group.MapPost("/", CreateProductAsync)
            .WithName("CreateProduct");

        return app;
    }

    private static async Task<IResult> CreateProductAsync(CreateProductRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(request.Name, request.Sku, request.Price, request.Category);

        var result = await sender.Send(command, cancellationToken);

        return result.ToCreated(product => $"/products/{product.Id}");
    }
}
