using ProductCatalog.Application.Models.Products;
using ProductCatalog.IntegrationTests.Common;

namespace ProductCatalog.IntegrationTests.Products;

[Collection(IntegrationTestCollection.Name)]
public sealed class GetProductByIdEndpointTests(ProductCatalogApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetById_WithExistingProduct_ReturnsProduct()
    {
        // Arrange
        var product = await SeedProductAsync();

        // Act
        var response = await Client.GetAsync($"/products/{product.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<ProductDto>(JsonOptions);
        dto.ShouldNotBeNull();
        dto.Id.ShouldBe(product.Id);
        dto.Name.ShouldBe(product.Name);
        dto.Sku.ShouldBe(product.Sku);
    }

    [Fact]
    public async Task GetById_WithNonExistingProduct_ReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync($"/products/{Guid.CreateVersion7()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
