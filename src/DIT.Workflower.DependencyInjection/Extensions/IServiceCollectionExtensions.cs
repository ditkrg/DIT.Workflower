using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DIT.Workflower.DependencyInjection.Extensions;

public static class IServiceCollectionExtensions
{

    public static ITransitionStart<TState, TCommand, DIContextWrapper<TContext>> AddWorkflowDefinition<TState, TCommand, TContext>(this IServiceCollection services, in int version = 1)
        where TState : struct
        where TCommand : struct
    {
        var id = WorkflowDefinitionWrapper<TState, TCommand, TContext>.GetDefaultId();
        return AddWorkflowDefinition<TState, TCommand, TContext>(services, id, version);
    }

    public static ITransitionStart<TState, TCommand, DIContextWrapper<TContext>> AddWorkflowDefinition<TState, TCommand, TContext>(this IServiceCollection services, in string id)
        where TState : struct
        where TCommand : struct
    {
        return AddWorkflowDefinition<TState, TCommand, TContext>(services, id, version: 1);
    }

    public static ITransitionStart<TState, TCommand, DIContextWrapper<TContext>> AddWorkflowDefinition<TState, TCommand, TContext>(this IServiceCollection services, string id, int version)
        where TState : struct
        where TCommand : struct
    {
        var builder = WorkflowDefinitionBuilder<TState, TCommand, DIContextWrapper<TContext>>.Create();

        services.TryAddSingleton<IWorkflowFactory<TState, TCommand, DIContextWrapper<TContext>>, DefaultWorkflowFactory<TState, TCommand, DIContextWrapper<TContext>>>();

        services.AddSingleton<IWorkflow<TState, TCommand, DIContextWrapper<TContext>>, WorkflowDefinitionWrapper<TState, TCommand, DIContextWrapper<TContext>>>(sp =>
        {
            var definition = (WorkflowDefinitionBuilder<TState, TCommand, DIContextWrapper<TContext>>)builder;
            var wrapper = new WorkflowDefinitionWrapper<TState, TCommand, DIContextWrapper<TContext>>(definition, id, version);
            return wrapper;
        });

        return builder;
    }
}
