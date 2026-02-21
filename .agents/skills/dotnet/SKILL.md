---
name: dotnet
description: Instructions for AI agents on how to operate with C# / .NET / dotnet projects.
metadata:
  applyTo: '**/*.cs'
---

You are staff software engineer assisting with a **C#/.NET** codebase that aims for **high quality, maintainable, and
well-tested** code.  
Follow these rules carefully and consistently.

Do not hallucinate or make up code or details. If you are unsure about something, ask for clarifications.

---

## 1. General Quality & Architecture

- Prefer **clean, readable code** over clever one-liners.
- Follow **SOLID** principles and good separation of concerns.
- Follow DRY (Don't Repeat Yourself); avoid code duplication.
- Extract repeatable logic into **reusable methods** or **services**.
- Prefer **composition to inheritance**.
- Use `var` instead of explicit types.
- Minimize mutable state; prefer **immutable** data structures and records where appropriate.
- Always use **async/await** for I/O and long-running operations (follow the `Async` suffix naming convention).
- Avoid "magic values" – use **constants**, **enums**, or **options/config**.
- Use **dependency injection** for external services, clocks, loggers, and repositories.
- Keep methods **small and focused**; if a method is doing too much, split it.
- Validate inputs early; fail fast with clear exceptions or result types.
- Prefer **explicitness** to implicit behavior, especially in public APIs.
- Prefer to use async over sync methods in blocking calls.
- Use await when calling async methods to avoid blocking calls.
- Trust dependencies; throw only on nullable.

### 1.1 Structure

```
FeatureController
|- Input: FooRequest
|- Output: FooResponse

FeatureHandler
|- Input: FooCommand / FooQuery
|- Output: FooResult (DTO)

FeatureService
|- Input: DomainEntity
|- Output: DomainEntity (internal business logic)
|- Output: FooResult (application logic)
```

```
<feature>
|-- /Controllers
|   |-- FooController.cs
|-- /Requests
|   |-- FooRequest.cs
|-- /Responses
|   |-- FooResponse.cs
|-- /Commands
|   |-- FooCommand.cs
|-- /Queries
|   |-- FooQuery.cs
|-- /Handlers
|   |-- FooCommandHandler.cs
|   |-- FooQueryHandler.cs
|-- /Services
|   |-- FooService.cs
|-- /Data
|   |-- FooDto.cs
|-- /Domain
|   |-- Foo.cs
|-- /Extensions
    |-- FooExtensions.cs
    |-- /DependencyInjection
        |-- ServiceCollectionExtensions.cs # for DI registrations
```

---

## 2. Language & Framework Conventions

- Use **C# latest language features** that improve clarity and safety (e.g., pattern matching, `switch` expressions,
  `record` types when appropriate).
- Use **nullable reference types** and respect compiler warnings.
- Use `var` when the type is obvious from the right-hand side; otherwise use explicit types.
- Use **expression-bodied** members only when they remain readable.
- Use `using` directives at the top of the file; enable file-scoped namespaces where appropriate.
- Prefer **interfaces** for abstractions that might have multiple implementations.

---

## 3. Date and Time Handling

> **Important:** For backend code, use `DateTimeOffset` instead of `DateTime`.

- Use `DateTimeOffset` for all time-related values.
- Use `TimeProvider` when you need the current time to facilitate testing and time abstraction.
- When adding new APIs, method parameters, or properties involving time, use `DateTimeOffset`.
- Prefer UTC times for storage and transmission.
- Use `TimeSpan` for durations and intervals. Do not use numeric types to represent time.
- **Never use `DateTime`** in backend/domain code.

---

## 4. Logging

> **Important:** In tests use `FakeLogger<T>` from `Microsoft.Extensions.Diagnostics.Testing`.

- In production code, use `ILogger<T>` from `Microsoft.Extensions.Logging`.
- Log at appropriate levels (`Trace`, `Debug`, `Information`, `Warning`, `Error`, `Critical`).
- Prefer **structured logging** (e.g., `logger.LogInformation("User {UserId} created", userId)`).
- In tests:
    - Use `FakeLogger<T>` to verify log messages and levels.
    - Avoid custom test loggers unless there is a strong reason.

---

## 5. Testing

> **Important:**
> - Use **TUnit** for tests.
> - Use **NSubstitute** for mocks.
> - Use **AwesomeAssertions** (do **not** use xUnit’s built-in `Assert` APIs).
> - Reuse preparation/setup code via constructors or helper methods.

### 5.1 Test Framework

- All tests must be written with **xUnit**:
    - Use `[Fact]` for single tests.
    - Use `[Theory]` with `[InlineData]` or other data sources for parameterized tests.
- **Do not** reference or use `NUnit`, `MSTest`, or other test frameworks.

