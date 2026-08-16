---
name: root-cause-analysis
description: Investigate bugs, exceptions, unexpected behavior, build failures, test failures, and data inconsistencies in AbyssRpg by reproducing and tracing the problem before proposing a fix. Use when the cause of a problem is unknown or when previous fixes only addressed symptoms.
---

# Root Cause Analysis

Investigate a problem in AbyssRpg systematically before modifying code.

Do not start by changing code.

The primary objective is to determine why the problem occurs.

## Phase 1 - Define the symptom

Clearly identify:

- expected behavior;
- actual behavior;
- error message if available;
- affected feature;
- reproducibility.

Separate observed facts from assumptions.

## Phase 2 - Reproduce

Attempt to reproduce the problem using the smallest reliable scenario.

Depending on the issue, use:

- existing tests;
- targeted test execution;
- `dotnet build`;
- application execution;
- HTTP request;
- database inspection when available.

Do not modify implementation merely to make reproduction easier unless necessary.

## Phase 3 - Locate the execution path

Trace the problem through the relevant layers.

For API flows:

Controller
→ Application
→ Domain
→ Repository
→ Infrastructure
→ Database

Identify where expected behavior first diverges from actual behavior.

## Phase 4 - Gather evidence

Inspect:

- source code;
- call sites;
- exception stack;
- logs when available;
- configuration;
- entity mappings;
- migrations;
- tests;
- relevant Git diff when the bug is recent.

Do not assume the first suspicious line is the root cause.

## Phase 5 - Form hypotheses

Generate a small set of plausible causes.

For each hypothesis identify:

- evidence supporting it;
- evidence contradicting it;
- how it can be verified.

Prefer verification over speculation.

## Phase 6 - Identify the root cause

State:

### Symptom

What the user observes.

### Immediate cause

What directly triggers the incorrect behavior.

### Root cause

The underlying design, logic, state, configuration, or persistence condition that allows the problem to occur.

Example:

Symptom:
Character creation returns HTTP 500.

Immediate cause:
Database unique constraint throws an exception.

Root cause:
Application does not perform the expected duplicate-name conflict handling before persistence.

## Phase 7 - Determine blast radius

Check whether the same underlying problem may affect:

- other endpoints;
- other entities;
- other handlers;
- other repositories;
- existing persisted data.

Do not assume the bug is isolated to the reported endpoint.

## Phase 8 - Proposed fix

Only after identifying the root cause:

1. propose the smallest correct fix;
2. explain where the fix belongs architecturally;
3. identify possible side effects;
4. identify tests required to prevent regression.

Do not modify code unless explicitly requested.

## Phase 9 - Regression protection

When implementation is requested:

1. create or identify a test that reproduces the bug;
2. confirm that it fails before the fix when practical;
3. implement the minimal correction;
4. confirm the regression test passes;
5. run related tests;
6. run `dotnet build`.

Never disable a failing test simply to make the suite pass.

## Investigation report

Return:

### Problem

Short description.

### Reproduction

How the issue can be reproduced.

### Root cause

The underlying cause.

### Evidence

Files, methods, queries, tests, or logs supporting the conclusion.

### Impact

Affected behavior and possible blast radius.

### Recommended fix

Smallest correct solution.

### Regression test

Test that should protect against recurrence.

### Confidence

Use:

- High
- Medium
- Low

If confidence is not high, explain what additional evidence would be necessary.