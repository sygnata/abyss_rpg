---
name: create-domain-behavior
description: Add or modify business behavior in the AbyssRpg Domain layer. Use when a feature introduces domain rules, invariants, state transitions, entity behavior, or value objects.
---

# Create Domain Behavior

Implement business behavior inside the AbyssRpg Domain layer.

## Principles

Prefer rich domain behavior over external mutation of entity state.

Domain entities should protect their own invariants.

Avoid anemic-domain implementations where Application directly manipulates entity state.

## Before implementation

1. Locate the aggregate or entity responsible for the behavior.
2. Inspect its existing methods and invariants.
3. Identify whether the requested rule belongs to:
   - Entity;
   - Aggregate;
   - Value Object;
   - Domain Service.
4. Prefer the smallest appropriate domain abstraction.

## Implementation

Business behavior should:

- have meaningful method names;
- protect invariants;
- reject invalid state transitions;
- avoid exposing setters solely for external mutation;
- remain independent of infrastructure.

Domain must not depend on:

- ASP.NET Core;
- EF Core;
- repositories;
- DbContext;
- HTTP;
- Infrastructure;
- API.

## Errors

Follow existing Domain exception/result conventions.

Do not introduce a new error strategy without necessity.

## Verification

After implementation:

1. inspect all call sites;
2. make sure Application invokes the behavior instead of duplicating it;
3. run domain/unit tests;
4. run `dotnet build`;
5. explain the invariant being protected.