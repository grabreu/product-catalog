using ProductCatalog.Application.Models.Products;
using ProductCatalog.IntegrationTests.Common;

namespace ProductCatalog.IntegrationTests.Products;

[Collection(IntegrationTestCollection.Name)]
public sealed class ReactivateProductEndpointTests(ProductCatalogApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Reactivate_WithExistingProduct_ReturnsReactivatedProduct()
    {
        // Arrange
        var product = await SeedProductAsync(p => p.Deactivate());

        // Act
        var response = await Client.PostAsync($"/products/{product.Id}/reactivate", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<ProductDto>(JsonOptions);
        dto.ShouldNotBeNull();
        dto.IsActive.ShouldBeTrue();
    }

    [Fact]
    public async Task Reactivate_WithNonExistingProduct_ReturnsNotFound()
    {
        // Act
        var response = await Client.PostAsync($"/products/{Guid.CreateVersion7()}/reactivate", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
