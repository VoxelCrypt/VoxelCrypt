namespace Kernel.Models.Exceptions;

public sealed class ResourceValidationException : Exception
{
	public ResourceValidationException(string message, IReadOnlyList<string> errors)
		: base(message)
	{
		Errors = errors;
	}

	public IReadOnlyList<string> Errors { get; }
}
