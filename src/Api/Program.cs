using BlueXT.Api;
using BlueXT.Application;
using BlueXT.Infrastructure;
using BlueXT.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    using var scope = app.Services.CreateScope();
    var migrator = scope.ServiceProvider.GetRequiredService<ApplicationContextMigrator>();
    await migrator.MigrateAsync();
}

app.UseHealthChecks("/health");
app.UseStaticFiles();

app.UseSwaggerUi3(settings => {
    settings.Path = "/api";
    settings.DocumentPath = "/api/v1/specification.json";
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);

app.MapControllers();
app.Run();
