namespace CleanArchStarter.Application.UseCases;

using CleanArchStarter.Application.InputPorts;
using CleanArchStarter.Application.InputPorts.Commands;
using CleanArchStarter.Application.OutputPorts;
using CleanArchStarter.Domain.Common;
using CleanArchStarter.Domain.Entities;
using CleanArchStarter.Domain.ValueObjects;

public sealed class CreateToDoItemUseCase(IToDoItemRepository toDoItemRepository) : ICreateToDoItemUseCase
{
    // PRINCIPLE(use-case-orchestrates-only): turns the command's raw values into value
    // objects, calls the domain, calls the repository — no business rule is decided in
    // this method itself; each value object and the entity own theirs.
    public Result Execute(CreateToDoItemCommand command)
    {
        var idResult = ToDoItemId.ValidateThenCreate(command.Id);
        if (idResult.IsFailure)
        {
            return Result.Failure(idResult.Error!);
        }

        var titleResult = Title.ValidateThenCreate(command.Title);
        if (titleResult.IsFailure)
        {
            return Result.Failure(titleResult.Error!);
        }

        var priorityResult = Priority.ValidateThenCreate(command.Priority);
        if (priorityResult.IsFailure)
        {
            return Result.Failure(priorityResult.Error!);
        }

        var toDoItemResult = ToDoItem.ValidateThenCreate(idResult.Value, titleResult.Value, priorityResult.Value, command.DueDate);
        if (toDoItemResult.IsFailure)
        {
            return Result.Failure(toDoItemResult.Error!);
        }

        return toDoItemRepository.Add(toDoItemResult.Value);
    }
}
