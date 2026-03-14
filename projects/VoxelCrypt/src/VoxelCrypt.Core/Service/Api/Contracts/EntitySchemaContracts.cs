namespace Service.Api.Contracts;

public sealed record EntitySchemaRequest(
	string Name,
	string Identifier,
	int Version,
	Dictionary<string, string>? Properties,
	List<string>? Required);

public sealed record EntitySchemaResponse(
	Guid Id,
	string Name,
	string Identifier,
	int Version,
	Dictionary<string, string> Properties,
	List<string> Required);
