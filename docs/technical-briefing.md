# Technical Briefing

This document outlines a proposed technical implementation for an open-source backend-as-a-service. It highlights design decisions and trade-offs that should be resolved before implementation.

# Architecture

## Top-level Architecture

We recommend using macroservices (coarse-grained services) as the primary architectural style. Compared with monoliths or fine-grained microservices, macroservices make it easier to:
- collaborate across teams
- scale predictably and cost-effectively
- iterate and refactor components independently
- design features within well-bounded scopes

For inter-service messaging, prefer RabbitMQ (e.g., via MassTransit) or similar technologies that avoid future licensing constraints.

## Service Architecture

Each macroservice should follow a hexagonal (ports-and-adapters) architecture. This pattern emphasizes clear input and output boundaries and keeps the domain kernel implementation-agnostic.

See: https://herbertograca.com/2017/11/16/explicit-architecture-01-ddd-hexagonal-onion-clean-cqrs-how-i-put-it-all-together/

Hexagonal architecture simplifies building inbound adapters (controllers, message consumers) and outbound ports (clients, producers) while keeping use cases and domain logic transport-agnostic.

For implementation, we suggest using .NET 8, Entity Framework, and Wolverine (CQRS).

# Features

We should prioritize "easy wins" that deliver tangible value quickly. The sections below outline an initial scope.

## Gateway

Start with a lightweight API gateway providing authentication, structured logging, and reverse proxying to downstream services. If we adopt GraphQL (with schema stitching), the gateway will host the combined schema.

## Entities

The core capability is CRUD for dynamic entities. Evaluate SQL, NoSQL, and NewSQL options: traditional SQL works well for fixed schemas but can be cumbersome for dynamic columns; some NoSQL/NewSQL systems provide better native support for JSON and schema flexibility.

## Dashboard

Deliver a minimal, usable dashboard early. A simple prototype can likely be generated with AI from the existing API surface to accelerate feedback and usability testing.

---

# Comms (place to talk)

J: Any comments?