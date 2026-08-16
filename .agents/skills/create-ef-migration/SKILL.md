---
name: create-ef-migration
description: Create and review an Entity Framework Core migration for AbyssRpg. Use when entity mappings, columns, constraints, relationships, indexes, or other persisted schema elements change.
---

# Create EF Core Migration

Create an EF Core migration safely for AbyssRpg.

## Before migration

1. Inspect the entity changes.
2. Inspect EntityTypeConfiguration changes.
3. Confirm that a schema change is actually required.
4. Identify the Infrastructure project and API startup project from the repository.

Do not guess project paths when they can be discovered.

## Migration

1. Choose a meaningful migration name.
2. Run the appropriate `dotnet ef migrations add` command.
3. Inspect the generated migration.
4. Inspect both `Up` and `Down`.
5. Inspect the updated model snapshot.

## Review

Verify:

- columns;
- nullability;
- lengths;
- foreign keys;
- delete behavior;
- indexes;
- unique constraints;
- defaults.

Pay special attention to potentially destructive operations.

## Safety rule

Do NOT run `dotnet ef database update` unless the user explicitly asks to apply the migration.

Creating a migration and applying a migration are separate operations.

## Verification

1. run `dotnet build`;
2. report the migration name;
3. list generated files;
4. explain the schema change;
5. explicitly state whether the database was modified.