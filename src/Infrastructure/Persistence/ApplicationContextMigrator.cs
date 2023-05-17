using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlueXT.Infrastructure.Persistence;

public class ApplicationContextMigrator
{
    private readonly ApplicationContext _context;
    private readonly ILogger<ApplicationContextMigrator> _logger;

    public ApplicationContextMigrator(ApplicationContext context, ILogger<ApplicationContextMigrator> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task MigrateAsync()
    {
        try
        {
            if (_context.Database.IsNpgsql()) await _context.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Database migration failed");
            throw;
        }
    }
}
