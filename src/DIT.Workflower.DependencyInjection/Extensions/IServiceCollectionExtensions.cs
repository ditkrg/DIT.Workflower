using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DIT.Workflower.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static ITransitionStart<TState, TCommand, ContextWrapper<TContext>> AddWorkflowDefinition<TState, TCommand, TContext>(
        this IServiceCollection services, in int version = 1)
        where TState : struct
        where TCommand : struct
    {
        var id = WorkflowDefinitionWrapper<TState, TCommand, TContext>.GetDefaultId();
        return AddWorkflowDefinition<TState, TCommand, TContext>(services, id, version);
    }

    public static ITransitionStart<TState, TCommand, ContextWrapper<TContext>> AddWorkflowDefinition<TState, TCommand, TContext>(
        this IServiceCollection services, in string id)
        where TState : struct
        where TCommand : struct
    {
        return AddWorkflowDefinition<TState, TCommand, TContext>(services, id, version: 1);
    }

    public static ITransitionStart<TState, TCommand, ContextWrapper<TContext>> AddWorkflowDefinition<TState, TCommand, TContext>(
        this IServiceCollection services, string id, int version)
        where TState : struct
        where TCommand : struct
    {
        var builder = WorkflowDefinitionBuilder<TState, TCommand, ContextWrapper<TContext>>.Create();
        var definition = (WorkflowDefinitionBuilder<TState, TCommand, ContextWrapper<TContext>>)builder;

        services.TryAddScoped<IWorkflowFactory<TState, TCommand, TContext>, DefaultWorkflowFactory<TState, TCommand, TContext>>();

        services.AddScoped<IWorkflow<TState, TCommand, TContext>, WorkflowDefinitionWrapper<TState, TCommand, TContext>>(sp =>
        {
            var wrapper = new WorkflowDefinitionWrapper<TState, TCommand, TContext>(definition, sp, id, version);
            return wrapper;
        });

        return builder;
    }
}
