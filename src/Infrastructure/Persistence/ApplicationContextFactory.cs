using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlueXT.Infrastructure.Persistence;

public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>().UseNpgsql();
        return new ApplicationContext(options.Options, null!);
    }
}
