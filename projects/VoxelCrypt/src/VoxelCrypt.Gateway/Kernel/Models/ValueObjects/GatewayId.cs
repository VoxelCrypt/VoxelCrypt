namespace Kernel.Models.ValueObjects;

public readonly record struct GatewayId
{
    public Guid Value { get; }

    private GatewayId(Guid value)
    {
        Value = value;
    }

    public static GatewayId New() => new(Guid.NewGuid());

    internal static GatewayId From(Guid value) => new(value);
}
