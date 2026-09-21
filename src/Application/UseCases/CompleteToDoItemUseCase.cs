namespace CleanArchStarter.Application.UseCases;

using CleanArchStarter.Application.InputPorts;
using CleanArchStarter.Application.InputPorts.Commands;
using CleanArchStarter.Application.OutputPorts;
using CleanArchStarter.Domain.Common;
using CleanArchStarter.Domain.ValueObjects;

public sealed class CompleteToDoItemUseCase(IToDoItemRepository repository) : ICompleteToDoItemUseCase
{
    // PRINCIPLE(use-case-orchestrates-only): loads, asks the domain to complete, saves —
    // whether an item *may* be completed is decided by ToDoItem.Complete, not here.
    public Result Execute(CompleteToDoItemCommand command)
    {
        var idResult = ToDoItemId.ValidateThenCreate(command.Id);
        if (idResult.IsFailure)
        {
            return Result.Failure(idResult.Error!);
        }

        var getResult = repository.GetById(idResult.Value);
        if (getResult.IsFailure)
        {
            return Result.Failure(getResult.Error!);
        }

        var completeResult = getResult.Value.Complete();
        if (completeResult.IsFailure)
        {
            return Result.Failure(completeResult.Error!);
        }

        return repository.Update(completeResult.Value);
    }
}
