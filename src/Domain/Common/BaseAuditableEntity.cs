namespace BlueXT.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
}
