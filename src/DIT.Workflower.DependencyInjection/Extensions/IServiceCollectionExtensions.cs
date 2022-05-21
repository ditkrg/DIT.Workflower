using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DIT.Workflower.DependencyInjection.Extensions;

public static class IServiceCollectionExtensions
{

    public static WorkflowDefinitionBuilder<TState, TCommand, TContext> AddWorkflowDefinition<TState, TCommand, TContext>(this IServiceCollection services)
        where TState : struct
        where TCommand : struct
    {
        return AddWorkflowDefinition<TState, TCommand, TContext>(services, version: 1);
    }

    public static WorkflowDefinitionBuilder<TState, TCommand, TContext> AddWorkflowDefinition<TState, TCommand, TContext>(this IServiceCollection services, int version)
        where TState : struct
        where TCommand : struct
    {
        var builder = new WorkflowDefinitionBuilder<TState, TCommand, TContext>();

        services.TryAddSingleton<IWorkflowFactory<TState, TCommand, TContext>, DefaultWorkflowFactory<TState, TCommand, TContext>>();

        services.AddSingleton<IWorkflow<TState, TCommand, TContext>, WorkflowDefinitionWrapper<TState, TCommand, TContext>>(sp =>
        {
            var workflow = builder.Build();
            var wrapper = new WorkflowDefinitionWrapper<TState, TCommand, TContext>(builder, version);
            return wrapper;
        });

        return builder;
    }
}
