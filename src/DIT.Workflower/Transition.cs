namespace DIT.Workflower;

public record Transition<TState, TCommand>
    where TState : struct
    where TCommand : struct
{
    public TState From { get; set; }

    public TState To { get; set; }

    public TCommand Command { get; set; }

    public object? Meta { get; set; }
}

public record TransitionDefinition<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{
    public TState From { get; set; }

    public TState To { get; set; }

    public TCommand Command { get; set; }

    public object? Meta { get; set; }

    public List<Func<TContext, bool>>? Conditions { get; internal set; }

    public Transition<TState, TCommand> ToTransition()
    {
        return (Transition<TState, TCommand>)this;
    }

    public static explicit operator Transition<TState, TCommand>(TransitionDefinition<TState, TCommand, TContext> definition)
    {
        return new()
        {
            From = definition.From,
            To = definition.To,
            Command = definition.Command,
            Meta = definition.Meta,
        };
    }

}
