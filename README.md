# DIT.Workflower

Workflower is a library based on .NET Standard, To handle Finite State Machines (FSM) and workflow management.

## Getting Started

### Nuget

Install the latest nuget package into your ASP.NET Core application.

    ```sh
    dotnet add package DIT.Workflower
    ```

    You can also download support for Dependency Injection:

    ```sh
    dotnet add package DIT.Workflower.DependencyInjection
    ```

### Workflow Builder

```csharp
using DIT.Workflower;
```

```csharp

var builder = new WorkflowDefinitionBuilder<TState, TCommand, TContext>()
                .From(TState.State_1)
                    .On(TCommand.GoNext)
                    .To(TState.State_2)

                .From(TState.State_2)
                    .On(TCommand.GoNext)
                    .To(TState.State_3)
                    .AndOn(TCommand.GoBack)
                    .To(TState.State_1)

var workflow = builder.Build();

var allowedTransitions = workflow.GetAllowedTransitions(from: TState.State_2);

// allowedTransitions will be 2 transitions
// State_2 -> State_3 (GoNext)
// State_2 -> State_1 (GoBack)

```

### Dependency Injection

In the `Program.cs`, register the workflow factory, defining one or more workflow definitions.


```csharp
public record PhoneCall(bool Active);

services.AddWorkflowDefinition<PhoneState, PhoneCommand, PhoneCall>(id: "constant-id", version: 1)
        // Idle -> Ringing (Incoming)
        .From(PhoneState.Idle)
            .On(PhoneCommand.IncomingCall)
            .To(PhoneState.Ringing)

        // Ringing -> Connected (Accept & ctx.Active)
        .From(PhoneState.Ringing)
            .On(PhoneCommand.Accept)
            .To(PhoneState.Connected)
            .When(ctx => ctx.Active) // An instance of PhoneCall will become the ctx.

        // Ringing -> Declined (Decline)
        .From(PhoneState.Ringing)
            .On(PhoneCommand.Decline)
            .To(PhoneState.Declined)
```
```csharp
[Route("[controller]")]
public class SampleController : JsonApiControllerBase
{

    private readonly IWorkflow<PhoneState, PhoneCommand, PhoneCall> _workflow;

    public SampleController(IWorkflowFactory<PhoneState, PhoneCommand, PhoneCall> factory)
    {
        _workflow = factory.CreateWorkflow(id: "constant-id", version: 1); // Optional version param to support versioning on workflows.
    }

    [HttpGet("{state}")]
    [ProducesResponseType(typeof(JsonApiSingleDataResponse<InitiateRequest>), StatusCodes.Status200OK)]
    public ActionResult GetRequest(PhoneState state, bool isActive)
    {
        var phoneCall = new PhoneCall(isActive)
        var transitions = _workflow.GetAllowedTransitions(context: phoneCall, state);
        return Ok(transitions)
    }
}
```

Note: If you do not specify an id for the workflow, the default id is:
```csharp
$"{typeof(TState).Name}_{typeof(TCommand).Name}_{typeof(TContext).Name}";
```
