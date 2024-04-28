namespace DIT.Workflower.DependencyInjection;

public record DIContextWrapper<TContext>(TContext Context, IServiceProvider ServiceProvider);
