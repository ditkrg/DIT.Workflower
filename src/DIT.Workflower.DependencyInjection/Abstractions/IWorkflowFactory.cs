namespace DIT.Workflower.DependencyInjection.Abstractions;

public interface IWorkflowFactory<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{
    public IWorkflow<TState, TCommand, TContext> CreateWorkflow(int version = 1);

    public IWorkflow<TState, TCommand, TContext> CreateWorkflow(string id, int version = 1);

}
