namespace BlueXT.Domain.Common;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    protected static bool EqualOperator(ValueObject? left, ValueObject? right)
    {
        if (left is null || right is null) return false;

        return ReferenceEquals(left, right) || left.Equals(right);
    }

    protected static bool NotEqualOperator(ValueObject? left, ValueObject? right) => !EqualOperator(left, right);

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;

        return obj is ValueObject other && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(_ => GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(ValueObject? left, ValueObject? right) => EqualOperator(left, right);
    public static bool operator !=(ValueObject? left, ValueObject? right) => NotEqualOperator(left, right);
}
