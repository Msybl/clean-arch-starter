using CleanArchStarter.Application.InputPorts.Commands;
using CleanArchStarter.Domain.Common;

namespace CleanArchStarter.Application.InputPorts;

// PRINCIPLE(dependency-inversion): the core defines this interface; Api depends on
// it, never on the concrete CreateToDoItemUseCase class.
// PRINCIPLE(command-query-separation): a command changes state and answers only "worked"
// or "failed" — it returns no data.
// Why: an operation that both changes state and hands data back is harder to reason about
// and to retry; reading is a separate query.
// Here: the client supplies the id, so it already knows where to read the item back.
public interface ICreateToDoItemUseCase
{
    Result Execute(CreateToDoItemCommand command);
}
