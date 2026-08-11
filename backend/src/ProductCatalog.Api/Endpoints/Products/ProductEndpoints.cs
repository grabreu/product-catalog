using ProductCatalog.Application.Commands.Products.Create;
using ProductCatalog.Application.Queries.Products.GetById;
using ProductCatalog.Application.Queries.Products.List;

namespace ProductCatalog.Api.Endpoints.Products;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products")
            .WithTags("Products");

        group.MapGet("/", GetProductsAsync)
            .WithName("GetProducts");

        group.MapGet("/{id:guid}", GetProductByIdAsync)
            .WithName("GetProductById");

        group.MapPost("/", CreateProductAsync)
            .WithName("CreateProduct");

        return app;
    }

    private static async Task<IResult> GetProductsAsync(ISender sender, CancellationToken cancellationToken, int page = 1, int pageSize = 20)
    {
        var query = new GetProductsQuery(page, pageSize);

        var result = await sender.Send(query, cancellationToken);

        return result.ToOk();
    }

    private static async Task<IResult> GetProductByIdAsync(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);

        var result = await sender.Send(query, cancellationToken);

        return result.ToOk();
    }

    private static async Task<IResult> CreateProductAsync(CreateProductRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(request.Name, request.Sku, request.Price, request.Category);

        var result = await sender.Send(command, cancellationToken);

        return result.ToCreated(product => $"/products/{product.Id}");
    }
}
