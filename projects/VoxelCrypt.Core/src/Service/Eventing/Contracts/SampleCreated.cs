namespace Service.Eventing.Contracts;

public sealed record SampleCreated(Guid SampleId, string Name, DateTimeOffset CreatedAt);
