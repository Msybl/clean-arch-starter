namespace CleanArchStarter.Domain.Common;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Unexpected
}

// PRINCIPLE(simple-until-proven-insufficient): flat Code + Message + Type, not a
// sealed hierarchy of error subtypes — upgrade only once a concrete need is felt.
// PRINCIPLE(adapter-owns-protocol-translation): pure business vocabulary, no
// HTTP-shaped field here — Api/Contracts/ErrorMapping.cs is the only place that
// knows HTTP exists.
public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Validation);

// PRINCIPLE(result-over-exceptions): expected failures are values, not throws.
// PRINCIPLE(static-analysis-enforced-invariant): nothing in C# forces a caller to look at a
// Result. The ErrorProne.NET analyzer (EPC13, set to error in .editorconfig) fails the
// build if one is ignored — the analyzer enforces it, not the compiler itself.
public sealed class Result<T>
{
    private readonly T? _value;
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    // The throw here is the *other half* of result-over-exceptions, not a
    // contradiction of it: a caller accessing Value without checking IsSuccess
    // first is a programmer mistake, exactly the case exceptions stay reserved for.
    public T Value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access Value of a failed Result.");

    private Result(bool isSuccess, T? value, Error? error) =>
        (IsSuccess, _value, Error) = (isSuccess, value, error);

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(Error error) => new(false, default, error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess(Value) : onFailure(Error!);
}

public sealed class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    private Result(bool isSuccess, Error? error) =>
        (IsSuccess, Error) = (isSuccess, error);

    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess() : onFailure(Error!);
}