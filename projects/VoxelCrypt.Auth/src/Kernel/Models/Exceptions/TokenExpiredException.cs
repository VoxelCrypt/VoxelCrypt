namespace Kernel.Models.Exceptions;

public sealed class TokenExpiredException(string message) : Exception(message)
{
    public TokenExpiredException()
        : this("Token has expired.")
    {
    }
}
