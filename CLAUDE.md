# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

A .NET 9 solution scaffolded as a generic **Clean Architecture + DDD** skeleton. It has no fixed business domain yet — the `Samples` feature (a trivial `SampleItem` aggregate) exists only to prove the layers wire together end-to-end (Web API → MediatR → Domain → EF Core/SQLite) and to show the intended shape for real features. Replace/delete `Samples` once the actual domain is defined.

## Commands

Run from the repository root (where `VibeCoding.sln` lives):

- Build: `dotnet build`
- Run the API: `dotnet run --project src/VibeCoding.Api`
- Run all tests: `dotnet test`
- Run one test project: `dotnet test tests/VibeCoding.Domain.UnitTests`
- Run a single test: `dotnet test --filter "FullyQualifiedName~SampleItemTests.Complete_WhenAlreadyCompleted_Throws"`

The API uses SQLite via EF Core with `Database.EnsureCreatedAsync()` on startup (see `Program.cs`) — there are no EF Core migrations set up. If you introduce schema changes and want proper migrations, add the `dotnet-ef` tool and switch `Program.cs` from `EnsureCreatedAsync` to `MigrateAsync`.

## Architecture

Four `src/` projects, strictly layered (each only references the layer(s) inward of it):

```
VibeCoding.Api  →  VibeCoding.Infrastructure  →  VibeCoding.Application  →  VibeCoding.Domain
      (also: Api → Application directly)
```

- **`VibeCoding.Domain`** — no dependencies on any other project or framework. Contains:
  - `Common/`: the DDD base types every aggregate/entity/value object builds on — `Entity<TId>` (identity-based equality), `AggregateRoot<TId>` (adds a `DomainEvents` list via `RaiseDomainEvent`/`ClearDomainEvents`), `ValueObject` (structural equality via `GetEqualityComponents()`), `IDomainEvent`, `DomainException`.
  - Feature folders (currently just `Samples/`) hold aggregates, value objects, and repository interfaces (`I*Repository`) for that feature. Repository interfaces live in Domain; their implementations live in Infrastructure.
  - Entities expose a private parameterless constructor labeled `// Required by EF Core for materialization` — this is the accepted trade-off for mapping immutable-looking aggregates with EF Core without exposing public setters. Mutating members that must only be called by the aggregate root (e.g. a child entity's internal state transitions) are marked `internal`.

- **`VibeCoding.Application`** — depends only on Domain. Implements use cases with **MediatR**: each use case is a `Commands/<Name>/` or `Queries/<Name>/` folder containing an `IRequest`/`IRequest<TResponse>` record plus its `IRequestHandler`. DTOs returned to callers live in `Dtos/`. `Common/Interfaces` defines outward-facing abstractions implemented by Infrastructure (`IUnitOfWork`, `IDateTimeProvider`); `Common/Exceptions` defines `NotFoundException` (maps to 404) and the base for other handler-thrown exceptions. `DependencyInjection.cs` registers MediatR handlers from this assembly via `AddApplication()`.

- **`VibeCoding.Infrastructure`** — depends on Application (and transitively Domain). EF Core + SQLite implementation:
  - `Persistence/AppDbContext.cs` + `Persistence/Configurations/*` (`IEntityTypeConfiguration<T>`) — value objects are mapped either as owned types (`OwnsOne`, for multi-property VOs) or via `HasConversion` (for single-value VOs like `Title`). Child-entity collections backing a read-only navigation property use `.Navigation(...).HasField("_backingField").UsePropertyAccessMode(PropertyAccessMode.Field)` so EF can populate the private list directly.
  - `Persistence/Repositories/*` implement the Domain's `I*Repository` interfaces.
  - `Persistence/UnitOfWork.cs` implements `IUnitOfWork` by delegating to `AppDbContext.SaveChangesAsync`.
  - `Services/*` implement other Application interfaces (e.g. `SystemDateTimeProvider`).
  - `DependencyInjection.cs` wires the `DbContext` (reads `ConnectionStrings:Default` from configuration) and all repositories via `AddInfrastructure()`.

- **`VibeCoding.Api`** — ASP.NET Core minimal API (no controllers). `Program.cs` composes the app: `AddApplication()` + `AddInfrastructure()`, then maps feature endpoint groups from `Endpoints/*.cs` (each a `MapXEndpoints(this WebApplication app)` extension sending commands/queries through `ISender`). `Middleware/ExceptionHandlingMiddleware.cs` is a single inline middleware that maps `NotFoundException` → 404, `DomainException` → 400, anything else → 500, all as RFC 9110 `application/problem+json` via `Results.Problem`.

### Adding a new feature/aggregate

1. Domain: new feature folder with the aggregate root (extends `AggregateRoot<Guid>`), any value objects (extend `ValueObject`), and an `I<Aggregate>Repository` interface.
2. Application: `Commands/`/`Queries/` subfolders per use case (record + handler), a `Dtos/` folder for response shapes, referencing only Domain interfaces/types.
3. Infrastructure: an `IEntityTypeConfiguration<T>` per entity, add the `DbSet<T>` to `AppDbContext`, implement the repository interface, register both in `DependencyInjection.AddInfrastructure`.
4. Api: a `Map<Feature>Endpoints` extension in `Endpoints/`, called from `Program.cs`.
5. Tests: unit tests for aggregate invariants in `VibeCoding.Domain.UnitTests`, and for handlers (with mocked repository/`IUnitOfWork` via Moq) in `VibeCoding.Application.UnitTests`.

## Tests

Two xUnit projects under `tests/`, each referencing only the `src/` project it tests:

- **`VibeCoding.Domain.UnitTests`** — pure domain logic (entity/value-object equality, aggregate invariants/guard clauses). No mocking needed since Domain has no outward dependencies.
- **`VibeCoding.Application.UnitTests`** — command/query handlers with repository and `IUnitOfWork` dependencies mocked via **Moq**.

Both use **FluentAssertions** — pinned to **v7.1.0** (the last Apache-2.0-licensed release; v8+ requires a paid license for commercial use). Keep new test dependencies on v7.x unless that licensing changes.

## Solution layout

```
VibeCoding.sln
src/
  VibeCoding.Domain/
  VibeCoding.Application/
  VibeCoding.Infrastructure/
  VibeCoding.Api/
tests/
  VibeCoding.Domain.UnitTests/
  VibeCoding.Application.UnitTests/
```

Note: this directory is not yet a git repository.
