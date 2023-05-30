using BlueXT.Infrastructure.Persistence;
using ZymLabs.NSwag.FluentValidation;

namespace BlueXT.Api;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddHttpContextAccessor();
        services.AddHealthChecks().AddDbContextCheck<ApplicationContext>();
        services.AddEndpointsApiExplorer();
        services.AddControllers();

        services.AddScoped<FluentValidationSchemaProcessor>(provider => {
            var rules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, rules, loggerFactory);
        });

        services.AddOpenApiDocument((settings, provider) => {
            var fluentValidationSchemaProcessor = provider.CreateScope()
                .ServiceProvider.GetRequiredService<FluentValidationSchemaProcessor>();

            settings.SchemaProcessors.Add(fluentValidationSchemaProcessor);
            settings.Title = "BlueXT Api";
        });

        return services;
    }
}
