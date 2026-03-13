using System.Text;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using Kernel.Models.ValueObjects;

namespace Kernel.Services;

public sealed class PasswordHashService : IPasswordHashService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 3;
    private const int MemorySizeKb = 65_536;
    private const int DegreeOfParallelism = 4;
    private const char Separator = '$';

    public HashedPassword Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.", nameof(password));
        }

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var argon2 = new Argon2id(passwordBytes)
        {
            Salt = salt,
            Iterations = Iterations,
            MemorySize = MemorySizeKb,
            DegreeOfParallelism = DegreeOfParallelism
        };

        var hash = argon2.GetBytes(HashSize);

        var hashedPassword = string.Join(
            Separator,
            "argon2id",
            Iterations,
            MemorySizeKb,
            DegreeOfParallelism,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));

        return HashedPassword.From(hashedPassword);
    }

    public bool Verify(string password, HashedPassword passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        var parts = passwordHash.Value.Split(Separator);
        if (parts.Length != 6 || !string.Equals(parts[0], "argon2id", StringComparison.Ordinal))
        {
            return false;
        }

        if (!int.TryParse(parts[1], out var iterations) || iterations <= 0)
        {
            return false;
        }

        if (!int.TryParse(parts[2], out var memorySizeKb) || memorySizeKb <= 0)
        {
            return false;
        }

        if (!int.TryParse(parts[3], out var degreeOfParallelism) || degreeOfParallelism <= 0)
        {
            return false;
        }

        byte[] salt;
        byte[] expectedHash;

        try
        {
            salt = Convert.FromBase64String(parts[4]);
            expectedHash = Convert.FromBase64String(parts[5]);
        }
        catch (FormatException)
        {
            return false;
        }

        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var argon2 = new Argon2id(passwordBytes)
        {
            Salt = salt,
            Iterations = iterations,
            MemorySize = memorySizeKb,
            DegreeOfParallelism = degreeOfParallelism
        };

        var computedHash = argon2.GetBytes(expectedHash.Length);
        return CryptographicOperations.FixedTimeEquals(computedHash, expectedHash);
    }
}
