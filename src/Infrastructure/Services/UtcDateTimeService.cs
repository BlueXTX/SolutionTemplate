using BlueXT.Application.Common.Interfaces;

namespace BlueXT.Infrastructure.Services;

public class UtcDateTimeService : IDateTimeService
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}
