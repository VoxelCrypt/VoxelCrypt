using Kernel.Models.Entities;
using Kernel.Models.ValueObjects;

namespace Service.Persistence;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> ListAsync(CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task UpsertAsync(User user, CancellationToken cancellationToken = default);

    Task DeleteAsync(UserId id, CancellationToken cancellationToken = default);
}
