using ProductCatalog.Application.Models.Products;
using ProductCatalog.IntegrationTests.Common;

namespace ProductCatalog.IntegrationTests.Products;

[Collection(IntegrationTestCollection.Name)]
public sealed class DeactivateProductEndpointTests(ProductCatalogApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Deactivate_WithExistingProduct_ReturnsNoContentAndDeactivatesProduct()
    {
        // Arrange
        var product = await SeedProductAsync();

        // Act
        var response = await Client.DeleteAsync($"/products/{product.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var getResponse = await Client.GetAsync($"/products/{product.Id}");
        var dto = await getResponse.Content.ReadFromJsonAsync<ProductDto>(JsonOptions);
        dto.ShouldNotBeNull();
        dto.IsActive.ShouldBeFalse();
    }

    [Fact]
    public async Task Deactivate_WithNonExistingProduct_ReturnsNotFound()
    {
        // Act
        var response = await Client.DeleteAsync($"/products/{Guid.CreateVersion7()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
