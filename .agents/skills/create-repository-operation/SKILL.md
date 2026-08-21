---
name: create-repository-operation
description: Add a persistence operation to an existing AbyssRpg repository abstraction and Infrastructure implementation. Use when a use case needs to read, check, insert, update, or delete persisted data.
---

# Create Repository Operation

Implement the minimum persistence operation required by an AbyssRpg use case.

## Before implementation

1. Find the repository abstraction.
2. Find its Infrastructure implementation.
3. Inspect similar methods.
4. Inspect entity configuration when relevant.

## Repository abstraction

Expose intent-oriented operations.

Prefer:

- `GetByIdAsync`;
- `ExistsByNameAsync`;
- `AddAsync`;

over abstractions that unnecessarily expose EF Core concepts.

Do not expose:

- DbSet;
- IQueryable unless the current architecture intentionally uses it;
- DbContext;
- EF-specific types to Application or Domain.

## Infrastructure

Use EF Core according to the query's purpose.

For read-only queries, consider `AsNoTracking()` when appropriate.

Avoid:

- unnecessary Includes;
- materializing full collections for existence checks;
- synchronous database calls;
- unnecessary SaveChanges calls.

Use CancellationToken.

## Query efficiency

For existence checks, prefer database-side existence operations.

For DTO queries, consider projection when consistent with the architecture.

## Verification

1. verify interface and implementation signatures;
2. verify async behavior;
3. inspect generated SQL conceptually for obvious inefficiencies;
4. run `dotnet build`;
5. run relevant tests;
6. explain why the repository operation belongs at this layer.