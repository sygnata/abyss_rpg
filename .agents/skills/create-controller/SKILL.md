---
name: create-controller
description: Create a new ASP.NET Core API controller in AbyssRpg following the project's existing conventions. Use when a new resource requires its own controller. Do not use only to add an endpoint to an existing controller.
---

# Create Controller

Create a new controller in the AbyssRpg API following the conventions already present in the repository.

## Before changing code

1. Inspect existing controllers in `AbyssRpg.Api`.
2. Identify the project's current conventions for:
   - routes
   - controller naming
   - constructor injection
   - HTTP response types
   - request and response DTOs
   - exception handling
   - CancellationToken usage
3. Reuse existing patterns instead of introducing a new architectural style.
4. Do not modify code until the existing pattern has been understood.

## Controller responsibilities

The controller must remain thin.

The controller may:

- receive HTTP input;
- validate HTTP-specific information when necessary;
- delegate execution to the Application layer;
- translate the application result into an HTTP response.

The controller must not:

- contain business rules;
- access EF Core directly;
- access DbContext directly;
- contain persistence logic;
- reproduce domain validation;
- instantiate repositories;
- perform complex mapping that belongs elsewhere.

## Creation workflow

1. Determine the resource represented by the controller.
2. Determine the base route based on existing API conventions.
3. Create `<Resource>Controller`.
4. Apply the attributes used by existing controllers.
5. Inject only dependencies necessary for the controller.
6. Add only endpoints requested by the task.
7. Use appropriate HTTP status codes.
8. Use `CancellationToken` for asynchronous operations.
9. Follow existing request and response DTO conventions.
10. Ensure dependencies point toward the Application layer rather than Infrastructure.

## HTTP conventions

Prefer:

- `200 OK` for successful reads;
- `201 Created` for resource creation;
- `204 No Content` for successful commands that return no body;
- `400 Bad Request` for invalid HTTP input;
- `404 Not Found` when the requested resource does not exist;
- `409 Conflict` when an operation conflicts with the current state of a resource.

When creating a resource and a GET-by-id endpoint exists, prefer `CreatedAtAction` when consistent with the current project.

## Validation

Do not duplicate domain or application validation inside the controller.

If validation belongs to the domain, keep it in Domain.

If validation belongs to a use case, keep it in Application.

## Verification

After implementation:

1. Run `dotnet build`.
2. Report compilation errors if any.
3. Review the resulting controller for business logic leakage.
4. List all files created or modified.
5. Briefly explain each change.