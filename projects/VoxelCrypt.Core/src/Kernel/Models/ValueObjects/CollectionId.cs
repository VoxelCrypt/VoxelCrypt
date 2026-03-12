namespace Kernel.Models.ValueObjects;

public readonly record struct CollectionId
{
    public Guid Value { get; }

    private CollectionId(Guid value)
    {
        Value = value;
    }

    public static CollectionId New() => new(Guid.NewGuid());

    internal static CollectionId From(Guid value) => new(value);
}
