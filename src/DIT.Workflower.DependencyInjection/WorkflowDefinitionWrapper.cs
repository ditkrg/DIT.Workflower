namespace DIT.Workflower.DependencyInjection;

public sealed class WorkflowDefinitionWrapper<TState, TCommand, TContext> : IWorkflow<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{

    private readonly IServiceProvider _serviceProvider;
    private readonly WorkflowDefinition<TState, TCommand, ContextWrapper<TContext>> _definition;

    public string Id { get; }

    public int Version { get; }
    public List<TransitionDefinition<TState, TCommand, ContextWrapper<TContext>>> GetAllTransitionDefinitions()
    {
        return _definition.GetAllTransitionDefinitions();
    }

    public List<Transition<TState, TCommand>> GetAllowedTransitions(TState from)
    {
        return _definition.GetAllowedTransitions(from);
    }

    public List<Transition<TState, TCommand>> GetAllowedTransitions(TContext context, TState from)
    {
        return _definition.GetAllowedTransitions(GetContextWrapper(context), from);
    }

    public IEnumerable<Transition<TState, TCommand>> GetAllowedTransitions(ListTransitionsRequest<TState, TCommand, TContext> request)
    {
        var wrappedRequest = new ListTransitionsRequest<TState, TCommand, ContextWrapper<TContext>>
        {
            From = request.From,
            To = request.To,
            Command = request.Command,
            Context = request.Context is null ? null : GetContextWrapper(request.Context)
        };

        return _definition.GetAllowedTransitions(wrappedRequest);
    }

    public WorkflowDefinitionWrapper(WorkflowDefinitionBuilder<TState, TCommand, ContextWrapper<TContext>> builder, IServiceProvider serviceProvider, string id, int version)
    {
        _definition = builder.Build();
        _serviceProvider = serviceProvider;

        Id = id;
        Version = version;
    }

    private ContextWrapper<TContext> GetContextWrapper(in TContext context)
        => new(context, _serviceProvider);

    public static string GetDefaultId()
    {
        var ctxType = typeof(TContext);
        var ctxName = ctxType.Name;

        if (ctxType.IsGenericType)
            ctxName = string.Join("+", typeof(TContext).GetGenericArguments().Select(x => x.Name));

        return $"{typeof(TState).Name}_{typeof(TCommand).Name}_{ctxName}";
    }

}
