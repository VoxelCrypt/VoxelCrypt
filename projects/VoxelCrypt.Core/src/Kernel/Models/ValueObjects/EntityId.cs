namespace Kernel.Models.ValueObjects;

public readonly record struct EntityId
{
    public Guid Value { get; }

    private EntityId(Guid value)
    {
        Value = value;
    }

    public static EntityId New() => new(Guid.NewGuid());

    internal static EntityId From(Guid value) => new(value);
}
