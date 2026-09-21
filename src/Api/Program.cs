using CleanArchStarter.Api;
using CleanArchStarter.Api.Contracts;
using CleanArchStarter.Application.InputPorts;
using CleanArchStarter.Application.InputPorts.Commands;
using CleanArchStarter.Application.InputPorts.Queries;
using CleanArchStarter.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// PRINCIPLE(composition-root): wiring lives here and in CompositionRoot.cs, isolated from
// business code — nothing in Domain, Application's use cases, or this file's endpoint
// bodies constructs a concrete dependency with `new`.
builder.Services
    .AddInfrastructure()
    .AddApplication()
    .AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => "Hello World!");
app.MapPost("/todoitem", (
    CreateToDoItemRequest request,
    [FromServices] ICreateToDoItemUseCase useCase) =>
    {
        var commandResult = CreateToDoItemCommand.ValidateThenCreate(request.Id, request.Title, request.Priority, request.DueDate);
        if (commandResult.IsFailure)
        {
            return commandResult.Error!.ToProblem();
        }

        var executeResult = useCase.Execute(commandResult.Value);

        return executeResult.Match(
            () => Results.Created($"/todoitem/{commandResult.Value.Id}", null),
            error => error.ToProblem());
    })
    .WithName("CreateToDoItem")
    .WithSummary("Creates a to-do item.")
    .WithDescription("Creates a to-do item with the client-supplied id. Responds 201 with a Location header and no body, or 409 if the id is already taken.")
    .WithTags("ToDoItems")
    .Produces(StatusCodes.Status201Created)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status409Conflict);

app.MapGet("/todoitem/{id}", (
    string id,
    [FromServices] IGetToDoItemByIdUseCase useCase) =>
    {
        var queryResult = GetToDoItemByIdQuery.ValidateThenCreate(id);
        if(queryResult.IsFailure)
        {
            return queryResult.Error!.ToProblem();
        }

        var executeResult = useCase.Execute(queryResult.Value);

        return executeResult.Match(
            toDoItemDto => Results.Ok(ToDoItemResponse.FromDto(toDoItemDto)),
            error => error.ToProblem());
    })
    .WithName("GetToDoItemById")
    .WithSummary("Gets a to-do item by id.")
    .WithTags("ToDoItems")
    .Produces<ToDoItemResponse>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status404NotFound)
    .ProducesProblem(StatusCodes.Status400BadRequest);

// Completing is a state change, so it is modelled as a sub-resource of the item rather
// than a verb in the path.
app.MapPost("/todoitem/{id}/completion", (
    string id,
    [FromServices] ICompleteToDoItemUseCase useCase) =>
    {
        var commandResult = CompleteToDoItemCommand.ValidateThenCreate(id);
        if (commandResult.IsFailure)
        {
            return commandResult.Error!.ToProblem();
        }

        var executeResult = useCase.Execute(commandResult.Value);

        return executeResult.Match(
            () => Results.NoContent(),
            error => error.ToProblem());
    })
    .WithName("CompleteToDoItem")
    .WithSummary("Completes a to-do item.")
    .WithDescription("Marks the to-do item as done and responds 204 with no body. An archived item cannot be completed.")
    .WithTags("ToDoItems")
    .Produces(StatusCodes.Status204NoContent)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound)
    .ProducesProblem(StatusCodes.Status409Conflict);

await app.RunAsync();
