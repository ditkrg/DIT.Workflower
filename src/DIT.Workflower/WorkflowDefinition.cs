namespace DIT.Workflower;

public record WorkflowDefinition<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{

    private readonly List<TransitionDefinition<TState, TCommand, TContext>> _transitions;

    public WorkflowDefinition(List<TransitionDefinition<TState, TCommand, TContext>> transitions)
    {
        _transitions = transitions;
    }

    /// <summary>
    /// Lists all allowed transitions from current state for the given context.
    /// </summary>
    public List<Transition<TState, TCommand>> GetAllowedTransitions(TState from)
    {
        var query = _transitions.Where(doc => doc.From.Equals(from));

        return query.Select(x => x.ToTransition()).ToList();
    }

    /// <summary>
    /// Lists all allowed transitions from current state for the given context.
    /// </summary>
    public List<Transition<TState, TCommand>> GetAllowedTransitions(TContext context, TState from)
    {
        var query = _transitions.Where(doc => doc.From.Equals(from));

        query = query.Where(doc => doc.Conditions is null || !doc.Conditions.Any(cond => !cond(context)));

        return query.Select(x => x.ToTransition()).ToList();
    }
}
