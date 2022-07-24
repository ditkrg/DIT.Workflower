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
        var ctxType = typeof(TContext);
        var ctxName = ctxType.Name;

        if (ctxType.IsGenericType)
            ctxName = string.Join("+", typeof(TContext).GetGenericArguments().Select(x => x.Name));

        return $"{typeof(TState).Name}_{typeof(TCommand).Name}_{ctxName}";
    }

}
