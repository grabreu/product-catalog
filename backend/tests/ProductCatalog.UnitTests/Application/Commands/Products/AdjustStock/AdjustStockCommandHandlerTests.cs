using ProductCatalog.Application.Commands.Products.AdjustStock;
using ProductCatalog.Domain.Products;
using ProductCatalog.Domain.Products.Exceptions;
using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.UnitTests.Application.Commands.Products.AdjustStock;

public class AdjustStockCommandHandlerTests
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AdjustStockCommandHandler _handler;

    public AdjustStockCommandHandlerTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new AdjustStockCommandHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WithExistingProduct_AdjustsStockAndSavesChanges()
    {
        // Arrange
        var product = Product.Create("Widget", "SKU-001", 10m, ProductCategory.Other);
        _repository.GetByIdAsync(product.Id, Arg.Any<CancellationToken>()).Returns(product);

        // Act
        var result = await _handler.Handle(new AdjustStockCommand(product.Id, 5), CancellationToken.None);

        // Assert
        result.IsError.ShouldBeFalse();
        result.Value.StockQuantity.ShouldBe(5);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNonExistingProduct_ReturnsNotFoundWithoutSavingChanges()
    {
        // Arrange
        var id = Guid.CreateVersion7();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Product?)null);

        // Act
        var result = await _handler.Handle(new AdjustStockCommand(id, 5), CancellationToken.None);

        // Assert
        result.IsError.ShouldBeTrue();
        result.FirstError.Type.ShouldBe(ErrorType.NotFound);
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithDeltaResultingInNegativeStock_ThrowsNegativeStockException()
    {
        // Arrange
        var product = Product.Create("Widget", "SKU-001", 10m, ProductCategory.Other);
        _repository.GetByIdAsync(product.Id, Arg.Any<CancellationToken>()).Returns(product);

        // Act & Assert
        await Should.ThrowAsync<NegativeStockException>(() => _handler.Handle(new AdjustStockCommand(product.Id, -1), CancellationToken.None).AsTask());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
