using System.Reflection;
using BlueXT.Application.Common.Interfaces;
using BlueXT.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace BlueXT.Infrastructure.Persistence;

public class ApplicationContext : DbContext, IApplicationContext
{
    private readonly AuditableEntitySaveChangeInterceptor _auditableEntitySaveChangeInterceptor;

    public ApplicationContext(DbContextOptions<ApplicationContext> options,
        AuditableEntitySaveChangeInterceptor auditableEntitySaveChangeInterceptor) : base(options)
    {
        _auditableEntitySaveChangeInterceptor = auditableEntitySaveChangeInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangeInterceptor);
    }
}
