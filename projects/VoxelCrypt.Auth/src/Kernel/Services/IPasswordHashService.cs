namespace Kernel.Services;
using Kernel.Models.ValueObjects;

public interface IPasswordHashService
{
    HashedPassword Hash(string password);

    bool Verify(string password, HashedPassword passwordHash);
}
