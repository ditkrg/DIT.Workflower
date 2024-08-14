namespace DIT.Workflower.DependencyInjection;

public record ContextWrapper<TContext>(TContext Context, IServiceProvider ServiceProvider);
