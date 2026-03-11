# Architecture Baseline: Hexagonal Microservices with Standard Layering

*Date:* 2026-03-11
*Status:* Proposed
*Decision ID:* 0001-architecture-hexagonal-microservice-layering
*Related ADRs:* None
*Approved by:* Pending

## Context

VoxelCrypt is organized as a set of projects that evolve as independent microservices.

To keep services consistent, testable, and maintainable, each microservice should share a common internal structure:
- `Kernel` for domain logic.
- `Service` for application orchestration and infrastructure adapters.

The repository already reflects this structure in sample projects, with tests separated by layer (`Kernel.Tests`, `Service.Tests`).

The team also needs flexibility for additional delivery/consumer layers depending on runtime needs (for example API, CLI, MessageBus workers), without coupling domain logic to transport or hosting details.

## Decision

Adopt and document the following architecture baseline for all microservices in VoxelCrypt:

1. Architectural style
- Use Hexagonal Architecture (Ports and Adapters) as the default service design.
- Keep domain logic isolated from framework, transport, and persistence concerns.

2. Service decomposition
- Treat each project-level service as an independently deployable microservice boundary.
- Keep contracts explicit across boundaries (events, DTOs, integration contracts).

3. Mandatory layers per microservice
- `Kernel` layer (Domain):
  - Entities, value objects, enums, domain exceptions, and domain services.
  - Pure business rules with no dependency on infrastructure/framework concerns.
- `Service` layer (Application + Infrastructure):
  - Application services/use cases and orchestration.
  - Port definitions and adapter implementations.
  - Composition/wiring (for example DI registration).

4. Optional consumer layers
- Additional outer layers may be added as needed (for example API, CLI, MessageBus consumers, schedulers).
- These layers must depend inward on `Service`/ports and must not introduce direct dependencies from `Kernel` to transport/runtime concerns.

5. Testing baseline
- Maintain test projects per layer (at minimum `Kernel.Tests` and `Service.Tests`).
- Prioritize domain tests in `Kernel.Tests` and application/integration behavior tests in `Service.Tests`.

Alternatives considered:
- Layered monolith style with shared infrastructure coupling across modules.
- Feature-first vertical slices without explicit hexagonal boundaries.

These alternatives were not selected because they increase the risk of infrastructure leakage into domain logic and reduce consistency across microservices.

## Consequences

Positive:
- Consistent service structure across the repository.
- Better separation of concerns and easier unit testing of domain behavior.
- Clear path to add new consumers (API/CLI/MessageBus) without rewriting core logic.
- Improved maintainability and onboarding through repeatable microservice shape.

Negative / Trade-offs:
- Additional project and abstraction overhead (ports, adapters, contracts).
- Slightly higher initial implementation effort for simple features.
- Requires discipline to prevent accidental cross-layer shortcuts.

Implications:
- Future services should follow the same baseline unless superseded by a new ADR.
- Structural deviations should be explicit and justified in follow-up ADRs.

## Implementation Steps

1. Use this ADR as the default template for all new microservices.
2. For each new microservice, create mandatory projects/layers:
- `Kernel` (domain)
- `Service` (application + infrastructure)
- Corresponding tests (`Kernel.Tests`, `Service.Tests`)
3. Add optional consumer projects only when needed (API, CLI, MessageBus, etc.) while preserving dependency direction toward inner layers.
4. Validate in code reviews that:
- `Kernel` has no infrastructure/transport dependencies.
- Ports/adapters are implemented in `Service` or outer consumer layers.
5. Create new ADRs when introducing architectural exceptions or major extensions.

## References

- Hexagonal Architecture (Ports and Adapters)
- Repository structure under `projects/`
- Agent workflow instructions in `AGENTS.md`

## Revision History

| Date       | Agent (Model)             | Description               | Approved by |
| ---------- | ------------------------- | ------------------------- | ----------- |
| 2026-03-11 | GitHub Copilot (GPT-5.3-Codex) | Initial draft of the ADR. | Pending     |
