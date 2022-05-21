namespace DIT.Workflower;

public class WorkflowDefinitionBuilder<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{
    private TransitionDefinition<TState, TCommand, TContext>? _current;

    public List<TransitionDefinition<TState, TCommand, TContext>> Transitions { get; } = new();


    public WorkflowDefinitionBuilder<TState, TCommand, TContext> From(in TState state)
    {
        if (_current != null)
            Transitions.Add(_current);

        _current = new() { From = state };

        return this;
    }

    public WorkflowDefinitionBuilder<TState, TCommand, TContext> To(in TState state)
    {
        if (_current == null)
            throw new InvalidOperationException($"From needs to be called first");

        _current = _current with { To = state };

        return this;
    }

    public WorkflowDefinitionBuilder<TState, TCommand, TContext> On(in TCommand command)
    {
        if (_current == null)
            throw new InvalidOperationException($"From needs to be called first");

        _current = _current with { Command = command };

        return this;
    }

    public WorkflowDefinitionBuilder<TState, TCommand, TContext> WithMeta(in object meta)
    {
        if (_current == null)
            throw new InvalidOperationException($"From needs to be called first");

        _current.Meta = meta;

        return this;
    }

    public WorkflowDefinitionBuilder<TState, TCommand, TContext> When(Func<TContext, bool> condition)
    {
        if (_current == null)
            throw new InvalidOperationException($"From needs to be called first");

        if (_current.Conditions is null)
            _current.Conditions = new();

        _current.Conditions.Add(condition);

        return this;
    }

    public WorkflowDefinition<TState, TCommand, TContext> Build()
    {
        if (_current != null)
            Transitions.Add(_current);

        if (!Transitions.Any())
            throw new InvalidOperationException("No transitions are added");

        return new(Transitions);
    }
}
