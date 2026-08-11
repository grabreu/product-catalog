using ProductCatalog.Application.Models.Products;

namespace ProductCatalog.Application.Queries.Products;

public interface IProductQueries
{
    Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
