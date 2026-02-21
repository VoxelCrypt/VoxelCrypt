---
name: architecture
description: Guidelines and instructions related to project architecture.
metadata:
  applyTo: '**'
---

# Architecture instructions

## Tl;dr

- Follow **Clean Architecture** and **Vertical Slice** principles.
- Ensure high cohesion and low coupling between components.
- Use **Dependency Injection** for managing dependencies.
- Separate business logic from infrastructure and UI concerns. Ensure domain entities / business logic dtos are not returned from API or UI layers.
- Organize code by feature (vertical slices) rather than by technical layers.
- Write modular, testable, and maintainable code.
- Write unit tests for business logic and integration tests for interactions between components.

## Principles

### DRY (Don't Repeat Yourself)

Avoid code duplication by abstracting common functionality into reusable components or services.

- Use parametrized tests or test helpers to reduce redundancy.

## Naming Conventions

Organize files and directories based on features or modules rather than technical layers.
Each feature/module should be placed in `/[Mm]odules/<feature-name>/` directory.

- Controllers -> typical API controllers, entry points
- Requests -> DTO for input to API layer
- Responses -> DTOs for output from API layer
- Services -> business logic services
- Repositories -> data access layer
- Queries -> CQRS queries
- Commands -> CQRS commands
- Handlers -> CQRS handlers (commands/queries)
- Factories -> object creation logic


## Implementation Guidelines

- Map DTOs between layers using extension methods or static methods, example: `from()` / `to()`.
