using CleanArchStarter.Application.Dtos;
using CleanArchStarter.Application.InputPorts.Queries;
using CleanArchStarter.Domain.Common;

namespace CleanArchStarter.Application.InputPorts;

public interface IGetToDoItemByIdUseCase
{
    public Result<ToDoItemDto> Execute(GetToDoItemByIdQuery query);
}