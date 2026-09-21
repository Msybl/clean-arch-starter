namespace CleanArchStarter.Application.InputPorts.Queries;

using CleanArchStarter.Domain.Common;

public sealed record GetToDoItemByIdQuery
{
    public Guid Id { get; }

    private GetToDoItemByIdQuery(Guid id) => Id = id;

    public static Result<GetToDoItemByIdQuery> ValidateThenCreate(string input)
    {
        if(!Guid.TryParse(input, out var id))
        {
            return Result<GetToDoItemByIdQuery>.Failure(new Error("GetToDoItemByIdQuery.Id", "Invalid format — not a Guid at all"));
        }
        
        return Result<GetToDoItemByIdQuery>.Success(new GetToDoItemByIdQuery(id));
    }
}