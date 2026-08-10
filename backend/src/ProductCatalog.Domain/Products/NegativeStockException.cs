using ProductCatalog.Domain.Common;

namespace ProductCatalog.Domain.Products;

public sealed class NegativeStockException() : DomainException("Stock quantity cannot be negative.");
