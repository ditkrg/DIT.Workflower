using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DIT.Workflower.DependencyInjection.Extensions;

public static class IServiceCollectionExtensions
{

    public static ITransitionOn<TState, TCommand, TContext> AddWorkflowDefinition<TState, TCommand, TContext>(this IServiceCollection services, TState initial)
        where TState : struct
        where TCommand : struct
    {
        return AddWorkflowDefinition<TState, TCommand, TContext>(services, initial, version: 1);
    }

    public static ITransitionOn<TState, TCommand, TContext> AddWorkflowDefinition<TState, TCommand, TContext>(this IServiceCollection services, TState initial, int version)
        where TState : struct
        where TCommand : struct
    {
        var builder = WorkflowDefinitionBuilder<TState, TCommand, TContext>.Initial(initial);

        services.TryAddSingleton<IWorkflowFactory<TState, TCommand, TContext>, DefaultWorkflowFactory<TState, TCommand, TContext>>();

        services.AddSingleton<IWorkflow<TState, TCommand, TContext>, WorkflowDefinitionWrapper<TState, TCommand, TContext>>(sp =>
        {
            var definition = ((WorkflowDefinitionBuilder<TState, TCommand, TContext>)builder);
            var wrapper = new WorkflowDefinitionWrapper<TState, TCommand, TContext>(definition, version);
            return wrapper;
        });

        return builder;
    }
}
