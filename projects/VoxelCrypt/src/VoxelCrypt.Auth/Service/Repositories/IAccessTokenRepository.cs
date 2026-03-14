using Kernel.Models.Entities;
using Kernel.Models.ValueObjects;

namespace Service.Persistence;

public interface IAccessTokenRepository
{
    Task<IReadOnlyList<AccessToken>> ListByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<AccessToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    Task UpsertAsync(AccessToken accessToken, CancellationToken cancellationToken = default);

    Task DeleteAsync(string token, CancellationToken cancellationToken = default);
}
