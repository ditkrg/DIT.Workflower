namespace DIT.Workflower.DependencyInjection.Abstractions;

public interface IWorkflow<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{
    string Id { get; }

    int Version { get; }

    string Reference => $"{Id}.v{Version}";

    /// <summary>
    /// Gets a list of allowed transitions without any condition checks.
    /// </summary>
    /// <param name="from">The incoming state</param>
    /// <returns>A list of available transitions</returns>
    public List<Transition<TState, TCommand>> GetAllowedTransitions(TState from);

    /// <summary>
    /// Gets a list of allowed transitions evaluated for the current context.
    /// </summary>
    /// <param name="context">The given context</param>
    /// <param name="from">The incoming state</param>
    /// <returns>A list of available transitions for the current context</returns>
    public List<Transition<TState, TCommand>> GetAllowedTransitions(TContext context, TState from);
}
