namespace Kernel.Models.Exceptions;

public sealed class GatewayValidationException(string message) : Exception(message);
