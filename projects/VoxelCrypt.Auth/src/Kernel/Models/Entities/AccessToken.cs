namespace Kernel.Models.Entities;
using Kernel.Models.ValueObjects;

public sealed class AccessToken
{
    private AccessToken(User user, string token, DateTimeOffset expiresAt)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token cannot be empty.", nameof(token));
        }

        User = user ?? throw new ArgumentNullException(nameof(user));
        UserId = user.Id;
        Token = token.Trim();
        ExpiresAt = expiresAt;
    }

    public UserId UserId { get; }

    public User User { get; }

    public string Token { get; }

    public DateTimeOffset ExpiresAt { get; }

    public static AccessToken Create(User user, string token, DateTimeOffset expiresAt)
    {
        return new AccessToken(user, token, expiresAt);
    }
}
