using ProductCatalog.Application.Commands.Products.Create;
using ProductCatalog.Domain.Products;
using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.UnitTests.Application.Commands.Products.Create;

public class CreateProductCommandHandlerTests
{
    private readonly IProductRepository _repository = Substitute.For<IProductRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private CreateProductCommandHandler Handler => new(_repository, _unitOfWork);

    private static CreateProductCommand ValidCommand => new("Widget", "SKU-001", 10m, ProductCategory.Other);

    [Fact]
    public async Task Handle_WithValidCommand_AddsProductAndSavesChanges()
    {
        // Arrange
        _repository.SkuExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await Handler.Handle(ValidCommand, CancellationToken.None);

        // Assert
        result.IsError.ShouldBeFalse();
        await _repository.Received(1).AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsResultMatchingCreatedProduct()
    {
        // Arrange
        _repository.SkuExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);

        Product? addedProduct = null;
        _repository.AddAsync(Arg.Do<Product>(p => addedProduct = p), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        // Act
        var result = await Handler.Handle(ValidCommand, CancellationToken.None);

        // Assert
        addedProduct.ShouldNotBeNull();
        result.Value.Id.ShouldBe(addedProduct.Id);
        result.Value.Name.ShouldBe(addedProduct.Name);
        result.Value.Sku.ShouldBe(addedProduct.Sku);
        result.Value.Price.ShouldBe(addedProduct.Price);
        result.Value.Category.ShouldBe(addedProduct.Category);
        result.Value.StockQuantity.ShouldBe(addedProduct.StockQuantity);
        result.Value.IsActive.ShouldBe(addedProduct.IsActive);
        result.Value.CreatedAt.ShouldBe(addedProduct.CreatedAt);
        result.Value.UpdatedAt.ShouldBe(addedProduct.UpdatedAt);
    }

    [Fact]
    public async Task Handle_WithDuplicateSku_ReturnsConflictWithoutAddingProduct()
    {
        // Arrange
        _repository.SkuExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await Handler.Handle(ValidCommand, CancellationToken.None);

        // Assert
        result.IsError.ShouldBeTrue();
        result.FirstError.Type.ShouldBe(ErrorType.Conflict);
        await _repository.DidNotReceive().AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
