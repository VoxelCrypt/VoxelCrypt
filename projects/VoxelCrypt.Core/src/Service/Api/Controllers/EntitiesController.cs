using Kernel.Models.Entities;
using Kernel.Models.Exceptions;
using Kernel.Models.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Contracts;
using Service.Persistence;

namespace Service.Api.Controllers;

[ApiController]
[Route("api/entities")]
public sealed class EntitiesController(IEntityRepository repository) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IReadOnlyList<EntityResponse>>> ListAsync(CancellationToken cancellationToken)
	{
		var entities = await repository.ListAsync(cancellationToken);
		return Ok(entities.Select(Map).ToList());
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<EntityResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var entity = await repository.GetByIdAsync(EntityId.From(id), cancellationToken);
		if (entity is null)
		{
			return NotFound();
		}

		return Ok(Map(entity));
	}

	[HttpPost]
	public async Task<ActionResult<EntityResponse>> CreateAsync([FromBody] EntityRequest request, CancellationToken cancellationToken)
		=> await UpsertInternalAsync(null, request, cancellationToken);

	[HttpPut("{id:guid}")]
	public async Task<ActionResult<EntityResponse>> UpdateAsync(
		Guid id,
		[FromBody] EntityRequest request,
		CancellationToken cancellationToken)
		=> await UpsertInternalAsync(id, request, cancellationToken);

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		await repository.DeleteAsync(EntityId.From(id), cancellationToken);
		return NoContent();
	}

	private async Task<ActionResult<EntityResponse>> UpsertInternalAsync(
		Guid? id,
		EntityRequest request,
		CancellationToken cancellationToken)
	{
		try
		{
			var entity = id.HasValue
				? new Entity { Id = EntityId.From(id.Value) }
				: new Entity();

			entity.SetSchema(EntitySchemaId.From(request.EntitySchemaId), request.SchemaVersion);
			entity.ReplaceContent(request.Content ?? new Dictionary<string, object>());

			await repository.UpsertAsync(entity, cancellationToken);

			if (id.HasValue)
			{
				return Ok(Map(entity));
			}

			return CreatedAtAction(nameof(GetByIdAsync), new { id = entity.Id.Value }, Map(entity));
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { error = ex.Message });
		}
		catch (ResourceNotFoundException ex)
		{
			return NotFound(new { error = ex.Message });
		}
		catch (ResourceValidationException ex)
		{
			return BadRequest(new { error = ex.Message, errors = ex.Errors });
		}
	}

	private static EntityResponse Map(Entity entity)
		=> new(
			entity.Id.Value,
			entity.EntitySchemaId.Value,
			entity.SchemaVersion,
			entity.Schema?.Identifier,
			new Dictionary<string, object>(entity.Content));
}
