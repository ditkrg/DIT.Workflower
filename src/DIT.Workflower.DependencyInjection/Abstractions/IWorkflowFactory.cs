namespace DIT.Workflower.DependencyInjection.Abstractions;

public interface IWorkflowFactory<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{

    public IWorkflow<TState, TCommand, TContext> CreateWorkflow(string id);

    public IWorkflow<TState, TCommand, TContext> CreateWorkflow(string id, int version);

}
