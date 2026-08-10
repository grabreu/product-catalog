using ProductCatalog.Infrastructure.Data;

namespace ProductCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<ProductCatalogDbContext>("ProductCatalogDb");

        return builder;
    }
}
