using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DIT.Workflower.DependencyInjection.Extensions;

public static class IServiceCollectionExtensions
{

    public static ITransitionStart<TState, TCommand, TContext> AddWorkflowDefinition<TState, TCommand, TContext>(this IServiceCollection services, string id)
        where TState : struct
        where TCommand : struct
    {
        return AddWorkflowDefinition<TState, TCommand, TContext>(services, id, version: 1);
    }

    public static ITransitionStart<TState, TCommand, TContext> AddWorkflowDefinition<TState, TCommand, TContext>(this IServiceCollection services, string id, int version)
        where TState : struct
        where TCommand : struct
    {
        var builder = WorkflowDefinitionBuilder<TState, TCommand, TContext>.Create();

        services.TryAddSingleton<IWorkflowFactory<TState, TCommand, TContext>, DefaultWorkflowFactory<TState, TCommand, TContext>>();

        services.AddSingleton<IWorkflow<TState, TCommand, TContext>, WorkflowDefinitionWrapper<TState, TCommand, TContext>>(sp =>
        {
            var definition = (WorkflowDefinitionBuilder<TState, TCommand, TContext>)builder;
            var wrapper = new WorkflowDefinitionWrapper<TState, TCommand, TContext>(definition, id, version);
            return wrapper;
        });

        return builder;
    }
}
