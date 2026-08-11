using ProductCatalog.Domain.Products;
using ProductCatalog.Domain.SeedWork;
using ProductCatalog.Infrastructure.Persistence;
using ProductCatalog.Infrastructure.Persistence.Interceptors;
using ProductCatalog.Infrastructure.Persistence.Repositories;

namespace ProductCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DispatchDomainEventsInterceptor>();

        services.AddDbContext<ProductCatalogDbContext>((sp, options) =>
            options.UseSqlServer(configuration.GetConnectionString("ProductCatalogDb"))
                .AddInterceptors(sp.GetRequiredService<DispatchDomainEventsInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ProductCatalogDbContext>());

        services.AddScoped<SeedData>();

        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
