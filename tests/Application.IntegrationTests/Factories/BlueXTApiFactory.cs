using System.Data.Common;
using BlueXT.Application.Common.Interfaces;
using BlueXT.Infrastructure.Persistence;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;
using Respawn.Graph;

namespace BlueXT.Application.IntegrationTests.Factories;

internal class BlueXTApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IContainer _dbContainer = new ContainerBuilder()
        .WithImage("postgres:latest")
        .WithEnvironment("POSTGRES_USER", "postgres")
        .WithEnvironment("POSTGRES_PASSWORD", "admin")
        .WithPortBinding(5432, true)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .Build();

    private DbConnection? _dbConnection;
    private Respawner? _respawner;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => {
            services.RemoveAll<ApplicationContext>();
            services.RemoveAll<DbContextOptions<ApplicationContext>>();

            services.AddDbContext<IApplicationContext, ApplicationContext>(optionsBuilder =>
                optionsBuilder.UseNpgsql(_dbConnection!));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        string connectionString =
            $"Host=localhost;Port={_dbContainer.GetMappedPublicPort(5432)};Username=postgres;Password=admin;";
        _dbConnection = new NpgsqlConnection(connectionString);
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" },
            TablesToIgnore = new[] { new Table("__EFMigrationsHistory") }
        });
    }

    public new async Task DisposeAsync() => await _dbContainer.StopAsync();
    public async Task ResetDatabaseAsync() => await _respawner!.ResetAsync(_dbConnection!);
}
