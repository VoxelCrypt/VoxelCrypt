---
name: vue-pinia
description: Pinia usage conventions for shared state.
metadata:
  applyTo: '**/*.ts, **/*.vue'
---

# Pinia default instructions

## Default rule

- Use Pinia for shared cross-component or app-level state.
- Keep component-local ephemeral UI state as local `ref`/`computed` when not shared.

## Store style

- Use setup stores by default:
  - `defineStore('store-id', () => { ... })`
- Prefer explicit typed state and action return values.

## Boundaries

- Do not move repository/data-adapter ownership into stores by default.
- Repositories remain resolved through existing DI mechanisms unless a separate ADR explicitly changes this.

## Testing

- For component tests that rely on stores, mount with `@pinia/testing` and `createTestingPinia({ stubActions: false })`.
- Add focused store tests for store-only logic (persistence, listeners, derived state).
