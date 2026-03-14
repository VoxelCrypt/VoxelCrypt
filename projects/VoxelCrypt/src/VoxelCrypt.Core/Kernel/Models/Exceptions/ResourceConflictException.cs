using Kernel.Models.Entities;

namespace Kernel.Models.Exceptions;

public sealed class ResourceConflictException(string message) : Exception(message);

public sealed class ResourceConflictException<TResource> : Exception where TResource : Resource
{
	public ResourceConflictException()
		: base($"{typeof(TResource).Name} conflict detected.")
	{
	}

	public ResourceConflictException(string message)
		: base(message)
	{
	}
}
