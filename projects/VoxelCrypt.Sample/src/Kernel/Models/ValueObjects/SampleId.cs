namespace Kernel.Models.ValueObjects;

public readonly record struct SampleId
{
    public Guid Value { get; }

    private SampleId(Guid value)
    {
        Value = value;
    }

    public static SampleId New() => new(Guid.NewGuid());

    internal static SampleId From(Guid value) => new(value);
}
