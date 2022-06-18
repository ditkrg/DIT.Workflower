namespace DIT.Workflower.DependencyInjection;

public class DefaultWorkflowFactory<TState, TCommand, TContext> : IWorkflowFactory<TState, TCommand, TContext>
        where TState : struct
        where TCommand : struct
{

    private readonly IServiceProvider _serviceProvider;

    public DefaultWorkflowFactory(IServiceProvider sp)
    {
        _serviceProvider = sp;
    }

    public IWorkflow<TState, TCommand, TContext> CreateWorkflow(int version = 1)
    {
        var id = WorkflowDefinitionWrapper<TState, TCommand, TContext>.GetDefaultId();
        return CreateWorkflow(id, version);
    }

    public IWorkflow<TState, TCommand, TContext> CreateWorkflow(string id, int version = 1)
    {
        var reference = $"{id}.v{version}";
        var service = _serviceProvider.GetServices<IWorkflow<TState, TCommand, TContext>>()
            .FirstOrDefault(x => x.Reference == reference);

        if (service is null)
            throw new KeyNotFoundException($"Workflow reference {id}.v{version} does not exist");

        return service;
    }
}
