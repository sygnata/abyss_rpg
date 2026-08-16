---
name: add-endpoint
description: Add an HTTP endpoint to an existing AbyssRpg ASP.NET Core controller. Use when adding GET, POST, PUT, PATCH, or DELETE behavior to a controller that already exists. Do not create an entirely new controller unless required.
---

# Add Endpoint

Add an endpoint to an existing AbyssRpg controller while respecting the project's architecture and existing conventions.

## Before implementation

1. Locate the target controller.
2. Read the entire controller.
3. Inspect related endpoints.
4. Identify:
   - route conventions;
   - dependency injection pattern;
   - request DTO conventions;
   - response DTO conventions;
   - Application layer dependency;
   - exception handling strategy.
5. Locate the related Application flow before changing the controller.

## Determine endpoint semantics

Identify:

- HTTP method;
- route;
- input;
- output;
- expected success status;
- expected failure cases.

Do not invent requirements that were not requested.

## Implementation rules

The endpoint must:

1. accept HTTP input;
2. delegate to Application;
3. pass `CancellationToken` when asynchronous;
4. return the appropriate HTTP result.

Do not:

- query DbContext directly;
- call EF Core directly;
- implement domain rules;
- perform repository logic;
- duplicate validation already handled elsewhere.

## Status codes

Use project conventions first.

Typical defaults:

- GET -> `200 OK`;
- POST create -> `201 Created`;
- PUT/PATCH -> `200 OK` or `204 No Content`;
- DELETE -> `204 No Content`;
- missing resource -> `404 Not Found`;
- state conflict -> `409 Conflict`;
- invalid request -> `400 Bad Request`.

## Created resources

If the endpoint creates a new resource and a corresponding GET-by-id endpoint exists, use `CreatedAtAction` when appropriate.

## Verification

After changing the endpoint:

1. inspect the complete request flow;
2. run `dotnet build`;
3. run relevant tests when available;
4. verify that no business logic was introduced into the controller;
5. list modified files;
6. explain the resulting flow.