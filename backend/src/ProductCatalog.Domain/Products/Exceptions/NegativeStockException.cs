using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.Domain.Products.Exceptions;

public sealed class NegativeStockException() : DomainException("Stock quantity cannot be negative.");
