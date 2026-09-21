using CleanArchStarter.Domain.Common;

namespace CleanArchStarter.Application.InputPorts.Commands;

public sealed record CreateToDoItemCommand
{
    public Guid Id { get; }
    public string Title { get; }
    public string Priority { get; }
    public DateOnly? DueDate { get; }

    private CreateToDoItemCommand(Guid id, string title, string priority, DateOnly? dueDate) =>
        (Id, Title, Priority, DueDate) = (id, title, priority, dueDate);

    // PRINCIPLE(single-source-of-truth-for-invariants): the command only checks what can be
    // seen from the value alone — that the id parses as a Guid. Title and priority pass
    // through as raw text; their rules live in Title and Priority, so they are not written
    // a second time here.
    public static Result<CreateToDoItemCommand> ValidateThenCreate(string id, string title, string priority, DateOnly? dueDate)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            return Result<CreateToDoItemCommand>.Failure(new Error("CreateToDoItemCommand.Id", "Invalid format — not a Guid at all"));
        }

        return Result<CreateToDoItemCommand>.Success(new CreateToDoItemCommand(guid, title, priority, dueDate));
    }
}
