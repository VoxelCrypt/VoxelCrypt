# Database per Service for PostgreSQL

*Date:* 2026-03-13
*Status:* Proposed
*Decision ID:* 0006-architecture-database-per-service
*Related ADRs:* [0001-architecture-hexagonal-microservice-layering](./0001-architecture-hexagonal-microservice-layering.md), [0005-architecture-drop-aspire-use-docker-compose](./0005-architecture-drop-aspire-use-docker-compose.md)
*Approved by:* Pending

## Context

VoxelCrypt services are split by bounded context (Core, Auth, Gateway). Reusing a single PostgreSQL database across services risks table overlap, migration coupling, and accidental cross-service persistence coupling.

## Decision

1. Each service must use a dedicated PostgreSQL database, even when sharing the same PostgreSQL server instance.
2. Initial database naming convention:
   - `voxelcrypt_core`
   - `voxelcrypt_auth`
   - `voxelcrypt_gateway`
3. Service-owned migrations and schema changes are restricted to that service's database.

## Consequences

Positive:
- Stronger service data boundaries.
- Lower risk of table-name collisions during data replication or schema evolution.
- Cleaner ownership and migration lifecycle per service.

Trade-offs:
- More database objects to provision/manage.
- Slightly more setup complexity for local development environments.

## Implementation Steps

1. Update local Postgres initialization to create service-specific databases.
2. Update service connection strings to target service-owned databases.
3. Keep future service hosts aligned with this naming convention.

## Revision History

| Date       | Agent (Model)                  | Description                                       | Approved by |
| ---------- | ------------------------------ | ------------------------------------------------- | ----------- |
| 2026-03-13 | GitHub Copilot (GPT-5.3-Codex) | Initial draft for per-service database policy. | Pending     |
