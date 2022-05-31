using DIT.Workflower.DependencyInjection.Abstractions;
using DIT.Workflower.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DIT.Workflower.Tests.DependencyInjection;

public class DependencyInjectionTests
{

    [Fact]
    public void Test()
    {

        var sc = new ServiceCollection();
        var id = "test";

        sc.AddWorkflowDefinition<PhoneState, PhoneCommand, PhoneCall>(id, version: 1)
            .From(PhoneState.Idle)
                .On(PhoneCommand.IncomingCall)
                .To(PhoneState.Ringing)

            .From(PhoneState.Ringing)
                .On(PhoneCommand.Decline)
                .To(PhoneState.Connected)
            ;

        sc.AddWorkflowDefinition<PhoneState, PhoneCommand, PhoneCall>(id, version: 2)
            .From(PhoneState.Idle)
                .On(PhoneCommand.IncomingCall)
                .To(PhoneState.Ringing)

            .From(PhoneState.Ringing)
                .On(PhoneCommand.Accept)
                .To(PhoneState.Connected)

            .From(PhoneState.Ringing)
                .On(PhoneCommand.Decline)
                .To(PhoneState.Declined)
            ;

        var sp = sc.BuildServiceProvider();

        var workflowFactory = sp.GetService<IWorkflowFactory<PhoneState, PhoneCommand, PhoneCall>>()!;


        var v1 = workflowFactory.CreateWorkflow(id);
        var v2 = workflowFactory.CreateWorkflow(id, version: 2);

        Assert.NotNull(workflowFactory);
        Assert.NotNull(v1);
        Assert.NotNull(v2);


        Assert.Single(v1!.GetAllowedTransitions(PhoneState.Idle));
        Assert.Single(v2!.GetAllowedTransitions(PhoneState.Idle));

        Assert.Single(v1.GetAllowedTransitions(PhoneState.Ringing));
        Assert.Equal(2, v2.GetAllowedTransitions(PhoneState.Ringing).Count);
    }

}
