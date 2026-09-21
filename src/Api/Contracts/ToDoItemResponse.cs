namespace CleanArchStarter.Api.Contracts;

using CleanArchStarter.Application.Dtos;

// PRINCIPLE(dtos-at-the-boundary): the HTTP response is its own type, mapped from the
// application's DTO — the DTO is not the wire shape, and the domain ToDoItem never
// reaches this layer at all.
// Why: each model can then change for its own audience; a new field or a rename in one
// does not silently change the public contract of another.
// Here: FromDto is the only place that decides what the API returns.
public sealed record ToDoItemResponse(Guid Id, string Title, string Priority, string Status, DateOnly? DueDate)
{
    public static ToDoItemResponse FromDto(ToDoItemDto dto) =>
        new(dto.Id, dto.Title, dto.Priority, dto.Status, dto.DueDate);
}
