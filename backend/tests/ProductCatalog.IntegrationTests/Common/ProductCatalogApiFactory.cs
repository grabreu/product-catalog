using ProductCatalog.Infrastructure.Persistence;

namespace ProductCatalog.IntegrationTests.Common;

public sealed class ProductCatalogApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build();
    private Respawner _respawner = null!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductCatalogDbContext>();
        await dbContext.Database.MigrateAsync();

        await using var connection = new SqlConnection(_dbContainer.GetConnectionString());
        await connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await using var connection = new SqlConnection(_dbContainer.GetConnectionString());
        await connection.OpenAsync();

        await _respawner.ResetAsync(connection);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(
            [
                new KeyValuePair<string, string?>("ConnectionStrings:ProductCatalogDb", _dbContainer.GetConnectionString())
            ]);
        });
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}
