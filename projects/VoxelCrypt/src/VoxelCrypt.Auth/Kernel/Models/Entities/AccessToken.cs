namespace Kernel.Models.Entities;
using Kernel.Models.ValueObjects;

public sealed class AccessToken
{
    private AccessToken()
    {
        User = null!;
        Token = string.Empty;
    }

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

    public UserId UserId { get; private set; }

    public User User { get; private set; }

    public string Token { get; private set; }

    public DateTimeOffset ExpiresAt { get; private set; }

    public static AccessToken Create(User user, string token, DateTimeOffset expiresAt)
    {
        return new AccessToken(user, token, expiresAt);
    }

    public void UpdateExpiration(DateTimeOffset expiresAt)
    {
        ExpiresAt = expiresAt;
    }
}