### 5.2 Assertions

- Use **AwesomeAssertions** for all assertions.
- **Never** use xUnit’s `Assert.*` methods.
- Prefer fluent and expressive tests, e.g.:
    - `result.Should().Be(...)` / `result.Should().NotBeNull()` (or whatever the AwesomeAssertions API provides).
- Tests should be **clear and intention-revealing** (“Arrange-Act-Assert” structure).

### 5.3 Mocks & Test Doubles

- Use **NSubstitute** for creating mocks, stubs, and spies.
- Prefer interface-based dependencies and mock those using NSubstitute:
    - `var service = Substitute.For<IMyService>();`
- Setup behavior using NSubstitute’s idioms, e.g. `service.DoWork().Returns(value);`
- Verify interactions using NSubstitute, e.g. `service.Received(1).DoWork();`
- Do **not** use Moq, FakeItEasy, or other mocking libraries.

### 5.4 Test Quality

- Each test should be **independent** and deterministic.
- No reliance on external services (DB, HTTP, file system) without proper abstraction and mocking.
- Prefer many small, focused tests over large, complex ones.
- Name tests descriptively, e.g. `MethodName_Condition_ExpectedResult`.

---

## 6. Error Handling & Validation

- Use clear, specific exception types where appropriate.
- Validate input arguments using:
    - Guard clauses (e.g. `ArgumentNullException.ThrowIfNull(param);`).
- Avoid swallowing exceptions; either handle them meaningfully or let them propagate.
- When appropriate, prefer result types (e.g. `Result<T>`, `OneOf`, or similar patterns) instead of using exceptions for
  regular control flow.

---

## 7. API & Public Surface

- Public methods and classes should have **XML documentation comments** when they are part of a public API or library.
- Names should be **clear, concise, and intention revealing**.
- Avoid leaking internal implementation details via public interfaces.
- Use **DTOs** for data transfer in APIs; avoid exposing domain entities directly.
- Prefer to organize files into vertical slices (by feature/module) instead of horizontal layers.

---

## 9. Code Style Examples

When in doubt, follow these examples:

```csharp
public sealed class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly INullableDependency? _nullableDependency;

    public OrderService(ILogger<OrderService> logger, IOrderRepository orderRepository, INullableDependency? nullableDependency = null)
    {
        // thrust in dependencies
        _logger = logger;
        _orderRepository = orderRepository;

        // throw only on nullable
        _nullableDependency = nullableDependency ?? throw new ArgumentNullException(nameof(nullableDependency));
    }

    public async Task<Order> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var now = DateTimeOffset.UtcNow;

        var order = new Order(request.CustomerId, request.Items, now);

        await _orderRepository.AddAsync(order, cancellationToken);

        _logger.LogInformation("Order {OrderId} created for customer {CustomerId} at {CreatedAt}", 
            order.Id, order.CustomerId, order.CreatedAt);

        return order;
    }
}
```

Example test pattern:

```csharp
public class OrderServiceTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly FakeLogger<OrderService> _logger;

    private readonly OrderService _subject;

    public OrderServiceTests()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _logger = new FakeLogger<OrderService>();
        _subject = new OrderService(_orderRepository, _logger);
    }

    [Fact]
    public async Task CreateOrderAsync_WithValidRequest_CreatesOrderAndLogsInformation()
    {
        // Arrange
        var request = new CreateOrderRequest(customerId: Guid.NewGuid(), items: new[] { "item-1" });

        // Act
        var result = await _subject.CreateOrderAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.CustomerId.Should().Be(request.CustomerId);

        await _orderRepository.Received(1)
                              .AddAsync(result, Arg.Any<CancellationToken>());

        _logger.Entries.Should()
                       .Contain(entry =>
            entry.LogLevel == LogLevel.Information
            && entry.Message.Contains("Order")
            && entry.Message.Contains("created"));
    }
}
```

10. Things to Avoid

- Do not put business logic in controllers or API endpoints.
- Do not use static classes for services that have dependencies (use DI and interfaces).
- Do not use `DateTime` in backend code (always use `DateTimeOffset`).
- Do not test framework's `Assert.*` methods (use **AwesomeAssertions** instead).
- Do not introduce other test frameworks or mocking libraries (stick to **TUnit** + **NSubstitute**).
- Do not write code without tests logic.
- Do not introduce tight coupling between layers or bypass abstractions (use interfaces).
- Do not leave TODO comments without context; prefer creating issues if work is deferred.
- Do not `GetAwaiter().GetResult()` or `Wait()`, await methods with `await`. If you must, create `// HACK: reason` comment.

---

## Summary

Adhering to these conventions will help maintain a high standard of code quality, readability, and testability in the
C#/.NET codebase.

Always generate code and tests that respect these conventions by default.
