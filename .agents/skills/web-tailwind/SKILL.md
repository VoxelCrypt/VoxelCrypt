---
name: web-tailwind
description: Set of instructions and best practices for development using Vue.js with TypeScript.
metadata:
  applyTo: '**/*.vue, **/*.ts, **/*.html, **/*.css'
---

# Tailwind instructions

Use Tailwind v4 best practices for writing and organizing CSS code.

## Responsive design

- Always design mobile first, ensure good mobile experience before adding desktop-specific styles.
- Use Tailwind's responsive prefixes like `sm:`, `md:`, `lg:`, `xl:`, and `2xl:` to apply styles conditionally.

## Typography

- Use semantic font size classes like `text-sm`, `text-base`, `text-lg` instead of arbitrary values like `text-[14px]`.
- Prefer using Tailwind's built-in typography utilities over custom CSS.
- Prefer (omitting) using `text-base` as the default font size for body text.
- Default body/content text should be `text-base`.
- Keep `text-xs` only for intentional microcopy (meta labels, compact captions).
- Do not use arbitrary text sizes (`text-[...]`) unless explicitly approved for a one-off exception.
- Use clear hierarchy:
  - page/section headers: `text-lg` to `text-3xl` depending on context
  - body content: `text-base`
  - microcopy only: `text-xs`

## DaisyUI-first usage

- Use daisyUI class patterns as the primary styling vocabulary (see [web-semantic-styling skill](/.agents/skills/web-semantic-styling/SKILL.md)).
- Prefer daisyUI component classes for controls and containers; use Tailwind utilities for layout and spacing.
- Do not reintroduce legacy custom semantic token utility naming from the previous design-system layer.

## Sematic values

- Prefer semantic classes over raw arbitrary values: use `text-sm` instead of `text-[12px]`. If you need to define custom, probably you should not!

## Utilities

### Add new class

- use the `@utility` directive to define new utility classes.
