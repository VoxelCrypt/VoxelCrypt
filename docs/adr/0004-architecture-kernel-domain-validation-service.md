# Kernel Domain Validation Service for Entities

*Date:* 2026-03-13
*Status:* Proposed
*Decision ID:* 0004-architecture-kernel-domain-validation-service
*Related ADRs:* [0001-architecture-hexagonal-microservice-layering](./0001-architecture-hexagonal-microservice-layering.md), [0003-architecture-entity-schema-validation](./0003-architecture-entity-schema-validation.md)
*Approved by:* Pending

## Context

Entity schema validation currently lives in Service persistence code. This mixes domain rules with infrastructure concerns and requires JSON-oriented value handling in the validator implementation.

We want repository code to prepare data and keep persistence-specific serialization details outside Kernel.

## Decision

1. Move entity validation rules into Kernel as a domain service.
2. Keep repository responsibilities in Service limited to loading data and persisting changes.
3. Normalize persistence payload values in Service before invoking Kernel validation to avoid leaking JSON-oriented types into Kernel logic.

## Consequences

Positive:
- Domain rules become infrastructure-agnostic and reusable.
- Clear separation between validation logic and data access.
- Reduced risk of persistence details leaking into domain code.

Trade-offs:
- Slightly more orchestration in repository code.
- One extra abstraction between loading schema and validating entity data.

## Implementation Steps

1. Add Kernel validation service interface and implementation.
2. Add Service schema loader/normalizer for prepared validation input.
3. Update repository flow to call domain validation service.
4. Remove old Service validator that contained domain rules.

## References

- [0003-architecture-entity-schema-validation](./0003-architecture-entity-schema-validation.md)
- AGENTS.md

## Revision History

| Date       | Agent (Model)                  | Description                                             | Approved by |
| ---------- | ------------------------------ | ------------------------------------------------------- | ----------- |
| 2026-03-13 | GitHub Copilot (GPT-5.3-Codex) | Initial draft for moving validation logic to Kernel.   | Pending     |
