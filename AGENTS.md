# Instructions:

## Workflow

1. Analyze task and ask for clarifications if needed; avoid assumptions.
2. Create new plan file `plan.md` inside `/.plans/<task-id>-<short-task-name>` directory.
  - If no `<task-id>` is available, check branch name (`git branch --show-current`) or use timestamp `(YYYY-mm-dd-[000])` instead.
  - Increment `[000]` for each new plan created on the same day.
3. All architectural changes must be approved and documented in `/docs/adr/`.
  - Use template from `.ai/adr/template.md`.
  - Always add reference to related ADRs in the plan.
  - Follow naming convention: `<sequential-number>-<category>-<short-descriptive-name>.md`.

## RULES

### Implementation stage

- Do not introduce breaking changes without prior approval.
- Ensure that code compiles and passes all tests before finalizing any changes.
- Ask for approval before installing any new dependencies or libraries.
- Do not remove or alter existing functionality unless explicitly instructed.
- Always prioritize code quality and maintainability over quick fixes.

### Subagents
- ALWAYS wait for all subagents to complete before yielding.
- Spawn subagents automatically when work is **independent and easy to parallelize**:
  - Multiple tasks from the plan that do not share state/files heavily
  - Long-running or blocking steps a worker can run alone (build, tests, scans)
  - Isolation for risky changes or checks

- Do NOT spawn subagents when work is **tightly coupled**:
  - Task B depends on code/decisions/artifacts from Task A (sequential handoff needed)
  - Tasks require frequent coordination in the same files or a single coherent design

- Rule of thumb:
  - Use subagents only if dependencies are minimal or can be handled via a clear contract (inputs/outputs, interfaces). Otherwise keep it in one agent.

## Instructions

Depending on language and framework, follow best practices described in instructions for that specific technology stack.

- [Python](/.ai/instructions/python/)
- [C# / .NET / dotnet](/.ai/instructions/dotnet/)
- [TypeScript](/.ai/instructions/typescript/)
- [Vue](/.ai/instructions/vue/)
- [Web (HTML, CSS)](/.ai/instructions/web/)
