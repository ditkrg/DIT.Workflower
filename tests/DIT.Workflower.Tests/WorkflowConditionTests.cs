namespace DIT.Workflower.Tests;

public class WorkflowConditionTests
{
    private WorkflowDefinitionBuilder<PhoneState, PhoneCommand, PhoneCall> GetDefaultBuilder()
    {
        return new();
    }

    [Fact]
    public void SingleConditionTests()
    {
        var phone = new PhoneCall(Active: false);
        var builder1 = GetDefaultBuilder();
        var builder2 = GetDefaultBuilder();

        var a = "b";

        builder1
            .From(PhoneState.Idle)
            .When((res) => a == "n");

        builder2
            .From(PhoneState.Idle)
            .When((res) => a == "b" && res.Active is false);

        Assert.Empty(builder1.Build().GetAllowedTransitions(phone, PhoneState.Idle));
        Assert.Single(builder2.Build().GetAllowedTransitions(phone, PhoneState.Idle));
    }

    [Fact]
    public void MultiConditionTests()
    {
        var phone = new PhoneCall();
        var builder1 = GetDefaultBuilder();
        var builder2 = GetDefaultBuilder();

        var a = "b";
        var other = a;

        builder1
            .From(PhoneState.Idle)
            .When((res) => a == "c")
            .When((res) => other == a);

        builder2
            .From(PhoneState.Idle)
            .When((res) => a == "b")
            .When((res) => other == a);

        Assert.Empty(builder1.Build().GetAllowedTransitions(phone, PhoneState.Idle));
        Assert.Single(builder2.Build().GetAllowedTransitions(phone, PhoneState.Idle));
    }
}
