using ProductCatalog.Application.Models;
using ProductCatalog.Application.Models.Products;
using ProductCatalog.Application.Queries.Products;
using ProductCatalog.Application.Queries.Products.List;
using ProductCatalog.Domain.Products;

namespace ProductCatalog.UnitTests.Application.Queries.Products.List;

public class GetProductsQueryHandlerTests
{
    private readonly IProductQueries _queries = Substitute.For<IProductQueries>();

    private GetProductsQueryHandler Handler => new(_queries);

    private static ProductDto SampleProduct() => new(
        Guid.CreateVersion7(),
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
    public async Task Handle_WithValidQuery_ReturnsPagedResultFromQueries()
    {
        // Arrange
        var pagedResult = new PagedResult<ProductDto>([SampleProduct()], Page: 1, PageSize: 20, TotalCount: 1);
        _queries.GetPagedAsync(1, 20, Arg.Any<CancellationToken>()).Returns(pagedResult);

        // Act
        var result = await Handler.Handle(new GetProductsQuery(1, 20), CancellationToken.None);

        // Assert
        result.IsError.ShouldBeFalse();
        result.Value.ShouldBe(pagedResult);
    }
}
