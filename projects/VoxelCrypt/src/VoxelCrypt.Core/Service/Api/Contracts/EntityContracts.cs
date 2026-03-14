namespace Service.Api.Contracts;

public sealed record EntityRequest(
	Guid EntitySchemaId,
	int SchemaVersion,
	Dictionary<string, object>? Content);

public sealed record EntityResponse(
	Guid Id,
	Guid EntitySchemaId,
	int SchemaVersion,
	string? SchemaIdentifier,
	Dictionary<string, object> Content);
