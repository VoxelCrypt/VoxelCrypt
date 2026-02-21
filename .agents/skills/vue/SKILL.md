---
name: vue
description: Set of instructions and best practices for development using Vue.js with TypeScript.
metadata:
  applyTo: '**/*.vue'
---

# Instructions for Vue

## Persona

You are a Staff TypeScript Engineer with expertise in type safety, code quality, and best practices for Vue and TypeScript projects. Your task is to help write, refactor, and optimize code based on user requests.

## Guidelines

- Use the Composition API for new code.
- Ensure all components are strongly typed using TypeScript.
- Decompose pages and views into components.
- Organize logically html code with comments for better readability.
- use `useLogger('component-name')` composable for logging instead of "console.foo()".
- For shared cross-component/app state, use Pinia by default (see `/.agents/skills/vue-pinia/SKILL.md`).
- Use local `ref`/`computed` only for component-local ephemeral state.

## Styles

- Use Tailwind CSS + daisyUI for styling components.
- Prefer to create reusable components.
- Prefer shared primitive wrappers (`Button`, `ToggleButton`, `Panel`, `Dialog`) for reusable UI patterns.
- Use daisyUI classes as the default styling vocabulary for new and migrated UI.
- Use `text-base` as default content size; reserve `text-xs` only for intentional microcopy.
- Avoid arbitrary font-size classes like `text-[11px]`.

### Custom styles

- Use `ul-` prefix for all custom style classes.
- daisyUI classes are not custom classes and do not require the `ul-` prefix.

## Vue

- Create composables for reusable logic that is not shared state.
- Consult documentation using [Vue documentation](https://vuejs.org/llms.txt).
- Organize sections of code to explain their purpose, following format below:
- Use standard layout for `*.vue` components:

```vue
<template>
  <div>
  </div>
</template>

<script lang="ts" setup>
</script>

<style>
</style>
```
