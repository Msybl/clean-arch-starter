namespace CleanArchStarter.Domain.ValueObjects;

// PRINCIPLE-GAP(value-objects-for-status): a bare enum, unlike Priority. That is fine while
// only ToDoItem ever sets it, because nothing untrusted can put an undefined Status in.
// Revisit when items are loaded from storage (Enum.Parse("99") succeeds on undefined
// values) or when a second state transition appears.
public enum Status
{
    ToDo,
    InProgress,
    Blocked,
    Done,
    Archived
}
