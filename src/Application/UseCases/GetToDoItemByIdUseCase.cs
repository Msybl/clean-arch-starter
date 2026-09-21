namespace CleanArchStarter.Application.UseCases;

using CleanArchStarter.Application.Dtos;
using CleanArchStarter.Application.InputPorts;
using CleanArchStarter.Application.InputPorts.Queries;
using CleanArchStarter.Application.OutputPorts;
using CleanArchStarter.Domain.Common;
using CleanArchStarter.Domain.ValueObjects;

public class GetToDoItemByIdUseCase(IToDoItemRepository repository) : IGetToDoItemByIdUseCase
{
    public Result<ToDoItemDto> Execute(GetToDoItemByIdQuery query)
    {
        var idResult = ToDoItemId.ValidateThenCreate(query.Id);
        if(idResult.IsFailure)
        {
            return Result<ToDoItemDto>.Failure(idResult.Error!);
        }

        var getByIdResult = repository.GetById(idResult.Value);
        if(getByIdResult.IsFailure)
        {
            return Result<ToDoItemDto>.Failure(getByIdResult.Error!);
        }

        return Result<ToDoItemDto>.Success(ToDoItemDto.FromDomain(getByIdResult.Value));
    }
}
