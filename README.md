# Clean Architecture Starter

A .NET 10 starter for hexagonal / clean architecture, built around a small ToDo example.

## Structure

```
src/
  Domain/          Entities, value objects, Result. No dependencies.
  Application/     Use cases, input ports, output ports. Depends on Domain.
  Infrastructure/  Output port implementations. Depends on Application.
  Api/             HTTP entry point and composition root. Depends on Application, Infrastructure.
```

Package versions are managed centrally in `src/Directory.Packages.props`.

## Run

```bash
dotnet build
dotnet run --project src/Api
```

## Not yet applicable in this project

- **Command Bus / Query Bus** — not implemented. `CreateToDoItemUseCase` is still
  called directly. This is also when the `handle` naming would need revisiting: a bus
  dispatches to a handler by its `Handle` method, and this project's
  `UseCase`/`Execute` naming has no equivalent until a bus exists.
- **Decorator pattern around the bus** for cross-cutting concerns like
  logging/metrics — depends on the bus existing first.
- **Domain Events** — no side effects yet that would need to be decoupled from the
  primary use case.
- **Sealed `Error` hierarchy / accumulating `ValidationErrors`** — this project's
  flat `Error` + `ErrorType` is the deliberately simpler alternative (simple until
  proven insufficient).
- **`Validation<T>`** (applicative, accumulate-every-error) vs. today's fail-fast
  `Result<T>` — the revisit trigger for the current choice of `Result<T>`.
- **Aggregates spanning multiple entities, Domain Services** — `ToDoItem` is a
  single entity; nothing to compose yet.
