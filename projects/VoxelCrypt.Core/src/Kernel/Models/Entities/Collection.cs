using Kernel.Models.ValueObjects;

namespace Kernel.Models.Entities;

public sealed class Collection<TResource> : Resource where TResource : Resource
{
	public CollectionId Id { get; } = CollectionId.New();
}
