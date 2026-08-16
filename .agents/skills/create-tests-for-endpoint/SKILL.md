---
name: create-tests-for-endpoint
description: Create or extend tests for an AbyssRpg API endpoint and its underlying use case. Use after implementing or changing endpoint behavior.
---

# Create Tests For Endpoint

Create meaningful tests for an AbyssRpg API feature.

## Before implementation

1. Inspect the existing test projects.
2. Follow existing testing frameworks and conventions.
3. Reuse fixtures, builders, factories, and helpers already present.

Do not introduce a new testing library unless required.

## Test scenarios

Identify at least:

1. happy path;
2. invalid input;
3. resource not found;
4. relevant business-rule violation;
5. persistence-related behavior when applicable.

Do not create meaningless tests solely to increase coverage.

## Unit tests

When testing Application:

- isolate external dependencies when consistent with existing tests;
- verify behavior and results;
- avoid testing implementation details.

When testing Domain:

- test invariants directly;
- test state transitions;
- test rejected invalid operations.

## API integration tests

When integration infrastructure exists, verify:

- route;
- HTTP method;
- status code;
- request serialization;
- response serialization;
- important error responses.

## Verification

1. run relevant tests;
2. report failures;
3. fix failures caused by the implementation;
4. do not hide or disable failing tests;
5. summarize covered scenarios.