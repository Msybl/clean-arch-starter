namespace CleanArchStarter.Domain.Entities;

using CleanArchStarter.Domain.Common;
using CleanArchStarter.Domain.ValueObjects;

public sealed class ToDoItem
{
    public ToDoItemId Id { get; }
    public Title Title { get; }
    public Status Status { get; }
    public Priority Priority { get; }
    public DateOnly? DueDate { get; }

    // PRINCIPLE(always-valid-domain-model): private ctor + ValidateThenCreate below
    // is the only way in — a ToDoItem breaking its own rules can never exist.
    private ToDoItem(ToDoItemId id, Title title, Status status, Priority priority, DateOnly? dueDate)
    {
        Id = id;
        Title = title;
        Status = status;
        Priority = priority;
        DueDate = dueDate;
    }

    // PRINCIPLE(result-over-exceptions): failure is a returned value, not a throw.
    // PRINCIPLE(rich-domain-model): the id, title and priority arrive already valid; the
    // rule left here spans two fields (priority and due date), so neither value object
    // could own it — which is why this factory still returns a Result.
    public static Result<ToDoItem> ValidateThenCreate(ToDoItemId id, Title title, Priority priority, DateOnly? dueDate)
    {
        if (priority.Level is PriorityLevel.Critical or PriorityLevel.Blocker && dueDate is null)
        {
            return Result<ToDoItem>.Failure(
                new Error("ToDoItem.DueDateRequired", "A Critical or Blocker item must have a due date."));
        }

        return Result<ToDoItem>.Success(new ToDoItem(id, title, Status.ToDo, priority, dueDate));
    }

    // PRINCIPLE(rich-domain-model): a business rule lives on the object that owns the state.
    // Why: rules kept in use cases get copied around and drift apart.
    // Here: only ToDoItem decides whether an item can be completed; the use case just asks.
    // Returns a new instance rather than changing this one: the in-memory repository hands
    // out the stored reference, so mutating would alter it under concurrent readers and make
    // Update a no-op, hiding a forgotten Update until a real database arrives.
    public Result<ToDoItem> Complete()
    {
        if (Status is Status.Archived)
        {
            return Result<ToDoItem>.Failure(
                new Error("ToDoItem.Archived", "An archived item cannot be completed.", ErrorType.Conflict));
        }

        return Result<ToDoItem>.Success(new ToDoItem(Id, Title, Status.Done, Priority, DueDate));
    }
}
