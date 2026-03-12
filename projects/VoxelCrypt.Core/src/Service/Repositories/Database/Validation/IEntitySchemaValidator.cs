using Kernel.Models.Entities;

namespace Service.Persistence.Database.Validation;

public interface IEntitySchemaValidator
{
	Task ValidateAsync(Entity entity, CancellationToken cancellationToken = default);
}
