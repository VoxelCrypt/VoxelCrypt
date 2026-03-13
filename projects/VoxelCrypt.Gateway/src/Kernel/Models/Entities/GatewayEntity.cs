using Kernel.Models.Enums;
using Kernel.Models.Exceptions;
using Kernel.Models.ValueObjects;

namespace Kernel.Models.Entities;

public sealed class GatewayEntity
{
    private GatewayEntity(GatewayId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new GatewayValidationException("Name cannot be empty.");
        }

        Id = id;
        Name = name.Trim();
        Status = GatewayStatus.Alpha;
    }

    public static GatewayEntity Create(string name)
    {
        return new GatewayEntity(GatewayId.New(), name);
    }

    public GatewayId Id { get; }

    public string Name { get; }

    public GatewayStatus Status { get; private set; }

    public void Activate()
    {
        Status = GatewayStatus.Beta;
    }
}
