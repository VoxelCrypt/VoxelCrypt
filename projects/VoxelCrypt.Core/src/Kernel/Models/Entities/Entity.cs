using Kernel.Models.ValueObjects;

namespace Kernel.Models.Entities;

public sealed class Entity : Resource
{
	public EntityId Id { get; } = EntityId.New();
}
