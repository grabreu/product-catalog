using ProductCatalog.Application.Commands.Products.AdjustStock;
using ProductCatalog.Application.Commands.Products.Create;
using ProductCatalog.Application.Commands.Products.Deactivate;
using ProductCatalog.Application.Commands.Products.Update;
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

        group.MapPut("/{id:guid}", UpdateProductAsync)
            .WithName("UpdateProduct");

        group.MapDelete("/{id:guid}", DeactivateProductAsync)
            .WithName("DeactivateProduct");

        group.MapPatch("/{id:guid}/stock", AdjustStockAsync)
            .WithName("AdjustStock");

        return app;
    }

    private static async Task<IResult> GetProductsAsync(ISender sender, CancellationToken cancellationToken, int page = 1, int pageSize = 20, bool? isActive = null)
    {
        var query = new GetProductsQuery(page, pageSize, isActive);

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

    private static async Task<IResult> UpdateProductAsync(Guid id, UpdateProductRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(id, request.Name, request.Description, request.Category);

        var result = await sender.Send(command, cancellationToken);

        return result.ToOk();
    }

    private static async Task<IResult> DeactivateProductAsync(Guid id, ISender sender, CancellationToken cancellationToken)
    {
        var command = new DeactivateProductCommand(id);

        var result = await sender.Send(command, cancellationToken);

        return result.ToNoContent();
    }

    private static async Task<IResult> AdjustStockAsync(Guid id, AdjustStockRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new AdjustStockCommand(id, request.QuantityDelta);

        var result = await sender.Send(command, cancellationToken);

        return result.ToOk();
    }
}
