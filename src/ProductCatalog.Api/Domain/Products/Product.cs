using ProductCatalog.Api.Domain.Products.Events;
using ProductCatalog.Api.Domain.Products.Exceptions;
using ProductCatalog.Api.Domain.SeedWork;

namespace ProductCatalog.Api.Domain.Products;

public class Product : HasDomainEventsBase
{
    private Product()
    {
    }

    public Product(string name, string sku, decimal price, ProductCategory category)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        Sku = sku;
        Description = string.Empty;
        Price = price;
        Category = category;
        StockQuantity = 0;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;

        AddDomainEvent(new ProductCreatedDomainEvent(Id));
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Sku { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }
    public ProductCategory Category { get; private set; }
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public void Update(string name, string description, ProductCategory category)
    {
        Name = name;
        Description = description;
        Category = category;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
        {
            throw new InvalidPriceException();
        }

        Price = newPrice;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void AdjustStock(int quantityDelta)
    {
        var newQuantity = StockQuantity + quantityDelta;
        if (newQuantity < 0)
        {
            throw new NegativeStockException();
        }

        StockQuantity = newQuantity;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Reactivate()
    {
        IsActive = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
