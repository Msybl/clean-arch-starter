namespace CleanArchStarter.Application.InputPorts.Commands;

using CleanArchStarter.Domain.Common;

public sealed record CompleteToDoItemCommand
{
    public Guid Id { get; }

    private CompleteToDoItemCommand(Guid id) => Id = id;

    public static Result<CompleteToDoItemCommand> ValidateThenCreate(string input)
    {
        if(!Guid.TryParse(input, out var id))
        {
            return Result<CompleteToDoItemCommand>.Failure(new Error("CompleteToDoItemCommand.Id", "Invalid format — not a Guid at all"));
        }

        return Result<CompleteToDoItemCommand>.Success(new CompleteToDoItemCommand(id));
    }
}
