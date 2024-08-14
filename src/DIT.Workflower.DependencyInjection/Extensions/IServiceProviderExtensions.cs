namespace DIT.Workflower.DependencyInjection.Extensions;

public static class ServiceProviderExtensions
{
    public static IWorkflowFactory<TState, TCommand, TContext> GetRequiredWorkflowFactory<TState, TCommand, TContext>(this IServiceProvider sp)
        where TState : struct
        where TCommand : struct
    {
        return sp.GetRequiredService<IWorkflowFactory<TState, TCommand, TContext>>();
    }

    public static IWorkflowFactory<TState, TCommand, TContext>? GetWorkflowFactory<TState, TCommand, TContext>(this IServiceProvider sp)
        where TState : struct
        where TCommand : struct
    {
        return sp.GetService<IWorkflowFactory<TState, TCommand, TContext>>();
    }

    public static IWorkflow<TState, TCommand, TContext> CreateWorkflow<TState, TCommand, TContext>(this IServiceProvider sp, int version = 1)
        where TState : struct
        where TCommand : struct
    {
        var factory = GetRequiredWorkflowFactory<TState, TCommand, TContext>(sp);
        return factory.CreateWorkflow(version);
    }

    public static IWorkflow<TState, TCommand, TContext> CreateWorkflow<TState, TCommand, TContext>(this IServiceProvider sp, string id, int version = 1)
        where TState : struct
        where TCommand : struct
    {
        var factory = GetRequiredWorkflowFactory<TState, TCommand, TContext>(sp);
        return factory.CreateWorkflow(id, version);
    }

}
