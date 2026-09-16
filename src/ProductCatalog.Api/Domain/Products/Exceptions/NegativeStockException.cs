using ProductCatalog.Api.Domain.SeedWork;

namespace ProductCatalog.Api.Domain.Products.Exceptions;

public class NegativeStockException() : DomainException("Stock quantity cannot be negative.");
