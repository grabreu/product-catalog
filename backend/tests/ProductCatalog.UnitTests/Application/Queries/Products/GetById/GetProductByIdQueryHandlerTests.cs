using ProductCatalog.Application.Models.Products;
using ProductCatalog.Application.Queries.Products;
using ProductCatalog.Application.Queries.Products.GetById;
using ProductCatalog.Domain.Products;

namespace ProductCatalog.UnitTests.Application.Queries.Products.GetById;

public class GetProductByIdQueryHandlerTests
{
    private readonly IProductQueries _queries;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        _queries = Substitute.For<IProductQueries>();
        _handler = new GetProductByIdQueryHandler(_queries);
    }

    private static ProductDto SampleProduct(Guid id) => new(
        id,
        "Widget",
        "SKU-001",
        "A widget",
        10m,
        ProductCategory.Other,
        5,
        true,
        DateTimeOffset.UtcNow,
        DateTimeOffset.UtcNow);

    [Fact]
    public async Task Handle_WithExistingProduct_ReturnsProduct()
    {
        // Arrange
        var id = Guid.CreateVersion7();
        var product = SampleProduct(id);
        _queries.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(product);

        // Act
        var result = await _handler.Handle(new GetProductByIdQuery(id), CancellationToken.None);

        // Assert
        result.IsError.ShouldBeFalse();
        result.Value.ShouldBe(product);
    }

    [Fact]
    public async Task Handle_WithNonExistingProduct_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.CreateVersion7();
        _queries.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((ProductDto?)null);

        // Act
        var result = await _handler.Handle(new GetProductByIdQuery(id), CancellationToken.None);

        // Assert
        result.IsError.ShouldBeTrue();
        result.FirstError.Type.ShouldBe(ErrorType.NotFound);
    }
}
