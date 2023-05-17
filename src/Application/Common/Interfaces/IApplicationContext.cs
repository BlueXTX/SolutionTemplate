namespace BlueXT.Application.Common.Interfaces;

public interface IApplicationContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
