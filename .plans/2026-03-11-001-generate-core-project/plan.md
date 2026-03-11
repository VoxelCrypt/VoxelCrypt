# Plan: Generate Core Project from Sample

## Task
Generate a new project based on `VoxelCrypt.Sample` named `VoxelCrypt.Core`, keeping existing sample classes unchanged for now.

## Scope
- Replicate source and test layers from sample into a new `projects/VoxelCrypt.Core` tree.
- Keep class names and behavior the same.
- Update project naming and references where needed for consistency.
- Ensure the solution includes the new projects.
- Validate build and tests.

## Steps
1. Inspect sample project files and namespaces.
2. Scaffold `projects/VoxelCrypt.Core` with `src/Kernel`, `src/Service`, `tests/Kernel.Tests`, and `tests/Service.Tests`.
3. Preserve sample classes unchanged.
4. Add new projects to solution file.
5. Run build and tests for the new projects.

## Related ADRs
- `0001-architecture-hexagonal-microservice-layering`
