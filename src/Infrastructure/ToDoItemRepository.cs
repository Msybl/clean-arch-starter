namespace CleanArchStarter.Infrastructure;

using System.Collections.Concurrent;
using CleanArchStarter.Application.OutputPorts;
using CleanArchStarter.Domain.Common;
using CleanArchStarter.Domain.Entities;
using CleanArchStarter.Domain.ValueObjects;

// PRINCIPLE(repository-as-collection-illusion): callers just do Add(x), same as
// adding to a list — nothing about ConcurrentDictionary leaks past this class.
public sealed class ToDoItemRepository : IToDoItemRepository
{
    private readonly ConcurrentDictionary<ToDoItemId, ToDoItem> _items = new();

    public Result Add(ToDoItem toDoItem)
    {
        // TryAdd rather than the indexer: the client chooses the id, so an id that is
        // already taken must be refused, not silently overwrite someone else's item.
        if (!_items.TryAdd(toDoItem.Id, toDoItem))
        {
            return Result.Failure(new Error(
                "ToDoItemRepository.DuplicateId",
                $"A to-do item with id '{toDoItem.Id.Value}' already exists.",
                ErrorType.Conflict));
        }

        return Result.Success();
    }

    public Result<ToDoItem> GetById(ToDoItemId id)
    {
        if (!_items.TryGetValue(id, out var toDoItem))
        {
            return Result<ToDoItem>.Failure(new Error(
                "ToDoItemRepository.Id",
                $"To-do item '{id.Value}' was not found.",
                ErrorType.NotFound));
        }

        return Result<ToDoItem>.Success(toDoItem);
    }

    public Result Update(ToDoItem toDoItem)
    {
        if (!_items.ContainsKey(toDoItem.Id))
        {
            return Result.Failure(new Error(
                "ToDoItemRepository.Id",
                $"To-do item '{toDoItem.Id.Value}' was not found.",
                ErrorType.NotFound));
        }

        _items[toDoItem.Id] = toDoItem;
        return Result.Success();
    }
}
