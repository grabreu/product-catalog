using ProductCatalog.Api.Errors;
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

        return result.Match(
            product => TypedResults.Created($"/products/{product.Id}", ToResponse(product)),
            errors => errors.ToProblem());
    }

    private static ProductResponse ToResponse(CreateProductResult result)
    {
        return new ProductResponse(
            result.Id,
            result.Name,
            result.Sku,
            result.Description,
            result.Price,
            result.Category,
            result.StockQuantity,
            result.IsActive,
            result.CreatedAt,
            result.UpdatedAt);
    }
}
