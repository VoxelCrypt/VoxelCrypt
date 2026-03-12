# BaaS Entity Structure Baseline

*Date:* 2026-03-12
*Status:* Proposed
*Decision ID:* 0002-architecture-baas-entity-structure
*Related ADRs:* [0001-architecture-hexagonal-microservice-layering](./0001-architecture-hexagonal-microservice-layering.md)
*Approved by:* Pending

## Context

VoxelCrypt is evolving toward BaaS-style capabilities where domain resources are exposed and consumed by multiple application surfaces.

To keep resource modeling consistent across services, we need explicit conventions for how entities, collections, and data definitions are structured in the `Kernel` domain model.

Recent modeling changes introduced:
- Domain resources represented by concrete classes (`Entity`, `Collection<TResource>`, `Data`).
- Strongly typed identifier value objects for identity-bearing models.
- A preference for explicit semantic types over generic primitive usage.

Without a baseline, each service could model these concepts differently, increasing integration cost and ambiguity.

## Decision

Adopt the following BaaS entity structure baseline for VoxelCrypt domain models:

1. Resource categories
- `Entity`: a domain object with identity and lifecycle.
- `Collection<TResource>`: a logical group of resources, also identity-bearing.
- `Data`: definition-style domain data (for example dictionaries/constants), not treated as an instance-lifecycle entity.

2. Identity strategy
- `Entity` uses a dedicated value object identifier (`EntityId`).
- `Collection<TResource>` uses a dedicated value object identifier (`CollectionId`).
- `Data` does not use GUID-style instance identity by default.

3. Data identification
- `Data` exposes a stable identifier for discovery and referencing.
- For internal-only usage, a string identifier is acceptable.
- For cross-boundary usage (API/events/storage contracts), use a dedicated value object for the identifier.

4. Modeling rules
- Keep identity primitives wrapped in value objects within `Kernel`.
- Avoid exposing raw primitives (`Guid`, `string`) as primary domain identity where a semantic identifier type exists.
- Keep resource behavior and invariants in `Kernel`; transport/infrastructure concerns remain outside `Kernel` per ADR 0001.

Alternatives considered:
- Using raw primitive IDs (`Guid`/`string`) everywhere.
- Treating `Data` as lifecycle entities with instance IDs.
- Using a single shared generic identifier type for all resource kinds.

These alternatives were not selected because they weaken domain semantics, reduce compile-time safety, and blur lifecycle differences between entities and data definitions.

## Consequences

Positive:
- Stronger type safety for domain identity.
- Clearer distinction between lifecycle entities and definition-style data.
- More consistent modeling across microservices and BaaS surfaces.
- Reduced risk of identifier mix-ups across resource categories.

Negative / Trade-offs:
- Additional value-object boilerplate.
- Slightly higher initial modeling effort.
- Need for explicit mapping at integration boundaries where primitives are still required.

Implications:
- New services should follow this structure unless superseded by a newer ADR.
- Existing services should align gradually when touched for feature work.

## Implementation Steps

1. In `Kernel/Models/Entities`, model resources using `Entity`, `Collection<TResource>`, and `Data` roles.
2. In `Kernel/Models/ValueObjects`, define semantic identifier types for identity-bearing models (`EntityId`, `CollectionId`, etc.).
3. Keep `Data` as definition-style resources and provide stable identifiers.
4. Promote `Data` identifier from string to value object when used across external contracts.
5. Validate in code review that identity semantics remain explicit and consistent.

## References

- [0001-architecture-hexagonal-microservice-layering](./0001-architecture-hexagonal-microservice-layering.md)
- Repository structure under `projects/VoxelCrypt.Core/src/Kernel/Models`
- Agent workflow instructions in `AGENTS.md`

## Revision History

| Date       | Agent (Model)                        | Description                             | Approved by |
| ---------- | ------------------------------------ | --------------------------------------- | ----------- |
| 2026-03-12 | GitHub Copilot (GPT-5.3-Codex)       | Initial draft of BaaS entity structure. | Pending     |
