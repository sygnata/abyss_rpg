---
name: document-endpoint
description: Generate technical documentation for an existing AbyssRpg HTTP endpoint based on its real implementation. Use when documenting API routes, requests, responses, validation rules, status codes, business behavior, and examples. Do not invent behavior that cannot be confirmed from the code.
---

# Document Endpoint

Generate accurate technical documentation for an AbyssRpg endpoint based on the implementation currently present in the repository.

Do not change application code.

## Source of truth

Inspect the actual implementation.

At minimum locate:

- Controller;
- action;
- request model;
- response model;
- Application use case;
- relevant validation;
- important Domain rules;
- relevant exception handling.

Do not document behavior purely from the endpoint name.

## Endpoint identification

Document:

- HTTP method;
- route;
- purpose;
- authentication requirement when discoverable;
- authorization requirement when discoverable.

## Request

Document:

- route parameters;
- query parameters;
- headers when relevant;
- request body;
- field types;
- required versus optional fields.

## Validation

Document confirmed validation rules.

Examples:

- required;
- minimum/maximum length;
- valid numeric ranges;
- uniqueness;
- allowed enum values;
- domain restrictions.

Distinguish:

HTTP/input validation

from:

business/domain rules.

## Successful response

Document:

- status code;
- response body;
- field descriptions.

Provide a realistic JSON example when appropriate.

Do not expose fields that are not actually returned by the endpoint.

## Error responses

Document confirmed failures.

Examples:

- `400 Bad Request`;
- `401 Unauthorized`;
- `403 Forbidden`;
- `404 Not Found`;
- `409 Conflict`.

For each response explain the condition that produces it.

Do not claim that an error status exists unless it can be inferred from the implementation or global API error handling.

## Created resources

For `201 Created`, document the Location header when the endpoint uses `CreatedAtAction` or equivalent behavior.

Explain that the created representation and Location header are separate parts of the HTTP response.

## Behavior

Briefly explain the functional flow:

HTTP
→ Controller
→ Application
→ Domain
→ Persistence

Do not expose unnecessary internal implementation details in consumer-facing documentation.

## Example request

Provide an example using curl.

For example:

```bash
curl -X POST http://localhost:5000/api/characters \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Aldren"
  }'