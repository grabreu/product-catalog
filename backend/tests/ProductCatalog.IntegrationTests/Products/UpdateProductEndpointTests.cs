using ProductCatalog.Api.Endpoints.Products;
using ProductCatalog.Application.Models.Products;
using ProductCatalog.Domain.Products;
using ProductCatalog.IntegrationTests.Common;

namespace ProductCatalog.IntegrationTests.Products;

[Collection(IntegrationTestCollection.Name)]
public sealed class UpdateProductEndpointTests(ProductCatalogApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Update_WithExistingProduct_ReturnsUpdatedProduct()
    {
        // Arrange
        var product = await SeedProductAsync();
        var request = new UpdateProductRequest("Widget Pro", "Updated description", ProductCategory.Electronics);

        // Act
        var response = await Client.PutAsJsonAsync($"/products/{product.Id}", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<ProductDto>(JsonOptions);
        dto.ShouldNotBeNull();
        dto.Name.ShouldBe(request.Name);
        dto.Description.ShouldBe(request.Description);
        dto.Category.ShouldBe(request.Category);
    }

    [Fact]
    public async Task Update_WithNonExistingProduct_ReturnsNotFound()
    {
        // Arrange
        var request = new UpdateProductRequest("Widget Pro", "Updated description", ProductCategory.Electronics);

        // Act
        var response = await Client.PutAsJsonAsync($"/products/{Guid.CreateVersion7()}", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var product = await SeedProductAsync();
        var request = new UpdateProductRequest("", "Updated description", ProductCategory.Electronics);

        // Act
        var response = await Client.PutAsJsonAsync($"/products/{product.Id}", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
