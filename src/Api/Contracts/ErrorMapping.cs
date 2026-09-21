namespace CleanArchStarter.Api.Contracts;

using System.Net;
using CleanArchStarter.Domain.Common;

// PRINCIPLE(adapter-owns-protocol-translation): the only place that knows Error
// maps to HTTP at all — Domain's Error stays free of any HTTP-shaped field.
public static class ErrorMapping
{
    public static IResult ToProblem(this Error error) =>
        Results.Problem(
            detail: error.Message,
            statusCode: (int)StatusFor(error.Type),
            extensions: new Dictionary<string, object?> { ["code"] = error.Code });

    // PRINCIPLE(compiler-enforced-completeness): no `_` discard arm — CS8524 warns
    // if ErrorType ever grows a case this switch doesn't handle.
    private static HttpStatusCode StatusFor(ErrorType type) => type switch
    {
        ErrorType.Validation => HttpStatusCode.BadRequest,
        ErrorType.NotFound => HttpStatusCode.NotFound,
        ErrorType.Conflict => HttpStatusCode.Conflict,
        ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
        ErrorType.Unexpected => HttpStatusCode.InternalServerError,
    };
}
