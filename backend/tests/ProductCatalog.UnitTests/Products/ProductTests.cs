using ProductCatalog.Domain.Products;

namespace ProductCatalog.UnitTests.Products;

public class ProductTests
{
    private const string ValidName = "Widget";
    private const string ValidSku = "SKU-001";
    private const decimal ValidPrice = 10m;
    private const ProductCategory ValidCategory = ProductCategory.Other;

    [Fact]
    public void Create_WithValidData_SetsExpectedProperties()
    {
        // Act
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);

        // Assert
        product.Id.ShouldNotBe(Guid.Empty);
        product.Name.ShouldBe(ValidName);
        product.Sku.ShouldBe(ValidSku);
        product.Description.ShouldBe(string.Empty);
        product.Price.ShouldBe(ValidPrice);
        product.Category.ShouldBe(ValidCategory);
        product.StockQuantity.ShouldBe(0);
        product.IsActive.ShouldBeTrue();
        product.UpdatedAt.ShouldBe(product.CreatedAt);
    }

    [Fact]
    public void Create_WithValidData_RaisesProductCreatedEvent()
    {
        // Act
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);

        // Assert
        var domainEvent = product.DomainEvents.ShouldHaveSingleItem();
        var productCreatedEvent = domainEvent.ShouldBeOfType<ProductCreatedEvent>();
        productCreatedEvent.ProductId.ShouldBe(product.Id);
        productCreatedEvent.OccurredAt.ShouldBe(product.CreatedAt);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithNonPositivePrice_ThrowsInvalidPriceException(decimal price)
    {
        // Act & Assert
        Should.Throw<InvalidPriceException>(() => Product.Create(ValidName, ValidSku, price, ValidCategory));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidName_ThrowsArgumentException(string? name)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => Product.Create(name!, ValidSku, ValidPrice, ValidCategory));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidSku_ThrowsArgumentException(string? sku)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => Product.Create(ValidName, sku!, ValidPrice, ValidCategory));
    }

    [Fact]
    public void Update_WithValidData_UpdatesPropertiesAndTimestamp()
    {
        // Arrange
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);
        var before = DateTimeOffset.UtcNow;

        // Act
        product.Update("New name", "New description", ProductCategory.Electronics);

        // Assert
        var after = DateTimeOffset.UtcNow;
        product.Name.ShouldBe("New name");
        product.Description.ShouldBe("New description");
        product.Category.ShouldBe(ProductCategory.Electronics);
        product.UpdatedAt.ShouldBeInRange(before, after);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_WithInvalidName_ThrowsArgumentException(string? name)
    {
        // Arrange
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);

        // Act & Assert
        Should.Throw<ArgumentException>(() => product.Update(name!, "description", ValidCategory));
    }

    [Fact]
    public void ChangePrice_WithValidPrice_UpdatesPriceAndTimestamp()
    {
        // Arrange
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);
        var before = DateTimeOffset.UtcNow;

        // Act
        product.ChangePrice(20m);

        // Assert
        var after = DateTimeOffset.UtcNow;
        product.Price.ShouldBe(20m);
        product.UpdatedAt.ShouldBeInRange(before, after);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ChangePrice_WithNonPositivePrice_ThrowsInvalidPriceException(decimal newPrice)
    {
        // Arrange
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);

        // Act & Assert
        Should.Throw<InvalidPriceException>(() => product.ChangePrice(newPrice));
    }

    [Fact]
    public void AdjustStock_WithPositiveDelta_IncreasesStockAndTimestamp()
    {
        // Arrange
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);
        var before = DateTimeOffset.UtcNow;

        // Act
        product.AdjustStock(5);

        // Assert
        var after = DateTimeOffset.UtcNow;
        product.StockQuantity.ShouldBe(5);
        product.UpdatedAt.ShouldBeInRange(before, after);
    }

    [Fact]
    public void AdjustStock_WithNegativeDeltaWithinStock_DecreasesStock()
    {
        // Arrange
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);
        product.AdjustStock(10);

        // Act
        product.AdjustStock(-4);

        // Assert
        product.StockQuantity.ShouldBe(6);
    }

    [Fact]
    public void AdjustStock_ResultingInExactlyZero_Succeeds()
    {
        // Arrange
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);
        product.AdjustStock(5);

        // Act
        product.AdjustStock(-5);

        // Assert
        product.StockQuantity.ShouldBe(0);
    }

    [Fact]
    public void AdjustStock_ResultingInNegativeStock_ThrowsNegativeStockException()
    {
        // Arrange
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);
        product.AdjustStock(5);

        // Act & Assert
        Should.Throw<NegativeStockException>(() => product.AdjustStock(-6));
    }

    [Fact]
    public void Deactivate_SetsIsActiveFalseAndUpdatesTimestamp()
    {
        // Arrange
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);
        var before = DateTimeOffset.UtcNow;

        // Act
        product.Deactivate();

        // Assert
        var after = DateTimeOffset.UtcNow;
        product.IsActive.ShouldBeFalse();
        product.UpdatedAt.ShouldBeInRange(before, after);
    }

    [Fact]
    public void ClearDomainEvents_RemovesAllRaisedEvents()
    {
        // Arrange
        var product = Product.Create(ValidName, ValidSku, ValidPrice, ValidCategory);

        // Act
        product.ClearDomainEvents();

        // Assert
        product.DomainEvents.ShouldBeEmpty();
    }
}
