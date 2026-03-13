# Drop Aspire Hosting for Core Service and Use Docker Compose

*Date:* 2026-03-13
*Status:* Proposed
*Decision ID:* 0005-architecture-drop-aspire-use-docker-compose
*Related ADRs:* [0001-architecture-hexagonal-microservice-layering](./0001-architecture-hexagonal-microservice-layering.md), [0003-architecture-entity-schema-validation](./0003-architecture-entity-schema-validation.md)
*Approved by:* Pending

## Context

VoxelCrypt.Core Service was previously orchestrated with Aspire AppHost + ServiceDefaults. Current team direction is to run Core Service as a plain ASP.NET application and use Docker Compose for local orchestration with PostgreSQL.

## Decision

1. Remove Aspire-specific projects and references from Core runtime path.
2. Keep `VoxelCrypt.Core/src/Service` as the single application host.
3. Add Dockerfile for Service and docker-compose with PostgreSQL for local/dev startup.

## Consequences

Positive:
- Simpler local startup model with standard Docker tooling.
- Fewer moving parts for contributors and CI setup.
- Clear separation between app runtime and orchestration concerns.

Trade-offs:
- Lose Aspire dashboard and resource topology features.
- Health/dependency visibility becomes standard container/runtime responsibility.

## Implementation Steps

1. Remove Service reference to ServiceDefaults.
2. Remove AppHost and ServiceDefaults projects from solution/workspace flow.
3. Add Service health checks directly in Program.cs.
4. Add Dockerfile and docker-compose for Service + PostgreSQL.

## References

- AGENTS.md

## Revision History

| Date       | Agent (Model)                  | Description                                       | Approved by |
| ---------- | ------------------------------ | ------------------------------------------------- | ----------- |
| 2026-03-13 | GitHub Copilot (GPT-5.3-Codex) | Initial draft for removing Aspire in Core host. | Pending     |
