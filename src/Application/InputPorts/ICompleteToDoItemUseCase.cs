using CleanArchStarter.Application.InputPorts.Commands;
using CleanArchStarter.Domain.Common;

namespace CleanArchStarter.Application.InputPorts;

// PRINCIPLE(dependency-inversion): the core defines this interface; Api depends on
// it, never on the concrete CompleteToDoItemUseCase class.
// PRINCIPLE(command-query-separation): a command changes state and answers only "worked"
// or "failed" — it returns no data; the caller reads the item back with a query.
public interface ICompleteToDoItemUseCase
{
    Result Execute(CompleteToDoItemCommand command);
}
