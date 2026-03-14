using Kernel.Models.Entities;
using Kernel.Models.Exceptions;
using Kernel.Models.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Service.Api.Contracts;
using Service.Persistence;

namespace Service.Api.Controllers;

[ApiController]
[Route("api/entity-schemas")]
public sealed class EntitySchemasController(IEntitySchemaRepository repository) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IReadOnlyList<EntitySchemaResponse>>> ListAsync(CancellationToken cancellationToken)
	{
		var schemas = await repository.ListAsync(cancellationToken);
		return Ok(schemas.Select(Map).ToList());
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<EntitySchemaResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var schema = await repository.GetByIdAsync(EntitySchemaId.From(id), cancellationToken);
		if (schema is null)
		{
			return NotFound();
		}

		return Ok(Map(schema));
	}

	[HttpPost]
	public async Task<ActionResult<EntitySchemaResponse>> CreateAsync(
		[FromBody] EntitySchemaRequest request,
		CancellationToken cancellationToken)
	{
		try
		{
			var schema = new EntitySchema();
			schema.Define(request.Name, request.Identifier, request.Version, request.Properties, request.Required?.ToHashSet());

			await repository.UpsertAsync(schema, cancellationToken);
			return CreatedAtAction(nameof(GetByIdAsync), new { id = schema.Id.Value }, Map(schema));
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { error = ex.Message });
		}
		catch (ResourceValidationException ex)
		{
			return BadRequest(new { error = ex.Message, errors = ex.Errors });
		}
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult<EntitySchemaResponse>> UpdateAsync(
		Guid id,
		[FromBody] EntitySchemaRequest request,
		CancellationToken cancellationToken)
	{
		try
		{
			var schema = new EntitySchema { Id = EntitySchemaId.From(id) };
			schema.Define(request.Name, request.Identifier, request.Version, request.Properties, request.Required?.ToHashSet());

			await repository.UpsertAsync(schema, cancellationToken);
			return Ok(Map(schema));
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { error = ex.Message });
		}
		catch (ResourceValidationException ex)
		{
			return BadRequest(new { error = ex.Message, errors = ex.Errors });
		}
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		await repository.DeleteAsync(EntitySchemaId.From(id), cancellationToken);
		return NoContent();
	}

	private static EntitySchemaResponse Map(EntitySchema schema)
		=> new(
			schema.Id.Value,
			schema.Name,
			schema.Identifier,
			schema.Version,
			new Dictionary<string, string>(schema.Properties),
			schema.Required.ToList());
}
