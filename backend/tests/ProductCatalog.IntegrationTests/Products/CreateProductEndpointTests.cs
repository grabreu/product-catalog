using ProductCatalog.Api.Endpoints.Products;
using ProductCatalog.Application.Models.Products;
using ProductCatalog.Domain.Products;
using ProductCatalog.IntegrationTests.Common;

namespace ProductCatalog.IntegrationTests.Products;

[Collection(IntegrationTestCollection.Name)]
public sealed class CreateProductEndpointTests(ProductCatalogApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Create_WithValidRequest_ReturnsCreatedProduct()
    {
        // Arrange
        var request = new CreateProductRequest("Widget", "SKU-001", 10m, ProductCategory.Other);

        // Act
        var response = await Client.PostAsJsonAsync("/products", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var product = await response.Content.ReadFromJsonAsync<ProductDto>(JsonOptions);
        product.ShouldNotBeNull();
        product.Name.ShouldBe(request.Name);
        product.Sku.ShouldBe(request.Sku);
        product.Price.ShouldBe(request.Price);
        product.Category.ShouldBe(request.Category);
        product.IsActive.ShouldBeTrue();

        response.Headers.Location.ShouldBe(new Uri($"/products/{product.Id}", UriKind.Relative));
    }

    [Fact]
    public async Task Create_WithDuplicateSku_ReturnsConflict()
    {
        // Arrange
        var existingProduct = await SeedProductAsync();
        var request = new CreateProductRequest("Another Widget", existingProduct.Sku, 20m, ProductCategory.Other);

        // Act
        var response = await Client.PostAsJsonAsync("/products", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Create_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateProductRequest("", "SKU-001", 10m, ProductCategory.Other);

        // Act
        var response = await Client.PostAsJsonAsync("/products", request, JsonOptions);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
