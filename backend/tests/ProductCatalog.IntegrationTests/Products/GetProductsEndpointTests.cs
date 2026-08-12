using ProductCatalog.Application.Models;
using ProductCatalog.Application.Models.Products;
using ProductCatalog.IntegrationTests.Common;

namespace ProductCatalog.IntegrationTests.Products;

[Collection(IntegrationTestCollection.Name)]
public sealed class GetProductsEndpointTests(ProductCatalogApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetProducts_WithSeededProducts_ReturnsPagedResult()
    {
        // Arrange
        await SeedProductAsync();
        await SeedProductAsync();
        await SeedProductAsync();

        // Act
        var response = await Client.GetAsync("/products?page=1&pageSize=20");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResult<ProductDto>>(JsonOptions);
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(3);
        result.Items.Count.ShouldBe(3);
    }

    [Fact]
    public async Task GetProducts_WithIsActiveFilter_ReturnsOnlyMatchingProducts()
    {
        // Arrange
        await SeedProductAsync();
        await SeedProductAsync(p => p.Deactivate());

        // Act
        var response = await Client.GetAsync("/products?page=1&pageSize=20&isActive=false");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResult<ProductDto>>(JsonOptions);
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(1);
        result.Items.ShouldAllBe(p => !p.IsActive);
    }
}
