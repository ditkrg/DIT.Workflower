namespace DIT.Workflower.DependencyInjection.Abstractions;

public interface IWorkflow<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{
    string Id { get; }

    int Version { get; }

    string Reference => $"{Id}.v{Version}";

    /// <summary>
    /// Retrieves all transition definitions for the workflow.
    /// </summary>
    /// <typeparam name="TState">The type of the workflow state.</typeparam>
    /// <typeparam name="TCommand">The type of the workflow command.</typeparam>
    /// <typeparam name="TContext">The type of the workflow context.</typeparam>
    /// <returns>A list of transition definitions.</returns>
    public List<TransitionDefinition<TState, TCommand, TContext>> GetAllTransitionDefinitions();

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

    /// <summary>
    /// Retrieves a list of allowed transitions based on the provided request.
    /// </summary>
    /// <typeparam name="TState">The type of the workflow state.</typeparam>
    /// <typeparam name="TCommand">The type of the workflow command.</typeparam>
    /// <param name="request">The request object containing the necessary information.</param>
    /// <returns>An enumerable of allowed transitions.</returns>
    public IEnumerable<Transition<TState, TCommand>> GetAllowedTransitions(ListTransitionsRequest<TState, TCommand, TContext> request);
}
