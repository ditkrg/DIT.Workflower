using DIT.Workflower.Abstractions;

namespace DIT.Workflower;

public sealed class WorkflowDefinitionBuilder<TState, TCommand, TContext> :
    ITransitionStart<TState, TCommand, TContext>,
    ITransitionOn<TState, TCommand, TContext>,
    ITransitionExit<TState, TCommand, TContext>,
    ITransitionCondition<TState, TCommand, TContext>,
    ITransitionDone<TState, TCommand, TContext>

    where TState : struct
    where TCommand : struct
{
    private TransitionDefinition<TState, TCommand, TContext> _current;

    public List<TransitionDefinition<TState, TCommand, TContext>> Transitions { get; } = new();

    #region Constructor 

    private WorkflowDefinitionBuilder()
        => _current = new();

    public static ITransitionStart<TState, TCommand, TContext> Create()
        => new WorkflowDefinitionBuilder<TState, TCommand, TContext>();

    #endregion

    #region Methods

    public ITransitionOn<TState, TCommand, TContext> From(in TState state)
    {
        _current = new() { From = state };

        return this;
    }

    public ITransitionDone<TState, TCommand, TContext> To(in TState state)
    {
        _current = _current with { To = state };

        Transitions.Add(_current);

        return this;
    }

    public ITransitionCondition<TState, TCommand, TContext> On(in TCommand command)
    {
        _current = _current with { Command = command };

        return this;
    }

    public ITransitionCondition<TState, TCommand, TContext> WithMeta(in object meta)
    {
        _current.Meta = meta;

        return this;
    }

    public ITransitionCondition<TState, TCommand, TContext> When(Func<TContext, bool> condition)
    {
        if (_current.Conditions is null)
            _current.Conditions = new();

        _current.Conditions.Add(condition);

        return this;
    }

    public WorkflowDefinition<TState, TCommand, TContext> Build()
    {
        if (!Transitions.Any())
            throw new InvalidOperationException("No transitions are added");

        return new(Transitions);
    }

    #endregion
}
