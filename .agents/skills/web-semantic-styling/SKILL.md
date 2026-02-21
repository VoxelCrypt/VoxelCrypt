---
name: web-semantic-styling
description: Semantic styling instructions for frontend components.
metadata:
  applyTo: '**/*.vue, **/*.css, **/*.html'
---

## Purpose

Use daisyUI as the baseline styling system for components and theme semantics.

## Rules

- Prefer daisyUI component classes (`btn`, `card`, `modal`, `badge`, and related variants) for interactive and structural UI.
- Prefer shared primitive wrappers (`Button`, `ToggleButton`, `Panel`, `Dialog`) for reusable patterns.
- Use Tailwind utilities primarily for layout, spacing, sizing, and responsive behavior.
- Keep content typography at `text-base` by default; reserve `text-xs` for intentional microcopy.
- Do not introduce or reuse legacy custom semantic token utility naming from the previous design-system contract.

## Theme behavior

- App themes are `light` and `dark` through daisyUI.
- Theme preference options remain `system`, `light`, `dark`.
- Resolved theme is applied via `html[data-theme="light|dark"]`.

## Customization

- Prefer daisyUI built-in semantics before creating custom utility classes.
- Add custom CSS only when a requirement cannot be expressed cleanly with daisyUI + standard Tailwind utilities.
- All extended (custom) styles should be prefixed with `ul-` (e.g., `ul-dialog`) and added to [ulf.css](/projects/sativance-app/src/styles/ulf.css).
