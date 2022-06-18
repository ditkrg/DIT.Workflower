namespace DIT.Workflower.Abstractions;

public interface ITransitionStart<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{
    ITransitionOn<TState, TCommand, TContext> From(in TState state);
}

public interface ITransitionOn<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{
    ITransitionCondition<TState, TCommand, TContext> On(in TCommand command);
}

public interface ITransitionExit<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{
    ITransitionDone<TState, TCommand, TContext> To(in TState state);
}

public interface ITransitionCondition<TState, TCommand, TContext> : ITransitionExit<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{
    ITransitionCondition<TState, TCommand, TContext> WithMeta(in object meta);

    ITransitionCondition<TState, TCommand, TContext> When(Func<TContext, bool> condition);

}

public interface ITransitionDone<TState, TCommand, TContext>
    where TState : struct
    where TCommand : struct
{

    ITransitionOn<TState, TCommand, TContext> From(in TState state);

    WorkflowDefinition<TState, TCommand, TContext> Build();
}
