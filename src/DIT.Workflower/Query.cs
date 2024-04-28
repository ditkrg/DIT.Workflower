namespace DIT.Workflower;


public sealed class ListTransitionsRequest<TState, TCommand, TContext>
{
    public TState? From { get; set; }

    public TState? To { get; set; }

    public TCommand? Command { get; set; }

    public TContext? Context { get; set; }
}
