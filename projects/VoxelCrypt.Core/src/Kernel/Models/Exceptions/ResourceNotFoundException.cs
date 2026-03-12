using Kernel.Models.Entities;

namespace Kernel.Models.Exceptions;

public sealed class ResourceNotFoundException(string message) : Exception(message);

public sealed class ResourceNotFoundException<TResource> : Exception where TResource : Resource
{
	public ResourceNotFoundException()
		: base($"{typeof(TResource).Name} not found.")
	{
	}

	public ResourceNotFoundException(string message)
		: base(message)
	{
	}
}
