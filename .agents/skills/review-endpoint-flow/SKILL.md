---
name: review-endpoint-flow
description: Review an AbyssRpg API feature end-to-end from HTTP controller through Application, Domain, and Infrastructure. Use after implementing or changing an endpoint, or before committing a feature.
---

# Review Endpoint Flow

Perform an end-to-end review of an AbyssRpg API feature.

Do not modify code unless explicitly requested.

## Trace the flow

Follow:

Controller
→ Application
→ Domain
→ Repository abstraction
→ Infrastructure
→ Database

Identify every relevant file.

## Controller review

Check:

- route;
- HTTP method;
- status codes;
- thin-controller principle;
- CancellationToken;
- DTO boundaries.

## Application review

Check:

- orchestration;
- dependency direction;
- missing-resource behavior;
- duplicated business rules;
- asynchronous flow.

## Domain review

Check:

- invariants;
- entity behavior;
- state transitions;
- anemic-domain symptoms;
- inappropriate infrastructure dependencies.

## Infrastructure review

Check:

- EF Core query efficiency;
- tracking;
- indexes;
- constraints;
- relationship mappings;
- persistence correctness.

## Tests

Check whether important scenarios are covered.

## Findings

Classify findings as:

- Critical
- Important
- Improvement

For every finding include:

- affected file;
- issue;
- why it matters;
- suggested correction.

If no relevant issues are found, say so explicitly.