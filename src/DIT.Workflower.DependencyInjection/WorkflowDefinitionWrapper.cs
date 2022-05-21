namespace DIT.Workflower.DependencyInjection;

public record WorkflowDefinitionWrapper<TState, TCommand, TContext> : WorkflowDefinition<TState, TCommand, TContext>, IWorkflow<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{

    public int Version { get; }

    public WorkflowDefinitionWrapper(WorkflowDefinitionBuilder<TState, TCommand, TContext> builder, int version)
        : base(builder.Transitions)
    {
        Version = version;
    }

}
