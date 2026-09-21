namespace CleanArchStarter.Api.Contracts;

// PRINCIPLE(dtos-at-the-boundary): the HTTP request is its own type, in plain types.
// Why: the wire contract should not change just because a domain type does, and raw input
// must never be mistaken for something already validated.
// Here: everything is raw text (the priority is a level's name, such as "Major"); the
// command and the value objects turn it into something valid, or refuse it.
public sealed record CreateToDoItemRequest(string Id, string Title, string Priority, DateOnly? DueDate = null);
