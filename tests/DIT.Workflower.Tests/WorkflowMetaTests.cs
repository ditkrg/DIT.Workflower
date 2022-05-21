namespace DIT.Workflower.Tests;

public class WorkflowMetaTests
{

    [Fact]
    public void WorkflowNeedsAtLeastOneTransition()
    {
        var builder = new WorkflowDefinitionBuilder<PhoneState, PhoneCommand, PhoneCall>();

        Assert.Throws<InvalidOperationException>(builder.Build);
    }

}
