---
name: create-use-case
description: Create or extend an AbyssRpg Application-layer use case for a command or query. Use when implementing application orchestration for an API operation without placing business rules in controllers or persistence logic in Application.
---

# Create Use Case

Implement an AbyssRpg Application-layer use case following the architecture already present in the repository.

## Before implementation

1. Inspect similar use cases.
2. Identify the naming and folder conventions.
3. Identify how dependencies are injected.
4. Identify how results and DTOs are represented.
5. Identify how exceptions and validation are currently handled.

Follow existing conventions.

## Application responsibilities

Application may:

- coordinate the use case;
- load domain objects through abstractions;
- call domain behavior;
- persist changes through abstractions;
- create application DTOs/results;
- coordinate multiple operations.

Application must not:

- contain HTTP-specific behavior;
- return ASP.NET Core IActionResult types;
- depend directly on EF Core;
- depend on DbContext;
- reproduce domain invariants.

## Workflow

1. Define the use-case input.
2. Define the expected result.
3. Identify required repository abstractions.
4. Load required aggregates.
5. Handle missing resources according to project conventions.
6. Invoke Domain behavior for business decisions.
7. Persist state when necessary.
8. Return the appropriate Application result.

## Async

Use:

- async/await;
- CancellationToken;
- asynchronous repository operations.

Do not block asynchronous operations with `.Result` or `.Wait()`.

## Verification

1. Check dependency direction.
2. Ensure business rules remain in Domain where appropriate.
3. Run `dotnet build`.
4. Run relevant tests.
5. List files modified.
6. Explain the Application flow.