---
name: complete-api-feature
description: Implement an AbyssRpg API feature end-to-end across Domain, Application, Infrastructure, API, tests, and EF migration when required. Use for complete feature requests rather than isolated layer changes.
---

# Complete API Feature

Implement an AbyssRpg feature end-to-end while preserving the existing architecture.

## Phase 1 - Understand

Before modifying code:

1. inspect the repository structure;
2. locate similar features;
3. trace the existing architecture;
4. identify the files and layers likely to change;
5. identify business rules and persistence requirements.

Do not create duplicate abstractions when equivalent ones already exist.

## Phase 2 - Domain

If new business behavior is required:

1. place invariants in Domain;
2. add entity or aggregate behavior;
3. create Value Objects only when justified;
4. keep Domain independent from infrastructure.

Follow the principles from the `create-domain-behavior` workflow.

## Phase 3 - Application

Create or extend the required use case.

Application should orchestrate the operation without absorbing domain or persistence responsibilities.

Follow the principles from `create-use-case`.

## Phase 4 - Persistence

Add only the repository operations required by the use case.

Follow the principles from `create-repository-operation`.

If persistence mappings change, update the EntityTypeConfiguration.

## Phase 5 - API

If a controller already exists, add the endpoint.

If no suitable controller exists, create one.

Keep controllers thin.

Follow the principles from `add-endpoint` and `create-controller`.

## Phase 6 - Tests

Create or update appropriate tests.

Cover:

- successful behavior;
- important failure cases;
- relevant domain rules.

Follow the principles from `create-tests-for-endpoint`.

## Phase 7 - Database migration

If the persistence model changed:

1. create an EF Core migration;
2. review `Up` and `Down`;
3. do not apply the migration to the database unless explicitly requested.

Follow the principles from `create-ef-migration`.

## Phase 8 - Verify

Run:

1. `dotnet build`;
2. relevant tests;
3. broader tests when appropriate.

Resolve implementation-related failures.

## Phase 9 - Review

Review the complete flow:

API
→ Application
→ Domain
→ Infrastructure

Check for:

- architecture violations;
- duplicated rules;
- incorrect status codes;
- missing CancellationToken;
- inefficient EF Core queries;
- missing constraints;
- missing tests.

## Final report

Provide:

1. summary of the implementation;
2. files created;
3. files modified;
4. important architectural decisions;
5. build result;
6. test result;
7. migration created, if any;
8. recommended branch name;
9. recommended Conventional Commit message.