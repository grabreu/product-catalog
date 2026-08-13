using ProductCatalog.Api.Endpoints.Products;
using ProductCatalog.Application.Models.Products;
using ProductCatalog.IntegrationTests.Common;

namespace ProductCatalog.IntegrationTests.Products;

[Collection(IntegrationTestCollection.Name)]
public sealed class ChangePriceEndpointTests(ProductCatalogApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task ChangePrice_WithValidPrice_ReturnsUpdatedProduct()
    {
        // Arrange
        var product = await SeedProductAsync();
        var request = new ChangePriceRequest(25m);

        // Act
        var response = await Client.PatchAsJsonAsync($"/products/{product.Id}/price", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<ProductDto>(JsonOptions);
        dto.ShouldNotBeNull();
        dto.Price.ShouldBe(25m);
    }

    [Fact]
    public async Task ChangePrice_WithNonExistingProduct_ReturnsNotFound()
    {
        // Arrange
        var request = new ChangePriceRequest(25m);

        // Act
        var response = await Client.PatchAsJsonAsync($"/products/{Guid.CreateVersion7()}/price", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ChangePrice_WithNonPositivePrice_ReturnsBadRequest()
    {
        // Arrange
        var product = await SeedProductAsync();
        var request = new ChangePriceRequest(0m);

        // Act
        var response = await Client.PatchAsJsonAsync($"/products/{product.Id}/price", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
