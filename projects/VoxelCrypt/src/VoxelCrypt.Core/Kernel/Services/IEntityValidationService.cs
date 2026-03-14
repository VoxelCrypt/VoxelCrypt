using Kernel.Models.Entities;

namespace Kernel.Services;

public interface IEntityValidationService
{
	void Validate(Entity entity, EntitySchema schema);
}
