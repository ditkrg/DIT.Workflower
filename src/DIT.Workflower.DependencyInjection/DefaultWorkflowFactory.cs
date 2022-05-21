
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

    public IWorkflow<TState, TCommand, TContext> CreateWorkflow()
        => CreateWorkflow(version: 1);

    public IWorkflow<TState, TCommand, TContext> CreateWorkflow(int version)
    {
        var service = _serviceProvider.GetServices<IWorkflow<TState, TCommand, TContext>>()
            .FirstOrDefault(x => x.Version == version);

        if (service is null)
            throw new ArgumentOutOfRangeException(nameof(version), $"Version {version} of workflow does not exist");

        return service;
    }
}
