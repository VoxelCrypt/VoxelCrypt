using Kernel.Models.Entities;
using Kernel.Models.ValueObjects;

namespace Service.Persistence;

public interface IEntityRepository
{
    Task<Entity?> GetByIdAsync(EntityId id, CancellationToken cancellationToken = default);

    Task UpsertAsync(Entity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(EntityId id, CancellationToken cancellationToken = default);
}
