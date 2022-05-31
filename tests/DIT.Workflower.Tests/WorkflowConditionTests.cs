namespace DIT.Workflower.Tests;

public class WorkflowConditionTests
{
    private static ITransitionStart<PhoneState, PhoneCommand, PhoneCall> GetDefaultBuilder()
    {
        return WorkflowDefinitionBuilder<PhoneState, PhoneCommand, PhoneCall>.Create();
    }

    [Fact]
    public void SingleConditionTests()
    {
        var phone = new PhoneCall(Active: false);

        var a = "b";

        var builder1 = GetDefaultBuilder()
            .From(PhoneState.Ringing)
            .On(PhoneCommand.Decline)
            .When((res) => a == "n")
            .To(PhoneState.Declined);

        var builder2 = GetDefaultBuilder()
            .From(PhoneState.Ringing)
            .On(PhoneCommand.Decline)
            .When((res) => a == "b" && res.Active is false)
            .To(PhoneState.OnHold);

        Assert.Empty(builder1.Build().GetAllowedTransitions(phone, PhoneState.Ringing));
        Assert.Single(builder2.Build().GetAllowedTransitions(phone, PhoneState.Ringing));
    }

    [Fact]
    public void MultiConditionTests()
    {
        var phone = new PhoneCall();

        var a = "b";
        var other = a;

        var builder1 = GetDefaultBuilder()
            .From(PhoneState.OnHold)
            .On(PhoneCommand.Resume)
            .When((res) => a == "c")
            .When((res) => other == a)
            .To(PhoneState.Connected);

        var builder2 = GetDefaultBuilder()
            .From(PhoneState.OnHold)
            .On(PhoneCommand.Resume)
            .When((res) => a == "b")
            .When((res) => other == a)
            .To(PhoneState.Connected);

        Assert.Empty(builder1.Build().GetAllowedTransitions(phone, from: PhoneState.OnHold));
        Assert.Single(builder2.Build().GetAllowedTransitions(phone, from: PhoneState.OnHold));
    }
}
