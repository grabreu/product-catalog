using ProductCatalog.Api.Endpoints.Products;
using ProductCatalog.Application.Models.Products;
using ProductCatalog.IntegrationTests.Common;

namespace ProductCatalog.IntegrationTests.Products;

[Collection(IntegrationTestCollection.Name)]
public sealed class AdjustStockEndpointTests(ProductCatalogApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task AdjustStock_WithValidDelta_ReturnsUpdatedProduct()
    {
        // Arrange
        var product = await SeedProductAsync(p => p.AdjustStock(10));
        var request = new AdjustStockRequest(5);

        // Act
        var response = await Client.PatchAsJsonAsync($"/products/{product.Id}/stock", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<ProductDto>(JsonOptions);
        dto.ShouldNotBeNull();
        dto.StockQuantity.ShouldBe(15);
    }

    [Fact]
    public async Task AdjustStock_WithNonExistingProduct_ReturnsNotFound()
    {
        // Arrange
        var request = new AdjustStockRequest(5);

        // Act
        var response = await Client.PatchAsJsonAsync($"/products/{Guid.CreateVersion7()}/stock", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AdjustStock_WithDeltaResultingInNegativeStock_ReturnsBadRequest()
    {
        // Arrange
        var product = await SeedProductAsync(p => p.AdjustStock(5));
        var request = new AdjustStockRequest(-6);

        // Act
        var response = await Client.PatchAsJsonAsync($"/products/{product.Id}/stock", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
