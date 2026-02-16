---
applyTo: '**/*.py'
---

# Guide to Organizing Code for Python

## Project setup

### Architecture

- Follow clean architecture and vertical slice layout.
- Route imports inward to prevent circular dependencies.
- Use inversion of control (IoC) and dependency injection (DI) to decouple components. Do not create dependencies directly within classes.
- Inject infrastructure dependencies (databases, queues, blob stores) via constructors and release them in `cleanup()` / `close()` to avoid lingering connections.
- Mirror production repository interfaces in test doubles so unit tests remain API-compatible and decoupled from implementation details.

### 📁 Structure

```plaintext
/
├── src/
│   ├── common/                                  # cross-cutting, stable
│   │   ├── config/
│   │   │   ├── settings.py                      # Pydantic settings + env parsing
│   │   │   └── loaders.py                       # load YAML/ENV, merge profiles
│   │   ├── observability/
│   │   │   ├── logging.py                       # structlog setup
│   │   │   ├── tracing.py                       # OTEL telemetry
│   │   │   └── metrics.py                       # OTEL metrics
│   │   └── utils.py                             # keep minimal; prefer feature-local utils
│   │
│   ├── feature_a/
│   │   ├── domain/                              # pure business rules
│   │   │   ├── models.py                        # entities, value objects
│   │   │   ├── services.py                      # domain services (optional)
│   │   │   └── events.py                        # domain events (optional)
│   │   │
│   │   ├── application/                         # use cases (orchestrate domain + ports)
│   │   │   ├── commands.py                      # intent objects (optional)
│   │   │   ├── handlers.py                      # use case handlers
│   │   │   ├── ports.py                         # repo + external service interfaces (Protocols)
│   │   │   └── dto.py                           # input/output DTOs if needed
│   │   │
│   │   └── infrastructure/                      # implementations (DB, HTTP, blob, etc.)
│   │       ├── persistence/
│   │       │   └── foo_repository.py
│   │       ├── integrations/
│   │       │   └── external_client.py
│   │       └── unit_of_work.py
│   │
│   └── <app/module>/                            # app/module composition root
│       ├── main.py                              # start app, load settings, init container
│       ├── container.py                         # DI bindings: interface -> implementation (optional)
│       └── health.py                            # health/readiness endpoints (if service)
│
├── config/                                      # environment profiles
│   ├── base.yaml
│   ├── dev.yaml
│   ├── test.yaml
│   └── prod.yaml
│
├── tests/
│   ├── shared/
│   │   ├── test_config.py
│   │   ├── test_logging.py
│   │   ├── test_errors.py
│   │   └── test_types.py
│   ├── feature_a/
│   │   ├── domain/
│   │   ├── application/
│   │   ├── infrastructure/                      # integration tests live here
│   │   └── entrypoints/                         # API/consumer tests if needed
│   ├── feature_b/
│   │   └── ...
│   ├── conftest.py
│   └── factories.py                             # test factories/builders (optional)
│
├── pyproject.toml
└── uv.lock
```

## Deployment and Configuration

- Make all settings configurable via environment variables.
- Validate required configuration fields at startup.

## Best practices

- Prefer to use `uuid7` for generating UUIDs where time-based ordering is beneficial, otherwise use `uuid4` for random UUIDs.
- Suffix all async functions with `_async` (e.g., `fetch_data_async`).
- Use Use an `f-string`, the `%` operator, or the `format()` method for formatting strings.
- Do not use a backslash for explicit line continuation.
- Do not terminate your lines with semicolons, and do not use semicolons to put two statements on the same line.
- Break long expressions at the highest syntactic level and avoid vertical token alignment; prefer readability over clever one-liners.

## Code style

Refer to [PEP 8](https://peps.python.org/pep-0008/) for general style guidelines with the following conventions enhancing readability and maintainability.

## Documentation

- Use Sphinx documentation style for docstrings.
- Ensure all public functions and methods include type hints and docstrings.
- All class docstrings should start with a one-line summary that describes what the class instance represents.
- Document tests code with arrange / act / assert or arrange / act & assert comments to clarify test structure.

## Typing

- Use type hints for all functions and methods.
- Use explicit X | None instead of implicit.
- Use modern typing syntax (`str | None`, `list[int]`) over legacy (`Optional[str]`, `List[int]`).
- Utilize `from __future__ imports` as needed for forward compatibility.
- Use `from __future__ import annotations` to defer type evaluation.
- Keep `mypy` strict mode green by avoiding implicit `None`, redundant casts, and untyped helper functions.
- Use `str` for string/text data. For code that deals with binary data, use `bytes`.
- Use `pathlib.Path` for filesystem paths instead of plain strings.
- Prefer `namedtuple` when creating public-facing APIs, use `tuple` for private/internal purposes only.

## Imports

Use `import` statements for **packages** and **modules** only, not for individual types, classes, or functions.

- Use `import x` for importing packages and modules.
- Use `from x import y` where `x` is the package prefix and `y` is the module name with no prefix.
- Use `from x import y as z` in any of the following circumstances:
  - Two modules named `y` are to be imported.
  - `y` conflicts with a top-level name defined in the current module.
  - `y` conflicts with a common parameter name that is part of the public API (e.g., features).
  - `y` is an inconveniently long name.
  - `y` is too generic in the context of your code (e.g., from storage.file_system import options as fs_options).
- Use `import y as z` only when `z` is a standard abbreviation (e.g., `import numpy as np`).
- Utilize `typing.TYPE_CHECKING` to prevent circular imports and reduce runtime overhead for type-only imports.

For symbols (including types, functions, and constants) from the `typing` or `collections.abc` modules used to support static analysis and type checking, always import the symbol itself:

```python
from collections.abc import Mapping, Sequence
from typing import Any, Generic, cast, TYPE_CHECKING
```

## Closing

Consistency usually shouldn’t be an excuse to keep using outdated patterns without weighing the advantages of newer approaches or the fact that the codebase will naturally drift toward modern styles over time.

**Strive for greatness, be clear, stay consistent.**
