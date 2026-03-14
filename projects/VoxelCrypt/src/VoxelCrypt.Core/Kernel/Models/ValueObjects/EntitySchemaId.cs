namespace Kernel.Models.ValueObjects;

public readonly record struct EntitySchemaId
{
    public Guid Value { get; }

    private EntitySchemaId(Guid value)
    {
        Value = value;
    }

    public static EntitySchemaId New() => new(Guid.NewGuid());

    public static EntitySchemaId From(Guid value) => new(value);
}
