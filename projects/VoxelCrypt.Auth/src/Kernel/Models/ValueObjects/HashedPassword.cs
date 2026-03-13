namespace Kernel.Models.ValueObjects;

public readonly record struct HashedPassword
{
    public string Value { get; }

    private HashedPassword(string value)
    {
        Value = value;
    }

    public static HashedPassword From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Hashed password cannot be empty.", nameof(value));
        }

        return new HashedPassword(value.Trim());
    }

    public override string ToString() => Value;
}
