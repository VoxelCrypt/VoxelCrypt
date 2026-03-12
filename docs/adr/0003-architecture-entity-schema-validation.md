# Entity Schema Validation for User-Defined Entities

*Date:* 2026-03-13
*Status:* Proposed
*Decision ID:* 0003-architecture-entity-schema-validation
*Related ADRs:* [0001-architecture-hexagonal-microservice-layering](./0001-architecture-hexagonal-microservice-layering.md), [0002-architecture-baas-entity-structure](./0002-architecture-baas-entity-structure.md)
*Approved by:* Pending

## Context

VoxelCrypt.Core supports dynamic user-created entities where structure is not known at compile time. The current `Entity` model stores only `Content`, which lacks a persistent schema contract for validation and evolution.

We need a way to:
- Define fields and required constraints per user-created entity type.
- Validate each `Entity` instance against its schema before persistence.
- Track which schema version an instance follows.

This decision intentionally excludes compatibility mode and multi-tenant ownership metadata from the baseline to keep the initial implementation minimal.

## Decision

Adopt an `EntitySchema` + `Entity` instance model:

1. `EntitySchema` as metadata contract
- Contains stable identity, human-readable name/identifier, version, and schema definition JSON.
- Schema definition is stored as JSON with a simple baseline shape:
  - `properties`: dictionary of field names to primitive types (`string`, `number`, `boolean`, `object`, `array`).
  - `required`: list of required field names.

2. `Entity` as runtime instance
- References `EntitySchemaId` and `SchemaVersion`.
- Stores user payload in `Content`.
- Must pass schema validation before persistence.

3. Validation placement
- Validation is performed in Service layer before repository upsert is committed.
- Validation fails fast with a domain-specific exception that includes field-level messages.

Alternatives considered:
- Generating runtime CLR types per schema.
- Storing only free-form JSON without schema linkage.

These alternatives were rejected due to higher complexity and weaker governance over data quality.

## Consequences

Positive:
- Stronger data quality for dynamic entities.
- Explicit schema-to-instance linkage with version tracking.
- Easier future evolution toward migrations and richer rules.

Negative / Trade-offs:
- Additional metadata and validation complexity.
- Initial schema model supports only baseline primitive typing and required fields.

Implications:
- Future enhancements can add richer constraints without breaking existing baseline behavior.
- API/application layers should surface validation errors clearly to end users.

## Implementation Steps

1. Add `EntitySchema` model and `EntitySchemaId` value object in Kernel.
2. Extend `Entity` to reference schema id and schema version.
3. Add EF mappings and DbSet for schema persistence.
4. Add validation service and invoke it from repository upsert.
5. Keep compatibility mode and tenant/owner metadata out of this baseline.

## References

- [0001-architecture-hexagonal-microservice-layering](./0001-architecture-hexagonal-microservice-layering.md)
- [0002-architecture-baas-entity-structure](./0002-architecture-baas-entity-structure.md)
- AGENTS.md workflow and rules

## Revision History

| Date       | Agent (Model)                  | Description                                                 | Approved by |
| ---------- | ------------------------------ | ----------------------------------------------------------- | ----------- |
| 2026-03-13 | GitHub Copilot (GPT-5.3-Codex) | Initial draft of EntitySchema validation architecture ADR. | Pending     |
