namespace DIT.Workflower;

public class WorkflowDefinition<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{

    private readonly List<TransitionDefinition<TState, TCommand, TContext>> _transitions;

    public WorkflowDefinition(List<TransitionDefinition<TState, TCommand, TContext>> transitions)
    {
        _transitions = transitions;
    }

    public List<TransitionDefinition<TState, TCommand, TContext>> GetAllTransitionDefinitions() => _transitions;

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


    public IEnumerable<Transition<TState, TCommand>> GetAllowedTransitions(ListTransitionsRequest<TState, TCommand, TContext> request)
    {
        var query = _transitions.AsQueryable();

        if (request.From is { } from)
        {
            query = query.Where(doc => doc.From.Equals(from));
        }

        if (request.To is { } to)
        {
            query = query.Where(doc => doc.To.Equals(to));
        }

        if (request.Command is { } command)
        {
            query = query.Where(doc => doc.Command.Equals(command));
        }

        if (request.Context is not null)
        {
            query = query.Where(doc => doc.Conditions == null || doc.Conditions.All(cond => cond(request.Context)));
        }

        return query.Select(x => x.ToTransition());
    }

}
