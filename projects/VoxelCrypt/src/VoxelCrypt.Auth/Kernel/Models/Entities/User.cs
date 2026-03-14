namespace Kernel.Models.Entities;
using Kernel.Models.ValueObjects;

public sealed class User
{
    private readonly List<AccessToken> _accessTokens = [];

    private User(UserId id, string username, HashedPassword passwordHash)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        }

        Id = id;
        Username = username.Trim();
        PasswordHash = passwordHash;
    }

    public UserId Id { get; }

    public string Username { get; }

    public HashedPassword PasswordHash { get; private set; }

    public IReadOnlyCollection<AccessToken> AccessTokens => _accessTokens.AsReadOnly();

    public static User Create(string username, HashedPassword passwordHash)
    {
        return new User(UserId.New(), username, passwordHash);
    }

    public AccessToken IssueAccessToken(string token, DateTimeOffset expiresAt)
    {
        if (_accessTokens.Any(x => string.Equals(x.Token, token, StringComparison.Ordinal)))
        {
            throw new ArgumentException("Token must be unique for the user.", nameof(token));
        }

        var accessToken = AccessToken.Create(this, token, expiresAt);
        _accessTokens.Add(accessToken);
        return accessToken;
    }

    public void UpdatePassword(HashedPassword passwordHash)
    {
        PasswordHash = passwordHash;
    }
}
