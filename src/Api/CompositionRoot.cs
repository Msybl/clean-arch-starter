namespace CleanArchStarter.Api;

using CleanArchStarter.Application.InputPorts;
using CleanArchStarter.Application.UseCases;

// PRINCIPLE(composition-root): the one place that says which concrete class implements
// which use case interface.
// Why: if business code did its own wiring it would depend on concrete classes and could
// not be swapped or tested apart.
// Here: only this file and Program.cs know the use case classes; endpoints see just the
// interfaces, and the Application project has no DI package at all
// (PRINCIPLE core-must-not-depend-on-framework).
public static class CompositionRoot
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddScoped<ICreateToDoItemUseCase, CreateToDoItemUseCase>()
            .AddScoped<IGetToDoItemByIdUseCase, GetToDoItemByIdUseCase>()
            .AddScoped<ICompleteToDoItemUseCase, CompleteToDoItemUseCase>();
}
