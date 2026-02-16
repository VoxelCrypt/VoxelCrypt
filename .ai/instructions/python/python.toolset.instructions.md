---
name: Python toolset instructions
description: Instructions for AI Agents on how to operate with Python projects
---

# Python toolset instructions

## Package management

- use [uv](https://astral.sh/uv) to manage Python versions, virtual environments and dependencies.

## Linting and formatting

- Use **ty** for type checking -> `uvx ty`.
- Use **ruff** for linting & formatting:
  - lint: `uvx ruff check`
  - format: `uvx ruff format`

## Execute

- Use `uv run <script>` to execute Python scripts within the managed environment.
- Use `uv run pytest` to run tests.
