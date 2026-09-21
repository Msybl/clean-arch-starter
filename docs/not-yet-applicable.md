# Not yet applicable in this project

Principles from hexagonal / clean architecture that this project hasn't reached the
point of needing yet — no code site exists to comment on.

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
- **Multi-module build-boundary enforcement** (an ArchUnit/NetArchTest-equivalent) —
  a deliberate divergence, not yet implemented.
