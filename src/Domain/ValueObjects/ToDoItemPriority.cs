namespace CleanArchStarter.Domain.ValueObjects;

using CleanArchStarter.Domain.Common;

public enum PriorityLevel
{
    Trivial,
    Minor,
    Major,
    Critical,
    Blocker
}

// PRINCIPLE(value-objects): a priority is a type that can only hold a real level.
// Why: a C# enum is a named integer, so (PriorityLevel)99 is a legal value that can even
// bind straight from JSON — nothing about the enum itself stops it.
// Here: the only way in is ValidateThenCreate, which accepts a level's name (never a
// number), so a Priority always holds a defined level.
// PRINCIPLE(single-source-of-truth-for-invariants): the one place "is this a real priority"
// is decided; no other layer re-checks it.
public sealed record Priority
{
    public PriorityLevel Level { get; }

    private Priority(PriorityLevel level) => Level = level;

    public static Result<Priority> ValidateThenCreate(string value)
    {
        // Match on the name instead of using Enum.TryParse: TryParse also accepts "2" or
        // "99", which would let an undefined number through.
        var name = Enum.GetNames<PriorityLevel>()
            .FirstOrDefault(n => string.Equals(n, value, StringComparison.OrdinalIgnoreCase));

        if (name is null)
        {
            return Result<Priority>.Failure(new Error(
                "Priority.Invalid",
                $"Priority must be one of: {string.Join(", ", Enum.GetNames<PriorityLevel>())}."));
        }

        return Result<Priority>.Success(new Priority(Enum.Parse<PriorityLevel>(name)));
    }
}
