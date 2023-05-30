using BlueXT.Application.Common.Interfaces;
using BlueXT.Infrastructure.Persistence;
using BlueXT.Infrastructure.Persistence.Interceptors;
using BlueXT.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueXT.Infrastructure;

public static class ServiceCollectionExtension
{
    private const string UseInMemoryDatabaseKey = "UseInMemoryDatabase";
    private const string DefaultConnectionKey = "DefaultConnection";

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDatabase(configuration);
        services.AddScoped<AuditableEntitySaveChangeInterceptor>();
        services.AddScoped<ApplicationContextMigrator>();
        services.AddTransient<IDateTimeService, UtcDateTimeService>();

        return services;
    }

    private static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IApplicationContext, ApplicationContext>(builder => {
            if (configuration.GetValue<bool>(UseInMemoryDatabaseKey))
            {
                builder.UseInMemoryDatabase("BlueXTDb");
            }
            else
            {
                string? connectionString = configuration.GetConnectionString(DefaultConnectionKey);
                builder.UseNpgsql(connectionString, options =>
                    options.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName));
            }
        });
    }
}
