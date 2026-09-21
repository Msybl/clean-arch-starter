namespace CleanArchStarter.Domain.ValueObjects;

using CleanArchStarter.Domain.Common;

// PRINCIPLE(value-objects): "a valid title" is a type, not a rule someone remembers to
// apply — an invalid Title cannot be constructed, so no caller can hold one.
// PRINCIPLE(single-source-of-truth-for-invariants): the only place the title rules are
// written — the command and the entity receive a Title, never a raw string to re-check.
public sealed record Title
{
    private const int MaxLength = 200;

    public string Value { get; }

    private Title(string value) => Value = value;

    public static Result<Title> ValidateThenCreate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<Title>.Failure(new Error("Title.Empty", "Title must not be empty."));
        }

        if (value.Length > MaxLength)
        {
            return Result<Title>.Failure(new Error("Title.TooLong", $"Title must not exceed {MaxLength} characters."));
        }

        return Result<Title>.Success(new Title(value));
    }
}
