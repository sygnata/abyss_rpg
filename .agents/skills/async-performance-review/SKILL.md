---
name: async-performance-review
description: Review asynchronous C# and .NET code in AbyssRpg for blocking calls, incorrect async usage, missing CancellationToken propagation, unnecessary Task usage, sequential operations that may safely run concurrently, and other async performance or reliability issues. Use when reviewing handlers, repositories, services, controllers, background operations, or other asynchronous flows.
---

# Async Performance Review

Review asynchronous code in AbyssRpg for correctness, performance, resource usage, and cancellation propagation.

This skill is primarily a review skill.

Do not modify code unless explicitly requested.

## Scope

Inspect the relevant flow from entry point to external I/O.

Depending on the feature, trace:

Controller
→ Application
→ Domain when relevant
→ Repository abstraction
→ Infrastructure
→ Database or external service

Do not review a single method in isolation when the asynchronous flow continues into other layers.

## Look for blocking operations

Identify:

- `.Result`
- `.Wait()`
- `.GetAwaiter().GetResult()`
- synchronous database operations inside async flows
- synchronous file or network I/O when asynchronous APIs are available

Explain why blocking may reduce throughput or introduce deadlock risks depending on the execution environment.

## Review async methods

Check for:

- async methods without asynchronous work;
- unnecessary `async` / `await`;
- returning `Task` incorrectly;
- unnecessary `Task.FromResult`;
- unnecessary `Task.Run`;
- fire-and-forget operations;
- tasks that are created but never awaited;
- swallowed task exceptions.

Do not recommend removing `async` merely for stylistic reasons.

Only recommend changes that improve correctness, readability, or execution behavior.

## CancellationToken

Trace CancellationToken from the request boundary through the entire flow.

For ASP.NET Core operations, verify where appropriate:

Controller
→ Application
→ Repository
→ EF Core / external I/O

Identify:

- methods that should receive CancellationToken but do not;
- tokens that are accepted but not propagated;
- `CancellationToken.None` used unnecessarily;
- database calls that ignore an available token.

Do not introduce CancellationToken into pure synchronous Domain behavior without a reason.

## EF Core async operations

Check that asynchronous database paths use appropriate methods such as:

- `ToListAsync`
- `FirstOrDefaultAsync`
- `SingleOrDefaultAsync`
- `AnyAsync`
- `SaveChangesAsync`

when the existing architecture and operation require asynchronous I/O.

## Sequential versus parallel work

Look for independent I/O operations being executed sequentially.

Example:

```csharp
var character = await characterRepository.GetByIdAsync(characterId, cancellationToken);
var mission = await missionRepository.GetByIdAsync(missionId, cancellationToken);