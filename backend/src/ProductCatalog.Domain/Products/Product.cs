using ProductCatalog.Domain.Products.Events;
using ProductCatalog.Domain.Products.Exceptions;
using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.Domain.Products;

public sealed class Product : IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = [];

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

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Product()
    {
    }

    private Product(Guid id, string name, string sku, decimal price, ProductCategory category)
    {
        Id = id;
        Name = name;
        Sku = sku;
        Description = string.Empty;
        Price = price;
        Category = category;
        StockQuantity = 0;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public static Product Create(string name, string sku, decimal price, ProductCategory category)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);

        if (price <= 0)
        {
            throw new InvalidPriceException();
        }

        var product = new Product(Guid.CreateVersion7(), name, sku, price, category);
        product.AddDomainEvent(new ProductCreatedEvent(product.Id, product.CreatedAt));

        return product;
    }

    public void Update(string name, string description, ProductCategory category)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

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

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
