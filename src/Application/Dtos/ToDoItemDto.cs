namespace CleanArchStarter.Application.Dtos;

using CleanArchStarter.Domain.Entities;

// PRINCIPLE(dtos-at-the-boundary): what a query hands back is this flat copy in plain
// types, never the ToDoItem itself.
// Why: an entity carries behaviour (Complete()) and its shape follows the domain, so
// serializing it would change the JSON whenever the model is refactored. Title became a
// value object recently and this DTO absorbed that without its output changing.
// Here: FromDomain copies field by field into strings, so nothing domain-typed reaches the API.
public sealed record ToDoItemDto(Guid Id, string Title, string Priority, string Status, DateOnly? DueDate)
{
    public static ToDoItemDto FromDomain(ToDoItem toDoItem) =>
        new(
            toDoItem.Id.Value,
            toDoItem.Title.Value,
            toDoItem.Priority.Level.ToString(),
            toDoItem.Status.ToString(),
            toDoItem.DueDate);
}
