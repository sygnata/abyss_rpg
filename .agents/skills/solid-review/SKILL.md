---
name: solid-review
description: Review AbyssRpg C# code for meaningful SOLID design issues, excessive responsibilities, inappropriate coupling, poor abstractions, and unnecessary complexity. Use when reviewing classes, features, services, handlers, repositories, or architecture. Avoid recommending abstractions solely for theoretical purity.
---

# SOLID Review

Review AbyssRpg code using SOLID principles pragmatically.

The objective is maintainability and clear responsibility, not maximum abstraction.

Do not modify code unless explicitly requested.

## General rule

Do not report a SOLID violation merely because a theoretical alternative exists.

A finding should have a concrete maintenance, testing, coupling, extensibility, or clarity consequence.

Avoid overengineering.

## S - Single Responsibility Principle

Determine whether a class has multiple reasons to change.

Look for classes combining concerns such as:

- HTTP handling + business logic;
- business logic + persistence;
- persistence + mapping + orchestration;
- validation + infrastructure;
- unrelated use cases.

Do not treat method count as a measure of responsibility.

A class with several methods may still have one cohesive responsibility.

## O - Open/Closed Principle

Look for code that requires repeated modification whenever a legitimate variation is added.

Examples may include:

- large type-based switch statements;
- repeated conditionals for behavior variants;
- hard-coded strategies.

Do not introduce polymorphism when the variation is hypothetical.

Prefer simple code until actual variation justifies abstraction.

## L - Liskov Substitution Principle

Inspect inheritance and interface implementations.

Check whether implementations:

- violate expected contracts;
- throw unsupported exceptions for normal interface methods;
- require callers to know concrete implementation details;
- behave inconsistently with the abstraction.

Avoid inventing inheritance hierarchies to satisfy LSP analysis.

## I - Interface Segregation Principle

Look for interfaces that force implementations or callers to depend on operations they do not need.

Check for:

- large generic service interfaces;
- repository interfaces with unrelated operations;
- implementations throwing `NotSupportedException`.

Do not split small cohesive interfaces unnecessarily.

## D - Dependency Inversion Principle

Review dependency direction.

For AbyssRpg, pay particular attention to:

API
→ Application
→ Domain

and Infrastructure implementing abstractions required by inner layers.

Look for:

- Application depending directly on Infrastructure;
- Domain depending on EF Core;
- Domain depending on ASP.NET Core;
- Application depending on DbContext;
- controllers constructing concrete repositories.

## Other design smells

Also report when relevant:

- feature envy;
- anemic domain model;
- god classes;
- inappropriate static dependencies;
- primitive obsession;
- unnecessary generic abstractions;
- duplicated orchestration;
- abstraction layers that provide no value.

Do not label every smell as a SOLID violation.

## Findings

Classify:

### Strong violation

Clear architectural or maintainability problem.

### Moderate concern

Likely to become problematic as the feature evolves.

### Optional improvement

Reasonable refinement but not currently necessary.

For every finding include:

- SOLID principle when applicable;
- file;
- class or method;
- current responsibility;
- problem;
- concrete consequence;
- recommended design;
- complexity cost of the recommendation.

## Avoid overengineering

Explicitly call out when a proposed abstraction would make the code worse.

It is valid to conclude:

"No SOLID-related refactoring is currently justified."

## Verification after refactoring

If modifications are requested:

1. preserve behavior;
2. make changes incrementally;
3. run relevant tests;
4. run `dotnet build`;
5. compare complexity before and after;
6. report whether the refactor actually improved cohesion or coupling.