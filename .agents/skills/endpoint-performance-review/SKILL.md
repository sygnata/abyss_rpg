---
name: endpoint-performance-review
description: Review an AbyssRpg API endpoint end-to-end for performance and scalability issues across Controller, Application, Domain, repositories, EF Core, and database access. Use when investigating slow endpoints, reviewing new API flows, or checking whether an endpoint will scale as data volume increases.
---

# Endpoint Performance Review

Review an AbyssRpg HTTP endpoint end-to-end for potential performance and scalability problems.

This is primarily a diagnostic skill.

Do not change code unless explicitly requested.

## Step 1 - Identify the endpoint

Determine:

- controller;
- action;
- HTTP method;
- route;
- Application use case;
- repositories involved;
- database queries involved.

Trace the complete execution path.

## Step 2 - Controller

Check for:

- unnecessary work before calling Application;
- large request transformations;
- synchronous I/O;
- unnecessary serialization complexity;
- excessive response payloads.

Controllers should remain thin.

## Step 3 - Application

Check for:

- repeated repository calls;
- loading the same entity multiple times;
- sequential independent I/O;
- loops that trigger database calls;
- unnecessary transformations;
- unnecessary materialization of collections.

## Step 4 - Domain

Do not optimize Domain behavior merely because it contains loops.

Only flag Domain operations when complexity could meaningfully increase with realistic collection sizes.

Preserve domain clarity over micro-optimizations.

## Step 5 - Repository and EF Core

Look for:

- N+1 query patterns;
- unnecessary `Include`;
- missing projection;
- loading entire entities when only a few fields are needed;
- premature `ToList`;
- loading full tables;
- filtering in memory instead of the database;
- unnecessary tracking;
- repeated queries;
- expensive navigation loading;
- query execution inside loops;
- multiple `SaveChangesAsync` calls in one logical operation.

For read-only queries, evaluate whether `AsNoTracking()` is appropriate.

## Step 6 - Pagination

For endpoints returning collections, determine whether results can grow without bound.

If so, evaluate:

- pagination;
- maximum page size;
- ordering;
- stable pagination criteria.

Flag APIs that could eventually return thousands of records without limits.

## Step 7 - Database indexes

Based on query predicates and ordering, identify potential indexes.

Pay attention to:

- IDs other than primary key;
- names used for lookup;
- foreign keys;
- compound filters;
- uniqueness checks;
- sorting columns.

Do not recommend indexes automatically for every filtered property.

Explain the query pattern that motivates the index.

## Step 8 - Query count

Estimate conceptually how many database round trips the endpoint performs.

Example:

Request
→ Get Character
→ Check Mission
→ Load Discipline
→ Update Character
→ Save

Highlight avoidable round trips.

Do not combine queries purely to reduce query count if doing so damages clarity or correctness.

## Step 9 - Response size

Review DTOs for:

- unnecessary nested objects;
- large collections;
- entity graphs accidentally serialized;
- internal properties exposed unnecessarily.

## Step 10 - Async behavior

Apply relevant principles from `async-performance-review`.

Pay particular attention to:

- blocking I/O;
- sequential independent I/O;
- CancellationToken propagation.

## Findings

Classify findings as:

### High impact

Likely to become a serious performance or scalability issue.

### Medium impact

Worth correcting as usage grows.

### Low impact

Potential optimization with limited expected benefit.

For each finding include:

- layer;
- file;
- method;
- issue;
- expected impact;
- proposed optimization;
- tradeoff.

## Important

Do not recommend premature optimization.

If the implementation is already appropriate for the expected scale, explicitly state that no change is currently justified.

## Verification

If code changes are requested:

1. modify the smallest necessary scope;
2. preserve behavior;
3. run `dotnet build`;
4. run relevant tests;
5. summarize performance-related changes.