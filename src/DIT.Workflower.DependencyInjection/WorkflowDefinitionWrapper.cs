namespace DIT.Workflower.DependencyInjection;

public record WorkflowDefinitionWrapper<TState, TCommand, TContext> : WorkflowDefinition<TState, TCommand, TContext>, IWorkflow<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{

    public string Id { get; }

    public int Version { get; }

    public WorkflowDefinitionWrapper(WorkflowDefinitionBuilder<TState, TCommand, TContext> builder, string id, int version)
        : base(builder.Transitions)
    {
        Id = id;
        Version = version;
    }

    public static string GetDefaultId()
    {
        return $"{typeof(TState).Name}_{typeof(TCommand).Name}_{typeof(TContext).Name}";
    }

}
