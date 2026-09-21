namespace CleanArchStarter.Domain.ValueObjects;

using CleanArchStarter.Domain.Common;

// PRINCIPLE(value-objects): an id is its own type, not a bare Guid.
// Why: a raw Guid can be anything, including the empty one, and every caller would have to
// remember to check it.
// Here: an empty Guid can never become a ToDoItemId, so whatever holds one has a usable id.
public sealed record ToDoItemId
{
    public Guid Value { get; }

    private ToDoItemId(Guid value) => Value = value;

    public static Result<ToDoItemId> ValidateThenCreate(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Result<ToDoItemId>.Failure(new Error("ToDoItemId.Empty", "Id must not be an empty Guid."));
        }

        return Result<ToDoItemId>.Success(new ToDoItemId(value));
    }
}
