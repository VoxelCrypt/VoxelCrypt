using Kernel.Models.Entities;
using Kernel.Models.ValueObjects;

namespace Service.Persistence;

public interface IEntitySchemaRepository
{
	Task<EntitySchema?> GetByIdAsync(EntitySchemaId id, CancellationToken cancellationToken = default);

	Task<EntitySchema?> GetByIdentifierAsync(string identifier, int version, CancellationToken cancellationToken = default);

	Task UpsertAsync(EntitySchema entitySchema, CancellationToken cancellationToken = default);

	Task DeleteAsync(EntitySchemaId id, CancellationToken cancellationToken = default);
}
