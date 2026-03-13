namespace Service.Eventing.Contracts;

public sealed record GatewayCreated(Guid GatewayId, string Name, DateTimeOffset CreatedAt);
