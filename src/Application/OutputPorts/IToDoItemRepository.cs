using CleanArchStarter.Domain.Common;
using CleanArchStarter.Domain.Entities;
using CleanArchStarter.Domain.ValueObjects;

namespace CleanArchStarter.Application.OutputPorts;

// PRINCIPLE(dependency-inversion): Application owns this interface; Infrastructure
// implements it and depends inward, never the reverse.
// PRINCIPLE(role-interface-in-domain-language): the port is shaped by what the use cases
// need, in the domain's own words.
// Why: an interface copied from a storage technology drags that technology's vocabulary
// into the core; one shaped by its callers keeps it out.
// Here: no name mentions how items are stored, and the methods are only the ones a use
// case asked for.
// PRINCIPLE(simple-until-proven-insufficient): each method exists because a use case needed
// it (Add, GetById, Update) — no speculative GetAll/Delete/etc. ahead of one.
//
// This is the one port that speaks in domain entities (ToDoItem) instead of plain DTOs: a
// repository exists to store and rebuild them, so it cannot avoid it. Any other port a
// use case needs (a notifier, say) should take plain data instead.
public interface IToDoItemRepository
{
    Result Add(ToDoItem toDoItem);
    Result<ToDoItem> GetById(ToDoItemId id);
    Result Update(ToDoItem toDoItem);
}
