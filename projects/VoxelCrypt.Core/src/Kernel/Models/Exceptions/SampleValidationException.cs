namespace Kernel.Models.Exceptions;

public sealed class SampleValidationException(string message) : Exception(message);
