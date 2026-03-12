namespace Kernel.Models.Exceptions;

public sealed class EntitySchemaValidationException : Exception
{
	public EntitySchemaValidationException(IReadOnlyList<string> errors)
		: base("Entity content failed schema validation.")
	{
		Errors = errors;
	}

	public IReadOnlyList<string> Errors { get; }
}
