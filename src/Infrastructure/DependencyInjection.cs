namespace CleanArchStarter.Infrastructure;

using CleanArchStarter.Application.OutputPorts;
using Microsoft.Extensions.DependencyInjection;

// PRINCIPLE(composition-root): the only place that knows ToDoItemRepository is the
// concrete IToDoItemRepository implementation.
public static class DependencyInjection
{
    // PRINCIPLE(repository-as-collection-illusion): Singleton, deliberately — the
    // in-memory store has to survive across requests, unlike the default Scoped
    // lifetime most services use.
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services.AddSingleton<IToDoItemRepository, ToDoItemRepository>();
}